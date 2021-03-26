using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        var CollectionData = ARSelectManager.SelectedText;
        // int num = PlayFabController.CharaDataList.IndexOf(CollectionData);
        // NameText.GetComponent<Text>().text = PlayFabController.CharaDataList[]["Name"];

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
