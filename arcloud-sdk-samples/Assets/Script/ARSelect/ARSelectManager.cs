using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;      

public class ARSelectManager : MonoBehaviour
{
    [SerializeField]
    GameObject Content;

    [SerializeField]
     GameObject CollectionDataPanel;
    private GameObject Name;
    private GameObject TextNo;
    private GameObject EnName;
    private GameObject CharacterImg;

    [SerializeField]
    private EventSystem eventSystem;
    private GameObject SelectedObj;
    private GameObject SelectedName;

    // タップされたキャラクター名を保持しておくテキスト
    public static Text SelectedText;    


   // 図鑑一覧画面でタップされたキャラクター名を図鑑詳細画面に渡すためのスターテス管理変数
    public static string CharaSelectedData;   

    void Start()
    {
        for (int i=0; i< PlayFabController.CharaDataList.Count; i++)
        {
            // 画面読み込み時に選択一覧を表示する
            GameObject panel = Instantiate(CollectionDataPanel);
            panel.transform.SetParent(Content.transform);
            Name = panel.transform.Find("Name").gameObject;
            EnName = panel.transform.Find("EnName").gameObject;
            TextNo = panel.transform.Find("No/TextNo").gameObject;        
            CharacterImg = panel.transform.Find("CharacterImg").gameObject;
            Name.GetComponent<Text>().text = PlayFabController.CharaDataList[i]["Name"];
            EnName.GetComponent<Text>().text = PlayFabController.CharaDataList[i]["EnName"];
            TextNo.GetComponent<Text>().text = PlayFabController.CharaDataList[i]["ID"];
            CharacterImg.GetComponent<Image>().sprite = Resources.Load<Sprite>("Character/" + PlayFabController.CharaDataList[i]["AvatorName"]);
            panel.SetActive(true);

            // Debug.Log(PlayFabController.CharaDataList[i]["Name"]);
        }
    }
    public void OnToSceneButton()
    {
        SoundManager.instance.PlaySE();
    }

    // タップされたキャラクターの名前を取得して図鑑詳細画面に渡す
    public void CharacterSelected()
    {
        SelectedObj = eventSystem.currentSelectedGameObject.gameObject;
        SelectedName = SelectedObj.transform.Find("Name").gameObject;
        SelectedText = SelectedName.GetComponent<Text>();
        // Debug.Log(SelectedText);
        // SceneTransitionManager obj = new SceneTransitionManager();
        // obj.LoadTo("CollectionData");                
    }
}
