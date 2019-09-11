
using UnityEngine;
using System.Collections;

/// <summary>
/// 攻击状态
/// </summary>
public class RoleStateAttack : RoleStateAbstract
{
    // 物理攻击和技能攻击， 都在这个状态机里面去执行 
    // 动画控制器执行条件
    public string AnimatorCondition;

    // 旧的控制器执行条件.先调用上一个状态的离开
    public string m_OldAnimatorCondition;

    // 条件值
    public int AnimatorConditionValue;


    public RoleAnimatorState AnimatorCurState;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="roleFSMMgr">有限状态机管理器</param>
    public RoleStateAttack(RoleFSMMgr roleFSMMgr)
        : base(roleFSMMgr)
    {

    }

    /// <summary>
    /// 实现基类 进入状态
    /// </summary>
    public override void OnEnter()
    {
        if (RoleType.Monster == CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleType)
        {
            //Debug.LogError("进来了   "+ CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.RoleNickName+ "    AnimatorCurState:"+ AnimatorCurState +
            //    "   AnimatorConditionValue:"+ AnimatorConditionValue+ "   AnimatorCondition:"+ AnimatorCondition); 
        }
        base.OnEnter();
        CurrRoleFSMMgr.CurrRoleCtrl.PreFightTime = Time.time; 
        m_OldAnimatorCondition = AnimatorCondition;

        // 每个状态机，有两个条件，
        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(AnimatorCondition, AnimatorConditionValue);

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
        if (RoleType.Monster == CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleType)
        {
            //Debug.LogError("update 了   " + CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.RoleNickName + "    AnimatorCurState:" + AnimatorCurState +
            //    "   AnimatorConditionValue:" + AnimatorConditionValue + "   AnimatorCondition:" + AnimatorCondition);
        }
        base.OnUpdate();
        CurrRoleFSMMgr.CurrRoleCtrl.IsRigidity = true;
        CurrRoleAnimatorStateInfo = CurrRoleFSMMgr.CurrRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
        if (CurrRoleAnimatorStateInfo.IsName(AnimatorCurState.ToString()))
        {
            // 如果是当前状态， 那么此时， 让状态机在当前状态， 不出来，卡在这里，除非这次动作已经执行完毕
            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)AnimatorCurState);

            //如果动画执行了一遍 就切换待机
            if (CurrRoleAnimatorStateInfo.normalizedTime > 1)
            {
                CurrRoleFSMMgr.CurrRoleCtrl.IsRigidity = false;
                CurrRoleFSMMgr.CurrRoleCtrl.ToIdle(RoleIdleState.IdelFight);
            }
        }
        else
        {
            // 如果 不是当前需要的状态， 等于0的时候， 就是让他进入当前状态，
            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), 0);
        }
    }

    /// <summary>
    /// 实现基类 离开状态
    /// </summary>
    public override void OnLeave()
    {
        base.OnLeave();
        if (RoleType.Monster == CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleType)
        {
            //Debug.LogError("离开了   " + CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.RoleNickName + "    AnimatorCurState:" + AnimatorCurState +
            //    "   AnimatorConditionValue:" + AnimatorConditionValue + "   AnimatorCondition:" + AnimatorCondition);
        }
        CurrRoleFSMMgr.CurrRoleCtrl.IsRigidity = false;
        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(m_OldAnimatorCondition, 0);
        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), 0);
    }
}