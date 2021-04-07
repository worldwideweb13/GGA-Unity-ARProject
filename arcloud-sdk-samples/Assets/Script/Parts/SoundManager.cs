using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // シングルトンを使って音を管理する=シーン間でのデータ共有で使われる
    // ボタン押下時の画面遷移:Sceneが遷移する結果、ボタン効果音が保持されず、実行されない
    public static SoundManager instance;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public AudioSource audioSourceBGM; //BGMのスピーカー
    public AudioClip[] audioClipsBGM; //(0: Title)
    public AudioSource audioSourceSE; //SEのスピーカー
    public AudioClip audioClip; //ならす素材

    public AudioSource audioCameraSourceSE; //カメラシャッター音のスピーカー
    public AudioClip audioCameraClip; //シャッター音を鳴らす素材


    public void PlayBGM(string SceneName)
    {                
        audioSourceBGM.Stop();
        switch(SceneName)
        {
            default:
            case "Title":
                audioSourceBGM.clip = audioClipsBGM[0];
                break;
            case "TokugawaAR":
                audioSourceBGM.clip = audioClipsBGM[1];
                audioSourceBGM.volume = 0.3f;
                break;
            case "ShibusawaAR":
                audioSourceBGM.clip = audioClipsBGM[2];
                audioSourceBGM.volume = 0.15f;            
                break;
            case "KishimotoAR":
                audioSourceBGM.clip = audioClipsBGM[3]; 
                audioSourceBGM.volume = 0.3f;
                break;
            case "OdenAR":
                audioSourceBGM.clip = audioClipsBGM[4];
                audioSourceBGM.volume = 0.35f;
                break;
            case "AsakuraAR":
                audioSourceBGM.clip = audioClipsBGM[5];
                audioSourceBGM.volume = 0.15f;
                break;
            case "MakinoAR":
                audioSourceBGM.clip = audioClipsBGM[6];
                break;
            case "ARSelect":
                audioSourceBGM.clip = audioClipsBGM[7];
                 audioSourceBGM.volume = 1;               
                break;
            case "Collections":
                audioSourceBGM.clip = audioClipsBGM[7];
                 audioSourceBGM.volume = 1;
                 break; 
            case "CollectionData":
                audioSourceBGM.clip = audioClipsBGM[7];
                 audioSourceBGM.volume = 1;
                 break;                       
        }
        audioSourceBGM.Play();
    }

    public void PlaySE()
    {
        audioSourceSE.PlayOneShot(audioClip); //SEを一度だけならす
    }

    public void PlayCameraSE()
    {
        audioCameraSourceSE.PlayOneShot(audioCameraClip);
    }
}
