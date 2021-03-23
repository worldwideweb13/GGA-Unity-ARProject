using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // ヘッダー
    [SerializeField]
    private GameObject m_header;

    // フッター
    [SerializeField]
    private GameObject m_footer;

    // void Awake()
    // {
    //     SetMenuActive(false);
    // }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // メニューの表示 ON/OFF
    public void SetMenuActive(bool active)
    {
        m_header.gameObject.SetActive(active);
        m_footer.gameObject.SetActive(active);
    }

}
