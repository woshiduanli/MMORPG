using System;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UILink = NGUILink.UILink;

public static class UILink_Extension
{
    public static T GetComponent<T>(this UILink self) where T : Component
    {
        if (!self.LinkObj)
            return null;
        if (self.component && self.component is T)
            return self.component as T;
        self.component = self.LinkObj.GetComponent(typeof(T)) as T;
        return self.component as T;
    }

    //public static void ReBuildLinkMap(this NGUILink self)
    //{
    //    self.HashID = self.GetHashCode();
    //    self.CreateLinkID();
    //    if (self.Links != null)
    //    {
    //        self.all_objs.Clear();
    //        for (int i = 0; i < self.Links.Count; ++i)
    //        {
    //            NGUILink.UILink ul = self.Links[i];
    //            if (ul == null || !ul.LinkObj)
    //                continue;
    //            self.all_objs[ul.Name] = ul;
    //            self.HashMap[ul.HashID] = ul;
    //        }
    //    }
    //}

    public static T AddComponent<T>(this UILink self) where T : Component
    {
        if (!self.LinkObj)
            return null;
        if (self.component && self.component is T)
            return self.component as T;

        T result = null;
        result = self.LinkObj.GetComponent(typeof(T)) as T;
        if (!result)
            result = self.LinkObj.AddComponent(typeof(T)) as T;

        self.component = result;
        return result;
    }

    public static T AddBehaviour<T>(this UILink self, object arg) where T : CUIElement
    {
        if (!self.LinkObj)
            return null;
        if (self.UseLua)
            return null;
        if (self.component && self.component is T)
            return self.component as T;

        T result = CClientCommon.AddComponent<T>(self.LinkObj);
        result.Arg = arg;
        result.DoInitIfDont();
        self.component = result;
        return result;
    }
}

public static class NGUILink_Extension
{
    public static UILink GetUILink(this NGUILink self, string name)
    {
        self.DoInitIfDont();
        UILink link = null;
        self.all_objs.TryGetValue(name, out link);
        if (link == null)
        {
            LOG.Debug(string.Format("{0}[NGUILink] Get<T> object {1} is not exist", self.name, name));
            return null;
        }
        return link;
    }

    public static UILink GetUILink(this NGUILink self, int hash)
    {
        self.DoInitIfDont();
        UILink link = null;
        self.HashMap.TryGetValue(hash, out link);
        if (link == null)
        {
            LOG.Debug(string.Format("{0}[NGUILink] Get<T> object {1} is not exist", self.name, hash));
            return null;
        }
        return link;
    }

    public static T GetComponent<T>(this NGUILink self, string name) where T : Component
    {
        UILink link = self.GetUILink(name);
        if (link == null)
            return null;
        return link.GetComponent<T>();
    }

    public static T GetComponent<T>(this NGUILink self, int hash) where T : Component
    {
        UILink link = self.GetUILink(hash);
        if (link == null)
            return null;
        return link.GetComponent<T>();
    }

    public static GameObject Get(this NGUILink self, string name)
    {
        UILink link = self.GetUILink(name);
        if (link == null)
            return null;
        return link.LinkObj;
    }

    public static GameObject Get(this NGUILink self, int hash)
    {
        UILink link = self.GetUILink(hash);
        if (link == null)
            return null;
        return link.LinkObj;
    }

    public static int GetHash(this NGUILink self, string name)
    {
        UILink link = self.GetUILink(name);
        if (link == null)
            return -1;
        return link.HashID;
    }

    public static void ShowKeys(this NGUILink self)
    {
        self.DoInitIfDont();

        foreach (string key in self.all_objs.Keys)
        {
            LOG.Debug(CString.Concat(self.name, "[NGUILink] key : ", key));
        }
    }

    public static T AddBehaviour<T>(this NGUILink self, string obj_name, object arg = null) where T : CUIElement
    {
        UILink link = self.GetUILink(obj_name);
        if (link == null)
            return null;
        return link.AddBehaviour<T>(arg);
    }

    //------------------------event--------------------------------
    public static void SetEvent(this NGUILink self, string name, EventTriggerType eventID, Action<BaseEventData> callback)
    {
        if (null == callback)
            return;
        UILink link = self.GetUILink(name);
        if (link == null)
        {
            LOG.Debug(string.Format("{0}[NGUILink] AddEvent object {1} is not exist", self.name, name));
            return;
        }
        EventTrigger trigger = link.AddComponent<EventTrigger>();
        UnityAction<BaseEventData> unityAct = new UnityAction<BaseEventData>(callback);
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventID;
        entry.callback.AddListener(unityAct);
        trigger.triggers.Add(entry);
    }

    public static void SetEvent(this NGUILink self, int eid, EventTriggerType eventID, Action<BaseEventData> callback)
    {
        if (null == callback)
            return;
        UILink link = self.GetUILink(eid);
        if (link == null)
        {
            LOG.Debug(string.Format("{0}[NGUILink] AddEvent object {1} is not exist", self.name, eid));
            return;
        }
        EventTrigger trigger = link.AddComponent<EventTrigger>();
        UnityAction<BaseEventData> unityAct = new UnityAction<BaseEventData>(callback);
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventID;
        entry.callback.AddListener(unityAct);
        trigger.triggers.Add(entry);
    }

