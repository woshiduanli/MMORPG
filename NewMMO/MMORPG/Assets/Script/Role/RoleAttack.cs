using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RoleAttack
{


    RoleFSMMgr m_CurrRoleFSMMgr = null;
    // 当角色受伤的时候，
    public void SetFSM(RoleFSMMgr mgr)
    {
        m_CurrRoleFSMMgr = mgr;
    }

    // 攻击信息，是策划的配置表 相关的数据 
    public List<RoleAttackInfo> PhyAttackInfoList;

    public List<RoleAttackInfo> SkillAttackInfoList;

    private RoleStateAttack m_RoleStateAttack;


    private RoleAttackInfo GetRoleAttackInfo(RoleAttackType type, int index)
    {
        if (type == RoleAttackType.PhyAttack)
        {
            for (int i = 0; i < PhyAttackInfoList.Count; i++)
            {
                if (PhyAttackInfoList[i].Index == index)
                {
                    return PhyAttackInfoList[i];
                }
            }
        }
        else
        {

            for (int i = 0; i < SkillAttackInfoList.Count; i++)
            {
                if (SkillAttackInfoList[i].Index == index)
                {
                    return SkillAttackInfoList[i];
                }
            }
        }
        return null;
    }



    public void ToAttack(RoleAttackType type, int index)
    {
        if (m_CurrRoleFSMMgr == null || m_CurrRoleFSMMgr.CurrRoleCtrl.IsRigidity) return;

        RoleAttackInfo info = GetRoleAttackInfo(type, index);
        //DEBUG_ROLESTATE
#if DEBUG_ROLESTATE


        if (info != null)
        {
            // 在编辑器窗口中绘制，巡逻范围等
            m_CurrRoleFSMMgr.CurrRoleCtrl.roleAttackInfo = info;
            m_CurrRoleFSMMgr.CurrRoleCtrl.CurrAttackRange = info.AttackRange;

            GameObject obj = GameObject.Instantiate(info.EffectObject);
            // 特效的位置和转向和主角的位置和转向要一个方向
            obj.transform.position = m_CurrRoleFSMMgr.CurrRoleCtrl.transform.position;
            obj.transform.rotation = m_CurrRoleFSMMgr.CurrRoleCtrl.transform.rotation;
            Object.Destroy(obj, info.EffectLifeTime);
        }
#else
       // 



#endif
        // ab包中加载特效 if (info != null)
        if (info != null)
        {
            // 在编辑器窗口中绘制，巡逻范围等
            m_CurrRoleFSMMgr.CurrRoleCtrl.roleAttackInfo = info;
            m_CurrRoleFSMMgr.CurrRoleCtrl.CurrAttackRange = info.AttackRange;

            GameObject obj = EffectMgr.Instance.PlayEffect(info.EffectName).gameObject; 
            // 特效的位置和转向和主角的位置和转向要一个方向
            obj.transform.position = m_CurrRoleFSMMgr.CurrRoleCtrl.transform.position;
            obj.transform.rotation = m_CurrRoleFSMMgr.CurrRoleCtrl.transform.rotation;
            EffectMgr.Instance.DestroyEffect(obj.transform, info.EffectLifeTime); 
        }



        if (info != null && CameraCtrl.Instance != null && info.IsDoCameraShake)
        {
            CameraCtrl.Instance.ToDoCameraShake(info.CameraShakeDelay);
        }

        if (m_RoleStateAttack == null)
        {
            // 拿到这个攻击的动画状态机， 然后赋值告诉这个状态机，此时是物理攻击几，或者是技能攻击几  
            m_RoleStateAttack = (RoleStateAttack)m_CurrRoleFSMMgr.GetRoleState(RoleState.Attack);
        }

        m_RoleStateAttack.AnimatorCondition = string.Format(type == RoleAttackType.PhyAttack ? "ToPhyAttack" : "ToSkill");
        m_RoleStateAttack.AnimatorConditionValue = index;
        m_RoleStateAttack.AnimatorCurState = GameUtil.GetRoleAnimatorState(type, index);
        // 切换成攻击状态
        m_CurrRoleFSMMgr.ChangeState(RoleState.Attack);
    }


}
