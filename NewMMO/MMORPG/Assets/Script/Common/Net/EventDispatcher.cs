using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using XLua;


public interface IHander { void Call(byte[] arr); void Dispose(); }

public class EventDispatcher : Singleton<EventDispatcher>
{
    //委托原型
    //[CSharpCallLua]
    public delegate void OnActionHandler<T>(T buffer) where T : IProto;

    //[CSharpCallLua]
    public delegate void OnActionHandler(IProto buffer);

    //[CSharpCallLua]
    public delegate void OnLuaActionHandler(byte[] arr);

    //委托字典
    private Dictionary<ushort, IHander> dic = new Dictionary<ushort, IHander>();

    // 内存事件监听
    private Dictionary<string, IHander> dic2 = new Dictionary<string, IHander>();

   
    public struct Hander : IHander
    {
        public byte[] arr;
        public IHander p2;
        public void Create<T>(OnActionHandler<T> action) where T : IProto, new()
        {
            RealHandle<T> realHandle = new RealHandle<T>();
            realHandle.action = action;
            p2 = realHandle;
        }

        void IHander.Call(byte[] arr)
        {
            if (arr == null) return;
            p2.Call(arr);
        }

        void IHander.Dispose()
        {
            arr = null;
            if (p2 != null)
                p2.Dispose();
            p2 = null;
        }
    }

    public struct Hander2 : IHander
    {
        public delegate IProto OnActionGetIProto(byte[] buffer);
        //[CSharpCallLua]
        OnActionGetIProto getProto;
        OnActionHandler handle1;

        EventDispatcher.OnLuaActionHandler luaCall;

        public void Create(OnActionHandler d, IProto pro)
        {
            handle1 = d;
            getProto = pro.GetProto;
        }

        public void Create(EventDispatcher.OnLuaActionHandler getProto)
        {
            this.luaCall = getProto;
        }

        public void Call(byte[] arr)
        {
            if (arr == null) return;
            if (handle1 != null)
                handle1(getProto(arr));

            if (luaCall != null) luaCall(arr);
        }


        public void Dispose()
        {
            getProto = null;
            handle1 = null;
            luaCall = null;
        }
    }

    public struct RealHandle<T2> : IHander where T2 : IProto, new()
    {
        public OnActionHandler<T2> action;
        public void Call(byte[] arr)
        {
            if (arr == null) return;
            T2 t = new T2();
            IProto d = t.GetProto(arr);
            action((T2)d);
        }

        public void Dispose()
        {
            action = null;
        }
    }

    /// <summary>
    /// 添加监听
    /// </summary>
    /// <param name="protoCode"></param>
    /// <param name="handler"></param>
    public void RegProto<T>(ushort protoCode, OnActionHandler<T> handler) where T : IProto, new()
    {
        if (dic.ContainsKey(protoCode))
            return;
        Hander handle = new Hander();
        handle.Create<T>(handler);
        IHander ihandle = handle;
        dic.Add(protoCode, ihandle);
    }

    /// <summary>
    ///  lua注册事件
    /// </summary>
    /// <param name="protoCode"></param>
    /// <param name="handler"></param>
    /// <param name="type"></param>


    public void RegProto(ushort protoCode, OnLuaActionHandler handler)
    {
        if (dic.ContainsKey(protoCode))
            return;
        Hander2 handle2 = new Hander2();
        handle2.Create(handler);

        IHander ihandle = handle2;
        dic.Add(protoCode, ihandle);
    }

    public void RemoveProto(ushort protoCode)
    {
        if (!dic.ContainsKey(protoCode))
            return;
        dic[protoCode].Dispose();
        dic.Remove(protoCode);
    }

    System.Action<ushort, byte[]> recServer;
    public void RecServer(System.Action<ushort, byte[]> recServer)
    {
        this.recServer = recServer;
    }

    /// <summary>
    /// 派发协议
    /// </summary>
    /// <param name="protoCode"></param>
    /// <param name="param"></param>
    public void Dispatch(ushort protoCode, byte[] buffer)
    {
        MyDebug.debug("c#收到了服务器协议数据，其中：id是: " + protoCode);
        if (recServer != null)
            recServer(protoCode, buffer);

        if (dic.ContainsKey(protoCode))
        {
            IHander d = dic[protoCode];
            d.Call(buffer);
        }
    }
}

