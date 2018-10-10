using System;
using System.Collections.Generic;
using UniRx;


namespace ZCore
{
    public interface ObjArgs { }
    /*! CObject
    \brief
      基于CObject对象管理体系的基类，继承该类的对象在不使用时必须调用Dispose
      CObject提供RegEvent和RegProto接口，分别用来注册处理内部事件函数及网络消息
    \author	figo
    \date	March,9,2015
    \modified July, 7, 2017
    \version 2.0
    */
    public class CObject : IDisposable
    {
        public int obj_id;
        public string obj_name;
        public CObjectManager obj_mgr;
        public ObjArgs objs;

        public virtual void Initialize() { }

        public T CreateSingleT<T>(ObjArgs objs = null) where T : CObject, new()
        {
            if (this.obj_mgr == null)
            {
                //throw new Exception(" obj mgr is null");
                return null;
            }
            return this.obj_mgr.CreateSingleT<T>(objs);
        }

        public T CreateSingleT<U, T>()
            where U : CObject
            where T : CObject, U, new()
        {
            if (this.obj_mgr == null)
            {
                //throw new Exception(" obj mgr is null");
                return null;
            }
            return this.obj_mgr.CreateSingleT<U, T>();
        }

        public T GetSingleT<T>() where T : CObject
        {
            if (this.obj_mgr == null)
            {
                //throw new Exception(" obj mgr is null");
                return null;
            }
            return this.obj_mgr.GetSingleT<T>();
        }

        public CObject CreateSingleT(Type type)
        {
            if (this.obj_mgr == null)
            {
                //throw new Exception(" obj mgr is null");
                return null;
            }
            return this.obj_mgr.CreateSingleT(type);
        }

        public CObject GetSingleT(Type type)
        {
            if (this.obj_mgr == null)
            {
                //throw new Exception(" obj mgr is null");
                return null;
            }
            return this.obj_mgr.GetSingleT(type);
        }

        public T CreateObjectT<T>(string name) where T : CObject, new()
        {
            if (this.obj_mgr == null)
            {
                //throw new Exception(" obj mgr is null");
                return null;
            }
            return this.obj_mgr.CreateObjectT<T>(name);
        }

        public T FindObjectT<T>(string name) where T : CObject
        {
            if (this.obj_mgr == null)
            {
                //throw new Exception(" obj mgr is null");
                return null;
            }
            return this.obj_mgr.FindObjectT<T>(name);
        }

        public void DestroySingleT<T>() where T : CObject
        {
            T obj = GetSingleT<T>();
            if (obj != null)
                obj.Dispose();
        }

        public void DestroyObjectT<T>(string name) where T : CObject
        {
            T obj = FindObjectT<T>(name);
            if (obj != null)
                obj.Dispose();
        }

        public void Dispose()
        {
            if (disposed) return;

            if (this.obj_mgr != null)
                this.obj_mgr.RemoveObject(this);

            disposed = true;
            this.disposables.Dispose();
            Dispose(true);

            this.obj_mgr = null;

            if (mapProto != null)
            {
                for (mapProto.Begin(); mapProto.Next();)
                {
                    EventDispatcher.Instance.RemoveProto(mapProto.Key);
                }
            }

            GC.SuppressFinalize(this);
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposed) { }

        //-------------------------------------------------------------
        // 事件接口
        CompositeDisposable disposables = new CompositeDisposable(32);

        class Subscrption : IDisposable
        {
            CompositeDisposable composite = null;
            IDisposable disposable = null;

            public Subscrption(CompositeDisposable composite, IDisposable disposable)
            {
                this.composite = composite;
                this.disposable = disposable;
            }

            public void Dispose()
            {
                if (composite != null)
                    composite.Remove(this);
                if (disposable != null)
                    disposable.Dispose();
                this.composite = null;
                this.disposable = null;
            }
        }

        public IDisposable RegEvent<T>(Action<CObject, T> action)
        {
            if (this.obj_mgr == null) return null;
            IDisposable disposable = this.obj_mgr.RegEvent<T>(action);
            Subscrption subscrition = new Subscrption(disposables, disposable);
            disposables.Add(subscrition);
            return subscrition;
        }

        public void FireEvent<T>(T e)
        {
            if (this.obj_mgr == null) return;
            this.obj_mgr.FireEvent<T>(this, e);
        }

