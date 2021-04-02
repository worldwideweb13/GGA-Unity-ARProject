using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System.Collections.Generic;
using PlayFab.Json;

/// <summary>
/// PlayFabのログイン処理を行うクラス
/// </summary>
public class PlayFabController : MonoBehaviour {

  //アカウントを作成するか
  private bool _shouldCreateAccount;
  
  //ログイン時に使うID
  private string _customID;

  // ログイン成功後、ゲームキャラクターデータを取得するリスト
  public static List<Dictionary<string, string>> CharaDataList = new List<Dictionary<string,string>>();

  // スタート画面遷移のためにMainCanavasを読み込んでおく
  
  [SerializeField]
  GameObject MainCanvas;

  //=================================================================================
  //スタートボタンを押下した時の処理
  //=================================================================================

  [SerializeField]
  GameObject PressStartIcon;

  [SerializeField]
  GameObject TopMenu;

  
  public void SetMenu() {

      TopMenu.SetActive(true);
      // image.enabled = false;
  }

  //=================================================================================
  //ログイン処理
  //=================================================================================
  
  // public void Start() {
  //   Login();
  // }

  //ログイン実行
  public void Login() {
    _customID = LoadCustomID();
    var request = new LoginWithCustomIDRequest { CustomId = _customID,  CreateAccount = _shouldCreateAccount};
    PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
  }

  //ログイン成功
  private void OnLoginSuccess(LoginResult result){
    //アカウントを作成しようとしたのに、IDが既に使われていて、出来なかった場合
    if (_shouldCreateAccount && !result.NewlyCreated) {
      Debug.LogWarning($"CustomId : {_customID} は既に使われています。");
      Login();//ログインしなおし
      return;
    }
    // ログイン成功時点で、キャラクターデータをplayFabから取得
    GetTitleData();

    //アカウント作成時にIDを保存
    if (result.NewlyCreated) {
      SaveCustomID();
      // 初回ログインの場合は、オープニング画面に飛ばす
      var OpeningScene = MainCanvas.GetComponent<SceneTransitionManager>();
      OpeningScene.LoadTo("ARSelect");      
    }
    Debug.Log($"PlayFabのログインに成功\nPlayFabId : {result.PlayFabId}, CustomId : {_customID}\nアカウントを作成したか : {result.NewlyCreated}");
    // 2回目以降のログインの場合は、トップメニューを表示
    PressStartIcon.SetActive(false);
    SetMenu();
  }

  //ログイン失敗
  private void OnLoginFailure(PlayFabError error){
    Debug.LogError($"PlayFabのログインに失敗\n{error.GenerateErrorReport()}");
  }
  
  //=================================================================================
  //カスタムIDの取得
  //=================================================================================

  //IDを保存する時のKEY
  private static readonly string CUSTOM_ID_SAVE_KEY = "CUSTOM_ID_SAVE_KEY";
  
  //IDを取得
  private string LoadCustomID() {
    //IDを取得
    // string id = PlayerPrefs.GetString(string key)
    //  →端末に保存されているkeyに対応するvalueを取得する
    // 今回の場合、CUSTOM_ID_SAVE_KEY : "hogehoge" でデータ保存
    string id = PlayerPrefs.GetString(CUSTOM_ID_SAVE_KEY);

    //保存されていなければ新規生成
    _shouldCreateAccount = string.IsNullOrEmpty(id);
    return _shouldCreateAccount ? GenerateCustomID() : id;
  }

  //IDの保存
  private void SaveCustomID() {
    PlayerPrefs.SetString(CUSTOM_ID_SAVE_KEY, _customID);
  }
  
  //=================================================================================
  //カスタムIDの生成
  //=================================================================================
 
  //IDに使用する文字
  private static readonly string ID_CHARACTERS = "0123456789abcdefghijklmnopqrstuvwxyz";

  //IDを生成する
  private string GenerateCustomID() {
    int idLength = 32;//IDの長さ
    StringBuilder stringBuilder = new StringBuilder(idLength);
    var random = new System.Random();

    //ランダムにIDを生成
    for (int i = 0; i < idLength; i++){
      stringBuilder.Append(ID_CHARACTERS[random.Next(ID_CHARACTERS.Length)]);
    }

    return stringBuilder.ToString();
  }
  
  //=================================================================================
  //JSON形式でのタイトルデータの取得処理
  //=================================================================================

    public void GetTitleData()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result =>
            {
                if(result.Data.ContainsKey("CollectionData"))
                {
                    // デシリアライズしてリストに変換
                    var CharaMasterData = PlayFabSimpleJson.DeserializeObject<List<CollectionData>>(result.Data["CollectionData"]);
                    foreach(var chara in CharaMasterData)
                    {
                      var CharaData = new Dictionary<string, string>()
                      {
                        { "ID", chara.ID},
                        { "Name", chara.Name},
                        { "EnName", chara.EnName},
                        { "Time", chara.Time},
                        { "Category", chara.Category},
                        { "KeyWord", chara.KeyWord},
                        { "ImgTitle", chara.ImgTitle},
                        { "AvatorName", chara.AvatorName},
                        { "PanelText", chara.PanelText},
                        { "Location", chara.Location}
                      };
                      CharaDataList.Add(CharaData);
                    }
                }
            },
            error =>
            {
                Debug.Log(error.GenerateErrorReport());
            }
        );
    }
    public class CollectionData
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string EnName { get; set; }
        public string Time { get; set; }
        public string Category { get; set; }
        public string KeyWord { get; set; }
        public string ImgTitle { get; set; }
        public string AvatorName { get; set; }
        public string PanelText { get; set; }
        public string Location { get; set; }
    }

}






