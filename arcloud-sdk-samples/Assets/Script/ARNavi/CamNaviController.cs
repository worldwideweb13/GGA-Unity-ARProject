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
        Debug.Log("(0)カメラの処理実行確認");

        StartCoroutine(Authrization());
        // StartCamera();
        // var webCamTexture = new WebCamTexture();
        // RawImage.texture = webCamTexture;
        // webCamTexture.Play();
    }

    IEnumerator Authrization()
    {
        Debug.Log("(0)IEnumerator Authrization: カメラの使用許可を確認");
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        // ユーザーがカメラ起動の許可をしていた場合はカメラを起動
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Debug.Log("(0)ユーザー許可：カメラを起動します");
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
        var width = RawImage.GetComponent<RectTransform>().sizeDelta.x;
        var height = RawImage.GetComponent<RectTransform>().sizeDelta.y;
        int Pwidth = (int)width;
        int Pheight = (int)height;
        // int Pwidth = 828;
        // int Pheight = 1792;
        Debug.Log("width: " + Pwidth + ",height: " + Pheight);
        Debug.Log("_startCamera(): 実行");
        // 指定カメラを起動させる
        webCam = new WebCamTexture(WebCamTexture.devices[0].name, Pwidth, Pheight);
        Debug.Log("(1)カメラのテクスチャ取得→(2)RawImageのオイラー角を取得");
        Vector3 angles = RawImage.GetComponent<RectTransform>().eulerAngles;

        // 接続しているデバイスのリストをシステムに問い合わせ
        var devices = WebCamTexture.devices;
        if(devices.Length > 0)
        {
            // Iphoneの場合はカメラの向きを縦向きにする
            if(Application.platform == RuntimePlatform.IPhonePlayer)
            {
                // RawImageをZ軸に90°回転
                RawImage.transform.localRotation = Quaternion.Euler(angles.x, angles.y, angles.z -90 );
            }
        }

        // カメラ起動
        webCam.Play();
        Debug.Log("(1)(2)→(3)webCam.Play()起動");       
        // angles.z = -webCam.videoRotationAngle;
        // Debug.Log("(1)(2)(3)→(4)カメラテクスチャの右回りの角度を-x°の形式で取得してオイラー角のZ軸に代入");

        Debug.Log("(1)(2)(3)(4)→(5)RawImageのRectTransformにオイラー角を代入");
        RawImage.GetComponent<RectTransform>().eulerAngles = angles;
        // widthはx、heightはyでサイズ調整
        // 全体を表示させるように、大きい方のサイズを表示枠に合わせる
        float scaler;
        Vector2 sizetmp = RawImage.GetComponent<RectTransform>().sizeDelta;
        Debug.Log("(1)(2)(3)(4)(5)→(6)"+ sizetmp);
        // if()        
        // RawImageのテクスチャにWebCamTextureのインスタンスを設定
        if(webCam.width > webCam.height)
        {
            scaler = sizetmp.x / webCam.width;
            Debug.Log("(1)(2)(3)(4)(5)(6)→(7)幅 > 高さ");
        }
        else
        {
            scaler = sizetmp.y / webCam.height;
            Debug.Log("(1)(2)(3)(4)(5)(6)→(7)幅 < 高さ");
        }
        Debug.Log("(1)(2)(3)(4)(5)(6)(7)→(8)幅高さ調整");
        // サイズ調整
        sizetmp.x = webCam.width * scaler;        
        sizetmp.y = webCam.height * scaler;        
        RawImage.GetComponent<RectTransform>().sizeDelta = sizetmp;
        Debug.Log("(1)(2)(3)(4)(5)(6)(7)(8)→(9)RawImageのテクスチャにwebCamを代入");

        RawImage.texture = webCam;       
        Debug.Log("処理完了");
        // while(webCam.width != webCam.requestedWidth)
        // {
        //     Debug.Log("webCam.widthの幅がrequestした高さと合ってなければ次のフレームで再開");
        //     // widthが指定したものになっていない場合は処理を抜けて次のフレームで再開
        //     yield return null;
        // }
        yield break;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
