
using UnityEngine;
using System.Collections;

/// <summary>
/// 死亡状态
/// </summary>
public class RoleStateDie : RoleStateAbstract
{
    public System.Action OnDie;
    public System.Action OnDestroy;
    public bool m_Is_Destory = false;
    public float m_BeginDieTime = 0f; 
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="roleFSMMgr">有限状态机管理器</param>
    public RoleStateDie(RoleFSMMgr roleFSMMgr)
        : base(roleFSMMgr)
    {

    }

    /// <summary>
    /// 实现基类 进入状态
    /// </summary>
    public override void OnEnter()
    {
        base.OnEnter();
        MyDebug.debug("角色死亡");



        Transform trans = EffectMgr.Instance.PlayEffect("Effect_PenXue");
        trans.position = CurrRoleFSMMgr.CurrRoleCtrl.gameObject.transform.position;
        trans.rotation = CurrRoleFSMMgr.CurrRoleCtrl.gameObject.transform.rotation;
        EffectMgr.Instance.DestroyEffect(trans, 6);
        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToDie.ToString(), true);
        m_BeginDieTime = 0;  
        if (OnDie != null) OnDie();
    }

    /// <summary>
    /// 实现基类 执行状态
    /// </summary>
    public override void OnUpdate()
    {
        base.OnUpdate();
        m_BeginDieTime += Time.deltaTime;
        if (m_BeginDieTime >= 6)
        {
            if (OnDestroy != null)
            {
                if (m_Is_Destory) return;

                m_Is_Destory = true;
                OnDestroy();
            }
            return; 
        }
        CurrRoleAnimatorStateInfo = CurrRoleFSMMgr.CurrRoleCtrl.Animator.GetCurrentAnimatorStateInfo(0);

        // 如果现在状态已经是死亡了，那么，
        if (CurrRoleAnimatorStateInfo.IsName(RoleAnimatorState.Die.ToString()))
        {
            CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleAnimatorState.Die);

            //如果动画执行了一遍 就切换待机
            //if (CurrRoleAnimatorStateInfo.normalizedTime > 1 && CurrRoleFSMMgr.CurrRoleCtrl.OnRoleDie != null)
            //{
            //    CurrRoleFSMMgr.CurrRoleCtrl.OnRoleDie(CurrRoleFSMMgr.CurrRoleCtrl);
            //}
        }
       
    }

    /// <summary>
    /// 实现基类 离开状态
    /// </summary>
    public override void OnLeave()
    {
        base.OnLeave();
        CurrRoleFSMMgr.CurrRoleCtrl.Animator.SetBool(ToAnimatorCondition.ToDie.ToString(), false);
    }
}