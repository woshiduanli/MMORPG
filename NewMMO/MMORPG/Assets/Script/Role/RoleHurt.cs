using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleHurt
{
    RoleFSMMgr m_CurrRoleFSMMgr = null;
    public System.Action OnRoleHurt;
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

        Debug.LogError("juese shoushang1 ：" + roleTransferAttackInfo.BeAttackRoleId + "  " + m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP+ "  "+ roleTransferAttackInfo.HurtValue);
        // 1 减血 ____ 这是才是真实值
        //m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP -= roleTransferAttackInfo.HurtValue;
        // ___测试值默认， 减血20 
        m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP -= 5;
        int fontSize = 1;
        Color c = Color.red;
        if (roleTransferAttackInfo.isCri)
        {
            fontSize = 8;
            c = Color.yellow; 
        }
        // 减血的飘雪动画播放
        UISceneCtrl.Instance.CurrentUIScene.HudText.NewText("- 5", m_CurrRoleFSMMgr.CurrRoleCtrl.gameObject.transform, c, fontSize, 20,-1,2.2f,   Random.Range(0,2)==1?bl_Guidance.RightDown: bl_Guidance.LeftDown);
    

        //m_CurrRoleFSMMgr.CurrRoleCtrl.bar

        // 头顶血条的变化
        if (OnRoleHurt != null) OnRoleHurt();
        // 1.1 如果死亡

        if (m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP <= 0)
        {
            Debug.LogError("die ：" + roleTransferAttackInfo.BeAttackRoleId);
            m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP = 0;
            m_CurrRoleFSMMgr.CurrRoleCtrl.ToDie();
            yield break;
        }

        //Debug.LogError("juese shoushang 2：" + roleTransferAttackInfo.BeAttackRoleId + "  " + m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP);
        // 2 播放受伤特效
        Transform trans = EffectMgr.Instance.PlayEffect("Effect_Hurt");
        trans.position = m_CurrRoleFSMMgr.CurrRoleCtrl.gameObject.transform.position;
        trans.rotation = m_CurrRoleFSMMgr.CurrRoleCtrl.gameObject.transform.rotation;
        EffectMgr.Instance.DestroyEffect(trans, 2);

        // 3 弹出受伤数字。若有暴击， 显示暴击数字
        // 4 屏幕泛红
        // 5 
        m_CurrRoleFSMMgr.ChangeState(RoleState.Hurt);



    }

}
