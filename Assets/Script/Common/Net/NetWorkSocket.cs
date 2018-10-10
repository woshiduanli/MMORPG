using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System;

public class NetWorkSocket : SingletonMono<NetWorkSocket>
{

    private Socket client;

    private byte[] buffer = new byte[2048];

    // 发送消息的队列
    private Queue<byte[]> m_sendQueue = new Queue<byte[]>();

    private System.Action m_checkSendQueue;
    // 压缩数组的长度
    private const int m_CompressLen = 200;

    public void Connect(string ip, int port)
    {
        if (client != null && client.Connected)
            return;
        client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        try
        {
            client.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            m_checkSendQueue = OnCheckSendQueueCallBack;
            ReceiveMsg();
            Debug.Log("连接成功");
        }
        catch (System.Exception)
        {
            Debug.Log("连接失败");
            throw;
        }
    }

    // 检查队列的委托回调
    private void OnCheckSendQueueCallBack()
    {
        lock (m_sendQueue)
        {
            if (m_sendQueue.Count > 0)
            {
                // 真正发送数据包
                Send(m_sendQueue.Dequeue());
            }
        }
    }

    #region 封装数据包
    // 封装数据包
    byte[] MakeData(byte[] data)
    {
        byte[] retBuffer = null;
        // 1.1 压缩
        bool isCompress = data.Length > m_CompressLen;
        if (isCompress)
        {
            data = ZlibHelper.CompressBytes(data);
        }
        // 2 异或加密
        data = SecurityUtil.Xor(data);
        // 3 加验证
        ushort crc = Crc16.CalculateCrc16(data);

        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort((ushort)(data.Length + 3));
            ms.WriteBool(isCompress);
            ms.WriteUShort(crc);
            ms.Write(data, 0, data.Length);

            retBuffer = ms.ToArray();
        }

