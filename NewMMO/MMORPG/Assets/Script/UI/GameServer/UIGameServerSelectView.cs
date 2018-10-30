
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class UIGameServerSelectView : UIWindowViewBase
{
    protected override void OnStart()
    {
        base.OnStart();

        for (int i = 0; i < 10; i++)
        {
            GameObject obj = Instantiate(GameServerItemPrefab) as GameObject;
            obj.transform.parent = GameServerGrid.transform;
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            obj.SetActive(false);
            m_GameServerObjList.Add(obj);
        }
    }

    #region 页签
    /// <summary>
    /// 页签项预设
    /// </summary>
    [SerializeField]
    private GameObject GameServerPageItemPrefab;

    /// <summary>
    /// 页签列表
    /// </summary>
    [SerializeField]
    private GridLayoutGroup GameServerPageGrid;

    public Action<int> OnPageClick;

    public void SetGameServerPageUI(List<RetGameServerPageEntity> lst)
    {
        if (lst == null || GameServerPageItemPrefab == null) return;
        for (int i = 0; i < lst.Count; i++)
        {
            GameObject obj = Instantiate(GameServerPageItemPrefab) as GameObject;
            obj.transform.parent = GameServerPageGrid.transform;
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;

            UIGameServerPageItemView view = obj.GetComponent<UIGameServerPageItemView>();

            if (view != null)
            {
                view.SetUI(lst[i]);
                view.OnGameServerPageClick = OnGameServerPageClick;
            }
        }
    }

    private void OnGameServerPageClick(int obj)
    {
        if (OnPageClick != null) OnPageClick(obj);
    }
    #endregion

    #region 服务器列表

    /// <summary>
    /// 服务器预设
    /// </summary>
    [SerializeField]
    private GameObject GameServerItemPrefab;

    /// <summary>
    /// 服务器列表
    /// </summary>
    [SerializeField]
    private GridLayoutGroup GameServerGrid;

    private List<GameObject> m_GameServerObjList = new List<GameObject>();

    public Action<RetGameServerEntity> OnGameServerClick ;

    /// <summary>
    /// 设置服务器列表
    /// </summary>
    /// <param name="lst"></param>
    public void SetGameServerUI(List<RetGameServerEntity> lst)
    {
        if (lst == null || GameServerItemPrefab == null) return;

        for (int i = 0; i < m_GameServerObjList.Count; i++)
        {
            if (i > lst.Count - 1)
            {
                m_GameServerObjList[i].SetActive(false);
            }
        }

        for (int i = 0; i < lst.Count; i++)
        {
            GameObject obj = m_GameServerObjList[i];

            if (!obj.activeSelf)
            {
                obj.SetActive(true);
            }

            UIGameServerItemView view = obj.GetComponent<UIGameServerItemView>();
            if (view != null)
            {
                view.SetUI(lst[i]);
                view.OnGameServerClick = OnGameServerClickCallBack;
            }
        }
    }

    public void OnGameServerClickCallBack(RetGameServerEntity obj)
    {
        if (OnGameServerClick != null) OnGameServerClick(obj);
    }

    #endregion

    #region 已选择的服务器

    /// <summary>
    /// 已选择的View
    /// </summary>
    [SerializeField]
    private UIGameServerItemView m_CurrSelectGameServer;

    /// <summary>
    /// 设置已选择的服务器
    /// </summary>
    /// <param name="entity"></param>
    public void SetSelectGameServerUI(RetGameServerEntity entity)
    {
        if (m_CurrSelectGameServer == null) return;
        m_CurrSelectGameServer.SetUI(entity);
    }

    #endregion

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();

        GameServerPageItemPrefab = null;
        GameServerPageGrid = null;
        OnPageClick = null;
        GameServerItemPrefab = null;
        GameServerGrid = null;
        m_GameServerObjList.ToArray().SetNull();
        OnGameServerClick = null;

        m_CurrSelectGameServer = null;
    }
}