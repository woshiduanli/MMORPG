//===================================================

using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 客户端发送登录区服消息
/// </summary>
public struct RoleOperation_LogOnGameServerProto : IProto
{
    public ushort ProtoCode { get { return 10001; } }

    public int AccountId; //账户ID

    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteInt(AccountId);
            return ms.ToArray();
        }
    }

    //public static RoleOperation_LogOnGameServerProto GetProto(byte[] buffer)
    //{
    //    RoleOperation_LogOnGameServerProto proto = new RoleOperation_LogOnGameServerProto();
    //    using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
    //    {
    //        proto.AccountId = ms.ReadInt();
    //    }
    //    return proto;
    //}

    public IProto GetProto(byte[] buffer)
    {
        RoleOperation_LogOnGameServerProto proto = new RoleOperation_LogOnGameServerProto();
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            proto.AccountId = ms.ReadInt();
        }
        return proto;
    }
}