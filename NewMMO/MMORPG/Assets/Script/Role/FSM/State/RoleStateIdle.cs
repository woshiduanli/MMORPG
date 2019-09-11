
using UnityEngine;
using System.Collections;

/// <summary>
/// 待机状态
/// </summary>
public class RoleStateIdle : RoleStateAbstract
{
    float m_NextChangeTime;
    float m_changeStep = 5;
    bool m_isXiuXian;
    float m_RuningTime;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="roleFSMMgr">有限状态机管理器</param>
    public RoleStateIdle(RoleFSMMgr roleFSMMgr)
        : base(roleFSMMgr)
    {

    }

    /// <summary>
    /// 实现基类 进入状态
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        if (CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleType == RoleType.MainPlayer)
        {
            if (CurrRoleFSMMgr.CurIdelState == RoleIdleState.IdelNormal)
            {
                m_NextChangeTime = Time.time + m_changeStep;
                m_isXiuXian = false;
                // 此时这里，直接进入状态， 让他等于当前的状态
                CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToIdleNormal.ToString(), true);
            }
            else
            {
                CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToIdleFight.ToString(), true);
            }
            m_RuningTime = 0;
        }
        else
        {
            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToIdleFight.ToString(), true);
        }
    }

    /// <summary>
    /// 实现基类 执行状态
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();

        if (CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleType == RoleType.MainPlayer)
        {

            if (!IsChangeOver)
            {
                if (CurrRoleFSMMgr.CurIdelState == RoleIdleState.IdelNormal)
                {

                    CurrRoleAnimatorStateInfo = CurrRoleFSMMgr.CurrRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
                    if (!m_isXiuXian)
                    {
                        // 为什么要设置一个true,同时，要设置一个int，让当前的值，不等于，这个int，为了让他进入的时候， 只进入一次
                        if (CurrRoleAnimatorStateInfo.IsName(RoleAnimatorState.Idle_Normal.ToString()))
                        {
                            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleAnimatorState.Idle_Normal);
                            m_RuningTime += Time.deltaTime;
                            if (m_RuningTime > 0.2f)
                            {
                                IsChangeOver = true;
                            }
                        }
                    }
                    else
                    {
                        if (CurrRoleAnimatorStateInfo.IsName(RoleAnimatorState.XiuXian.ToString()))
                        {
                            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleAnimatorState.XiuXian);
                            IsChangeOver = true;
                        }

                    }
                }
                else
                {
                    CurrRoleAnimatorStateInfo = CurrRoleFSMMgr.CurrRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
                    if (CurrRoleAnimatorStateInfo.IsName(RoleAnimatorState.Idle_Fight.ToString()))
                    {
                        // 防止进入相同动画
                        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleAnimatorState.Idle_Fight);
                        IsChangeOver = true;
                    }
                }
            }

            // ------------------- 待机和休闲的状态 --------------
            if (CurrRoleFSMMgr.CurIdelState == RoleIdleState.IdelNormal)
            {
                if (Time.time > m_NextChangeTime)
                {
                    m_NextChangeTime = Time.time + m_changeStep;
                    m_isXiuXian = true;
                    IsChangeOver = false;
                    CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToXiuXian.ToString(), true);
                    CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToIdleNormal.ToString(), false);
                }

                if (m_isXiuXian)
                {
                    CurrRoleAnimatorStateInfo = CurrRoleFSMMgr.CurrRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
                    if (CurrRoleAnimatorStateInfo.IsName(RoleAnimatorState.XiuXian.ToString()) && CurrRoleAnimatorStateInfo.normalizedTime > 1)
                    {
                        m_isXiuXian = false;
                        IsChangeOver = false;
                        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToXiuXian.ToString(), false);
                        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToIdleNormal.ToString(), true);
                    }
                }
            }

        }
        else
        {
            // 这里怪物的状态机
            CurrRoleAnimatorStateInfo = CurrRoleFSMMgr.CurrRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
            if (CurrRoleAnimatorStateInfo.IsName(RoleAnimatorState.Idle_Fight.ToString()))
            {
                CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleAnimatorState.Idle_Fight);
                IsChangeOver = true;
            }
            else {
                // 防止怪物原地跑步
                CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), 0);
            }
        }


    }

    /// <summary>
    /// 实现基类 离开状态
    /// </summary>
    public override void OnLeave()
    {
        base.OnLeave();

        if (CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleType == RoleType.MainPlayer)
        {
            if (CurrRoleFSMMgr.CurIdelState == RoleIdleState.IdelNormal)
            {
                CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToIdleNormal.ToString(), false);
                CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToXiuXian.ToString(), false);
            }
            else
            {
                CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToIdleFight.ToString(), false);
            }
        }
        else
        {
            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToIdleFight.ToString(), false);
        }
    }
}