
using UnityEngine;
using System.Collections;

#region SceneType 场景类型
/// <summary>
/// 场景类型
/// </summary>
public enum SceneType
{
    LogOn,
    WorldMap,
    ShaMo,
    SelectRole,
    GameLevel

}
#endregion

#region WindowUIType 窗口类型
/// <summary>
/// 窗口类型
/// </summary>
public enum WindowUIType
{
    /// <summary>
    /// 未设置
    /// </summary>
    None,
    /// <summary>
    /// 登录窗口
    /// </summary>
    LogOn,
    /// <summary>
    /// 注册窗口
    /// </summary>
    Reg,
    /// <summary>
    /// 角色信息窗口
    /// </summary>
    RoleInfo,
    GameLevelMap,
    GameLevelDetail,

    GameServerEnter,
    GameServerSelect,
    WorldMap
}
#endregion

#region WindowUIContainerType UI容器类型
/// <summary>
/// UI容器类型
/// </summary>
public enum WindowUIContainerType
{
    /// <summary>
    /// 左上
    /// </summary>
    TopLeft,
    /// <summary>
    /// 右上
    /// </summary>
    TopRight,
    /// <summary>
    /// 左下
    /// </summary>
    BottomLeft,
    /// <summary>
    /// 右下
    /// </summary>
    BottomRight,
    /// <summary>
    /// 居中
    /// </summary>
    Center
}
#endregion

#region WindowShowStyle 窗口打开方式
/// <summary>
/// 窗口打开方式
/// </summary>
public enum WindowShowStyle
{
    /// <summary>
    /// 正常打开
    /// </summary>
    Normal,
    /// <summary>
    /// 从中间放大
    /// </summary>
    CenterToBig,
    /// <summary>
    /// 从上往下
    /// </summary>
    FromTop,
    /// <summary>
    /// 从下往上
    /// </summary>
    FromDown,
    /// <summary>
    /// 从左向右
    /// </summary>
    FromLeft,
    /// <summary>
    /// 从右向左
    /// </summary>
    FromRight
}
#endregion

#region RoleType 角色类型
/// <summary>
/// 角色类型
/// </summary>
public enum RoleType
{
    /// <summary>
    /// 未设置
    /// </summary>
    None = 0,
    /// <summary>
    /// 当前玩家
    /// </summary>
    MainPlayer = 1,
    /// <summary>
    /// 怪
    /// </summary>
    Monster = 2,
    OtherPlayer = 3


}
#endregion

/// <summary>
/// 角色状态
/// </summary>
public enum RoleState
{
    /// <summary>
    /// 未设置
    /// </summary>
    None = 0,
    /// <summary>
    /// 待机
    /// </summary>
    Idle = 1,
    /// <summary>
    /// 跑了
    /// </summary>
    Run = 2,
    /// <summary>
    /// 攻击
    /// </summary>
    Attack = 3,
    /// <summary>
    /// 受伤
    /// </summary>
    Hurt = 4,
    /// <summary>
    /// 死亡
    /// </summary>
    Die = 5,
    Select = 6
}

public enum RoleAttackType
{
   PhyAttack,
   SkillAttack
}

public enum RoleIdleState
{
    IdelNormal,
    IdelFight
}
/// <summary>
/// 角色动画状态名称
/// </summary>
public enum RoleAnimatorState
{
    Idle_Normal = 1,
    Idle_Fight = 2,
    Run = 3,
    Hurt = 4,
    Die = 5,


    Select = 6,
    XiuXian = 7,

    PhyAttack1 = 11,
    PhyAttack2 = 12,
    PhyAttack3 = 13,


    Skill1 = 14,
    Skill2 = 15,
    Skill3 = 16,
    Skill4 = 17,
    Skill5 = 18,
    Skill6 = 19

}

public enum ToAnimatorCondition
{
    ToIdleNormal,
    ToIdleFight,
    ToRun,
    ToHurt,
    ToDie,
    ToPhyAttack,
    ToXiuXian,
    ToSelect,
    CurrState
}


public enum ValueChangeType
{
   Add = 0,
   SubTrack = 1
}


public enum GameLevelGrade
{
    Normal = 0,
    Hard = 1,
    Hell = 2
}

public enum GoodsType
{
    Equip,
    Item,
    Material
}