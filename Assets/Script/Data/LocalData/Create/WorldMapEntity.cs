

using UnityEngine;
using System.Collections;

/// <summary>
/// WorldMap实体
/// </summary>
public partial class WorldMapEntity : AbstractEntity
{
    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 场景名称
    /// </summary>
    public string SceneName { get; set; }

    /// <summary>
    /// NPC列表
    /// </summary>
    public string NPCList { get; set; }

    /// <summary>
    /// 主角出生点坐标
    /// </summary>
    public string RoleBirthPos { get; set; }

    /// <summary>
    /// 摄像机旋转角度
    /// </summary>
    public string CameraRotation { get; set; }

}
