using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoleInfoSkill  {

    public int SkillId; //技能编号
    public int SkillLevel; //技能等级
    public byte SlotsNo; //技能槽编号

    private float skillCDTime; // 技能冷却时间
    private int spendMP;// 消耗的mp 
    public float SkillCDendTime; // 冷却结束时间

    // 冷却时间
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

    // 消耗的
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
