////===================================================
////作    者：边涯  http://www.u3dol.com  QQ群：87481002
////创建时间：2018-09-12 13:29:17
////备    注：
////===================================================
//using System.Collections;
//using System.Collections.Generic;
//using System;

///// <summary>
///// 新建协议
///// </summary>
//public struct test2Proto : IProto
//{
//    public ushort ProtoCode { get { return 17001; } }

//    public int ID; //编号

//    public byte[] ToArray()
//    {
//        using (MMO_MemoryStream ms = new MMO_MemoryStream())
//        {
//            ms.WriteUShort(ProtoCode);
//            ms.WriteInt(ID);
//            return ms.ToArray();
//        }
//    }

//    public static test2Proto GetProto(byte[] buffer)
//    {
//        test2Proto proto = new test2Proto();
//        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
//        {
//            proto.ID = ms.ReadInt();
//        }
//        return proto;
//    }
//}