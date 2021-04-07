using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIFooter : MonoBehaviour
{
    // フッターの各ボタンを取得する
    // [SerializeField]
    // private GameObject ButtonA;
    // [SerializeField]
    // private GameObject ButtonB;
    // [SerializeField]
    // private GameObject BUttonC;
    // [SerializeField]
    // private GameObject ButtonD;

    // タップされたフッターのボタン要素を取得する
    [SerializeField]
    private EventSystem eventSystem;
    private GameObject SelectedButton;
    private GameObject SelectedText;


    // ボタンを押した時、ボタン画像の色を青にする
    public void PointerDown()
    {
        Debug.Log("ボタンが押されたよ");
        SelectedButton = eventSystem.currentSelectedGameObject.gameObject;
        SelectedButton.GetComponent<Image>().color = Color.blue;
        SelectedText = SelectedButton.transform.Find("Text").gameObject;
        SelectedText.GetComponent<Text>().color = Color.blue;
        // GoalLocationInfo = SelectedParentObj.GetComponent<NamePanelTable>().Location;
    }

    // // PC操作時:ボタンから指を離した時、ボタン画像の色を黒に戻す
    // public void PointerExit()
    // {
    //     SelectedButton.GetComponent<Image>().color = Color.black;
    // }

    // スマホ操作時:ボタンから指を離した時、ボタン画像の色を黒に戻す
    public void PointerUp()
    {
        Debug.Log("ボタンが押されたよ");
        SelectedButton.GetComponent<Image>().color = Color.black;
        SelectedText.GetComponent<Text>().color = Color.black;
    }
}
