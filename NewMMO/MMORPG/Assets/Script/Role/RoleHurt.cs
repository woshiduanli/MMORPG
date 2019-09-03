using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleHurt
{
    RoleFSMMgr m_CurrRoleFSMMgr = null;
    // 当角色受伤的时候，
    public RoleHurt(RoleFSMMgr mgr)
    {
        m_CurrRoleFSMMgr = mgr;
    }

    //public void ToHurt(int atackValue)
    //{
    //    if (m_CurrRoleFSMMgr == null) return;
    //    m_CurrRoleFSMMgr.ChangeState(RoleState.Hurt);
    //}

    //public void ToHurt(RoleTransferAttackInfo roleTransferAttackInfo)
    //{
    //    if (m_CurrRoleFSMMgr == null) return;
    //    m_CurrRoleFSMMgr.ChangeState(RoleState.Hurt);
    //}

    public IEnumerator ToHurt(RoleTransferAttackInfo roleTransferAttackInfo)
    {
        if (m_CurrRoleFSMMgr == null) yield break;
        // 直接返回，
        if (m_CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Die) yield break;

        SkillEntity skillEntity = SkillDBModel.Instance.Get(roleTransferAttackInfo.SkillId);
        SkillLevelEntity skillLevelEntity = SkillLevelDBModel.Instance.Get(roleTransferAttackInfo.SKillLevel);
        if (skillEntity == null || skillLevelEntity == null) yield break; 

        // 延迟时间
        yield return new WaitForSeconds(skillEntity.ShowHurtEffectDelaySecond);

        // 1 减血
        m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP -= roleTransferAttackInfo.HurtValue;
        // 1.1 如果死亡

        if (m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP<=0) {
            Debug.LogError("角色死亡："+ roleTransferAttackInfo.BeAttackRoleId);
            m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP = 0; 
            m_CurrRoleFSMMgr.CurrRoleCtrl.ToDie();
            yield break;
        }
        Debug.LogError("角色受伤：" + roleTransferAttackInfo.BeAttackRoleId+ "  " + m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP);


        // 2 播放受伤特效
        // 3 弹出受伤数字。若有暴击， 显示暴击数字
        // 4 屏幕泛红
        // 5 
        m_CurrRoleFSMMgr.ChangeState(RoleState.Hurt);



    }

}
