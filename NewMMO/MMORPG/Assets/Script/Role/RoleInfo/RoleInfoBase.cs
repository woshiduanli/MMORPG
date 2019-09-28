
using UnityEngine;
using System.Collections;
using System.Collections.Generic; 

/// <summary>
/// 角色信息基类
/// </summary>
public class RoleInfoBase 
{
    public int RoldId; //角色编号
    public string RoleNickName; //角色昵称
    public int Exp; //经验
    public int MaxHP; //最大HP
    public int MaxMP; //最大MP
    public int CurrHP; //当前HP
    public int CurrMP; //当前MP
    public int Attack; //攻击力
    public int Defense; //防御
    public int Hit; //命中
    public int Dodge; //闪避
    public int Cri; //暴击
    public int Level; //暴击
    public int Res; //抗性
    public int Fighting; //综合战斗力

    public List<RoleInfoSkill> SkillList;
    public int[] PhySKillIds; 

    public RoleInfoBase()
    {
        SkillList = new List<RoleInfoSkill>();
    }

    public int  GetSkillLevel(int SKillId)
    {
        if (SkillList == null) return 1; 
        for (int i = 0; i < SkillList.Count; i++)
        {

            if (SkillList[i].SkillId == SKillId)
            {
                return SkillList[i].SkillLevel; 
            }
        }
        return 1;
    }

    public void   SetPhySkilId(string phySKillIds)
    {
        string[] ids = phySKillIds.Split(';');

        PhySKillIds = new int[ids.Length];


        for (int i = 0; i < ids.Length; i++)
        {
            PhySKillIds[i] = ids[i].ToInt();
        }
    }

    public void SetSkillCdEndTime(int skillId)
    {
        if (SkillList.Count > 0)
        {

            for (int i = 0; i < SkillList.Count; i++)
            {
                if (SkillList[i].SkillId == skillId)
                {
                    SkillList[i].SkillCDendTime = SkillList[i].SkillCDendTime + Time.time;
                    break;
                }
            }

        }
    }

    public int GetCanUseSkillId()
    {
        if (SkillList.Count > 0)
        {

            for (int i = 0; i < SkillList.Count; i++)
            {
                if (Time.time > SkillList[i].SkillCDendTime && CurrMP >= SkillList[i].SpendMP)
                {

                    return SkillList[i].SkillId;
                }
            }

        }
        return 0;
    }
}