
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
    int m_PhyIndex = 0; 
    public void DoAI()
    {
        //执行AI

        //1.如果我有锁定敌人 就行攻击
        if (CurrRole.LockEnemy != null)
        {
            // 敌人死亡直接返回
            if (CurrRole.LockEnemy.CurrRoleInfo.CurrHP <= 0)
            {
                CurrRole.LockEnemy = null;
                return;
            }

            // 主玩家在idle 那么直接攻击
            if (CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Idle)
            {
                int skillId = CurrRole.CurrRoleInfo.PhySKillIds[m_PhyIndex];

                // 循环物理攻击
                CurrRole.ToAttackBySkilId(RoleAttackType.PhyAttack, skillId);
                ++m_PhyIndex;
                if (m_PhyIndex >= CurrRole.CurrRoleInfo.PhySKillIds.Length) m_PhyIndex = 0;
            }
        }
    }
}