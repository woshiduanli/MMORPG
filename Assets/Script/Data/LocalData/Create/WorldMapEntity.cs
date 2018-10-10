
//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-09-19 22:05:16
//备    注：此代码为工具生成 请勿手工修改
//===================================================
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
