/*===============================================================================
Copyright (C) 2020 Immersal Ltd. All Rights Reserved.

This file is part of the Immersal SDK.

The Immersal SDK cannot be copied, distributed, or made available to
third-parties for commercial purposes without written permission of Immersal Ltd.

Contact sdk@immersal.com for licensing requests.
===============================================================================*/

using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Threading.Tasks;
using Immersal.REST;

namespace Immersal.AR
{
    public class ARLocalizer : LocalizerBase
	{
		public event MapChanged OnMapChanged = null;
		public event PoseFound OnPoseFound = null;
		public delegate void MapChanged(int newMapHandle);
		public delegate void PoseFound(LocalizerPose newPose);

		private static ARLocalizer instance = null;

		private void ARSessionStateChanged(ARSessionStateChangedEventArgs args)
		{
			CheckTrackingState(args.state);
		}

		private void CheckTrackingState(ARSessionState newState)
		{
			isTracking = newState == ARSessionState.SessionTracking;

			if (!isTracking)
			{
				foreach (KeyValuePair<Transform, SpaceContainer> item in ARSpace.transformToSpace)
					item.Value.filter.InvalidateHistory();
			}
		}

		public static ARLocalizer Instance
		{
			get
			{
#if UNITY_EDITOR
				if (instance == null && !Application.isPlaying)
				{
					instance = UnityEngine.Object.FindObjectOfType<ARLocalizer>();
				}
#endif
				if (instance == null)
				{
					Debug.LogError("No ARLocalizer instance found. Ensure one exists in the scene.");
				}
				return instance;
			}
		}

		void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
			if (instance != this)
			{
				Debug.LogError("There must be only one ARLocalizer object in a scene.");
				UnityEngine.Object.DestroyImmediate(this);
				return;
			}
		}

        public override void Start()
        {
            base.Start();
			m_Sdk.Localizer = instance;
        }

		public override void OnEnable()
		{
			base.OnEnable();
#if !UNITY_EDITOR
			CheckTrackingState(ARSession.state);
			ARSession.stateChanged += ARSessionStateChanged;
#endif
		}

		public override void OnDisable()
		{
#if !UNITY_EDITOR
			ARSession.stateChanged -= ARSessionStateChanged;
#endif
			base.OnDisable();
		}

        public override async void LocalizeServer(SDKMapId[] mapIds)
        {
#if PLATFORM_LUMIN
            XRCameraImage image;
#else
            XRCpuImage image;
#endif
            ARCameraManager cameraManager = m_Sdk.cameraManager;
            var cameraSubsystem = cameraManager.subsystem;

#if PLATFORM_LUMIN
            if (cameraSubsystem.TryGetLatestImage(out image))
#else
            if (cameraSubsystem.TryAcquireLatestCpuImage(out image))
#endif
            {
				stats.localizationAttemptCount++;

                JobLocalizeServerAsync j = new JobLocalizeServerAsync();

                byte[] pixels;
                Camera cam = Camera.main;
                Vector3 camPos = cam.transform.position;
                Quaternion camRot = cam.transform.rotation;
				Vector4 intrinsics;
                int channels = 1;
                int width = image.width;
                int height = image.height;

                ARHelper.GetIntrinsics(out intrinsics);
				ARHelper.GetPlaneData(out pixels, image);

				float startTime = Time.realtimeSinceStartup;

                Task<(byte[], icvCaptureInfo)> t = Task.Run(() =>
                {
                    byte[] capture = new byte[channels * width * height + 1024];
                    icvCaptureInfo info = Immersal.Core.CaptureImage(capture, capture.Length, pixels, width, height, channels);
                    Array.Resize(ref capture, info.captureSize);
                    return (capture, info);
                });

                await t;

                j.image = t.Result.Item1;
				j.position = camPos;
				j.rotation = camRot;
				j.intrinsics = intrinsics;
				j.mapIds = mapIds;

                j.OnResult += async (SDKLocalizeResult result) =>
                {
					float elapsedTime = Time.realtimeSinceStartup - startTime;

					if (result.success)
					{
						Debug.Log("*************************** On-Server Localization Succeeded ***************************");
						Debug.Log(string.Format("Relocalized in {0} seconds", elapsedTime));

						int mapServerId = result.map;

						if (mapServerId > 0 && ARSpace.mapHandleToOffset.ContainsKey(mapServerId))
						{
							if (mapServerId != lastLocalizedMapHandle)
							{
								if (resetOnMapChange)
								{
									Reset();
								}
								
								lastLocalizedMapHandle = mapServerId;

								OnMapChanged?.Invoke(mapServerId);
							}

							MapOffset mo = ARSpace.mapHandleToOffset[mapServerId];
							stats.localizationSuccessCount++;
							
							Matrix4x4 responseMatrix = Matrix4x4.identity;
							responseMatrix.m00 = result.r00; responseMatrix.m01 = result.r01; responseMatrix.m02 = result.r02; responseMatrix.m03 = result.px;
							responseMatrix.m10 = result.r10; responseMatrix.m11 = result.r11; responseMatrix.m12 = result.r12; responseMatrix.m13 = result.py;
							responseMatrix.m20 = result.r20; responseMatrix.m21 = result.r21; responseMatrix.m22 = result.r22; responseMatrix.m23 = result.pz;
							
							Vector3 pos = responseMatrix.GetColumn(3);
							Quaternion rot = responseMatrix.rotation;
							ARHelper.GetRotation(ref rot);

							Matrix4x4 offsetNoScale = Matrix4x4.TRS(mo.position, mo.rotation, Vector3.one);
							Vector3 scaledPos = Vector3.Scale(pos, mo.scale);
							Matrix4x4 cloudSpace = offsetNoScale * Matrix4x4.TRS(scaledPos, rot, Vector3.one);
							Matrix4x4 trackerSpace = Matrix4x4.TRS(camPos, camRot, Vector3.one);
							Matrix4x4 m = trackerSpace * (cloudSpace.inverse);

							if (useFiltering)
								mo.space.filter.RefinePose(m);
							else
								ARSpace.UpdateSpace(mo.space, m.GetColumn(3), m.rotation);

							JobEcefAsync je = new JobEcefAsync();
							je.id = mapServerId;
							je.OnResult += (SDKEcefResult result2) =>
							{
								LocalizerPose localizerPose;
								LocalizerBase.GetLocalizerPose(out localizerPose, mapServerId, pos, rot, m.inverse, result2.ecef);
								OnPoseFound?.Invoke(localizerPose);
							};

							await je.RunJobAsync();

							if (ARSpace.mapHandleToMap.ContainsKey(mapServerId))
							{
								ARMap map = ARSpace.mapHandleToMap[mapServerId];
								map.NotifySuccessfulLocalization(mapServerId);
							}
						}
						else
						{
							Debug.Log(string.Format("Localization attempt failed after {0} seconds", elapsedTime));
						}
					}
					else
					{
						Debug.Log("*************************** On-Server Localization Failed ***************************");
					}
                };

				await j.RunJobAsync();

                image.Dispose();
            }

			base.LocalizeServer(mapIds);
        }

