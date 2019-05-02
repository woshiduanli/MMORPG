
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 怪AI
/// </summary>
public class RoleMonsterAI : IRoleAI
{
    /// <summary>
    /// 当前角色控制器
    /// </summary>
    public RoleCtrl CurrRole
    {
        get;
        set;
    }

    /// <summary>
    /// 下次巡逻时间
    /// </summary>
    private float m_NextPatrolTime = 0f;

    /// <summary>
    /// 下次攻击时间
    /// </summary>
    private float m_NextAttackTime = 0f;

    public RoleMonsterAI(RoleCtrl roleCtrl)
    {
        CurrRole = roleCtrl;
    }

    public void DoAI()
    {
        return; 
        if (CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Die) return;

        if (CurrRole.LockEnemy == null)
        {
            //如果是待机状态
            if (CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Idle)
            {
                if (Time.time > m_NextPatrolTime)
                {
                    m_NextPatrolTime = Time.time + UnityEngine.Random.Range(5f, 10f);
                    //进行巡逻
                    CurrRole.MoveTo(new Vector3(CurrRole.BornPoint.x + UnityEngine.Random.Range(CurrRole.PatrolRange * -1, CurrRole.PatrolRange), CurrRole.BornPoint.y, CurrRole.BornPoint.z + UnityEngine.Random.Range(CurrRole.PatrolRange * -1, CurrRole.PatrolRange)));
                }
            }

            //如果主角在怪的视野范围内
            if (Vector3.Distance(CurrRole.transform.position, GlobalInit.Instance.CurrPlayer.transform.position) <= CurrRole.ViewRange)
            {
                CurrRole.LockEnemy = GlobalInit.Instance.CurrPlayer;
            }
        }
        else
        {
            //if (CurrRole.LockEnemy.CurrRoleInfo.CurrHP <= 0)
            //{
            //    CurrRole.LockEnemy = null;
            //    return;
            //}

            //如果有锁定敌人
            //1.如果我和锁定敌人的距离 超过了我的视野范围 则取消锁定
            if (Vector3.Distance(CurrRole.transform.position, GlobalInit.Instance.CurrPlayer.transform.position) > CurrRole.ViewRange)
            {
                CurrRole.LockEnemy = null;
                return;
            }


            if (Vector3.Distance(CurrRole.transform.position, GlobalInit.Instance.CurrPlayer.transform.position) <= CurrRole.AttackRange)
            {
                //2.如果在攻击范围 直接攻击

                if (Time.time > m_NextAttackTime && CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum != RoleState.Attack)
                {
                    m_NextAttackTime = Time.time + UnityEngine.Random.Range(3f, 5f);
                    CurrRole.ToAttack();
                }
            }
            else
            {
                //3.如果在我的视野范围之内 进行追击
                if (CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Idle)
                {
                    CurrRole.MoveTo(new Vector3(CurrRole.LockEnemy.transform.position.x + UnityEngine.Random.Range(CurrRole.AttackRange * -1, CurrRole.AttackRange), CurrRole.BornPoint.y, CurrRole.LockEnemy.transform.position.z + UnityEngine.Random.Range(CurrRole.AttackRange * -1, CurrRole.AttackRange)));
                }
            }
        }
    }
}