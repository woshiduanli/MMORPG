
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

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
        if (CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Die) return;
        if (CurrRole.m_Attack.IsAutoFight)
        {

            AutoFight();
        }
        else
        {
            NormalState();
        }
    }

    List<Collider> m_SerachList = new List<Collider>();
    List<RoleCtrl> m_EnenmyList = new List<RoleCtrl>();
    // 玩家进入自动战斗
    private void AutoFight()
    {
        //if (CurrRole.IsRigidity) return;
        // 关卡的主控制器，当前区域的属性
        // 当前区域没有怪物，那么
        if (CurrRole.LockEnemy == null)
        {
            /// 发射射线找怪物 
            Collider[] serachList = Physics.OverlapSphere(CurrRole.transform.position, 10000, 1 << LayerMask.NameToLayer("Role"));
            if (serachList != null && serachList.Length > 0)
            {
                m_SerachList.Clear();
                for (int i = 0; i < serachList.Length; i++)
                {
                    RoleCtrl roleCtrl = serachList[i].GetComponent<RoleCtrl>();
                    if (roleCtrl != null && roleCtrl.CurrRoleInfo.RoldId != CurrRole.CurrRoleInfo.RoldId && CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum != RoleState.Die)
                    {
                        m_SerachList.Add(serachList[i]);
                    }

                }
            }
            if (m_SerachList.Count > 0)
            {
                m_EnenmyList.Clear();
                m_SerachList.Sort((c1, c2) =>
                {
                    int ret = 0;
                    if (Vector3.Distance(c1.gameObject.transform.position, CurrRole.gameObject.transform.position) <
                      Vector3.Distance(c2.gameObject.transform.position, CurrRole.gameObject.transform.position))
                    {
                        ret = -1;
                    }
                    else
                    {
                        ret = 1;
                    }
                    return ret;
                });
                CurrRole.LockEnemy = m_SerachList[0].GetComponent<RoleCtrl>();
                m_EnenmyList.Add(CurrRole.LockEnemy);
            }
        }
        else
        {
            if (CurrRole.LockEnemy.CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Die)
            {
                CurrRole.LockEnemy = null; 
            return;
            }
            int skillId = CurrRole.CurrRoleInfo.GetCanUseSkillId();
            RoleAttackType type;
            if (skillId > 0)
            {
                type = RoleAttackType.SkillAttack;
            }
            else
            {
                skillId = CurrRole.CurrRoleInfo.PhySKillIds[m_PhyIndex];
                ++m_PhyIndex;
                if (m_PhyIndex >= CurrRole.CurrRoleInfo.PhySKillIds.Length) m_PhyIndex = 0;

                // 否则这里物理连击 
                type = RoleAttackType.PhyAttack;
            }
            SkillEntity entity = SkillDBModel.Instance.Get(skillId);
            //Debug.LogError("type:" + type + "  " + skillId);
            // 如果敌人在我攻击范围内， 马上开始攻击
            if (entity == null) return;

            // 暂时屏蔽视野范围
            if (Vector3.Distance(CurrRole.transform.position , CurrRole.LockEnemy.transform.position) <= 1)
                //if (Vector3.Distance(CurrRole.transform.position, GlobalInit.Instance.CurrPlayer.transform.position) <= entity.AttackRange)
            {
                //2.如果在攻击范围首先怪物朝向主角，然后攻击 直接攻击
                CurrRole.transform.LookAt(new Vector3(CurrRole.LockEnemy.transform.position.x, CurrRole.transform.position.y, CurrRole.LockEnemy.transform.position.z));
                //Debug.LogError(":::::"+ CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum);
                if (CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Idle)
                {
                    if (type == RoleAttackType.SkillAttack)
                    {
                        PlayerCtrl.Instance.OnSkillClick(skillId);
                    }
                    else
                    {
                        CurrRole.ToAttackBySkilId(type, skillId);
                    }
                }
            }
            else
            {
                //3.如果在我的视野范围之内 进行追击
                if (CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Idle)
                {
                    // 移动到敌人，攻击范围内的随机点
                    CurrRole.MoveTo(OnPostRender(CurrRole.transform.position,CurrRole.LockEnemy.transform.position,1 * UnityEngine.Random.Range(0.8f, 1)));
                }
            }


        }
    }

    public Vector3 OnPostRender(Vector3 curPos, Vector3 enePos, float radius)
    {
        Vector3 v = (curPos - enePos).normalized;

        //Vector3 v = new Vector3(0, 0, 1);
        v = Quaternion.Euler(0, UnityEngine.Random.Range(-90, 90), 0) * v;
        Vector3 pos = v * radius;
        Vector3 newPos = enePos + pos;
        return newPos;
    }

    private void NormalState()
    {
        if (CurrRole.PreFightTime != 0)
        {
            if (Time.time > CurrRole.PreFightTime + 30)
            {
                CurrRole.ToIdle();
                CurrRole.PreFightTime = 0;
            }
        }


        //执行AI
        if (CurrRole.LockEnemy != null && CurrRole.LockEnemy.CurrRoleInfo.RoldId == CurrRole.CurrRoleInfo.RoldId)
        {
            CurrRole.LockEnemy = null;
        }
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
                // 如果有后续技能， 那么使用技能
                if (CurrRole.m_Attack.FollowSkillId > 0)
                {
                    //Debug.LogError("11111111111111111111111111111执行后续技能"+ CurrRole.m_Attack.FollowSkillId);
                    CurrRole.ToAttackBySkilId(RoleAttackType.SkillAttack, CurrRole.m_Attack.FollowSkillId);
                }
                else
                {
                    // 否则这里物理连击 
                    int skillId = CurrRole.CurrRoleInfo.PhySKillIds[m_PhyIndex];

                    // 循环物理攻击
                    CurrRole.ToAttackBySkilId(RoleAttackType.PhyAttack, skillId);
                    ++m_PhyIndex;
                    if (m_PhyIndex >= CurrRole.CurrRoleInfo.PhySKillIds.Length) m_PhyIndex = 0;
                }
            }
        }

    }
}