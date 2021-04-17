using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CamNaviController : MonoBehaviour
{
    public RawImage RawImage;
    public WebCamTexture webCam;

    void Start()
    {
        
        // StartCamera();
        // var webCamTexture = new WebCamTexture();
        // RawImage.texture = webCamTexture;
        // webCamTexture.Play();
    }

    IEnumerator Authrization()
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        // ユーザーがカメラ起動の許可をしていた場合はカメラを起動
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            // UnityのWebCamTextureで処理
            StartCamera();
        }
        
    }

    public void StartCamera()
    {
        // 処理はコルーチンで実行する
        StartCoroutine(_startCamera());
    }

    IEnumerator _startCamera()
    {
        // 指定カメラを起動させる
        webCam = new WebCamTexture(WebCamTexture.devices[0].name, 640, 480);

        // RawImageのテクスチャにWebCamTextureのインスタンスを設定
        RawImage.texture = webCam;
        // カメラ起動
        webCam.Play();
        while(webCam.width != webCam.requestedWidth)
        {
            // widthが指定したものになっていない場合は処理を抜けて次のフレームで再開
            yield return null;
        }
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
