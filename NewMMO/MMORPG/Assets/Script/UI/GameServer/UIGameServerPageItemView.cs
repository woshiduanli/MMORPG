
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class UIGameServerPageItemView : MonoBehaviour
{
    /// <summary>
    /// 页码
    /// </summary>
    private int m_PageIndex;

    /// <summary>
    /// 名称
    /// </summary>
    [SerializeField]
    private Text m_GameServerPageName;

    public Action<int> OnGameServerPageClick;

    void Start () 
	{
        GetComponent<Button>().onClick.AddListener(gameServerPageClick);
	}

    void OnDestroy()
    {
        m_GameServerPageName = null;
        OnGameServerPageClick = null;
    }

    private void gameServerPageClick()
    {
        if (OnGameServerPageClick != null)
        {
            OnGameServerPageClick(m_PageIndex);
        }
    }

    public void SetUI(RetGameServerPageEntity entity)
    {
        m_PageIndex = entity.PageIndex;
        m_GameServerPageName.text = entity.Name;
    }
}