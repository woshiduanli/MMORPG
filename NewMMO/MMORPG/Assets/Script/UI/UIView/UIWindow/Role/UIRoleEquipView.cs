
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIRoleEquipView : UISubViewBase
{
    /// <summary>
    /// 角色模型容器
    /// </summary>
    [SerializeField]
    private Transform RoleModelContainer;

    /// <summary>
    /// 昵称
    /// </summary>
    [SerializeField]
    private Text lblNickName;

    /// <summary>
    /// 角色等级
    /// </summary>
    [SerializeField]
    private Text lblLevel;

    /// <summary>
    /// 综合战斗力
    /// </summary>
    [SerializeField]
    private Text lblFighting;

    /// <summary>
    /// 职业编号
    /// </summary>
    private int m_JobId;

    protected override void OnStart()
    {
        base.OnStart();
        CloneRoleModel();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public void SetUI(TransferData data)
    {
        m_JobId = data.GetValue<byte>(ConstDefine.JobId);
        lblNickName.text = data.GetValue<string>(ConstDefine.NickName);
        lblLevel.text = string.Format("Lv.{0}", data.GetValue<int>(ConstDefine.Level));
        lblFighting.text = string.Format("综合战斗力：<color='#ff0000'>{0}</color>", data.GetValue<int>(ConstDefine.Fighting));
    }

    /// <summary>
    /// 克隆角色模型
    /// </summary>
    public void CloneRoleModel()
    {
        RoleInfoMainPlayer data = (RoleInfoMainPlayer)GlobalInit.Instance.CurrPlayer.CurrRoleInfo;
        GameObject obj = RoleMgr.Instance.LoadPlayer(data.JobId);
        obj.SetParent(RoleModelContainer);
        obj.SetLayer("UI");
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();

        RoleModelContainer = null;
        lblNickName = null;
        lblLevel = null;
        lblFighting = null;
    }
}