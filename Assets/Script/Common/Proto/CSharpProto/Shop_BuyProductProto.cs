using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 客户端发送购买商城物品消息
/// </summary>
public struct Shop_BuyProductProto : IProto
{
    public ushort ProtoCode { get { return 16001; } }

    public int ProductId; //商品编号

    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteInt(ProductId);
            return ms.ToArray();
        }
    }

  

    IProto IProto.GetProto(byte[] buffer)
    {
        Shop_BuyProductProto proto = new Shop_BuyProductProto();
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            proto.ProductId = ms.ReadInt();
        }
        return proto;
    }
}