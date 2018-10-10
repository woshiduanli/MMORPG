
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Message数据管理
/// </summary>
public partial class MessageDBModel : AbstractDBModel<MessageDBModel, MessageEntity>
{
    /// <summary>
    /// 文件名称
    /// </summary>
    protected override string FileName { get { return "Message.data"; } }

    /// <summary>
    /// 创建实体
    /// </summary>
    /// <param name="parse"></param>
    /// <returns></returns>
    protected override MessageEntity MakeEntity(GameDataTableParser parse)
    {
        MessageEntity entity = new MessageEntity();
        entity.Id = parse.GetFieldValue("Id").ToInt();
        entity.Msg = parse.GetFieldValue("Msg");
        entity.Module = parse.GetFieldValue("Module");
        entity.Description = parse.GetFieldValue("Description");
        return entity;
    }
}
