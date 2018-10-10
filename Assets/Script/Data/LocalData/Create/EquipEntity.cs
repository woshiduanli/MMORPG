
//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-09-19 22:05:06
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using UnityEngine;
using System.Collections;

/// <summary>
/// Equip实体
/// </summary>
public partial class EquipEntity : AbstractEntity
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 角色等级要求
    /// </summary>
    public int Level { get; set; }

    /// <summary>
    /// 装备品阶类型
    /// </summary>
    public int Quality { get; set; }

    /// <summary>
    /// 装备种类
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 售价
    /// </summary>
    public int SellMoney { get; set; }

    /// <summary>
    /// 攻击
    /// </summary>
    public int Attack { get; set; }

    /// <summary>
    /// 防御
    /// </summary>
    public int Defense { get; set; }

    /// <summary>
    /// 命中
    /// </summary>
    public int Hit { get; set; }

    /// <summary>
    /// 闪避
    /// </summary>
    public int Dodge { get; set; }

    /// <summary>
    /// 暴击
    /// </summary>
    public int Cri { get; set; }

    /// <summary>
    /// 抗性
    /// </summary>
    public int Res { get; set; }

    /// <summary>
    /// HP增加值
    /// </summary>
    public int HP { get; set; }

    /// <summary>
    /// MP增加值
    /// </summary>
    public int MP { get; set; }

    /// <summary>
    /// 装备的最大孔数
    /// </summary>
    public int maxHole { get; set; }

    /// <summary>
    /// 各个孔可以镶嵌的材料的子类型的总范围
    /// </summary>
    public string embedProps { get; set; }

    /// <summary>
    /// 强化时所用的材料的ID
    /// </summary>
    public int StrengthenItem { get; set; }

    /// <summary>
    /// 强化等级上限
    /// </summary>
    public int StrengthenLvMax { get; set; }

    /// <summary>
    /// 强化增加能力
    /// </summary>
    public int StrengthenAblity { get; set; }

    /// <summary>
    /// 每级强化的能力值系数
    /// </summary>
    public string StrengthenValue { get; set; }

    /// <summary>
    /// 强化需要材料个数系数
    /// </summary>
    public string StrengthenItemNumber { get; set; }

    /// <summary>
    /// 强化需要金币系数
    /// </summary>
    public string StrengthenGold { get; set; }

    /// <summary>
    /// 强化成功率系数
    /// </summary>
    public string StrengthenRatio { get; set; }

}
