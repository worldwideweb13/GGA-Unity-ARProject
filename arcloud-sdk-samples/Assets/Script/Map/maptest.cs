// using System.Collections;
// using UnityEngine;
// using UnityEngine.Networking;
 
// public class StaticMapController : MonoBehaviour
// {
 
//     //Google Maps Static API URL
//     // ${APIKey}を作成したapiキーに書き換えてください。
//     private const string STATIC_MAP_URL = "https://maps.googleapis.com/maps/api/staticmap?key=${APIKey}&zoom=15&size=640x640&scale=2&maptype=terrain&style=element:labels|visibility:off";
 
//     private int frame = 0;
 
//     // Start is called before the first frame update
//     void Start()
//     {
//         // 非同期処理
//         StartCoroutine(getStaticMap());
//     }
 
//     // Update is called once per frame
//     void Update()
//     {
//         // 5秒に一度の実行
//         if (frame >= 300)
//         {
//             StartCoroutine(getStaticMap());
//             frame = 0;
//         }
//         frame++;
//     }
 
//     IEnumerator getStaticMap()
//     {
//         var query = "";
 
//         // centerで取得するミニマップの中央座標を設定
//         query += "&center=" + UnityWebRequest.UnEscapeURL(string.Format("{0},{1}", Input.location.lastData.latitude, Input.location.lastData.longitude));
//         // markersで渡した座標(=現在位置)にマーカーを立てる
//         query += "&markers=" + UnityWebRequest.UnEscapeURL(string.Format("{0},{1}", Input.location.lastData.latitude, Input.location.lastData.longitude));
 
//         // リクエストの定義
//         var req = UnityWebRequestTexture.GetTexture(STATIC_MAP_URL + query);
//         // リクエスト実行
//         yield return req.SendWebRequest();
 
//         if (req.error == null)
//         {
//             // すでに表示しているマップを更新
//             Destroy(GetComponent<Renderer>().material.mainTexture);
//             GetComponent<Renderer>().material.mainTexture = ((DownloadHandlerTexture)req.downloadHandler).texture;
//         }
//     }
// }