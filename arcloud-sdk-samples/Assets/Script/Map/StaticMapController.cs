using System.Collections;
// using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class StaticMapController : MonoBehaviour
{
    private string GoogleApiKey = "AIzaSyCN773NQ0NQsV4OjQjSd-TuFv-5Z9IEbWU";
    private string BaseUrl = @"https://maps.googleapis.com/maps/api/staticmap?";
    // 緯度（経度）1度の距離(m)
    private const float Lat2Meter = 111319.491f;

    // 現在地表示のピン
    private string LocationIcon = "icon:https://user-images.githubusercontent.com/75665390/113117053-10f9b400-9249-11eb-90a7-8e0ac234a728.png";

    // ジーズアカデミー
    private string GoalLocationInfo = "";

    // 画像のzoomサイズ
    public int zoom = 17;
    // ズーム率調整用のため、前回のズーム値を保持しておく
    private int PreZoom = 17;

    // マップ更新の閾値
    private const float ThresholdDistance = 10f;

    // マップ更新時間
    private const float UpdateMapTime = 5f;

    // ダウンロードするマップのイメージサイズ
    private const int MapImageSize = 828;

    // 画面に表示するマップスプライトのサイズ
    private const int MapSpriteSize = 828;

    [SerializeField]
    private GameObject DirectionMapAPIManager;

    // ルート検索ボタン押下時にDirectionControllerからURLを受け取りマップを更新する
    public DirectionController directionUrl = null;
    DirectionController Script;

    // ルート探索監視用フラグ
    public static bool GoalFlag = false;

    [SerializeField] GameObject loading; //ダウンロード確認用オブジェクト
    // [SerializeField] Text txtLocation; //座標
    // [SerializeField] Text txtDistance; //距離
    [SerializeField] Image mapImage; //マップ Image 


    // 起動処理
    void Start()
    {
        // ローディング表示を非表示にしておく
        loading.SetActive(false);
        
        // GPS初期化 Input.LocationにてLocationServiceを呼び出すことが出来る
        // LocationService = 携帯端末が保持する位置情報を取得するためのクラス
        Input.location.Start();
        Input.compass.enabled = true; //コンパスの有効化

        // マップ取得
        StartCoroutine(updateMap());
        
    }

    // ズームボタン押下時に呼び出されるメソッド
    public void ZoomIn(){
        // ＋ボタン押下時のzoom値を取得しておく
        PreZoom = zoom;
        zoom--;
        StartCoroutine(updateMap());
    }

    public void ZoomOut(){
        // -ボタン押下時のzoom値を取得しておく
        PreZoom = zoom;
        zoom++;
        StartCoroutine(updateMap());
    }

    public void ReturnDirecAPI()
    {
        Debug.Log("コルーチン実行");
        StartCoroutine(updateMap());
    }

    // ルート探索ボタン押下時に呼び出されるメソッド（非同期処理バージョン）
    // public void RouteSearch()
    // {
    //     var task = GetDirecAPI();
    //     task.Wait();
    //     Debug.Log("task.Wait()の後");       
    //     StartCoroutine(updateMap());
    // }

    // private Task GetDirecAPI()
    // {
    //     GoalFlag = true;
    //     Script = DirectionMapAPIManager.GetComponent<DirectionController>();
    //     Script.GoalSearch();
    //     return GetDirecAPI();
    // }

    // ルート探索ボタン押下時に呼び出されるメソッド
    public void RouteSearch()
    {
        GoalLocationInfo = NamePanelTable.Location;
        GoalFlag = true;
        Script = DirectionMapAPIManager.GetComponent<DirectionController>();
        Script.GoalSearch();
    }


    // マップ更新
    private IEnumerator updateMap()
    {
        // LocationService.isEnabledByUser : 端末の設定でロケーション機能を有効にしているかどうかを返す
        if (!Input.location.isEnabledByUser)yield break;

        // サービスの状態が起動中になるまで待機
        while (Input.location.status != LocationServiceStatus.Running)
        {
            yield return new WaitForSeconds(2f);
        }

        // LocationInfo クラス = デバイスのロケーション(GPS)を表示するためのクラス。
        // LocationInfo curr = 最新の GPSを保持しておくためのクラス
        // LocationInfo prev = 前回の GPSを保持しておくためのクラス
        LocationInfo curr;
        LocationInfo prev = new LocationInfo();

        // zoom率変更ボタンが押下された時にマップを再描画
        if(PreZoom != zoom)
        {
            // LocationServiceStatus.Runningの時、Input.location.lastDataに取得した位置情報が適宜格納される
            curr = Input.location.lastData;            
            // マップを再描画
            yield return StartCoroutine(downloading(curr));
            // prevを更新して前回の経度、緯度を保持しておく
            prev = curr;
        }
        
        // ルート検索ボタンが押下された時にマップを再描画
        if(GoalFlag == true)
        {
            Debug.Log("GoalFlagがtrueなのでルート探索描画開始：★★★★★★★★★");
            // LocationServiceStatus.Runningの時、Input.location.lastDataに取得した位置情報が適宜格納される
            curr = Input.location.lastData;            
            // マップを再描画
            yield return StartCoroutine(downloading(curr));
            // prevを更新して前回の経度、緯度を保持しておく
            prev = curr;
            GoalFlag = false;
        }

        while(true)
        {
            // LocationServiceStatus.Runningの時、Input.location.lastDataに取得した位置情報が適宜格納される
            curr = Input.location.lastData;
            // txtLocation.text = dtring.

            // ThresholdDistance=閾値 と(現在地 - 前回地点) の差分を見て、再描画の有無を判断
            if(getDistanceFromLocation(curr, prev) >= ThresholdDistance)
            {
                // マップを再描画
                yield return StartCoroutine(downloading(curr));
                // prevを更新して前回の経度、緯度を保持しておく
                prev = curr;
            }
            // 待機
            yield return new WaitForSeconds(UpdateMapTime);
        }

        // マップ画像ダウンロード
        IEnumerator downloading(LocationInfo current)
        {
            loading.SetActive(true);
            // ベースURL
            string url = BaseUrl;
            // Debug.Log(url);
            // 中心座標
            url += "center=" + curr.latitude + "," + curr.longitude;
            // Debug.Log(url);
            // ズーム
            url += "&zoom=" + zoom;
            // Debug.Log(url);            
            // 画像サイズ（640x640が上限）
            url += "&size=" + MapImageSize + "x" + MapImageSize;
            // Debug.Log(url);            
            // 現在地アイコン
            url += "&markers=" + LocationIcon + "|shadow:false|" + curr.latitude + "," + curr.longitude;
            // Debug.Log(url);            
            // 目的地アイコン
            url += "&markers=ize:mid|color:red|" + GoalLocationInfo;
            Debug.Log(url); 

            // 経路に値が入っている場合は取得した状態でマップを描画
            if(DirectionController.GoalRoute != "")
            {
                Debug.Log("コルーチン実行★★★★★★★★★の後に実行されるべき処処理");
                url += "&path=color:red|" + curr.latitude + "," + curr.longitude + DirectionController.GoalRoute;
                Debug.Log("ルート探索のURL: "+url);
            }

            // API Key
            url += "&key=" + GoogleApiKey;
            Debug.Log("最終的なStaticAPIのURL: " + url);

            // 地図画像をダウンロード
            // UnityWebRequest.UnEscapeURL(url) = エスケープシーケンス（特殊文字）を変換し、ユーザーが使いやすいテキストを返す。
            url = UnityWebRequest.UnEscapeURL(url);
            // HTTP GET 経由でイメージをダウンロードする時に、イメージを Texture 形式に変換
            UnityWebRequest req = UnityWebRequestTexture.GetTexture(url);
            yield return req.SendWebRequest();

            // テクスチャ生成してスプライトを更新するクラスを呼び出し
            if(req.error == null) yield return StartCoroutine(updateSprite(req.downloadHandler.data));
            // updateDistance(0f);
            loading.SetActive(false);
        }

        // スプライトの更新
        IEnumerator updateSprite(byte[] data)
        {
            // Texture2D = 画像の情報を保持するクラス。
            Texture2D tex = new Texture2D(MapSpriteSize,MapSpriteSize);
            tex.LoadImage(data);
            if(tex == null) yield break;
            // スプライト(インスタンス)を明示的に開放
            if(mapImage.sprite != null)
            {
                Destroy(mapImage.sprite);
                yield return null;
                mapImage.sprite = null;
                yield return null;
            }
            // スプライト(インスタンス)を動的に生成
            mapImage.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero);
        }

        // 2点間の距離を取得する
        float getDistanceFromLocation(LocationInfo current, LocationInfo previous)
        {
            Vector3 cv = new Vector3((float)curr.longitude, 0, (float)curr.latitude);
            Vector3 pv = new Vector3((float)prev.longitude, 0, (float)prev.latitude);
            float dist = Vector3.Distance(cv,pv) * Lat2Meter;
            // updateDistance(dist);
            return dist;
        }
    }
}


