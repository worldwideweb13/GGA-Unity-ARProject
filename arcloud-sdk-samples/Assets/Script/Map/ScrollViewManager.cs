using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewManager : MonoBehaviour
{
    [SerializeField]
    GameObject Content;

    [SerializeField]
    GameObject NamePanel;

    private GameObject NameText;
    private GameObject EnNameText;

    // タップされたキャラクターのリストIDを保持しておくためのセット
    [SerializeField]
    private EventSystem eventSystem;
    private GameObject SelectedObj;

    // public static int SelectedListID;


    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i< PlayFabController.CharaDataList.Count; i++ )
        {
            // 画面読み込み時に選択一覧を表示する
            GameObject Panel = Instantiate(NamePanel);            
            Panel.transform.SetParent(Content.transform);
            NameText = Panel.transform.Find("NameText").gameObject;
            EnNameText = Panel.transform.Find("EnNameText").gameObject;
            NameText.GetComponent<Text>().text = PlayFabController.CharaDataList[i]["Name"];
            EnNameText.GetComponent<Text>().text = PlayFabController.CharaDataList[i]["EnName"];
            // "CharaDataList"のリスト番号をインスタンスにスクリプト経由で保持させておく
            Panel.GetComponent<NamePanelTable>().CharaDataListNo = i;
            // 経度緯度情報をインスタンスの値を保持するNamePanelTableに保持させておく
            NamePanelTable.Location = PlayFabController.CharaDataList[i]["Location"];
            Panel.SetActive(true);
        }  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
