
//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-09-19 22:05:05
//备    注：此代码为工具生成 请勿手工修改
//===================================================
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// GameLevel数据管理
/// </summary>
public partial class GameLevelDBModel : AbstractDBModel<GameLevelDBModel, GameLevelEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "GameLevel.data"; } }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected override GameLevelEntity MakeEntity(GameDataTableParser parse)
    {
        GameLevelEntity entity = new GameLevelEntity();
        entity.Id = parse.GetFieldValue("Id").ToInt();
        entity.ChapterID = parse.GetFieldValue("ChapterID").ToInt();
        entity.Name = parse.GetFieldValue("Name");
        entity.SceneName = parse.GetFieldValue("SceneName");
        entity.isBoss = parse.GetFieldValue("isBoss").ToInt();
        entity.Ico = parse.GetFieldValue("Ico");
        entity.PosInMap = parse.GetFieldValue("PosInMap");
        entity.DlgPic = parse.GetFieldValue("DlgPic");
        entity.CameraRotation = parse.GetFieldValue("CameraRotation");
        return entity;
    }
}
