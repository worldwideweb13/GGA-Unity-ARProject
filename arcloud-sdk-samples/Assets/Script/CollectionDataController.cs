using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.Json;


public class CollectionDataController : MonoBehaviour
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
    void GetTitleData()
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
