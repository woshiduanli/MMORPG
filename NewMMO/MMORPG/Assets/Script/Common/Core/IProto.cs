
using UnityEngine;
using System.Collections;

/// <summary>
/// 协议接口
/// </summary>
public interface IProto
{
    //协议编号
    ushort ProtoCode { get; }
    IProto GetProto(byte[] buffer);
}


