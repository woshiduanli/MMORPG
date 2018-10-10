
//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-09-19 22:05:16
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// WorldMap数据管理
/// </summary>
public partial class WorldMapDBModel : AbstractDBModel<WorldMapDBModel, WorldMapEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "WorldMap.data"; } }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected override WorldMapEntity MakeEntity(GameDataTableParser parse)
    {
        WorldMapEntity entity = new WorldMapEntity();
        entity.Id = parse.GetFieldValue("Id").ToInt();
        entity.Name = parse.GetFieldValue("Name");
        entity.SceneName = parse.GetFieldValue("SceneName");
        entity.NPCList = parse.GetFieldValue("NPCList");
        entity.RoleBirthPos = parse.GetFieldValue("RoleBirthPos");
        entity.CameraRotation = parse.GetFieldValue("CameraRotation");
        return entity;
    }
}
