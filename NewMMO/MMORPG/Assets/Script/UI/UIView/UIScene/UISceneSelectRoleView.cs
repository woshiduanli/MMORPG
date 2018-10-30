
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class UISceneSelectRoleView : UISceneViewBase
{
    /// <summary>
    /// 拖拽的视图
    /// </summary>
    public UISelectRoleDragView SelectRoleDragView;

    /// <summary>
    /// 职业项
    /// </summary>
    public UISelectRoleJobItemView[] JobItems;

    /// <summary>
    /// 当前选择的职业描述视图
    /// </summary>
    public UISelectRoleJobDescView SelectRoleJobDescView;

    //====================================
    /// <summary>
    /// 昵称
    /// </summary>
    public InputField txtNickName;

    /// <summary>
    /// 开始游戏按钮点击
    /// </summary>
    public Action OnBtnBeginGameClick;

    /// <summary>
    /// 删除角色按钮点击
    /// </summary>
    public Action OnBtnDeleteRoleClick;

    /// <summary>
    /// 返回按钮点击
    /// </summary>
    public Action OnBtnReturnClick;

    /// <summary>
    /// 新建角色按钮点击
    /// </summary>
    public Action OnBtnCreateRoleClick;

    //====================================

    /// <summary>
    /// 新建角色UI
    /// </summary>
    [SerializeField]
    private Transform[] UICreateRole;

    /// <summary>
    /// 选择角色UI
    /// </summary>
    [SerializeField]
    private Transform[] UISelectRole;

    /// <summary>
    /// 角色头像
    /// </summary>
    [SerializeField]
    private Sprite[] m_RoleHeadPic;

    /// <summary>
    /// 已有角色预设
    /// </summary>
    [SerializeField]
    private GameObject m_RoleItemPrefab;

    /// <summary>
    /// 已有角色列表的容器
    /// </summary>
    [SerializeField]
    private Transform m_RoleListContainer;

    /// <summary>
    /// 删除角色视图
    /// </summary>
    [SerializeField]
    private UISelectRoleDeleteRoleView m_DeleteRoleView;

    protected override void OnStart()
    {
        base.OnStart();
    }

    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "btnRandomName":
                RandomName();
                break;
            case "btnBeginGame":
                if (OnBtnBeginGameClick != null) OnBtnBeginGameClick();
                break;
            case "btnDeleteRole":
                if (OnBtnDeleteRoleClick != null) OnBtnDeleteRoleClick();
                break;
            case "btnReturn":
                if (OnBtnReturnClick != null) OnBtnReturnClick();
                break;
            case "btnCreateRole":
                if (OnBtnCreateRoleClick != null) OnBtnCreateRoleClick();
                break;
        }
    }

    /// <summary>
    /// 随机名字
    /// </summary>
    public void RandomName()
    {
        txtNickName.text = GameUtil.RandomName();
    }

    #region SetUICreateRoleShow 设置新建角色UI 是否显示
    /// <summary>
    /// 设置新建角色UI 是否显示
    /// </summary>
    /// <param name="isActive"></param>
    public void SetUICreateRoleActive(bool isActive)
    {
        if (UICreateRole != null && UICreateRole.Length > 0)
        {
            for (int i = 0; i < UICreateRole.Length; i++)
            {
                UICreateRole[i].gameObject.SetActive(isActive);
            }
        }
    }
    #endregion

    #region SetUISelectRoleActive 设置选择角色UI 是否显示
    /// <summary>
    /// 设置选择角色UI 是否显示
    /// </summary>
    /// <param name="isActive"></param>
    public void SetUISelectRoleActive(bool isActive)
    {
        if (UISelectRole != null && UISelectRole.Length > 0)
        {
            for (int i = 0; i < UISelectRole.Length; i++)
            {
                UISelectRole[i].gameObject.SetActive(isActive);
            }
        }
    }
    #endregion

    /// <summary>
    /// 已有角色的 ItemView列表
    /// </summary>
    private List<UISelectRoleRoleItemView> m_RoleItemViewList = new List<UISelectRoleRoleItemView>();

    /// <summary>
    /// 设置已有角色
    /// </summary>
    /// <param name="list"></param>
    public void SetRoleList(List<RoleOperation_LogOnGameServerReturnProto.RoleItem> list, Action<int> OnSelectRole)
    {
        ClearRoleListUI();

        for (int i = 0; i < list.Count; i++)
        {
            //克隆UI
            GameObject obj = Instantiate(m_RoleItemPrefab);
            UISelectRoleRoleItemView view = obj.GetComponent<UISelectRoleRoleItemView>();
            if (view != null)
            {
                view.SetUI(list[i].RoleId, list[i].RoleNickName, list[i].RoleLevel, list[i].RoleJob, m_RoleHeadPic[list[i].RoleJob - 1], OnSelectRole);
                m_RoleItemViewList.Add(view);
            }
            obj.transform.parent = m_RoleListContainer;
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = new Vector3(0, -100 * i, 0);
        }
    }

    /// <summary>
    /// 设置选择的已有角色UI
    /// </summary>
    /// <param name="selectRoleId"></param>
    public void SetSelected(int selectRoleId)
    {
        if (m_RoleItemViewList != null && m_RoleItemViewList.Count > 0)
        {
            for (int i = 0; i < m_RoleItemViewList.Count; i++)
            {
                m_RoleItemViewList[i].SetSelected(selectRoleId);
            }
        }
    }

    /// <summary>
    /// 清除已有角色UI
    /// </summary>
    public void ClearRoleListUI()
    {
        if (m_RoleItemViewList.Count > 0)
        {
            for (int i = 0; i < m_RoleItemViewList.Count; i++)
            {
                Destroy(m_RoleItemViewList[i].gameObject);
            }
            m_RoleItemViewList.Clear();
        }
    }

    public void DeleteSelectRole(string nickName, Action onBtnOkClick)
    {
        m_DeleteRoleView.Show(nickName, onBtnOkClick);
    }

    public void CloseDeleteRoleView()
    {
        m_DeleteRoleView.Close();
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();

        SelectRoleDragView = null;
        JobItems.SetNull();
        SelectRoleJobDescView = null;
        txtNickName = null;
        OnBtnBeginGameClick = null;
        OnBtnDeleteRoleClick = null;
        OnBtnReturnClick = null;
        OnBtnCreateRoleClick = null;
        UICreateRole.SetNull();
        UISelectRole.SetNull();
        m_RoleHeadPic.SetNull();

        m_RoleItemPrefab = null;
        m_RoleListContainer = null;
        m_DeleteRoleView = null;

        m_RoleItemViewList.ToArray().SetNull();
    }
}