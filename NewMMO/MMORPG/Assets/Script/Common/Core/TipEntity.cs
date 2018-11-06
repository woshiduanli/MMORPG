using UnityEngine;
using System.Collections;

public class TipEntity
{
    /// <summary>
    /// 类型 -1=物品 0=经验 1=金币
    /// </summary>
    public int Type;

    /// <summary>
    /// 提示文字 如果是物品 则不用传递
    /// </summary>
    public string Text;

    /// <summary>
    /// 物品类型
    /// </summary>
    public GoodsType GoodsType;

    /// <summary>
    /// 物品编号
    /// </summary>
    public int GoodsId;

    /// <summary>
    /// 物品名称
    /// </summary>
    public string GoodsName;

    /// <summary>
    /// 物品数量
    /// </summary>
    public int GoodsCount;
}
//public class GoodsType { }

