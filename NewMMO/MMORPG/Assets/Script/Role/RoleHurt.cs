using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleHurt
{
    RoleFSMMgr m_CurrRoleFSMMgr = null;
    // ����ɫ���˵�ʱ��
    public RoleHurt(RoleFSMMgr mgr)
    {
        m_CurrRoleFSMMgr = mgr;
    }

    //public void ToHurt(int atackValue)
    //{
    //    if (m_CurrRoleFSMMgr == null) return;
    //    m_CurrRoleFSMMgr.ChangeState(RoleState.Hurt);
    //}

    //public void ToHurt(RoleTransferAttackInfo roleTransferAttackInfo)
    //{
    //    if (m_CurrRoleFSMMgr == null) return;
    //    m_CurrRoleFSMMgr.ChangeState(RoleState.Hurt);
    //}

    public IEnumerator ToHurt(RoleTransferAttackInfo roleTransferAttackInfo)
    {
        if (m_CurrRoleFSMMgr == null) yield break;
        // ֱ�ӷ��أ�
        if (m_CurrRoleFSMMgr.CurrRoleStateEnum == RoleState.Die) yield break;

        SkillEntity skillEntity = SkillDBModel.Instance.Get(roleTransferAttackInfo.SkillId);
        SkillLevelEntity skillLevelEntity = SkillLevelDBModel.Instance.Get(roleTransferAttackInfo.SKillLevel);
        if (skillEntity == null || skillLevelEntity == null) yield break; 

        // �ӳ�ʱ��
        yield return new WaitForSeconds(skillEntity.ShowHurtEffectDelaySecond);

        // 1 ��Ѫ
        m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP -= roleTransferAttackInfo.HurtValue;
        // 1.1 �������

        if (m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP<=0) {
            Debug.LogError("��ɫ������"+ roleTransferAttackInfo.BeAttackRoleId);
            m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP = 0; 
            m_CurrRoleFSMMgr.CurrRoleCtrl.ToDie();
            yield break;
        }
        Debug.LogError("��ɫ���ˣ�" + roleTransferAttackInfo.BeAttackRoleId+ "  " + m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP);


        // 2 ����������Ч
        // 3 �����������֡����б����� ��ʾ��������
        // 4 ��Ļ����
        // 5 
        m_CurrRoleFSMMgr.ChangeState(RoleState.Hurt);



    }

}
