using System;
using System.Collections.Generic;
using UnityEngine;

//public interface IEventAggregator
//{
//    IDisposable RegEvent<T>(Action<CObject, T> action);
//    void FireEvent<T>(CObject sender, T e);
//}

////内部事件接口
//public interface IEvent { }
//public class CEventAggregator : IEventAggregator
//{
//    private Map<Type, PList> collection = new Map<Type, PList>();

//    abstract class Handle : PListNode, IDisposable
//    {
//        PList list;
//        public Handle(PList list) { this.list = list; }
//        public abstract void Execute(CObject sender, object e);
//        public void Dispose()
//        {
//            if (list != null) list.Remove(this);
//            list = null;
//            DoDispose();
//        }
//        public abstract void DoDispose();
//    }

//    class Handle<T> : Handle
//    {
//        Action<CObject, T> action;
//        public Handle(PList list, Action<CObject, T> action) : base(list) { this.action = action; }
//        public override void Execute(CObject sender, object e) { if (action != null) action(sender, (T)e); }
//        public override void DoDispose() { action = null; }
//    }

//    public IDisposable RegEvent<T>(Action<CObject, T> action)
//    {
//        PList list = null;
//        if (!collection.TryGetValue(typeof(T), out list))
//        {
//            list = new PList();
//            collection.Add(typeof(T), list);
//        }

//        Handle<T> handle = new Handle<T>(list, action);
//        list.AddTail(handle);
//        return handle;
//    }

//    public IDisposable RegEvent(Type type, Action<CObject, object> action)
//    {
//        PList list = null;
//        if (!collection.TryGetValue(type, out list))
//        {
//            list = new PList();
//            collection.Add(type, list);
//        }
//        Handle<object> handle = new Handle<object>(list, action);
//        list.AddTail(handle);
//        return handle;
//    }

//    public void FireEvent<T>(CObject sender, T e)
//    {
//        PList list = null;
//        if (!collection.TryGetValue(e.GetType(), out list))
//            return;
//        //循环执行方法语句
//        for (PListNode node = list.next, next; node != list; node = next)
//        {
//            next = node.next;
//            Handle handle = node as Handle;
//            handle.Execute(sender, e);
//        }
//    }
//}



////public interface IProtoAggregator
////{
////    IDisposable RegProto<T>(Action<T> action);
////    void FireProto(int pid, Proto.Object proto);
////}

////class CProtoAggregator : ClientMsgHandler, IProtoAggregator
//{
//    private Dictionary<Type, PList> collection = new Dictionary<Type, PList>();

//    class Handle<T> : PListNode, IDisposable
//    {
//        PList list;
//        CProtoAggregator parent;
//        int pid;
//        Action<T> action;

//        public Handle(CProtoAggregator parent, PList list, int pid, Action<T> action)
//        {
//            this.parent = parent;
//            this.list = list;
//            this.pid = pid;
//            this.action = action;
//        }

//        public void Execute(T p)
//        {
//            if (action != null)
//                action(p);
//        }

//        public void Dispose()
//        {
//            if (parent != null)
//            {
//                parent.UnregProto(pid, action);
//            }
//            if (list != null)
//                list.Remove(this);
//            parent = null;
//            list = null;
//            pid = 0;
//            action = null;
//        }
//    }

//    void UnregProto<T>(int pid, Action<T> action)
//    {
//        UnregHandler(pid, action);
//    }

//    public IDisposable RegProto<T>(Action<T> action)
//    {
//        PList list = null;
//        if (!collection.TryGetValue(typeof(T), out list))
//        {
//            list = new PList();
//            collection.Add(typeof(T), list);
//        }

//        int pid = ProtoID.GetProtoID(typeof(T));

//        RegHandler<T>(action);
//        Handle<T> handle = new Handle<T>(this, list, pid, action);
//        list.AddTail(handle);
//        return handle;
//    }

//    public void FireProto(int pid, Proto.Object proto)
//    {
//        CallHandler(pid, proto);
//    }
//}