        Map<ushort, byte> mapProto;
        public void RegProto<T>(ushort protoCode, EventDispatcher.OnActionHandler<T> handler) where T : IProto, new()
        {
            if (mapProto == null) mapProto = new Map<ushort, byte>();
            if (mapProto.ContainsKey(protoCode)) return;

            mapProto.Add(protoCode, 1);
            EventDispatcher.Instance.RegProto<T>(protoCode, handler);
        }
    }

    public class CObjectManager : IEventAggregator, IDisposable
    {
        private TypeSingleCollection singles = new TypeSingleCollection();
        private TypeGroupCollection groups = new TypeGroupCollection();
        private int id_gen;

        // 获得T类型的对象，单件模式
        public T GetSingleT<T>() where T : CObject
        {
            return singles[typeof(T), typeof(T)] as T;
        }

        public CObject GetSingleT(Type type)
        {
            return singles[type, type];
        }

        public T CreateSingleT<T>(ObjArgs objs = null) where T : CObject, new()
        {
            Type type = typeof(T);
            T obj = singles[type, type] as T;
            if (obj != null)
                return obj;

            obj = new T();
            obj.objs = objs;
            obj.obj_id = id_gen++;
            obj.obj_mgr = this;
            singles[type, type] = obj;
            obj.Initialize();
            return obj;
        }

        public T CreateSingleT<U, T>()
            where U : CObject
            where T : CObject, U, new()
        {
            Type base_type = typeof(U);
            Type type = typeof(T);

            T obj = singles[type, type] as T;
            if (obj != null)
                return obj;

            obj = new T();
            obj.obj_id = id_gen++;
            obj.obj_mgr = this;
            singles[base_type, type] = obj;
            obj.Initialize();
            return obj;
        }

        public CObject CreateSingleT(Type type)
        {
            CObject obj = singles[type, type];
            if (obj != null)
                return obj;

            obj = Activator.CreateInstance(type) as CObject;
            if (obj == null)
                return null;
            obj.obj_id = id_gen++;
            obj.obj_mgr = this;
            singles[type, type] = obj;
            obj.Initialize();
            return obj;
        }

        public T FindObjectT<T>(string name) where T : CObject
        {
            Type type = typeof(T);
            return groups[type, name] as T;
        }

        public T CreateObjectT<T>(string name) where T : CObject/*, new()*/
        {
            //判断该类型对象是否在single中存在
            if (GetSingleT<T>() != null)
                return null;
            Type type = typeof(T);

            T obj = Activator.CreateInstance<T>();
            if (obj == null)
                return null;
            obj.obj_id = id_gen++;
            obj.obj_name = name;
            obj.obj_mgr = this;
            groups[type, name] = obj;
            obj.Initialize();
            return obj;
        }

        public CObject CreateObjectT(Type type, string name, params object[] args)
        {
            //判断该类型对象是否在single中存在
            if (GetSingleT(type) != null)
                return null;
            if (groups[type, name] != null)
                return groups[type, name];
            CObject obj = Activator.CreateInstance(type, args) as CObject;
            if (obj == null)
                return null;
            obj.obj_id = id_gen++;
            obj.obj_name = name;
            obj.obj_mgr = this;
            groups[type, name] = obj;
            obj.Initialize();
            return obj;
        }

        internal void RemoveObject(CObject obj)
        {
            if (disposing)
                return;
            if (obj == null) return;
            Type type = obj.GetType();
            singles[type, type] = null;
            groups[type, obj.obj_name] = null;
        }

        private bool disposing = false;
        public void Dispose()
        {
            disposing = true;
            foreach (var v in singles)
            {
                if (v.Value.Value != null)
                    v.Value.Value.Dispose();
            }
            singles.Clear();

            foreach (var v in groups)
            {
                if (v.Value != null)
                    v.Value.Dispose();
            }
            groups.Clear();
            disposing = false;
        }

