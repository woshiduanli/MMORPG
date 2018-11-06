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

    public void SetMainCityRoleInfo()
    {
        RoleInfoMainPlayer mainPlayerRoleinfo = (RoleInfoMainPlayer)GlobalInit.Instance.CurrPlayer.CurrRoleInfo;

        string headPic = JobDBModel.Instance.Get(mainPlayerRoleinfo.JobId).HeadPic;


        UIMainCityRoleInfoView.Instance.SetUI(headPic, mainPlayerRoleinfo.RoleNickName, 1, mainPlayerRoleinfo.Money, mainPlayerRoleinfo.Gold,
            mainPlayerRoleinfo.CurrHP, mainPlayerRoleinfo.MaxHP, mainPlayerRoleinfo.CurrMP, mainPlayerRoleinfo.MaxMP);
    }



}
