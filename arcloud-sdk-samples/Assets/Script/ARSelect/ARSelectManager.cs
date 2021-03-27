using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using UnityEngine;
using UnityEngine.UI;   

public class ARSelectManager : MonoBehaviour
{
    public void OnToSceneButton()
    {
        SoundManager.instance.PlaySE();
    }
}
