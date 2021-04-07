using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class CollectionDataManager : MonoBehaviour
{

    [SerializeField]
    private GameObject NameText;
    [SerializeField]
    private GameObject NoText;
    [SerializeField]
    private GameObject PanelText;
    [SerializeField]
    private GameObject PersonImg;
    [SerializeField]
    private GameObject TimeAnswer;
    [SerializeField]
    private GameObject CateforyAnswer;
    [SerializeField]
    private GameObject TextAnswer;

    // Start is called before the first frame update
    void Start()
    {
        var CollectionData = CollectionsManager.SelectedListID;
        Debug.Log(CollectionData);
        NameText.GetComponent<Text>().text = PlayFabController.CharaDataList[CollectionData]["Name"];        
        NoText.GetComponent<Text>().text = PlayFabController.CharaDataList[CollectionData]["ID"];        
        PanelText.GetComponent<Text>().text = PlayFabController.CharaDataList[CollectionData]["PanelText"];        
        PersonImg.GetComponent<Image>().sprite = Resources.Load<Sprite>("Person/" + PlayFabController.CharaDataList[CollectionData]["ImgTitle"]);
        TimeAnswer.GetComponent<Text>().text = PlayFabController.CharaDataList[CollectionData]["Time"];        
        CateforyAnswer.GetComponent<Text>().text = PlayFabController.CharaDataList[CollectionData]["Category"];        
        TextAnswer.GetComponent<Text>().text = PlayFabController.CharaDataList[CollectionData]["KeyWord"];
        // Debug.Log(PlayFabController.CharaDataList[CollectionData]["AvatorName"]);
        GameObject Character = (GameObject)Resources.Load("PreFab/" + PlayFabController.CharaDataList[CollectionData]["AvatorName"]);
        GameObject instance = (GameObject)Instantiate(Character);
    }

    // ボタン押下時にタップ音
    public void OnToSceneButton()
    {
        SoundManager.instance.PlaySE();
    }
}
