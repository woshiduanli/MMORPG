using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RoleAttack
{


    RoleFSMMgr m_CurrRoleFSMMgr = null;
    RoleCtrl m_CurRoleCtrl;
    List<RoleCtrl> m_EnenmyList;
    List<Collider> m_SerachList;
    // 当角色受伤的时候，
    public void SetFSM(RoleFSMMgr mgr)
    {
        m_CurrRoleFSMMgr = mgr;
        m_CurRoleCtrl = m_CurrRoleFSMMgr.CurrRoleCtrl;
        m_EnenmyList = new List<RoleCtrl>();
        m_SerachList = new List<Collider>();
    }

    // 攻击信息，是策划的配置表 相关的数据 
    public List<RoleAttackInfo> PhyAttackInfoList;

    public List<RoleAttackInfo> SkillAttackInfoList;

    private RoleStateAttack m_RoleStateAttack;

    // 根据索引号获取攻击信息
    private RoleAttackInfo GetRoleAttackInfoByIndex(RoleAttackType type, int index)
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

    private RoleAttackInfo GetRoleAttackInfoBySkillID(int SkillId)
    {

        for (int i = 0; i < PhyAttackInfoList.Count; i++)
        {
            if (PhyAttackInfoList[i].SkillId == SkillId)
            {
                return PhyAttackInfoList[i];
            }
        }

        for (int i = 0; i < SkillAttackInfoList.Count; i++)
        {
            if (SkillAttackInfoList[i].SkillId == SkillId)
            {
                return SkillAttackInfoList[i];
            }
        }
        return null;
    }

    public void ToAttackByIndex(RoleAttackType type, int index)
    {
        if (m_CurrRoleFSMMgr == null || m_CurrRoleFSMMgr.CurrRoleCtrl.IsRigidity) return;
        // 攻击和普通的行动不一样，首先采集攻击信息， 然后根据攻击信息， 分别进行状态机的转换， 特效的作用
        RoleAttackInfo info = GetRoleAttackInfoByIndex(type, index);
        if (info == null) return;
        if (info != null)
        {
            // 在编辑器窗口中绘制，巡逻范围等
            m_CurrRoleFSMMgr.CurrRoleCtrl.roleAttackInfo = info;
            m_CurrRoleFSMMgr.CurrRoleCtrl.CurrAttackRange = info.AttackRange;
            if (info.EffectObject != null)
            {
                GameObject obj = GameObject.Instantiate(info.EffectObject);
                // 特效的位置和转向和主角的位置和转向要一个方向
                obj.transform.position = m_CurrRoleFSMMgr.CurrRoleCtrl.transform.position;
                obj.transform.rotation = m_CurrRoleFSMMgr.CurrRoleCtrl.transform.rotation;
                Object.Destroy(obj, info.EffectLifeTime);
            }
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

    public bool ToAttack(RoleAttackType type = RoleAttackType.PhyAttack, int SkillID = 0, int SkillLevel = 0)
    {
        if (m_CurrRoleFSMMgr == null || m_CurrRoleFSMMgr.CurrRoleCtrl.IsRigidity) return false;
        // 攻击和普通的行动不一样，首先采集攻击信息， 然后根据攻击信息， 分别进行状态机的转换， 特效的作用
        RoleAttackInfo info = GetRoleAttackInfoBySkillID(SkillID);
        if (info == null) return false;
        //DEBUG_ROLESTATE
#if DEBUG_ROLESTATE


        //if (info != null)
        //{
        //    // 在编辑器窗口中绘制，巡逻范围等
        //    m_CurrRoleFSMMgr.CurrRoleCtrl.roleAttackInfo = info;
        //    m_CurrRoleFSMMgr.CurrRoleCtrl.CurrAttackRange = info.AttackRange;
        //    if (info.EffectObject != null)
        //    {
        //        GameObject obj = GameObject.Instantiate(info.EffectObject);
        //        // 特效的位置和转向和主角的位置和转向要一个方向
        //        obj.transform.position = m_CurrRoleFSMMgr.CurrRoleCtrl.transform.position;
        //        obj.transform.rotation = m_CurrRoleFSMMgr.CurrRoleCtrl.transform.rotation;
        //        Object.Destroy(obj, info.EffectLifeTime);
        //    }
        //}
#else
       // 



#endif

        // 1 只要主角和怪才能参与技能数值计算
        if (m_CurRoleCtrl.CurrRoleType == RoleType.MainPlayer || m_CurRoleCtrl.CurrRoleType == RoleType.Monster)
        {
            // 2 获取技能信息
            SkillEntity skillEntity = SkillDBModel.Instance.Get(SkillID);
            if (skillEntity == null) return false;

            // 3 验证主角的mp 
            SkillLevelEntity skillLevelEntity = SkillLevelDBModel.Instance.GetEntityBySkillIdAndLevel(SkillID, m_CurRoleCtrl.CurrRoleInfo.GetSkillLevel(SkillID));
            if (m_CurRoleCtrl.CurrRoleType == RoleType.MainPlayer)
            {
                // 如果 mp 不足， 不能使用释放技能
                if (skillLevelEntity.SpendMP > m_CurRoleCtrl.CurrRoleInfo.CurrMP)
                {
                    return false;
                }
            }
            m_EnenmyList.Clear();

            // 伤害目标数量
            int attackTargetCount = skillEntity.AttackTargetCount;
            // 4 开始找敌人

            // 此时表示没有锁定敌人， 此时单体攻击
            if (attackTargetCount == 1)
            {
                #region 单体攻击
                if (m_CurRoleCtrl.LockEnemy != null)
                {
                    // 此时表示有锁定敌人
                    m_EnenmyList.Add(m_CurRoleCtrl.LockEnemy);
                }
                else
                {
                    // 此时要找怪，通过发射圆形射线， 找到最近的怪
                    Collider[] serachList = Physics.OverlapSphere(m_CurRoleCtrl.gameObject.transform.position, skillEntity.AreaAttackRadius, 1 << LayerMask.NameToLayer("Role"));
                    if (serachList != null && serachList.Length > 0)
                        m_SerachList = new List<Collider>(serachList);
                    if (m_SerachList.Count > 0)
                    {
                        m_SerachList.Sort((c1, c2) =>
                        {
                            int ret = 0;
                            if (Vector3.Distance(c1.gameObject.transform.position, m_CurRoleCtrl.gameObject.transform.position) <
                              Vector3.Distance(c2.gameObject.transform.position, m_CurRoleCtrl.gameObject.transform.position))
                            {
                                ret = -1;
                            }
                            else
                            {
                                ret = 1;
                            }
                            return ret;
                        });
                        m_CurRoleCtrl.LockEnemy = m_SerachList[0].GetComponent<RoleCtrl>();
                        m_EnenmyList.Add(m_CurRoleCtrl.LockEnemy);
                    }
                }
                #endregion
            }
            else
            {
                #region 群体攻击

                int needAttackCount = attackTargetCount;
                //// 此时要找怪，通过发射圆形射线， 找到最近的怪
                Collider[] serachList = Physics.OverlapSphere(m_CurRoleCtrl.gameObject.transform.position, skillEntity.AreaAttackRadius, 1 << LayerMask.NameToLayer("Role"));
                if (serachList != null && serachList.Length > 0)
                    m_SerachList = new List<Collider>(serachList);
                if (m_SerachList.Count > 0)
                {
                    m_SerachList.Sort((c1, c2) =>
                    {
                        int ret = 0;
                        if (Vector3.Distance(c1.gameObject.transform.position, m_CurRoleCtrl.gameObject.transform.position) <
                          Vector3.Distance(c2.gameObject.transform.position, m_CurRoleCtrl.gameObject.transform.position))
                        {
                            ret = -1;
                        }
                        else
                        {
                            ret = 1;
                        }
                        return ret;
                    });
                }

                // 群攻也是看是否有锁定敌人
                if (m_CurRoleCtrl.LockEnemy != null)
                {
                    // 此时表示有锁定敌人
                    m_EnenmyList.Add(m_CurRoleCtrl.LockEnemy);
                    needAttackCount--;

                    if (m_SerachList.Count > 0)
                    {
                        for (int i = 0; i < m_SerachList.Count; i++)
                        {
                            RoleCtrl ctrl = m_SerachList[i].GetComponent<RoleCtrl>();
                            if (ctrl.CurrRoleInfo.RoldId != m_CurRoleCtrl.LockEnemy.CurrRoleInfo.RoldId)
                            {
                                if ((i + 1 > needAttackCount)) break;
                                m_EnenmyList.Add(ctrl);
                            }
                            // 加入
                        }

                    }
                }
                else
                {

                    if (m_SerachList.Count > 0)
                    {
                        m_CurRoleCtrl.LockEnemy = m_SerachList[0].GetComponent<RoleCtrl>();

                        for (int i = 0; i < m_SerachList.Count; i++)
                        {
                            RoleCtrl ctrl = m_SerachList[i].GetComponent<RoleCtrl>();
                            if ((i + 1 > needAttackCount)) break;
                            m_EnenmyList.Add(ctrl);
                        }
                    }
                }

                #endregion
            }

            // 5 让敌人受伤
            for (int i = 0; i < m_EnenmyList.Count; i++)
            {
                RoleTransferAttackInfo roleTransferAttackInfo = CalCulateHurtValue(m_EnenmyList[i], skillLevelEntity);
                m_EnenmyList[i].ToHurt(roleTransferAttackInfo);
            }
        }

        #region       动画特效相关
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
        m_RoleStateAttack.AnimatorConditionValue = info.Index;
        m_RoleStateAttack.AnimatorCurState = GameUtil.GetRoleAnimatorState(type, info.Index);
        // 切换成攻击状态
        m_CurrRoleFSMMgr.ChangeState(RoleState.Attack);
        #endregion


        return true;
    }

    /// <summary>
    ///  计算攻击信息
    /// </summary>
    /// <param name="enemy"></param>
    /// <param name="skillLevel"></param>
    /// <returns></returns>
    public RoleTransferAttackInfo CalCulateHurtValue(RoleCtrl enemy, SkillLevelEntity skillLevelEntity)
    {
        if (enemy == null || skillLevelEntity == null)
        {
            return null;
        }

        SkillEntity skillEntity = SkillDBModel.Instance.Get(skillLevelEntity.SkillId);

        if (skillEntity == null)
            return null;

        RoleTransferAttackInfo roleTransferAttackInfo = new RoleTransferAttackInfo();

        roleTransferAttackInfo.AttackRoleId = m_CurRoleCtrl.CurrRoleInfo.RoldId;
        roleTransferAttackInfo.AttackRolePos = m_CurRoleCtrl.gameObject.transform.position;
        roleTransferAttackInfo.BeAttackRoleId = enemy.CurrRoleInfo.RoldId;
        roleTransferAttackInfo.SkillId = skillLevelEntity.SkillId;
        roleTransferAttackInfo.SKillLevel = skillLevelEntity.Level;
        roleTransferAttackInfo.IsAbNormal = skillLevelEntity.AbnormalRatio == 1;
        // 计算伤害
        // 1 攻击数值， = 攻击方的综合战斗力* (技能伤害倍率*0.01f) 计算攻击数值的作用是为了计算基础伤害
        float attackValue = m_CurRoleCtrl.CurrRoleInfo.Fighting * (skillLevelEntity.HurtValueRate * 0.01f);

        // 2 基础伤害 =  攻击数值 * 攻击数值/(攻击数值+被攻击者的防御)
        float baseHurt = attackValue * attackValue / (attackValue + enemy.CurrRoleInfo.Defense);


        // 3 暴击概率  0.05f  + （攻击方的暴击/(攻击方的暴击+ 防御方的抗性)）* 0.1f ;
        float cri = 0.05f + m_CurRoleCtrl.CurrRoleInfo.Cri / (m_CurRoleCtrl.CurrRoleInfo.Cri + enemy.CurrRoleInfo.Res) * 0.1f;

        //
        if (cri > 0.5f) cri = 0.5f;

        // 4 是否暴击 0-1的随机数 < 暴击概率

        bool isCri = Random.Range(0, 1f) <= cri;

        // 5 暴击的伤害倍率 有暴击?1.5f:1f
        float criHurt = isCri ? 1.5f : 1f;

        // 6 随机数 = 0.9f 到1.1 之间
        float random = Random.Range(0.9f, 1.1f);

        // 7 最终伤害 = 基础伤害*暴击伤害率 * 随机数
        float hurtValue = Mathf.RoundToInt(baseHurt * criHurt * random);
        if (hurtValue < 1) hurtValue = 1;


        roleTransferAttackInfo.isCri = isCri;
        roleTransferAttackInfo.HurtValue = (int)hurtValue;

        return roleTransferAttackInfo;
    }
}