        return retBuffer;
    }
    #endregion

    // 
    public void SendMsg(byte[] data)
    {
        // 得到包体的数组
        byte[] Senddata = MakeData(data);

        lock (m_sendQueue)
        {
            // 把数据包加入队列
            m_sendQueue.Enqueue(Senddata);

            //启动委托
            if (m_checkSendQueue != null)
                m_checkSendQueue.BeginInvoke(null, null);
        }
    }

    // 真正发送数据包到服务器
    void Send(byte[] buffer)
    {
        client.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallBack, client);
    }

    private void SendCallBack(System.IAsyncResult ar)
    {
        client.EndSend(ar);
        OnCheckSendQueueCallBack();
    }

    #region 接收消息
    // ----------------------------- 接收消息的代码 --------------------------
    // 接受数据用到的线程
    private MMO_MemoryStream m_ReceiveMS = new MMO_MemoryStream();
    // 接收消息的队列
    Queue<byte[]> m_ReceiveQueue = new Queue<byte[]>();
    // 接受数据包的缓冲区
    private byte[] m_ReceiveBuffer = new byte[102400];

    // 接收的数据限制
    private int m_ReceiveCount = 0;
    private void ReceiveMsg()
    {
        client.BeginReceive(m_ReceiveBuffer, 0, m_ReceiveBuffer.Length, SocketFlags.None, ReceiveBack, client);
    }

    void Update()
    {
        CheckReceiveData();
    }

    private byte[] MakeDataContent(byte[] data)
    {
        byte[] retBuffer = null;
        // 1.1 压缩
        bool isCompress = data.Length > m_CompressLen;
        if (isCompress)
        {
            data = ZlibHelper.CompressBytes(data);
        }
        // 2 异或加密
        data = SecurityUtil.Xor(data);
        // 3 加验证
        ushort crc = Crc16.CalculateCrc16(data);

        using (MMO_MemoryStream ms = new MMO_MemoryStream())
        {
            ms.WriteUShort((ushort)(data.Length + 3));
            ms.WriteBool(isCompress);
            ms.WriteUShort(crc);
            ms.Write(data, 0, data.Length);

            retBuffer = ms.ToArray();
        }

        return null;
    }
    //byte[] retBuffer = null;
    //   // 1.1 压缩
    //   bool isCompress = data.Length > m_CompressLen;
    //   if (isCompress)
    //   {
    //       data = ZlibHelper.CompressBytes(data);
    //   }

    // 2 异或加密
    //   data = SecurityUtil.Xor(data);


    //   // 3 加验证
    //   ushort crc = Crc16.CalculateCrc16(data);



    //   using (MMO_MemoryStream ms = new MMO_MemoryStream())
    //   {
    //       ms.WriteUShort((ushort)(data.Length + 3));
    //       ms.WriteBool(isCompress);
    //       ms.WriteUShort(crc);
    //       ms.Write(data, 0, data.Length);

    //       retBuffer = ms.ToArray();
    //   }

    //   return null;

    // 每帧检查5个消息
    public void CheckReceiveData()
    {
        while (true)
        {
            if (m_ReceiveCount <= 5)
            {
                ++m_ReceiveCount;
                lock (m_ReceiveQueue)
                {
                    if (m_ReceiveQueue.Count > 0)
                    {
                        byte[] sourceData = m_ReceiveQueue.Dequeue();

                        // 拿到是否压缩， crc验证码的变量
                        bool isComress = false;
                        ushort oldCrc = 0;
                        byte[] newContent = new byte[sourceData.Length - 3];
                        using (MMO_MemoryStream ms = new MMO_MemoryStream(sourceData))
                        {
                            isComress = ms.ReadBool();
                            oldCrc = ms.ReadUShort();
                            ms.Read(newContent, 0, newContent.Length);
                        }

                        // 开始和最新的crc比较
                        ushort newCrc = Crc16.CalculateCrc16(newContent);
                        if (newCrc == oldCrc)
                        {
                            // 解开异或
                            newContent = SecurityUtil.Xor(newContent);

                            // 解开压缩
                            if (isComress)
                            {
                                newContent = ZlibHelper.DeCompressBytes(newContent);
                            }

                            ushort protoCode = 0;
                            byte[] realContent = new byte[newContent.Length];
                            using (MMO_MemoryStream ms = new MMO_MemoryStream(newContent))
                            {
                                protoCode = ms.ReadUShort();
                                // 读取最终的结果
                                ms.Read(realContent,0, realContent.Length);
                                EventDispatcher.Instance.Dispatch(protoCode, realContent);
                            }
                        }
                        else
                            break;

                        //if (isComress)
                        //{
                        //}

                        //EventDispatcher.Instance.Dispatch(protoCode, ProtoContent);
                        //if (protoCode == 888)
                        //{
                        //    TestProto test = TestProto.GetProto(ProtoContent);
                        //    Debug.LogError("name:" + test.Name);
                        //    Debug.LogError("id:" + test.Id);
                        //    Debug.LogError("price:" + test.price);
                        //    Debug.LogError("type:" + test.Type);
                        //}
                        //else
                        //{
                        //    Console.WriteLine("---------------------ype:{0}");

                        //}

                        using (MMO_MemoryStream stream = new MMO_MemoryStream())
                        {
                            // 不发消息
                            //stream.WriteUTF8String("客户发送出去的时间:" + System.DateTime.Now.ToString());
                            //SendMsg(stream.ToArray());
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                m_ReceiveCount = 0;
                break;
            }
        }
    }

    // 收到消息的回调
    private void ReceiveBack(System.IAsyncResult ar)
    {
        try
        {
            // 接收数据的回调
            int len = client.EndReceive(ar);

            if (len > 0)
            {
                // 已经接收到了数据, 放在缓冲数据流的尾部
                m_ReceiveMS.Position = m_ReceiveMS.Length;

                // 把指定长度的字节写入字节流
                m_ReceiveMS.Write(m_ReceiveBuffer, 0, len);

                // 如果长度大于2,至少有个不完整的包过来了, 为什么是2, 因为封装数据包的时候用的是ushort， 他的长度是2 
                if (m_ReceiveMS.Length > 2)
                {
                    while (true)
                    {
                        // 数据流指针放在0处 
                        m_ReceiveMS.Position = 0;
                        // 包体的长度， 因为封装的时候， 就把真实数据的长度长度保存在了这个里面
                        int bodyLength = m_ReceiveMS.ReadUShort();

                        // 总包的长度  = 包头长度+包体长度，整个包的长度，就是原来包体的长度，加上， 保存长度的ushort的长度 
                        int allLength = 2 + bodyLength;

                        // 说明至少收到了一个完整的包
                        if (m_ReceiveMS.Length >= allLength)
                        {
                            byte[] bufferBody = new byte[bodyLength];

                            // 流的位置转到2
                            m_ReceiveMS.Position = 2;// 这里转到2以后， 就不用再后面计算数据的长度
                            // 把包体读到数组中
                            m_ReceiveMS.Read(bufferBody, 0, bodyLength);

                            // 把数据加入到数据队列中去
                            lock (m_ReceiveQueue)
                            {
                                m_ReceiveQueue.Enqueue(bufferBody);
                            }


                            //------------- 处理剩余字节数组 -----------------
                            int remainLen = (int)m_ReceiveMS.Length - allLength;
                            // 说明有剩余字节
                            if (remainLen > 0)
                            {
                                // 把指针放在第一个包的尾部
                                m_ReceiveMS.Position = allLength;
                                byte[] remainBuff = new byte[remainLen];

                                m_ReceiveMS.Read(remainBuff, 0, remainLen);

                                // 清空数据流
                                m_ReceiveMS.Position = 0;
                                m_ReceiveMS.SetLength(0);

                                // 又重新写进流中
                                m_ReceiveMS.Write(remainBuff, 0, remainBuff.Length);
                                remainBuff = null;
                            }
                            else
                            {
                                // 没有剩余字节
                                // 清空数据流
                                m_ReceiveMS.Position = 0;
                                m_ReceiveMS.SetLength(0);
                                break;
                            }

                        }
                        else
                        {
                            break;
                            // 没有收到完整包
                        }

                    }
                }

                // 继续接受包
                ReceiveMsg();
            }
            else
            {
                // 说明客户端断开连接
                Debug.LogError("服务器{0}断开连接" + client.RemoteEndPoint.ToString());
            }
        }
        catch (Exception)
        {
            // 说明客户端断开连接
            Debug.LogError("服务器{0}断开连接" + client.RemoteEndPoint.ToString());

        }
    }

    #endregion


    void OnDestroy()
    {
        if (client != null && client.Connected)
        {
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
    }
}