    public static EventDelegate SetEvent(this NGUILink self, string name, EventTriggerType ev, EventDelegate.Callback callback, float interval = 0f)
    {
        UILink link = self.GetUILink(name);
        if (link == null)
        {
            LOG.Debug(string.Format("{0}[NGUILink] AddEvent object {1} is not exist", self.name, name));
            return null;
        }
        UIEventTrigger trigger = link.AddComponent<UIEventTrigger>();
        List<EventDelegate> list = trigger.GetDelegateList(ev);
        if (list != null)
            return EventDelegate.Set(list, callback, interval);
        else
            return null;
    }
}

/// <summary>
/// UI脚本基类
/// </summary>
public abstract partial class CUIElement : MonoBehaviour
{
    public bool isShow = true;
    public GameObject Empty;
    public object Arg;
    public GameObject CloneTarget;

    public bool initialized { private set; get; }
    public CGameUI root { private set; get; }
    public int HashID { private set; get; }

    public abstract void Initialize();
    public virtual void SetCloneData(object data) { }
    public virtual void InitListener() { }
    public virtual void DoActiveChange() { }

    public T GetRoot<T>() where T : CGameUI
    {
        if (root)
            return root as T;
        Transform tf = this.transform;
        T t = null;
        while (tf && !t)
        {
            tf = tf.parent;
            t = tf.GetComponent<T>();
        }
        root = t;
        return t;
    }

    private void Awake()
    {
        HashID = this.GetHashCode();
        DoInitIfDont();
        //if (root)
        //    root.AddChild(this);
    }

    internal void DoInitIfDont()
    {
        // 存在空reture 
        if (Empty)
            return;
        Empty = new GameObject("Empty");
        Empty.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
        DontDestroyOnLoad(this.Empty);
        if (!initialized)
        {
            CloneTarget = this.gameObject;
            GetRoot<CGameUI>();
            Initialize();
            InitListener();
            initialized = true;
        }
    }

    internal void Init(GameObject CloneTarget)
    {
        if (!initialized)
        {
            this.CloneTarget = CloneTarget;
            GetRoot<CGameUI>();
            Initialize();
            InitListener();
            initialized = true;
        }
    }

    /// <summary>
    /// 克隆一个对象，等同于NGUITools.AddChild(this.gameObject.transform.parent.gameObject, this.gameObject);
    /// </summary>
    /// <returns></returns>
    public CUIElement Clone(object arg = null)
    {
        GameObject go = AddChild(this.gameObject.transform.parent.gameObject, this.gameObject);
        if (!go)
            return null;

        this.DoInitIfDont();

        CUIElement clone = go.GetComponent(GetType()) as CUIElement;
        if (!clone)
            clone = go.AddComponent(GetType()) as CUIElement;



        clone.Arg = arg;
        clone.CloneTarget = this.gameObject;
        clone.root = this.root;
        clone.InitListener();
        return clone;
    }

    public static void BatchClone<T>(T res, List<T> list, object[] data) where T : CUIElement
    {
        if (list == null)
            return;

        for (int i = 0; i < list.Count; i++)
            list[i].isShow = false;

        if (data != null)
        {
            for (int i = 0; i < data.Length; i++)
            {
                T item = null;
                if (i < list.Count)
                    item = list[i];
                else
                {
                    item = res.Clone() as T;
                    list.Add(item);
                }
                item.isShow = true;
                item.SetCloneData(data[i]);
            }
        }

        for (int i = 0; i < list.Count; i++)
            list[i].SetActive(list[i].isShow);
    }

    static public GameObject AddChild(GameObject parent, GameObject prefab)
    {
        GameObject go = GameObject.Instantiate(prefab) as GameObject;
        if (go && parent)
        {
            Transform t = go.transform;
            t.SetParent(parent.transform);
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
        }
        return go;
    }

    public void SetActive(bool active)
    {
        if (this.gameObject.activeInHierarchy != active)
            this.gameObject.SetActive(active);
    }

    public bool SetSelfActive(bool active)
    {
        if (this.gameObject.activeSelf != active)
        {
            this.gameObject.SetActive(active);
            DoActiveChange();
            return true;
        }
        return false;
    }

    public bool Active
    {
        get { return this.gameObject.activeInHierarchy; }
    }

    public T AddBehaviour<T>(object arg = null) where T : CUIElement
    {
        T result = this.gameObject.AddComponent(typeof(T)) as T;
        if (result != null)
        {
            result.Arg = arg;
            result.DoInitIfDont();
        }
        return result;
    }

    public T AddComponent<T>() where T : Component
    {
        T result = null;
        result = this.gameObject.GetComponent(typeof(T)) as T;
        if (!result)
            result = this.gameObject.AddComponent(typeof(T)) as T;
        return result;
    }

    public void SetEvent(EventTriggerType eventID, Action<BaseEventData> callback)
    {
        if (null == callback)
            return;

        EventTrigger trigger = AddComponent<EventTrigger>();
        UnityAction<BaseEventData> unityAct = new UnityAction<BaseEventData>(callback);
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventID;
        entry.callback.AddListener(unityAct);
        trigger.triggers.Add(entry);
    }

}