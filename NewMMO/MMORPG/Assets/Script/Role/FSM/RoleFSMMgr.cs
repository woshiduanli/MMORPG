
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 角色有限状态机管理器
/// </summary>
public class RoleFSMMgr 
{
    /// <summary>
    /// 当前角色控制器
    /// </summary>
    public RoleCtrl CurrRoleCtrl { get; private set; }

    /// <summary>
    /// 当前角色状态枚举
    /// </summary>
    public RoleState CurrRoleStateEnum { get; private set; }

    /// <summary>
    /// 当前角色状态
    /// </summary>
    private RoleStateAbstract m_CurrRoleState = null;

    private Dictionary<RoleState, RoleStateAbstract> m_RoleStateDic;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="currRoleCtrl"></param>
    public RoleFSMMgr(RoleCtrl currRoleCtrl)
    {
        CurrRoleCtrl = currRoleCtrl;
        m_RoleStateDic = new Dictionary<RoleState, RoleStateAbstract>();
        m_RoleStateDic[RoleState.Idle] = new RoleStateIdle(this);
        m_RoleStateDic[RoleState.Run] = new RoleStateRun(this);
        m_RoleStateDic[RoleState.Attack] = new RoleStateAttack(this);
        m_RoleStateDic[RoleState.Hurt] = new RoleStateHurt(this);
        m_RoleStateDic[RoleState.Die] = new RoleStateDie(this);

        if (m_RoleStateDic.ContainsKey(CurrRoleStateEnum))
        {
            m_CurrRoleState = m_RoleStateDic[CurrRoleStateEnum];
        }
    }

    #region OnUpdate 每帧执行
    /// <summary>
    /// 每帧执行
    /// </summary>
    public void OnUpdate()
    {
        if (m_CurrRoleState != null)
        {
            m_CurrRoleState.OnUpdate();
        }
    }
    #endregion

    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="newState">新状态</param>
    public void ChangeState(RoleState newState)
    {
        if (CurrRoleStateEnum == newState) return;

        //调用以前状态的离开方法
        if (m_CurrRoleState != null)
            m_CurrRoleState.OnLeave();

        //更改当前状态枚举
        CurrRoleStateEnum = newState;

        //更改当前状态
        m_CurrRoleState = m_RoleStateDic[newState];

        ///调用新状态的进入方法
        m_CurrRoleState.OnEnter();
    }
}