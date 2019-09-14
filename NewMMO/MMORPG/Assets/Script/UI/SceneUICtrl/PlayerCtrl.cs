using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : Singleton<PlayerCtrl>, ISystemCtrl
{


    UIRoleInfoView m_UIRoleInfoView;

    void ISystemCtrl.OpenView(WindowUIType type)
    {
        switch (type)
        {

            case WindowUIType.RoleInfo:
                OpenRoleInfoView();
                break;
            case WindowUIType.GameServerEnter:
                break;
            case WindowUIType.GameServerSelect:
                break;
            default:
                break;
        }
    }

    void OpenRoleInfoView()
    {
        m_UIRoleInfoView = UIViewUtil.Instance.OpenWindow(WindowUIType.RoleInfo).GetComponent<UIRoleInfoView>();


        RoleInfoMainPlayer roleInfo = (RoleInfoMainPlayer)GlobalInit.Instance.CurrPlayer.CurrRoleInfo;

        TransferData data = new TransferData();

        data.SetValue(ConstDefine.JobId, roleInfo.JobId);
        data.SetValue(ConstDefine.NickName, roleInfo.RoleNickName);
        data.SetValue(ConstDefine.Level, roleInfo.Level);
        data.SetValue(ConstDefine.Fighting, roleInfo.Fighting);
        data.SetValue(ConstDefine.Money, roleInfo.Money);


        data.SetValue(ConstDefine.Gold, roleInfo.Gold);
        data.SetValue(ConstDefine.Attack, roleInfo.Attack);
        data.SetValue(ConstDefine.Defense, roleInfo.Defense);
        data.SetValue(ConstDefine.Hit, roleInfo.Hit);
        data.SetValue(ConstDefine.Dodge, roleInfo.Dodge);
        data.SetValue(ConstDefine.Cri, roleInfo.Cri);
        data.SetValue(ConstDefine.Res, roleInfo.Res);
        data.SetValue(ConstDefine.CurrHP, roleInfo.CurrHP);
        data.SetValue(ConstDefine.MaxHP, roleInfo.MaxHP);


        data.SetValue(ConstDefine.CurrMP, roleInfo.CurrMP);
        data.SetValue(ConstDefine.MaxMP, roleInfo.MaxMP);
        data.SetValue(ConstDefine.MaxMP, roleInfo.MaxMP);

        data.SetValue(ConstDefine.CurrExp, roleInfo.CurrExp);
        data.SetValue(ConstDefine.MaxExp, int.MaxValue);



        m_UIRoleInfoView.SetRoleInfo(data);
    }

    public void SetMainCityRoleData()
    {
        PlayerCtrl.Instance.SetMainCityRoleInfo();
        PlayerCtrl.Instance.SetMainCityRoleSkillInfo();
    }

    RoleInfoMainPlayer m_MainPlayerRoleinfo; 
    public void SetMainCityRoleInfo()
    {
        m_MainPlayerRoleinfo = (RoleInfoMainPlayer)GlobalInit.Instance.CurrPlayer.CurrRoleInfo;
        string headPic = JobDBModel.Instance.Get(m_MainPlayerRoleinfo.JobId).HeadPic;


        GlobalInit.Instance.CurrPlayer.OnHpChangeHandler = OnHpChangeCallBack;
        GlobalInit.Instance.CurrPlayer.OnMpChangeHandler = OnMpChangeCallBack;

        UIMainCityRoleInfoView.Instance.SetUI(headPic, m_MainPlayerRoleinfo.RoleNickName, 1, m_MainPlayerRoleinfo.Money, m_MainPlayerRoleinfo.Gold,
            m_MainPlayerRoleinfo.CurrHP, m_MainPlayerRoleinfo.MaxHP, m_MainPlayerRoleinfo.CurrMP, m_MainPlayerRoleinfo.MaxMP);
    }

    private void OnMpChangeCallBack(ValueChangeType type)
    {
        if (m_MainPlayerRoleinfo == null) return;
        UIMainCityRoleInfoView.Instance.SetMP(m_MainPlayerRoleinfo.CurrMP, m_MainPlayerRoleinfo.MaxMP);
    }

    private void OnHpChangeCallBack(ValueChangeType type)
    {
        if (m_MainPlayerRoleinfo == null) return; 
        UIMainCityRoleInfoView.Instance.SetHP(m_MainPlayerRoleinfo.CurrHP, m_MainPlayerRoleinfo.MaxHP);
    }

    // 设置主城ui上角色技能信息
    public void SetMainCityRoleSkillInfo()
    {
        RoleInfoMainPlayer mainPlayerRoleInfo = (RoleInfoMainPlayer)GlobalInit.Instance.CurrPlayer.CurrRoleInfo;
        List<TransferData> lst = new List<TransferData>();
        for (int i = 0; i < mainPlayerRoleInfo.SkillList.Count; i++)
        {
            TransferData data = new TransferData();
            data.SetValue(ConstDefine.SkillSlotsNo, mainPlayerRoleInfo.SkillList[i].SlotsNo);
            data.SetValue(ConstDefine.SkillId, mainPlayerRoleInfo.SkillList[i].SkillId);
            data.SetValue(ConstDefine.SkillLevel, mainPlayerRoleInfo.SkillList[i].SkillLevel);
            SkillEntity e = SkillDBModel.Instance.Get(mainPlayerRoleInfo.SkillList[i].SkillId);
            if (e != null)
            {
                data.SetValue(ConstDefine.SkillPic, e.SkillPic);
            }
            else
            {
                Debug.LogError("kong le -");
            }

            SkillLevelEntity e1 = SkillLevelDBModel.Instance.GetEntityBySkillIdAndLevel(mainPlayerRoleInfo.SkillList[i].SkillId, mainPlayerRoleInfo.SkillList[i].SkillLevel);

            if (e1 != null)
            {
                data.SetValue(ConstDefine.SkillCDTime, e1.SkillCDTime);

            }

            lst.Add(data);
        }
        Debug.LogError("kong le3333333333 -");

        UIMainCitySkillView.Instance.SetUI(lst, OnSkillClick);

    }

    private void OnSkillClick(int SkillId)
    {
        bool isSuccess = GlobalInit.Instance.CurrPlayer.ToAttackBySkilId(RoleAttackType.SkillAttack, SKillId: SkillId);
        if (isSuccess)
        {
            Debug.LogError("使用技能成功了");
            UIMainCitySkillView.Instance.BeginCD(SkillId);
        }

    }
}
