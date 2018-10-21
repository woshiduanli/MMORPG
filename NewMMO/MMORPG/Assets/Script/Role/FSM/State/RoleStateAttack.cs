
using UnityEngine;
using System.Collections;

/// <summary>
/// 攻击状态
/// </summary>
public class RoleStateAttack : RoleStateAbstract
{
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="roleFSMMgr">有限状态机管理器</param>
    public RoleStateAttack(RoleFSMMgr roleFSMMgr) : base(roleFSMMgr)
    {

    }

    /// <summary>
    /// 实现基类 进入状态
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.ToPhyAttack.ToString(), 1);

        if (CurrRoleFSMMgr.CurrRoleCtrl.LockEnemy != null)
        {
            CurrRoleFSMMgr.CurrRoleCtrl.transform.LookAt(new Vector3(CurrRoleFSMMgr.CurrRoleCtrl.LockEnemy.transform.position.x, CurrRoleFSMMgr.CurrRoleCtrl.transform.position.y, CurrRoleFSMMgr.CurrRoleCtrl.LockEnemy.transform.position.z));
        }
    }

    /// <summary>
    /// 实现基类 执行状态
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();

        CurrRoleAnimatorStateInfo = CurrRoleFSMMgr.CurrRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
        if (CurrRoleAnimatorStateInfo.IsName(RoleAnimatorName.PhyAttack1.ToString()))
        {
            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleState.Attack);

            //如果动画执行了一遍 就切换待机
            if (CurrRoleAnimatorStateInfo.normalizedTime > 1)
            {
                CurrRoleFSMMgr.CurrRoleCtrl.ToIdle();
            }
        }
        else
        {
            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), 0);
        }
    }

    /// <summary>
    /// 实现基类 离开状态
    /// </summary>
    public override void OnLeave()
    {
        base.OnLeave();
        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.ToPhyAttack.ToString(), 0);
    }
}