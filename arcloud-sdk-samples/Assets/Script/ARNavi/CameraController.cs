using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // // WebCamTexture WebCamTexture;    

    // // Start is called before the first frame update
    // void Start()
    // {
    //     yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
    //     if (Application.HasUserAuthorization(UserAuthorization.WebCam)) {
    //         // UnityのWebCamTextureで処理
    //     }
    //     StartCamera();
    //     // // 画面の向きを固定（端末の向きに応じて回転させない設定）
    //     // Screen.orientation = ScreenOrientation.Portrait;
    //     // // 端末のカメラを読み込み
    //     // WebCamDevice[] devices = WebCamTexture.devices;
    //     // // テクスチャとして背面カメラを設定(devices[0]で背面、devices[1]で前面）
    //     // WebCamTexture webCamTexture = new WebCamTexture(devices[0].name);
    //     // // このScriptを付与したオブジェクトにテクスチャを反映
    //     // GetComponent<Renderer>().material.mainTexture = webCamTexture;
    //     // // カメラを起動
    //     // webCamTexture.Play();
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }

    // public void StartCamera()
    // {
    //     // 処理はコルーチンで実行する
    //     StartCoroutine(_startCamera());
    // }

    // IEnumerator _startCamera()
    // {
    //     // 指定カメラを起動させる
    //     webCam = new WebCamTexture.devices[0].name, 640, 480);

    //     // RawImageのテクスチャにWebCamTextureのインスタンスを設定
    //     RawImage.texture = webCam;
    //     // カメラ起動
    //     webCam.Play();
    //     while(webCam.width != webCam.requestedWidth)
    //     {
    //         // widthが指定したものになっていない場合は処理を抜けて次のフレームで再開
    //         yield return null;
    //     }
    //     yield break;
    // }

    //     if (webCam.videoVerticallyMirrored)
    //     {
    //         //左右反転しているのを戻す
    //         Vector3 scaletmp = RawImage.GetComponent<RectTransform>().localScale;
    //         scaletmp.y = -1;
    //         RawImage.GetComponent<RectTransform>().localScale = scaletmp;
    //     }

    //     // 起動させて初めてvideoRotationAngle、width、heightに値が入り、
    //     // アスペクト比、何度回転させれば正しく表示されるかがわかる
    //     // 表示するRawImageを回転させる
    //     Vector3 angles = RawImage.GetComponent<RectTransform>().eulerAngles;
    //     angles.z = -webCam.videoRotationAngle;
    //     RawImage.GetComponent<RectTransform>().eulerAngles = angles;
    //     // widthはx、heightはyでサイズ調整
    //     // 全体を表示させるように、大きい方のサイズを表示枠に合わせる
    //     float scaler;
    //     Vector2 sizetmp = RawImage.GetComponent<RectTransform>().sizeDelta;
    //     if (webCam.width > webCam.height)
    //     {
    //         scaler = sizetmp.x / webCam.width;
    //     }
    //     else
    //     {
    //         scaler = sizetmp.y / webCam.height;
    //     }
    //     // サイズ調整
    //     sizetmp.x = webCam.width * scaler;
    //     sizetmp.y = webCam.height * scaler;
    //     // 表示領域サイズ設定
    //     RawImage.GetComponent<RectTransform>().sizeDelta = sizetmp;
    //     if (WebCamTexture.devices[0].isFrontFacing ) {
    //         // Face Cameraの場合は反転表示させる回転の向きによって、どの軸で回転させるかが異なる
    //         Vector3 scaletmp = RawImage.GetComponent<RectTransform>().localScale;
    //         if ((webCam.videoRotationAngle == 90) || (webCam.videoRotationAngle == 270))
    //         {
    //             scaletmp.y *= -1;
    //         } else
    //         {
    //             scaletmp.x *= -1;
    //         }
    //         RawImage.GetComponent<RectTransform>().localScale = scaletmp;
    //     }

}
