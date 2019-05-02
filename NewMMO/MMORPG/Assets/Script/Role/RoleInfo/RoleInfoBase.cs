
using UnityEngine;
using System.Collections;

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
}