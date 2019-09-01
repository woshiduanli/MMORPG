using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RoleAttack
{


    RoleFSMMgr m_CurrRoleFSMMgr = null;
    // ����ɫ���˵�ʱ��
    public void SetFSM(RoleFSMMgr mgr)
    {
        m_CurrRoleFSMMgr = mgr;
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

    private RoleAttackInfo GetRoleAttackInfoBySkillID( int SkillId)
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

    public bool ToAttack(RoleAttackType type = RoleAttackType.PhyAttack,  int SkillID = 0)
    {
        if (m_CurrRoleFSMMgr == null || m_CurrRoleFSMMgr.CurrRoleCtrl.IsRigidity) return false;
        // ��������ͨ���ж���һ�������Ȳɼ�������Ϣ�� Ȼ����ݹ�����Ϣ�� �ֱ����״̬����ת���� ��Ч������
        RoleAttackInfo info = GetRoleAttackInfoBySkillID( SkillID);
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
        return true;
    }
}
