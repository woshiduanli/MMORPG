using UnityEngine;
using System.Collections;
using LitJson;
using UnityEngine.UI;
using System;

public class testCode : MonoBehaviour
{
    private static string m_serverIP = "192.168.1.243";
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

    private void TestEvent(TestProto test)
    {



        Debug.LogError("我又收到了:-------------------------------------------------------------------------------");

        //TestProto test = TestProto.GetProto(buffer);
        Debug.LogError("na----me:{0}" + test.Name);
        Debug.LogError("price:{0}" + test.price);
        Debug.LogError("id:{0}" + test.Id);
        Debug.LogError("type:{0}" + test.Type);

    }
    public double GetTimestamp(DateTime d)
    {
        TimeSpan ts = d - new DateTime(1970, 1, 1);
        return ts.TotalMilliseconds;
    }
    // Update is called once per frame
    void Update()
    {


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
            //NetWorkSocket.Instance.Connect(m_serverIP, m_point);
            JsonData data = new JsonData();
            data["Type"] = 0;
            data["t"] = 123466; 
            data["UserName"] = "zhangsan2";
            data["deviceIdentifier"] = "deviceIdentifier";
            data["deviceModel"] = "deviceModel";
            data["sign"] = "sign";
            data["ChannelId"] = "22";
            data["Pwd"] = "123456";


            NetWorkHttp.Instance.SendData("http://localhost:8080/api/account", null, true, data.ToJson());
            //using (MMO_MemoryStream stream = new MMO_MemoryStream ())
            //{
            //    stream.WriteUTF8String("nihao");

            //    NetWorkSocket.Instance.SendMsg(stream.ToArray ()); 
            //}
            //NetWorkSocket.Instance.SendMsg("nihao"); 



            //TestProto proto = new TestProto();
            //proto.Id = 1;
            //proto.price = 100.8f;
            //proto.Type = 11;
            //proto.Name = "测试";

            //NetWorkSocket.Instance.SendMsg(proto.ToArray());

        }

    }
}