        #region COLLECTION
        class TypeSingleCollection : Dictionary<Type, KeyValuePair<Type, CObject>>
        {
            public CObject this[Type tbase, Type tsub]
            {
                get
                {
                    KeyValuePair<Type, CObject> kv = default(KeyValuePair<Type, CObject>);
                    if (this.TryGetValue(tbase, out kv))
                    {
                        return kv.Value;
                    }
                    else
                    {
                        if (this.TryGetValue(tsub, out kv))
                        {
                            return kv.Value;
                        }
                    }
                    return default(CObject);
                }
                set
                {
                    if (value == null)
                    {
                        KeyValuePair<Type, CObject> kv = default(KeyValuePair<Type, CObject>);
                        if (this.TryGetValue(tbase, out kv))
                        {
                            this.Remove(tbase);
                            this.Remove(kv.Key);
                        }
                        else
                        {
                            if (this.TryGetValue(tsub, out kv))
                            {
                                this.Remove(tsub);
                                this.Remove(kv.Key);
                            }
                        }
                        return;
                    }
                    this[tbase] = new KeyValuePair<Type, CObject>(tsub, value);
                    this[tsub] = new KeyValuePair<Type, CObject>(tbase, value);
                }
            }
        }

        class TypeGroupCollection : Dictionary<KeyValuePair<Type, string>, CObject>
        {
            public CObject this[Type type, string name = ""]
            {
                get
                {
                    KeyValuePair<Type, string> key = new KeyValuePair<Type, string>(type, name);
                    CObject obj = null;
                    if (this.TryGetValue(key, out obj))
                    {
                        return obj;
                    }
                    return null;
                }
                set
                {
                    KeyValuePair<Type, string> key = new KeyValuePair<Type, string>(type, name);
                    if (value == null)
                        this.Remove(key);
                    else
                        this[key] = value;
                }
            }
        }

        #endregion

        //-----------------------------------------------------------------
        private CEventAggregator EventAggregator = new CEventAggregator();

        public IDisposable RegEvent<T>(Action<CObject, T> action)
        {
            return EventAggregator.RegEvent<T>(action);
        }

        public IDisposable RegEvent(Type type, Action<CObject, object> action)
        {
            return EventAggregator.RegEvent(type, action);
        }

        public void FireEvent<T>(CObject sender, T e)
        {
            EventAggregator.FireEvent<T>(sender, e);
        }

        //------------------------------------------------------------------
        //private CProtoAggregator ProtoAggregator = new CProtoAggregator();

        //public IDisposable RegProto<T>(Action<T> action)
        //{
        //    return ProtoAggregator.RegProto<T>(action);
        //}
        //public void FireProto(int pid, Proto.Object proto)
        //{
        //    ProtoAggregator.FireProto(pid, proto);
        //}
    }

    public interface IEventAggregator
    {
        IDisposable RegEvent<T>(Action<CObject, T> action);
        void FireEvent<T>(CObject sender, T e);
    }

    public class CEventAggregator : IEventAggregator
    {
        private Dictionary<Type, PList> collection = new Dictionary<Type, PList>();

        abstract class Handle : PListNode, IDisposable
        {
            PList list;
            public Handle(PList list) { this.list = list; }
            public abstract void Execute(CObject sender, object e);
            public void Dispose()
            {
                if (list != null) list.Remove(this);
                list = null;
                DoDispose();
            }
            public abstract void DoDispose();
        }

        class Handle<T> : Handle
        {
            Action<CObject, T> action;
            public Handle(PList list, Action<CObject, T> action) : base(list) { this.action = action; }
            public override void Execute(CObject sender, object e) { if (action != null) action(sender, (T)e); }
            public override void DoDispose() { action = null; }
        }

        public IDisposable RegEvent<T>(Action<CObject, T> action)
        {
            PList list = null;
            if (!collection.TryGetValue(typeof(T), out list))
            {
                list = new PList();
                collection.Add(typeof(T), list);
            }

            Handle<T> handle = new Handle<T>(list, action);
            list.AddTail(handle);
            return handle;
        }

        public IDisposable RegEvent(Type type, Action<CObject, object> action)
        {
            PList list = null;
            if (!collection.TryGetValue(type, out list))
            {
                list = new PList();
                collection.Add(type, list);
            }
            Handle<object> handle = new Handle<object>(list, action);
            list.AddTail(handle);
            return handle;
        }

        public void FireEvent<T>(CObject sender, T e)
        {
            PList list = null;
            if (!collection.TryGetValue(e.GetType(), out list))
                return;
            for (PListNode node = list.next, next; node != list; node = next)
            {
                next = node.next;
                Handle handle = node as Handle;
                handle.Execute(sender, e);
            }
        }
    }

    //public interface IProtoAggregator
    //{
    //    IDisposable RegProto<T>(Action<T> action);
    //    void FireProto(int pid, Proto.Object proto);
    //}

    //class CProtoAggregator : ClientMsgHandler, IProtoAggregator
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
}

