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
    // ����ɫ���˵�ʱ��
    public void SetFSM(RoleFSMMgr mgr)
    {
        m_CurrRoleFSMMgr = mgr;
        m_CurRoleCtrl = m_CurrRoleFSMMgr.CurrRoleCtrl;
        m_EnenmyList = new List<RoleCtrl>();
        m_SerachList = new List<Collider>();
    }

    // ������Ϣ���ǲ߻������ñ� ��ص����� 
    public List<RoleAttackInfo> PhyAttackInfoList;

    public List<RoleAttackInfo> SkillAttackInfoList;

    private RoleStateAttack m_RoleStateAttack;

    // ���������Ż�ȡ������Ϣ
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
        // ��������ͨ���ж���һ�������Ȳɼ�������Ϣ�� Ȼ����ݹ�����Ϣ�� �ֱ����״̬����ת���� ��Ч������
        RoleAttackInfo info = GetRoleAttackInfoByIndex(type, index);
        if (info == null) return;
        if (info != null)
        {
            // �ڱ༭�������л��ƣ�Ѳ�߷�Χ��
            m_CurrRoleFSMMgr.CurrRoleCtrl.roleAttackInfo = info;
            m_CurrRoleFSMMgr.CurrRoleCtrl.CurrAttackRange = info.AttackRange;
            if (info.EffectObject != null)
            {
                GameObject obj = GameObject.Instantiate(info.EffectObject);
                // ��Ч��λ�ú�ת������ǵ�λ�ú�ת��Ҫһ������
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
            // �õ���������Ķ���״̬���� Ȼ��ֵ�������״̬������ʱ�����������������Ǽ��ܹ�����  
            m_RoleStateAttack = (RoleStateAttack)m_CurrRoleFSMMgr.GetRoleState(RoleState.Attack);
        }

        m_RoleStateAttack.AnimatorCondition = string.Format(type == RoleAttackType.PhyAttack ? "ToPhyAttack" : "ToSkill");
        m_RoleStateAttack.AnimatorConditionValue = index;
        m_RoleStateAttack.AnimatorCurState = GameUtil.GetRoleAnimatorState(type, index);
        // �л��ɹ���״̬
        m_CurrRoleFSMMgr.ChangeState(RoleState.Attack);

    }

    public bool ToAttack(RoleAttackType type = RoleAttackType.PhyAttack, int SkillID = 0, int SkillLevel = 0)
    {
        if (m_CurrRoleFSMMgr == null || m_CurrRoleFSMMgr.CurrRoleCtrl.IsRigidity) return false;
        // ��������ͨ���ж���һ�������Ȳɼ�������Ϣ�� Ȼ����ݹ�����Ϣ�� �ֱ����״̬����ת���� ��Ч������
        RoleAttackInfo info = GetRoleAttackInfoBySkillID(SkillID);
        if (info == null) return false;
        //DEBUG_ROLESTATE
#if DEBUG_ROLESTATE


        //if (info != null)
        //{
        //    // �ڱ༭�������л��ƣ�Ѳ�߷�Χ��
        //    m_CurrRoleFSMMgr.CurrRoleCtrl.roleAttackInfo = info;
        //    m_CurrRoleFSMMgr.CurrRoleCtrl.CurrAttackRange = info.AttackRange;
        //    if (info.EffectObject != null)
        //    {
        //        GameObject obj = GameObject.Instantiate(info.EffectObject);
        //        // ��Ч��λ�ú�ת������ǵ�λ�ú�ת��Ҫһ������
        //        obj.transform.position = m_CurrRoleFSMMgr.CurrRoleCtrl.transform.position;
        //        obj.transform.rotation = m_CurrRoleFSMMgr.CurrRoleCtrl.transform.rotation;
        //        Object.Destroy(obj, info.EffectLifeTime);
        //    }
        //}
#else
       // 



#endif

        // 1 ֻҪ���Ǻ͹ֲ��ܲ��뼼����ֵ����
        if (m_CurRoleCtrl.CurrRoleType == RoleType.MainPlayer || m_CurRoleCtrl.CurrRoleType == RoleType.Monster)
        {
            // 2 ��ȡ������Ϣ
            SkillEntity skillEntity = SkillDBModel.Instance.Get(SkillID);
            if (skillEntity == null) return false;

            // 3 ��֤���ǵ�mp 
            SkillLevelEntity skillLevelEntity = SkillLevelDBModel.Instance.GetEntityBySkillIdAndLevel(SkillID, m_CurRoleCtrl.CurrRoleInfo.GetSkillLevel(SkillID));
            if (m_CurRoleCtrl.CurrRoleType == RoleType.MainPlayer)
            {
                // ��� mp ���㣬 ����ʹ���ͷż���
                if (skillLevelEntity.SpendMP > m_CurRoleCtrl.CurrRoleInfo.CurrMP)
                {
                    return false;
                }
            }
            m_EnenmyList.Clear();

            // �˺�Ŀ������
            int attackTargetCount = skillEntity.AttackTargetCount;
            // 4 ��ʼ�ҵ���

            // ��ʱ��ʾû���������ˣ� ��ʱ���幥��
            if (attackTargetCount == 1)
            {
                #region ���幥��
                if (m_CurRoleCtrl.LockEnemy != null)
                {
                    // ��ʱ��ʾ����������
                    m_EnenmyList.Add(m_CurRoleCtrl.LockEnemy);
                }
                else
                {
                    // ��ʱҪ�ҹ֣�ͨ������Բ�����ߣ� �ҵ�����Ĺ�
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
                #region Ⱥ�幥��

                int needAttackCount = attackTargetCount;
                //// ��ʱҪ�ҹ֣�ͨ������Բ�����ߣ� �ҵ�����Ĺ�
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

                // Ⱥ��Ҳ�ǿ��Ƿ�����������
                if (m_CurRoleCtrl.LockEnemy != null)
                {
                    // ��ʱ��ʾ����������
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
                            // ����
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

            // 5 �õ�������
            for (int i = 0; i < m_EnenmyList.Count; i++)
            {
                RoleTransferAttackInfo roleTransferAttackInfo = CalCulateHurtValue(m_EnenmyList[i], skillLevelEntity);
                m_EnenmyList[i].ToHurt(roleTransferAttackInfo);
            }
        }

        #region       ������Ч���
        // ab���м�����Ч if (info != null)
        if (info != null)
        {
            // �ڱ༭�������л��ƣ�Ѳ�߷�Χ��
            m_CurrRoleFSMMgr.CurrRoleCtrl.roleAttackInfo = info;
            m_CurrRoleFSMMgr.CurrRoleCtrl.CurrAttackRange = info.AttackRange;

            GameObject obj = EffectMgr.Instance.PlayEffect(info.EffectName).gameObject;
            // ��Ч��λ�ú�ת������ǵ�λ�ú�ת��Ҫһ������
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
            // �õ���������Ķ���״̬���� Ȼ��ֵ�������״̬������ʱ�����������������Ǽ��ܹ�����  
            m_RoleStateAttack = (RoleStateAttack)m_CurrRoleFSMMgr.GetRoleState(RoleState.Attack);
        }

        m_RoleStateAttack.AnimatorCondition = string.Format(type == RoleAttackType.PhyAttack ? "ToPhyAttack" : "ToSkill");
        m_RoleStateAttack.AnimatorConditionValue = info.Index;
        m_RoleStateAttack.AnimatorCurState = GameUtil.GetRoleAnimatorState(type, info.Index);
        // �л��ɹ���״̬
        m_CurrRoleFSMMgr.ChangeState(RoleState.Attack);
        #endregion


        return true;
    }

    /// <summary>
    ///  ���㹥����Ϣ
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
        // �����˺�
        // 1 ������ֵ�� = ���������ۺ�ս����* (�����˺�����*0.01f) ���㹥����ֵ��������Ϊ�˼�������˺�
        float attackValue = m_CurRoleCtrl.CurrRoleInfo.Fighting * (skillLevelEntity.HurtValueRate * 0.01f);

        // 2 �����˺� =  ������ֵ * ������ֵ/(������ֵ+�������ߵķ���)
        float baseHurt = attackValue * attackValue / (attackValue + enemy.CurrRoleInfo.Defense);


        // 3 ��������  0.05f  + ���������ı���/(�������ı���+ �������Ŀ���)��* 0.1f ;
        float cri = 0.05f + m_CurRoleCtrl.CurrRoleInfo.Cri / (m_CurRoleCtrl.CurrRoleInfo.Cri + enemy.CurrRoleInfo.Res) * 0.1f;

        //
        if (cri > 0.5f) cri = 0.5f;

        // 4 �Ƿ񱩻� 0-1������� < ��������

        bool isCri = Random.Range(0, 1f) <= cri;

        // 5 �������˺����� �б���?1.5f:1f
        float criHurt = isCri ? 1.5f : 1f;

        // 6 ����� = 0.9f ��1.1 ֮��
        float random = Random.Range(0.9f, 1.1f);

        // 7 �����˺� = �����˺�*�����˺��� * �����
        float hurtValue = Mathf.RoundToInt(baseHurt * criHurt * random);
        if (hurtValue < 1) hurtValue = 1;


        roleTransferAttackInfo.isCri = isCri;
        roleTransferAttackInfo.HurtValue = (int)hurtValue;

        return roleTransferAttackInfo;
    }
}
