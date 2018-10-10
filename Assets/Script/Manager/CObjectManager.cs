//using System;
//using System.Collections.Generic;
//using UnityEngine;
//using ZCore;
////using XLua;

//public interface ObjArgs { }

///// <summary>
///// CObject 管理器
///// </summary>
//public class CObjectManager : CEventAggregator
//{
//    private Map<Type, CObject> single_objs = new Map<Type, CObject>();
//    private Map<Type, CObjectGroup> group_objs = new Map<Type, CObjectGroup>();
//    private int id_gen;

//    // 获得T类型的对象，单件模式
//    public T GetSingleT<T>() where T : CObject
//    {
//        CObject obj;
//        if (single_objs.TryGetValue(typeof(T), out obj))
//            return (T)obj;
//        else
//            return null;
//    }

//    public CObject GetSingleT(Type type)
//    {
//        CObject obj;
//        if (single_objs.TryGetValue(type, out obj))
//            return obj;
//        else
//            return null;
//    }

//    public T CreateSingleT<T>(ObjArgs args = null) where T : CObject, new()
//    {
//        Type type = typeof(T);
//        if (single_objs.ContainsKey(type))
//        {
//            T e_obj = single_objs[type] as T;
//            e_obj.Awake();
//            return e_obj;
//        }

//        T obj = new T();
//        obj.obj_id = id_gen++;
//        obj.obj_mgr = this;
//        single_objs.Add(type, obj);
//        obj.Args = args; 
//        obj.Awake();
//        obj.Initialize();
//        return obj;
//    }

//    public CObject CreateSingleT(Type type, ObjArgs args)
//    {
//        if (single_objs.ContainsKey(type))
//        {
//            CObject e_obj = single_objs[type];
//            e_obj.Awake();
//            return e_obj;
//        }

//        CObject obj = Activator.CreateInstance(type) as CObject;
//        if (obj == null)
//            return null;
//        obj.obj_id = id_gen++;
//        obj.obj_mgr = this;
//        single_objs.Add(type, obj);
//        obj.Args = args;
//        obj.Awake();
//        obj.Initialize();
//        return obj;
//    }

//    public T FindObjectT<T>(string name) where T : CObject
//    {
//        Type type = typeof(T);
//        CObjectGroup group;
//        if (group_objs.TryGetValue(type, out group))
//            return group.FindObjectT<T>(name) as T;
//        else
//            return null;
//    }

//    public CObject FindObjectT(Type type, string name)
//    {
//        CObjectGroup group;
//        if (group_objs.TryGetValue(type, out group))
//        {
//            return group.FindObjectT(name);
//        }
//        else
//            return null;
//    }

//    public T FindObjectT<T>(T t, string name) where T : CObject
//    {
//        return FindObjectT<T>(name);
//    }

//    public T CreateObjectT<T>(string name, ObjArgs args) where T : obj, new()
//    {
//        Type type = typeof(T);

//        //判断该类型对象是否在single中存在
//        if (single_objs.ContainsKey(type))
//            return null;

//        CObjectGroup group;
//        if (group_objs.TryGetValue(type, out group))
//            return group.CreateObjectT<T>(id_gen++, name, args) as T;
//        else
//        {
//            group = new CObjectGroup(type, this);
//            group_objs.Add(type, group);
//            return group.CreateObjectT<T>(id_gen++, name, args) as T;
//        }
//    }

//    public CObject CreateObjectT(Type type, string name, ObjArgs args)
//    {
//        //判断该类型对象是否在single中存在
//        if (single_objs.ContainsKey(type))
//            return null;

//        CObjectGroup group;
//        if (group_objs.TryGetValue(type, out group))
//            return group.CreateObjectT(type, id_gen++, name, args);
//        else
//        {
//            group = new CObjectGroup(type, this);
//            group_objs.Add(type, group);
//            return group.CreateObjectT(type, id_gen++, name, args);
//        }
//    }

//    public void RemoveObject(CObject obj)
//    {
//        if (obj == null) return;
//        Type type = obj.GetType();
//        single_objs.Remove(type);
//        CObjectGroup group;
//        if (group_objs.TryGetValue(type, out group))
//            group.RemoveObject(obj.obj_name);
//    }

//    //>--------------------------------------------------------------------
//    internal class CObjectGroup
//    {
//        //private Type type;
//        private CObjectManager obj_mgr;
//        private Map<string, CObject> objs = new Map<string, CObject>();

//        public CObjectGroup(Type type, CObjectManager obj_mgr)
//        {
//            if (type == null)
//                throw new ArgumentException("CObjectGroup param error,type is null.");
//            //this.type = type;
//            this.obj_mgr = obj_mgr;
//        }

//        public CObject CreateObjectT<T>(int id, string name, ObjArgs args) where T : CObject, new()
//        {
//            if (objs.ContainsKey(name))
//                return null;

//            CObject obj = new T();
//            obj.obj_id = id;
//            obj.obj_name = name;
//            obj.obj_mgr = obj_mgr;
//            objs.Add(name, obj);
//            obj.Args = args;
//            obj.Awake();
//            obj.Initialize();
//            return obj;
//        }

//        public CObject CreateObjectT(Type type, int id, string name, ObjArgs args)
//        {
//            if (objs.ContainsKey(name))
//                return null;
//            CObject obj = Activator.CreateInstance(type) as CObject;
//            if (obj == null)
//                return null;
//            obj.obj_id = id;
//            obj.obj_name = name;
//            obj.obj_mgr = obj_mgr;
//            objs.Add(name, obj);
//            obj.Args = args;
//            obj.Awake();
//            obj.Initialize();
//            return obj;
//        }

//        public CObject FindObjectT<T>(string name) where T : CObject
//        {
//            CObject obj;
//            if (objs.TryGetValue(name, out obj))
//                return obj;
//            else
//                return null;
//        }

//        public CObject FindObjectT(string name)
//        {
//            CObject obj;
//            if (objs.TryGetValue(name, out obj))
//                return obj;
//            else
//                return null;
//        }

//        public CObject FindObjectT<T>(T t, string name) where T : CObject
//        {
//            return FindObjectT<T>(name);
//        }

//        public void RemoveObject(string name)
//        {
//            objs.Remove(name);
//        }
//    }
//}