using System.Text;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

// public class CollectionDataController : MonoBehaviour
// {
//     void Start()
//     {
//         GetCollectionData();
//     }

//     void GetCollectionData()
//     {
//         PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(),
//             result =>
//             {
//                 var CollectionData = PlayFab.Json.PlayFabSimpleJson.DeserializeObject<List<CollectionData>>(result.Data("CollectionData"));
//                 foreach(var data in CollectionData)
//                 {
//                     Debug.Log($"ID; {data.ID}");
//                     Debug.Log($"ID; {data.Name}");
//                     Debug.Log($"ID; {data.AvatorName}");
//                 }
//             },
//             error =>
//             {
//                 Debug.Log(error.GenerateErrorReport());
//             }
//         );
//     }

// }
