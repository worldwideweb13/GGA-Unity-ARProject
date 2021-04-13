using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEffectManager : MonoBehaviour
{
    // タップされたフッターのボタン要素を取得する
    [SerializeField]
    private EventSystem eventSystem;
    private GameObject SelectedButton;
    private GameObject SelectedText;

    // タイトルボタン押下時の各ボタン処理
    private GameObject SelectedTitleImage;
    private GameObject SelectedTitleText;

    // ボタンを押した時、ボタン画像の色を青にする
    public void PointerDown()
    {
        SelectedButton = eventSystem.currentSelectedGameObject.gameObject;
        SelectedButton.GetComponent<Image>().color = Color.blue;
        // GoalLocationInfo = SelectedParentObj.GetComponent<NamePanelTable>().Location;
    }
    // スマホ操作時:ボタンから指を離した時、ボタン画像の色を黒に戻す
    public void PointerUp()
    {
        SelectedButton.GetComponent<Image>().color = Color.black;
    }

    // タイトルパーツ押下時のボタン処理
    public void TitlePointerDown()
    {
        SelectedButton = eventSystem.currentSelectedGameObject.gameObject;
        SelectedTitleImage = SelectedButton.transform.Find("Image").gameObject;
        SelectedTitleImage.GetComponent<Image>().color = Color.blue;
        SelectedTitleText = SelectedButton.transform.Find("Text").gameObject;
        SelectedTitleText.GetComponent<Text>().color = Color.blue;
    }

    public void TitlePointerUp()
    {
        SelectedTitleImage.GetComponent<Image>().color = Color.black;
        SelectedTitleText.GetComponent<Text>().color = Color.black;
    }
}
