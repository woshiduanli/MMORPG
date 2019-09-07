using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleHurt
{
    RoleFSMMgr m_CurrRoleFSMMgr = null;
    public System.Action OnRoleHurt;
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

        Debug.LogError("juese shoushang1 ��" + roleTransferAttackInfo.BeAttackRoleId + "  " + m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP+ "  "+ roleTransferAttackInfo.HurtValue);
        // 1 ��Ѫ ____ ���ǲ�����ʵֵ
        //m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP -= roleTransferAttackInfo.HurtValue;
        // ___����ֵĬ�ϣ� ��Ѫ20 
        m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP -= 5;
        int fontSize = 1;
        Color c = Color.red;
        if (roleTransferAttackInfo.isCri)
        {
            fontSize = 8;
            c = Color.yellow; 
        }
        // ��Ѫ��Ʈѩ��������
        UISceneCtrl.Instance.CurrentUIScene.HudText.NewText("- 5", m_CurrRoleFSMMgr.CurrRoleCtrl.gameObject.transform, c, fontSize, 20,-1,2.2f,   Random.Range(0,2)==1?bl_Guidance.RightDown: bl_Guidance.LeftDown);
    

        //m_CurrRoleFSMMgr.CurrRoleCtrl.bar

        // ͷ��Ѫ���ı仯
        if (OnRoleHurt != null) OnRoleHurt();
        // 1.1 �������

        if (m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP <= 0)
        {
            Debug.LogError("die ��" + roleTransferAttackInfo.BeAttackRoleId);
            m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP = 0;
            m_CurrRoleFSMMgr.CurrRoleCtrl.ToDie();
            yield break;
        }

        //Debug.LogError("juese shoushang 2��" + roleTransferAttackInfo.BeAttackRoleId + "  " + m_CurrRoleFSMMgr.CurrRoleCtrl.CurrRoleInfo.CurrHP);
        // 2 ����������Ч
        Transform trans = EffectMgr.Instance.PlayEffect("Effect_Hurt");
        trans.position = m_CurrRoleFSMMgr.CurrRoleCtrl.gameObject.transform.position;
        trans.rotation = m_CurrRoleFSMMgr.CurrRoleCtrl.gameObject.transform.rotation;
        EffectMgr.Instance.DestroyEffect(trans, 2);

        // 3 �����������֡����б����� ��ʾ��������
        // 4 ��Ļ����
        // 5 
        m_CurrRoleFSMMgr.ChangeState(RoleState.Hurt);



    }

}
