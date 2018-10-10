using UnityEngine;
using System.Collections;
using System;
using XLua; 

[GCOptimize]
[LuaCallCSharp]
public struct TestProto : IProto
{
    public ushort ProtoCode { get { return 888; } }
    public int Id;
    public string Name;
    public int Type;
    public float price;


    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode); 
            ms.WriteInt(Id);
            ms.WriteUTF8String(Name);
            ms.WriteInt(Type);
            ms.WriteFloat(price);
            return ms.ToArray();
        }
    }


    //public static TestProto GetProto(byte[] buffer)
    //{
      
    //    TestProto pro = new TestProto();
    //    using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
    //    {
    //        pro.Id = ms.ReadInt();
    //        pro.Name = ms.ReadUTF8String();
    //        pro.Type = ms.ReadInt();
    //        pro.price = ms.ReadFloat();

    //        return pro;
    //    }
    //}

   

    public IProto GetProto(byte[] buffer) 
    {
        MyDebug.debug("¹ýÀ´ÁË-----------------------------------------");
        TestProto pro = new TestProto();
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            pro.Id = ms.ReadInt();
            pro.Name = ms.ReadUTF8String();
            pro.Type = ms.ReadInt();
            pro.price = ms.ReadFloat();

            return pro ;
        }
    }

    //T IProto<TestProto>.GetProto<T>(byte[] buffer)
    //{
    //    TestProto pro = new TestProto();
    //    using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
    //    {
    //        pro.Id = ms.ReadInt();
    //        pro.Name = ms.ReadUTF8String();
    //        pro.Type = ms.ReadInt();
    //        pro.price = ms.ReadFloat();

    //        return pro;
    //    }
    //}


    //public T GetProto(byte[] buffer) where T: IProto
    //{
    //    TestProto pro = new TestProto();
    //    using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
    //    {
    //        pro.Id = ms.ReadInt();
    //        pro.Name = ms.ReadUTF8String();
    //        pro.Type = ms.ReadInt();
    //        pro.price = ms.ReadFloat();

    //        return (T)(pro);
    //    }
    //    return default(T); 
    //}
}
