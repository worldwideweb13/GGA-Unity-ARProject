using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.Json;
using System.Linq;

public class PlayFabLoginCustom : MonoBehaviour
{
    void Start()
    {
       PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest
       {
           TitleId = PlayFabSettings.TitleId,
           CustomId = "100",
           CreateAccount = true
       }, result =>
       {
           Debug.Log("ログイン成功！");
            GetTitleData();
       }, error =>
       {
           Debug.Log(error.GenerateErrorReport());
       });
    }


// プレイヤーデータの更新
    // void SetUserData()
    // {
    //     PlayFabClientAPI.UpdateUserData(
    //         new UpdateUserDataRequest
    //         {
    //             Data = new Dictionary<string, string>()
    //             {
    //                 {"Name", "Ogaxhan"},
    //                 {"Exp", "300"},
    //             }
    //         },
    //     result => { Debug.Log("プレイヤーデータ登録成功"); },
    //     error => { Debug.Log(error.GenerateErrorReport());}
    //     );    
    // }

    // JSON形式でのプレイヤーデータの更新／取得処理
    // void SetUserDataJson()
    // {
    //     var questInfos = new List<QuestInfo>
    //     {
    //         new QuestInfo { Id = 1, ClearTime = 20, Score = 100 },
    //         new QuestInfo { Id = 2, ClearTime = 30, Score = 200 },
    //     };

    //     PlayFabClientAPI.UpdateUserData(
    //         new UpdateUserDataRequest
    //         {
    //             Data = new Dictionary<string, string>()
    //             {
    //                 { "QuestInfo", PlayFab.Json.PlayFabSimpleJson.SerializeObject(questInfos) }
    //             }
    //         },
    //     result => { Debug.Log("プレイヤーデータの登録成功！！"); },
    //     error => { Debug.Log(error.GenerateErrorReport()); });
    // }

    // public class QuestInfo
    // {
    //     public int Id { get; set; }
    //     public int ClearTime { get; set; }
    //     public int Score { get; set; }
    // }

    // void GetUserData()
    // {
    //     PlayFabClientAPI.GetUserData(new GetUserDataRequest(),
    //     result =>
    //     {
    //         var questInfos = PlayFabSimpleJson.DeserializeObject<List<QuestInfo>>(result.Data["QuestInfo"].Value);
    //         foreach (var quest in questInfos)
    //         {
    //             Debug.Log($"Id: {quest.Id}");
    //             Debug.Log($"ClearTime: {quest.ClearTime}");
    //             Debug.Log($"Score: {quest.Score}");
    //         }
    //     },
    //     error =>
    //     {
    //         Debug.Log(error.GenerateErrorReport());
    //     });
    // }
    
    // JSON形式でのタイトルデータの更新／取得処理
    public void GetTitleData()
    {
        PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
            result =>
            {
                if(result.Data.ContainsKey("CollectionData"))
                {
                    // デシリアライズしてリストに変換
                    var CharaMaster = PlayFabSimpleJson.DeserializeObject<List<CollectionData>>(result.Data["CollectionData"]);
                    foreach(var chara in CharaMaster)
                    {
                        Debug.Log($"Id: {chara.ID}");
                        Debug.Log($"Id: {chara.Name}");
                        Debug.Log($"Id: {chara.EnName}");
                        Debug.Log($"Id: {chara.Time}");
                        Debug.Log($"Id: {chara.Category}");
                        Debug.Log($"Id: {chara.KeyWord}");
                        Debug.Log($"Id: {chara.ImgTitle}");
                        Debug.Log($"Id: {chara.AvatorName}");
                        Debug.Log($"Id: {chara.PanelText}");
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
        public int ID { get; set; }
        public string Name { get; set; }
        public string EnName { get; set; }
        public string Time { get; set; }
        public string Category { get; set; }
        public string KeyWord { get; set; }
        public string ImgTitle { get; set; }
        public string AvatorName { get; set; }
        public string PanelText { get; set; }
    }


}

    