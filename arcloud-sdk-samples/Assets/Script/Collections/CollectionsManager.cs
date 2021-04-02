using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using UnityEngine;
using UnityEngine.UI;   

public class CollectionsManager : MonoBehaviour
{
    [SerializeField]
    GameObject Content;

    [SerializeField]
    GameObject CollectionDataPanel;
    private GameObject Name;
    private GameObject TextNo;
    private GameObject ID;
    private GameObject EnName;
    private GameObject CharacterImg;


    // タップされたキャラクターのリストIDを保持しておくためのセット
    [SerializeField]
    private EventSystem eventSystem;
    private GameObject SelectedObj;
    public static int SelectedListID;


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
            // "CharaDataList"のリスト番号をインスタンスにスクリプト経由で保持させておく
            panel.GetComponent<CollectionDataPanelTable>().CharaDataListNo = i;
            CharacterImg.GetComponent<Image>().sprite = Resources.Load<Sprite>("Character/" + PlayFabController.CharaDataList[i]["AvatorName"]);
            panel.SetActive(true);
        }
    }
    public void OnToSceneButton()
    {
        SoundManager.instance.PlaySE();
    }

    // タップされたキャラクターのリスト番号を取得して図鑑詳細画面に渡す
    public void CharacterSelected()
    {
        SelectedObj = eventSystem.currentSelectedGameObject.gameObject;
        SelectedListID = SelectedObj.GetComponent<CollectionDataPanelTable>().CharaDataListNo;
        // Debug.Log(SelectedText);
        // SceneTransitionManager obj = new SceneTransitionManager();
        // obj.LoadTo("CollectionData");                
    }
}