        public override async void Localize()
		{
#if PLATFORM_LUMIN
			XRCameraImage image;
#else
			XRCpuImage image;
#endif
			ARCameraManager cameraManager = m_Sdk.cameraManager;
			var cameraSubsystem = cameraManager.subsystem;

#if PLATFORM_LUMIN
			if (cameraSubsystem != null && cameraSubsystem.TryGetLatestImage(out image))
#else
			if (cameraSubsystem != null && cameraSubsystem.TryAcquireLatestCpuImage(out image))
#endif
			{
				stats.localizationAttemptCount++;
				Vector4 intrinsics;
				Vector3 camPos = m_Cam.transform.position;
				Quaternion camRot = m_Cam.transform.rotation;
				ARHelper.GetIntrinsics(out intrinsics);
				ARHelper.GetPlaneDataFast(ref m_PixelBuffer, image);

				if (m_PixelBuffer != IntPtr.Zero)
				{
					Vector3 pos = Vector3.zero;
					Quaternion rot = Quaternion.identity;

					float startTime = Time.realtimeSinceStartup;

					Task<int> t = Task.Run(() =>
					{
						return Immersal.Core.LocalizeImage(out pos, out rot, image.width, image.height, ref intrinsics, m_PixelBuffer);
					});

					await t;

					int mapHandle = t.Result;
					float elapsedTime = Time.realtimeSinceStartup - startTime;

					if (mapHandle >= 0 && ARSpace.mapHandleToOffset.ContainsKey(mapHandle))
					{
						Debug.Log(string.Format("Relocalized in {0} seconds", elapsedTime));
						stats.localizationSuccessCount++;

						if (mapHandle != lastLocalizedMapHandle)
						{
							if (resetOnMapChange)
							{
								Reset();
							}
							
							lastLocalizedMapHandle = mapHandle;

							OnMapChanged?.Invoke(mapHandle);
						}

						ARHelper.GetRotation(ref rot);
						MapOffset mo = ARSpace.mapHandleToMap.ContainsKey(mapHandle) ? ARSpace.mapHandleToOffset[mapHandle] : null;

						if (mo == null)
						{
							return;
						}

						Matrix4x4 offsetNoScale = Matrix4x4.TRS(mo.position, mo.rotation, Vector3.one);
						Vector3 scaledPos = new Vector3
							(
								pos.x * mo.scale.x,
								pos.y * mo.scale.y,
								pos.z * mo.scale.z
							);
						Matrix4x4 cloudSpace = offsetNoScale * Matrix4x4.TRS(scaledPos, rot, Vector3.one);
						Matrix4x4 trackerSpace = Matrix4x4.TRS(camPos, camRot, Vector3.one);
						Matrix4x4 m = trackerSpace * (cloudSpace.inverse);

						if (useFiltering)
							mo.space.filter.RefinePose(m);
						else
							ARSpace.UpdateSpace(mo.space, m.GetColumn(3), m.rotation);

						LocalizerPose localizerPose;
						GetLocalizerPose(out localizerPose, mapHandle, pos, rot, m.inverse);
						OnPoseFound?.Invoke(localizerPose);

						if (ARSpace.mapHandleToMap.ContainsKey(mapHandle))
						{
							ARMap map = ARSpace.mapHandleToMap[mapHandle];
							map.NotifySuccessfulLocalization(mapHandle);
						}
					}
					else
					{
						Debug.Log(string.Format("Localization attempt failed after {0} seconds", elapsedTime));
					}
				}

				image.Dispose();
			}

			base.Localize();
		}
	}
}