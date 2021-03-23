using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARSelectManager : MonoBehaviour
{
    void Start()
    {
        //   UIManager.instance.SetMenuActive(true);  
    }
    public void OnToSceneButton()
    {
        SoundManager.instance.PlaySE();
    }
}
