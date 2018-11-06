
using UnityEngine;
using System.Collections;

public class UIRoleInfoView : UIWindowViewBase
{
    /// <summary>
    /// 角色装备View
    /// </summary>
    [SerializeField]
    private UIRoleEquipView m_UIRoleEquipView;

    /// <summary>
    /// 角色详情视图
    /// </summary>
    [SerializeField]
    private UIRoleInfoDetailView m_UIRoleInfoDetailView;

    /// <summary>
    /// 
    /// </summary>
    public void SetRoleInfo(TransferData data)
    {
        m_UIRoleEquipView.SetUI(data);
        m_UIRoleInfoDetailView.SetUI(data);
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
    }

    private void Update()
    {
        //if (transform.parent==null)
        //    gameObject.SetParent(SceneUIMgr.Instance.CurrentUIScene.Container_Center);
    }

    protected override void BeforeOnDestroy()
    {
        m_UIRoleEquipView = null;
        m_UIRoleInfoDetailView = null;
    }
}