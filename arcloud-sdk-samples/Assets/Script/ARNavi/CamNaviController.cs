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
        Debug.Log("カメラの処理実行確認");
        StartCoroutine(Authrization()); 
        // StartCamera();
        // var webCamTexture = new WebCamTexture();
        // RawImage.texture = webCamTexture;
        // webCamTexture.Play();
    }

    IEnumerator Authrization()
    {
        Debug.Log("IEnumerator Authrization: カメラの使用許可を確認");
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        // ユーザーがカメラ起動の許可をしていた場合はカメラを起動
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Debug.Log("ユーザー許可：カメラを起動します");
            // UnityのWebCamTextureで処理
            StartCoroutine(_startCamera());
        }
        
    }

    // public void StartCamera()
    // {
    //     Debug.Log("StartCamera(): コルーチンで_startCamera()実行");
    //     // 処理はコルーチンで実行する
    //     StartCoroutine(_startCamera());
    // }

    IEnumerator _startCamera()
    {
        Debug.Log("_startCamera(): 実行");
        // 指定カメラを起動させる
        webCam = new WebCamTexture(WebCamTexture.devices[0].name, 640, 480);
        Debug.Log("(1)カメラのテクスチャ取得→(2)RawImageのオイラー角を取得");
        Vector3 angles = RawImage.GetComponent<RectTransform>().eulerAngles;
        Debug.Log("(1)(2)→(3)カメラテクスチャの右回りの角度を-x°の形式で取得してオイラー角のZ軸に代入");
        // カメラ起動
        webCam.Play();
        Debug.Log("webCam.Play()起動");
        angles.z = -webCam.videoRotationAngle;
        Debug.Log("(1)(2)(3)→(4)RawImageのRectTransformにオイラー角を代入");
        RawImage.GetComponent<RectTransform>().eulerAngles = angles;
        // widthはx、heightはyでサイズ調整
        // 全体を表示させるように、大きい方のサイズを表示枠に合わせる
        float scaler;
        Vector2 sizemp = RawImage.GetComponent<RectTransform>().sizeDelta;
        Debug.Log("(1)(2)(3)(4)→(5)"+ sizemp);
        // if()


        // RawImageのテクスチャにWebCamTextureのインスタンスを設定
        RawImage.texture = webCam;       
        Debug.Log("RawImageのテクスチャにwebCamを代入");
      

        while(webCam.width != webCam.requestedWidth)
        {
            Debug.Log("webCam.widthの幅がrequestした高さと合ってなければ次のフレームで再開");
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
