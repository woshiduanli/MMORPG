using UnityEngine;
using System.Collections;
using LitJson;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class testCode : MonoBehaviour
{
    private static string m_serverIP = "192.168.0.101";
    private static int m_point = 1011;
    // Use this for initialization
    void Start()
    {

        //MyDebug.debug("Ip------------------------------");

        //Debug.LogError(proto.ToArray().Length);


        //TestProto test = TestProto.GetProto(proto.ToArray());


        //Debug.LogError("name:" + test.Name);
        //Debug.LogError("id:" + test.Id);
        //Debug.LogError("price:" + test.price);
        //Debug.LogError("type:" + test.Type);

        //string str = JsonMapper.ToJson(proto);
        //using (MMO_MemoryStream ms = new MMO_MemoryStream())
        //{
        //    ms.WriteUTF8String(str);

        //    Debug.LogError(ms.ToArray().Length);
        //}
        //Debug.LogError(str);
    }

    private void TestEvent(RoleOperation_LogOnGameServerReturnProto test)
    {


        Debug.LogError("客户端 我又收到了:-------------------------------------------------------------------------------" );

        Debug.LogError("客户端 我又收到了:-------------------------------------------------------------------------------"+ test.RoleList[0].RoleNickName);

        ////TestProto test = TestProto.GetProto(buffer);
        //Debug.LogError("na----me:{0}" + test.Name);
        //Debug.LogError("price:{0}" + test.price);
        //Debug.LogError("id:{0}" + test.Id);
        //Debug.LogError("type:{0}" + test.Type);

    }
    public double GetTimestamp(DateTime d)
    {
        TimeSpan ts = d - new DateTime(1970, 1, 1);
        return ts.TotalMilliseconds;
    }
    // Update is called once per frame
    void Update()
    {

        return; 
        if (Input.GetKeyDown(KeyCode.C))
        {
            transform.localScale = Vector3.one;
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            GameObject obj = GameObject.Find("btn_denglu");
            obj.AddComponent<Button>().onClick.AddListener(() => { MyDebug.debug("11"); });
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            //EventDispatcher.Instance.RegProto<TestProto>(888, TestEvent);

            //JsonData data = new JsonData();
            //data["Type"] = 0;
            //data["t"] = 123466; 
            //data["UserName"] = "zhangsan2";
            //data["deviceIdentifier"] = "deviceIdentifier";
            //data["deviceModel"] = "deviceModel";
            //data["sign"] = "sign";
            //data["ChannelId"] = "22";
            //data["Pwd"] = "123456";


            //NetWorkHttp.Instance.SendData("http://localhost:8080/api/account", null, true, data.ToJson());




            //using (MMO_MemoryStream stream = new MMO_MemoryStream ())
            //{
            //    stream.WriteUTF8String("nihao");

            //    NetWorkSocket.Instance.SendMsg(stream.ToArray ()); 
            //}
            //NetWorkSocket.Instance.SendMsg("nihao"); 


            NetWorkSocket.Instance.Connect(m_serverIP, m_point);
            RoleOperation_LogOnGameServerProto proto = new RoleOperation_LogOnGameServerProto();
            proto.AccountId = 1010;
            //NetWorkSocket.Instance.Connect(); 
            EventDispatcher.Instance.RegProto<RoleOperation_LogOnGameServerReturnProto>(10002, TestEvent);
            NetWorkSocket.Instance.SendMsg(proto.ToArray());

        }

    }
}


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

    IProto IProto.GetProto(byte[] buffer)
    {
        RoleOperation_LogOnGameServerProto proto = new RoleOperation_LogOnGameServerProto();
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            proto.AccountId = ms.ReadInt();
        }
        return proto;
    }
}


public struct RoleOperation_LogOnGameServerReturnProto : IProto
{
    public ushort ProtoCode { get { return 10002; } }

    public int RoleCount; //已有角色数量
    public List<RoleItem> RoleList; //角色项

    /// <summary>
    /// 角色项
    /// </summary>
    public struct RoleItem
    {
        public int RoleId; //角色编号
        public string RoleNickName; //角色昵称
        public byte RoleJob; //角色职业
        public int RoleLevel; //角色等级
    }

    public byte[] ToArray()
    {
        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort(ProtoCode);
            ms.WriteInt(RoleCount);
            for (int i = 0; i < RoleCount; i++)
            {
                ms.WriteInt(RoleList[i].RoleId);
                ms.WriteUTF8String(RoleList[i].RoleNickName);
                ms.WriteByte(RoleList[i].RoleJob);
                ms.WriteInt(RoleList[i].RoleLevel);
            }
            return ms.ToArray();
        }
    }

    IProto IProto.GetProto(byte[] buffer)
    {
        RoleOperation_LogOnGameServerReturnProto proto = new RoleOperation_LogOnGameServerReturnProto();
        using (MMO_MemoryStream ms = new MMO_MemoryStream(buffer))
        {
            proto.RoleCount = ms.ReadUShort();
            proto.RoleCount = ms.ReadInt();
            proto.RoleList = new List<RoleItem>();
            for (int i = 0; i < proto.RoleCount; i++)
            {
                RoleItem _Role = new RoleItem();
                _Role.RoleId = ms.ReadInt();
                _Role.RoleNickName = ms.ReadUTF8String();
                _Role.RoleJob = (byte)ms.ReadByte();
                _Role.RoleLevel = ms.ReadInt();
                proto.RoleList.Add(_Role);
            }
        }
        return proto;
    }

  
}



