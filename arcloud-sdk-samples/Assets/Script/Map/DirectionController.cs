using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Runtime.Serialization.Json;
using System.Text;
using System.IO;
using System.Collections.Generic;

public class DirectionController : MonoBehaviour
{
    [SerializeField]
    private GameObject MapAPIManager;

    StaticMapController StaticMapController;

    private const string GOOGLE_DIRECTIONS_API_URL = "https://maps.googleapis.com/maps/api/directions/json?key=AIzaSyCN773NQ0NQsV4OjQjSd-TuFv-5Z9IEbWU&mode=walking";

    // 現在地
    private LocationInfo CurrentLocation;
    // 目的地
    [SerializeField]
    private string Goal;

    // APIより取得した経路（StaticMapControllerに渡すためのパラメータ）
    public static string GoalRoute = "";


    public void GoalSearch()
    {
            // Debug.Log("GoalSearch()実行");
            Input.location.Start();
            CurrentLocation = Input.location.lastData;
            // Debug.Log(CurrentLocation);
            StartCoroutine(GetDirection());
    }

    private IEnumerator GetDirection()
    {
        // Debug.Log("GetDirection()実行");
        // origin=開始地点。現在地からの経路を出したいので現在地を渡す。
        var query = "&origin=" +  CurrentLocation.latitude + "," + CurrentLocation.longitude;
        // destination=目的地。InputFieldに入力した文字列をエスケープして渡す。
        Goal = StaticMapController.GoalLocationInfo;
        query += "&destination=" + Goal;
        // Debug.Log("(2)GetDirectionのquery: " + GOOGLE_DIRECTIONS_API_URL + query);
        // GoogleDirectionAPI実行。現在地と目的地と渡してJSON形式のデータを受け取る
        UnityWebRequest req = UnityWebRequest.Get(GOOGLE_DIRECTIONS_API_URL + query);
        yield return req.SendWebRequest();

        if(req.error == null)
        {
            // Debug.Log("返ってきたjsonをByte[]形式を処理1");
            // 返ってきたjsonをByte[]形式を処理できるMemoryStreamで受け取る
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(req.downloadHandler.text));

            // google DirectionのJSONをオブジェクトとして読み込めるようにシリアライザを作成
            var serializer = new DataContractJsonSerializer(typeof(GoogleDirectionData));
            // Debug.Log("返ってきたjsonをByte[]形式を処理2");
            // 作成したJSON用のクラスにデジシアラジイズ
            GoogleDirectionData googleDirectionData = (GoogleDirectionData)serializer.ReadObject(ms);

            // データの形式としてroutes[0].legs[0]は固定なので一旦変数に格納
            // Debug.Log("返ってきたjsonをByte[]形式を処理3");
            var leg = googleDirectionData.routes[0].legs[0];
            // 書き込み前に初期化
            GoalRoute = "";
            // Debug.Log("返ってきたjsonをByte[]形式を処理4");
            for (int i=0; i < leg.steps.Count; i++)
            {
                // Debug.Log("返ってきたjsonをByte[]形式を処理5");
                // 経路は|緯度,経度|という書き方になるので、受け取ったlatitude, longitudeをパイプとカンマを付けて追加していく
                GoalRoute += "|" + leg.steps[i].end_location.lat + "," + leg.steps[i].end_location.lng;
                // Debug.Log("(3)DirectionAPI実行後の受け取り文字列： " + GoalRoute);
                // 経路が多すぎるとUriFormatExceptionで落ちるため上限を設定しておく。
                if(i > 30)break;
                // ***ここでエラーメッセージを画面に出す処理を記述***
            }
        }
        // Debug.Log("DirectionController処理終了。呼び出し");
        StaticMapController = MapAPIManager.GetComponent<StaticMapController>();
        StaticMapController.ReturnDirecAPI();
    }
}
