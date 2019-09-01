
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 主角信息
/// </summary>
public class RoleInfoMainPlayer : RoleInfoBase
{


    public byte JobId; //职业编号

    public int Money; //元宝
    public int Gold; //金币
    public int Level;
    public int CurrExp = 8989;

    /// <summary>
    ///  角色技能列表
    /// </summary>
    public List<RoleInfoSkill> SkillList; 

    public int LastInWorldMapId;
    public int TotalRechargeMoney;

    public RoleInfoMainPlayer()
    {
        SkillList = new List<RoleInfoSkill>(); 
    }
    public RoleInfoMainPlayer(RoleOperation_SelectRoleInfoReturnProto roleInfoProto)
    {
        this.RoldId = roleInfoProto.RoldId; //角色编号               
        this.RoleNickName = roleInfoProto.RoleNickName; //角色昵称
        this.JobId = roleInfoProto.JobId; //职业编号
        this.Level = roleInfoProto.Level; //等级
        this.TotalRechargeMoney = roleInfoProto.TotalRechargeMoney; //总充值金额
        this.Money = roleInfoProto.Money; //元宝
        this.Gold = roleInfoProto.Gold; //金币
        this.Exp = roleInfoProto.Exp; //经验
        this.MaxHP = roleInfoProto.MaxHP; //最大HP
        this.MaxMP = roleInfoProto.MaxHP; //最大MP
        this.CurrHP = roleInfoProto.CurrHP; //当前HP
        this.CurrMP = roleInfoProto.MaxMP; //当前MP
        this.Attack = roleInfoProto.Attack; //攻击力
        this.Defense = roleInfoProto.Defense; //防御
        this.Hit = roleInfoProto.Hit; //命中
        this.Dodge = roleInfoProto.Dodge; //闪避
        this.Cri = roleInfoProto.Cri;
        this.Res = roleInfoProto.Res;
        this.Fighting = roleInfoProto.Fighting;
        this.LastInWorldMapId = roleInfoProto.LastInWorldMapId;

        SkillList = new List<RoleInfoSkill>();

    }

    /// <summary>
    ///  加载主角学会的技能
    /// </summary>
    /// <param name="proto"></param>
 public   void LoadSkill(RoleData_SkillReturnProto proto)
    {
        SkillList.Clear(); 
        for (int i = 0; i < proto.CurrSkillDataList.Count; i++)
        {
            SkillList.Add( new RoleInfoSkill() { SkillId =  proto.CurrSkillDataList[i].SkillId,

                SkillLevel= proto.CurrSkillDataList[i].SkillLevel,
                SlotsNo = proto.CurrSkillDataList[i].SlotsNo
            }
                ); 
        }
    }
}
