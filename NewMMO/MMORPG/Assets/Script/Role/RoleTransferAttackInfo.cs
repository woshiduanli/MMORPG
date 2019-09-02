using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// 角色传递的攻击信息
public class RoleTransferAttackInfo
{
    public int AttackRoleId;
    public int BeAttackRoleId;

    public Vector3 AttackRolePos;

    // 上海数值
    public int HurtValue;

    // 攻击者使用的技能Id
    public int SkillId;


    public int SKillLevel;

    // 是否异常状态
    public bool IsAbNormal;

    // 是否暴击
    public bool isCri;

}
