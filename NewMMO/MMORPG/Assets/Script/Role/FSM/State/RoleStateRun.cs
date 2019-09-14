
using UnityEngine;
using System.Collections;

/// <summary>
/// 跑状态
/// </summary>
public class RoleStateRun : RoleStateAbstract
{
    /// <summary>
    /// 转身速度
    /// </summary>
    private float m_RotationSpeed = 0.2f;

    /// <summary>
    /// 转身的目标方向
    /// </summary>
    private Quaternion m_TargetQuaternion;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="roleFSMMgr">有限状态机管理器</param>
    public RoleStateRun(RoleFSMMgr roleFSMMgr) : base(roleFSMMgr)
    {

    }

    /// <summary>
    /// 实现基类 进入状态
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();

        m_RotationSpeed = 0;
        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToRun.ToString(), true);
    }

    /// <summary>
    /// 实现基类 执行状态
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();


        CurrRoleAnimatorStateInfo = CurrRoleFSMMgr.CurrRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);
        if (CurrRoleAnimatorStateInfo.IsName(RoleAnimatorState.Run.ToString()))
        {
            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleAnimatorState.Run);
        }
        else
        {
            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), 0);
        }

        // 如果没有路
        if (CurrRoleFSMMgr.CurrRoleCtrl.AStartPath == null)
        {
            if (Time.time > CurrRoleFSMMgr.CurrRoleCtrl.PreFightTime + 30)
            {
                CurrRoleFSMMgr.CurrRoleCtrl.ToIdle();
            }
            else
            {
                CurrRoleFSMMgr.CurrRoleCtrl.ToIdle(RoleIdleState.IdelFight);
            }
            return;
        }

        if (CurrRoleFSMMgr.CurrRoleCtrl.AstartCurrWayPointIndex >= CurrRoleFSMMgr.CurrRoleCtrl.AStartPath.vectorPath.Count)
        {
            CurrRoleFSMMgr.CurrRoleCtrl.AStartPath = null;


            if (CurrRoleFSMMgr.CurrRoleCtrl.PreFightTime == 0 || Time.time > CurrRoleFSMMgr.CurrRoleCtrl.PreFightTime + 30)
            {
                CurrRoleFSMMgr.CurrRoleCtrl.ToIdle();
            }
            else
            {
                CurrRoleFSMMgr.CurrRoleCtrl.ToIdle(RoleIdleState.IdelFight);
            }

            //CurrRoleFSMMgr.CurrRoleCtrl.ToIdle();
            return;
        }



        Vector3 direction = Vector3.zero;

        Vector3 temp = new Vector3(CurrRoleFSMMgr.CurrRoleCtrl.AStartPath.vectorPath[CurrRoleFSMMgr.CurrRoleCtrl.AstartCurrWayPointIndex].x,
            CurrRoleFSMMgr.CurrRoleCtrl.transform.position.y,
            CurrRoleFSMMgr.CurrRoleCtrl.AStartPath.vectorPath[CurrRoleFSMMgr.CurrRoleCtrl.AstartCurrWayPointIndex].z);
        direction = temp - CurrRoleFSMMgr.CurrRoleCtrl.transform.position;
        direction = direction.normalized; //归一化
        direction = direction * Time.deltaTime * CurrRoleFSMMgr.CurrRoleCtrl.Speed;
        direction.y = 0;

        //让角色缓慢转身
        if (m_RotationSpeed <= 1)
        {
            m_RotationSpeed += 10f * Time.deltaTime;
            m_TargetQuaternion = Quaternion.LookRotation(direction);
            CurrRoleFSMMgr.CurrRoleCtrl.transform.rotation = Quaternion.Lerp(CurrRoleFSMMgr.CurrRoleCtrl.transform.rotation, m_TargetQuaternion, m_RotationSpeed);

            if (Quaternion.Angle(CurrRoleFSMMgr.CurrRoleCtrl.transform.rotation, m_TargetQuaternion) < 1)
            {
                m_RotationSpeed = 0;
            }
        }

        // 判断距离和下个点的距离的长度， 
        float dis = Vector3.Distance(CurrRoleFSMMgr.CurrRoleCtrl.transform.position, temp);
        // 如果小于1的情况
        if (dis < direction.magnitude + 0.1f)
        {
            CurrRoleFSMMgr.CurrRoleCtrl.AstartCurrWayPointIndex++;
        }

        CurrRoleFSMMgr.CurrRoleCtrl.CharacterController.Move(direction);
    }

    /// <summary>
    /// 实现基类 离开状态
    /// </summary>
    public override void OnLeave()
    {
        base.OnLeave();
        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToRun.ToString(), false);
    }
}