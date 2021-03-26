using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARSelectController : MonoBehaviour
{
    public GameObject prefab;
    // Start is called before the first frame update
    void Start()
    {
    //    GameObject obj = (GameObject) Resources.Load("PreFab/CollectionDataPanel");
       Instantiate (prefab);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
