using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : Singleton<PlayerCtrl>, ISystemCtrl
{
    void ISystemCtrl.OpenView(WindowUIType type)
    {

    }

    public void SetMainCityRoleInfo()
    {
        RoleInfoMainPlayer mainPlayerRoleinfo = (RoleInfoMainPlayer)GlobalInit.Instance.CurrPlayer.CurrRoleInfo;

        string headPic = JobDBModel.Instance.Get(mainPlayerRoleinfo.JobId).HeadPic;


        UIMainCityRoleInfoView.Instance.SetUI(headPic, mainPlayerRoleinfo.RoleNickName, 1, mainPlayerRoleinfo.Money, mainPlayerRoleinfo.Gold,
            mainPlayerRoleinfo.CurrHP, mainPlayerRoleinfo.MaxHP, mainPlayerRoleinfo.CurrMP, mainPlayerRoleinfo.MaxMP);




    }
}
