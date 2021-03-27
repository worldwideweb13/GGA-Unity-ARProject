using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogController : MonoBehaviour
{
    // メッセージキャンパス
    public GameObject DialogUICcanvas;
    // メッセージダイアログ
    public GameObject DialogPanel;
    // メッセージテキスト
    public Text DialogText;

    // キャラクター毎のメッセージテキスト
    [SerializeField]
    private string[] message = new string[]{};

    //*******************************************************************
    //                情報と基本メソッド
    //*******************************************************************
    
    // 書くスピード(短いほど早い)
    public float WriteSpeed = 0.05f;
    // メッセージ描画管理
    public bool IsWriting = false;
    // 文章の番号はkeyで表す
    public int key = 0;
    // テキストを書くメソッド


    // キャラクターがクリックされたらコメント欄を表示
    public void OnClickChar()
    {
        DialogPanel.SetActive(true);
        Clean();
        // ShibusawaData.DataProperty p = new ShibusawaData.DataProperty();
        if(key == 0){
            // key = 1;
            Write(message[key]);
        }
    }

    public void Write (string s)
    {
        //毎回、書くスピードを 0.2 に戻す------<戻したくない場合はここを消す>
        WriteSpeed = 0.05f;
        StartCoroutine(IEWrite(s));
    }

    // テキスト消すメソッド
    public void Clean()
    {
        DialogText.text = "";
    }

    //*******************************************************************
    //                表示するメッセージ
    //*******************************************************************

    public void OnClick ()
    {
        //前のメッセージを書いている途中かどうかで分ける
        if (IsWriting)
        {
            // 書いてる途中にタッチされた時
            // スピード（かかる時間）を0にして、メッセージを全て描画
            WriteSpeed = 0f;
        }
        else
        {
            // 描画した後でタッチされた時
            switch (key)
            {        
                case 4:
                    Clean();
                    key = 0;
                    DialogPanel.SetActive(false);
                    break;
                default:
                    Clean();
                    key++;
                    Write(message[key]);
                    break;              
            }
        }

    }

    // string[] message = new string[]
    // {
    //     "マロの名は徳川慶喜。\n徳川幕府15代将軍にて徳川家最後の将軍なのじゃ！",
    //     "先代の家康公は日本を治めるために幕府を開いたが、マロは幕府の時代を終わらせるために将軍職に就いたのじゃ",
    //     "1867年京都の二条城にてマロは大政奉還を施工した。",
    //     "これにより256年間続いた江戸幕府は終わりを迎えたのじゃ",
    //     "「この世をば しばしの夢と 聞きたれど おもへば長き 月日なりけり」マロの辞世の句じゃ！"
    // };

    //*******************************************************************
    //                メッセージパネルがタッチされた時の処理
    //*******************************************************************




    //*******************************************************************
    //                その他
    //*******************************************************************

    IEnumerator IEWrite(string s)
    {
        // メッセージ描画管理
        IsWriting = true;
        // 渡されたstringの文字の数だけループ
        for(int i=0; i < s.Length; i++)
        {
            // テキストにi番目の文字を付け足して表示する
            // String.Substring(i,x) Stringのi番目の文字をx個表示
            DialogText.text += s.Substring(i, 1);
            // 次の文字を表示するまで少し待つ
            yield return new WaitForSeconds(WriteSpeed);
        }
        // メッセージ描画管理
        IsWriting = false;
    }

    // ゲームスタート時の処理
    void Start()
    {
        // メッセージパネルに書かれている文字を消す
        Clean();
    }

}
