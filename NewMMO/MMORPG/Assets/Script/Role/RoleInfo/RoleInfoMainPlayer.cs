
using UnityEngine;
using System.Collections;

/// <summary>
/// 主角信息
/// </summary>
public class RoleInfoMainPlayer : RoleInfoBase
{


    public byte JobId; //职业编号

    public int Money; //元宝
    public int Gold; //金币


    public RoleInfoMainPlayer()
    {

    }
    public RoleInfoMainPlayer(RoleOperation_SelectRoleInfoReturnProto roleInfoProto)
    {
        this.RoldId = roleInfoProto.RoldId; //角色编号               
        this.RoleNickName = roleInfoProto.RoleNickName; //角色昵称
        this.JobId = roleInfoProto.JobId; //职业编号
        //this.Level = proto.Level; //等级
        //this.TotalRechargeMoney = proto.TotalRechargeMoney; //总充值金额
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
    }
}
