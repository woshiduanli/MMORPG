using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XLua;


public interface IHander { void Call(byte[] arr); void Dispose(); }

public class EventDispatcher : Singleton<EventDispatcher>
{
    //委托原型
    [CSharpCallLua]
    public delegate void OnActionHandler<T>(T buffer) where T : IProto;

    [CSharpCallLua]
    public delegate void OnActionHandler(IProto buffer);

    //委托字典
    private Map<ushort, IHander> dic = new Map<ushort, IHander>();

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
        delegate IProto OnActionGetIProto(byte[] buffer);
        OnActionGetIProto getProto;
        OnActionHandler handle1;

        public void Create(OnActionHandler d, IProto pro)
        {
            handle1 = d;
            getProto = pro.GetProto;
        }

        public void Call(byte[] arr)
        {
            if (arr == null || handle1 == null) return;
            handle1(getProto(arr));
        }

        public void Dispose()
        {
            getProto = null;
            handle1 = null;
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
    public void RegProto(ushort protoCode, OnActionHandler handler, IProto iProto)
    {
        if (dic.ContainsKey(protoCode))
            return;
        Hander2 handle2 = new Hander2();
        handle2.Create(handler, iProto);

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


    /// <summary>
    /// 派发协议
    /// </summary>
    /// <param name="protoCode"></param>
    /// <param name="param"></param>
    public void Dispatch(ushort protoCode, byte[] buffer)
    {
        if (dic.ContainsKey(protoCode))
        {
            IHander d = dic[protoCode];
            d.Call(buffer);
        }
    }
}

