using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleInfoSkill  {

    public int SkillId; //���ܱ��
    public int SkillLevel; //���ܵȼ�
    public byte SlotsNo; //���ܲ۱��

    private float skillCDTime; // ������ȴʱ��
    private int spendMP;// ���ĵ�mp 
    public float SkillCDendTime; // ��ȴ����ʱ��

    // ��ȴʱ��
    public float SkillCDTime
    {
        get
        {
            if (skillCDTime == 0)
            {
                skillCDTime = SkillLevelDBModel.Instance.GetEntityBySkillIdAndLevel(SkillId, SkillLevel).SkillCDTime; 

            }
            return skillCDTime;
        }

        set
        {
            skillCDTime = value;
        }
    }

    // ���ĵ�
    public int SpendMP
    {
        get
        {
            if (skillCDTime == 0)
            {
                skillCDTime = SkillLevelDBModel.Instance.GetEntityBySkillIdAndLevel(SkillId, SkillLevel).SpendMP;

            }
            return spendMP;
        }

        set
        {
            spendMP = value;
        }
    }
}
