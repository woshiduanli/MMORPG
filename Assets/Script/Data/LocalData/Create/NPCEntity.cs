

using UnityEngine;
using System.Collections;

/// <summary>
/// NPC实体
/// </summary>
public partial class NPCEntity : AbstractEntity
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 预设名称
    /// </summary>
    public string PrefabName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    public string HeadPic { get; set; }

    /// <summary>
    /// 半身像
    /// </summary>
    public string HalfBodyPic { get; set; }

    /// <summary>
    /// 自言自语
    /// </summary>
    public string Talk { get; set; }

}
