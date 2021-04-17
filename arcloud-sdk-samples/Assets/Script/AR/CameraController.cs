using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.InteropServices;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup FooterCanvas;

    [DllImport("__Internal")]
    private static extern void SaveToAlbum(string path);

    IEnumerator SaveToCameraroll(string path)
    {
        // ファイルが生成されるまで待つ
        while(true)
        {
            if(File.Exists(path))
                break;
            yield return null;
        }
        SaveToAlbum(path);
        // フッターを表示に切り替え（透明度を1にする）
        FooterCanvas.alpha = 1;        
    }

    public void ScreenShot()
    {
        // フッターを非表示（透明にする）
        FooterCanvas.alpha = 0;
        string filename = "ScreenShot.png";
        string path = Application.persistentDataPath + "/" + filename;
        // 以前のスクリーンショットを削除する
        File.Delete(path);

        // スクリーンショットを撮影する
        ScreenCapture.CaptureScreenshot(filename);
        // カメラロールに保存する
        StartCoroutine(SaveToCameraroll(path));
    }
}
