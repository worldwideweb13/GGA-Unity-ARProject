using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransitionManager : MonoBehaviour
{
    public static string PreSceneName;

    public void LoadTo(string NextSceneName)
    {
        SoundManager.instance.PlayBGM(NextSceneName);
        SceneManager.LoadScene(NextSceneName);
    }
    public void GetCurrentSceneName(string CurrSceneName)
    {
        PreSceneName = CurrSceneName;
    }
    
    // 戻るボタン
    public void BackTo()
    {
        SoundManager.instance.PlayBGM(PreSceneName);
        SceneManager.LoadScene(PreSceneName);
    }

}