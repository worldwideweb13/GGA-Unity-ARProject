using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransitionManager : MonoBehaviour
{

    public void LoadTo(string SceneName)
    {
        SoundManager.instance.PlayBGM(SceneName);
        SceneManager.LoadScene(SceneName);
        Debug.Log(SceneName);

    }

}