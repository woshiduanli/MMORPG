
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 怪AI
/// </summary>
public class GameLevel_RoleMonsterAI : IRoleAI
{
    // 下次思考时间
    float m_NextThinkTime = 0;
    // 是否发呆
    bool m_IsDaze; 
    /// <summary>
    /// 当前角色控制器
    /// </summary>
    public RoleCtrl CurrRole
    {
        get;
        set;
    }
    RoleInfoMonster m_info;
    /// <summary>
    /// 下次巡逻时间
    /// </summary>
    private float m_NextPatrolTime = 0f;

    /// <summary>
    /// 下次攻击时间
    /// </summary>
    private float m_NextAttackTime = 0f;

    public GameLevel_RoleMonsterAI(RoleCtrl roleCtrl, RoleInfoMonster info)
    {
        m_info = info;
        CurrRole = roleCtrl;
    }
    public RoleAttackType m_RoleAttackType;
    public void DoAI()
    {
        if (GlobalInit.Instance == null || GlobalInit.Instance.CurrPlayer == null) return;
        if (CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Die || CurrRole.IsRigidity) return;

        if (CurrRole.LockEnemy == null)
        {
            //如果是待机状态
            if (CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Idle)
            {
                if (Time.time > m_NextPatrolTime)
                {
                    m_NextPatrolTime = Time.time + UnityEngine.Random.Range(5f, 10f);

                    //进行巡逻, 怪物为圆心， 巡逻范围为半径画圆，然后在圆上随机一个点出来
                    float rangePoint1 = UnityEngine.Random.Range(CurrRole.PatrolRange * -1, CurrRole.PatrolRange);
                    float rangePoint2 = UnityEngine.Random.Range(CurrRole.PatrolRange * -1, CurrRole.PatrolRange);
                    CurrRole.MoveTo(new Vector3(CurrRole.BornPoint.x + rangePoint1, CurrRole.BornPoint.y, CurrRole.BornPoint.z + rangePoint2));
                }
            }

            //如果主角在怪的视野范围内
            if (Vector3.Distance(CurrRole.transform.position, GlobalInit.Instance.CurrPlayer.transform.position) <= CurrRole.ViewRange)
            {
                CurrRole.LockEnemy = GlobalInit.Instance.CurrPlayer;

                // 下次攻击的时间
                m_NextAttackTime = Time.time + m_info.SpriteEntity.DelaySec_Attack;
            }
        }
        else
        {
            //----------------------------此时有锁定敌人-------------------------------
            // 锁定敌人已经死亡
            if (CurrRole.LockEnemy.CurrRoleInfo.CurrHP <= 0)
            {
                CurrRole.LockEnemy = null;
                return;
            }
            // 总结， 这里代码意思就是怪物， 每隔，3.几秒就会停下来  休息 1.几秒左右，
            // 当前的时间大于当前思考时间，怪物开始待机， 发呆休息
            if (Time.time > m_NextThinkTime + UnityEngine.Random.Range(3, 3.6f))
            {
                CurrRole.ToIdle(RoleIdleState.IdelFight);
                m_NextThinkTime = Time.time; // 让角色休息
                m_IsDaze = true;// 开始休息
            }

            if (m_IsDaze)
            {
                // 休息结束
                if (Time.time > m_NextThinkTime + UnityEngine.Random.Range(1, 1.5f))
                {
                    m_IsDaze = false;
                }
                else
                {
                    return; 
                }
            }


            // 但是当前怪物或许在跑步等， 那么要直接返回，就是怪物不是在空闲状态，那么就不去追击敌人 , 如果怪物在跑步， 那么怪物就会在跑步的过程中， 朝向主角，不真实,所以这里要返回
            if (CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum != RoleState.Idle)
                return;

            //如果有锁定敌人
            //1.如果我和锁定敌人的距离 超过了我的视野范围 则取消锁定
            if (Vector3.Distance(CurrRole.transform.position, GlobalInit.Instance.CurrPlayer.transform.position) > CurrRole.ViewRange)
            {
                CurrRole.LockEnemy = null;
                return;
            }
            int useSkillId = 0;
            // 此时有锁定敌人
            // 准备开始攻击，第一步，找到使用的技能id ，也许是随机出技能id, 测试配置随机概率, 物理攻击和技能攻击也是随机，  
            //Debug.LogError("测试--------");
            int random = UnityEngine.Random.Range(0, 100);
            if (m_info.SpriteEntity.PhysicalAttackRate >= random)
            {
                // 概率随机成功， 
                useSkillId = m_info.SpriteEntity.UsedPhyAttackArr[UnityEngine.Random.Range(0, m_info.SpriteEntity.UsedPhyAttackArr.Length)];
                m_RoleAttackType = RoleAttackType.PhyAttack;
            }
            else
            {
                useSkillId = m_info.SpriteEntity.UsedSkillListArr[UnityEngine.Random.Range(0, m_info.SpriteEntity.UsedSkillListArr.Length)];
                m_RoleAttackType = RoleAttackType.SkillAttack;
            }
            // 此时已经计算出用哪个技能id ; 
            SkillEntity entity = SkillDBModel.Instance.Get(useSkillId);
            if (entity == null) return;
            //CurrRole.AttackRange = entity.AttackRange; 


            // 判断角色是否在攻击范围内， 
            if (Vector3.Distance(CurrRole.transform.position, GlobalInit.Instance.CurrPlayer.transform.position) <= entity.AttackRange)
            {
                //2.如果在攻击范围首先怪物朝向主角，然后攻击 直接攻击
                CurrRole.transform.LookAt(new Vector3(CurrRole.LockEnemy.transform.position.x, CurrRole.transform.position.y, CurrRole.LockEnemy.transform.position.z));

                if (Time.time > m_NextAttackTime && CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum != RoleState.Attack)
                {
                    m_NextAttackTime = Time.time + UnityEngine.Random.Range(0f, 1f) + m_info.SpriteEntity.Attack_Interval;
                    CurrRole.ToAttackBySkilId(m_RoleAttackType, useSkillId);
                }
            }
            else
            {
                //3.如果在我的视野范围之内 进行追击
                if (CurrRole.CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Idle)
                {
                    // 移动到敌人，攻击范围内的随机点
                    CurrRole.MoveTo(OnPostRender(CurrRole.transform.position,     CurrRole.LockEnemy.transform.position, entity.AttackRange * UnityEngine.Random.Range(0.8f, 1)));
                }
            }
        }
    }

    // 返回 用center用圆心， radius为半径的圆上的随机点 
    public Vector3 OnPostRender(Vector3 curPos,  Vector3 enePos, float radius)
    {
        Vector3 v = (curPos - enePos).normalized;

        //Vector3 v = new Vector3(0, 0, 1);
        v = Quaternion.Euler(0, UnityEngine.Random.Range(-90, 90), 0) * v;
        Vector3 pos = v * radius;
        Vector3 newPos = enePos + pos;
        return newPos;
    }

}