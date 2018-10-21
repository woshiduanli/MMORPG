
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 主角主城AI
/// </summary>
public class RoleMainPlayerCityAI : IRoleAI
{
    public RoleCtrl CurrRole
    {
        get;
        set;
    }

    public RoleMainPlayerCityAI(RoleCtrl roleCtrl)
    {
        CurrRole = roleCtrl;
    }

    public void DoAI()
    {
        //执行AI

        //1.如果我有锁定敌人 就行攻击
        if (CurrRole.LockEnemy != null)
        {
            if (CurrRole.LockEnemy.CurrRoleInfo.CurrHP <= 0)
            {
                CurrRole.LockEnemy = null;
                return;
            }

            if (CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum != RoleState.Attack)
            {
                CurrRole.ToAttack();
            }
        }
    }
}