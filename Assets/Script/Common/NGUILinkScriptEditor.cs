using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// NGUILink
/// 提供对UI资源进行操作的所有接口
/// </summary>
public class NGUILinkScriptEditor : MonoBehaviour
{
    [System.Serializable]
    public class UILink
    {
        public string Name;
        public string Tips;
        public GameObject LinkObj;
        public Component component;
        public bool UseLua;
        public GameObject Root;
        public int HashID;

        // 是否有点击事件
        [Tooltip("是否有点击事件")]
        public bool HaveClickEvent; 
    }

    public GameObject Root;
    public bool Show = true;
    public int HashID;
    public List<UILink> Links = new List<UILink>();
    public Dictionary<string, UILink> all_objs = new Dictionary<string, UILink>();
    public Dictionary<int, UILink> HashMap = new Dictionary<int, UILink>();
    private bool inited = false;

    void Awake()
    {
        DoInitIfDont();
    }

    public void DoInitIfDont()
    {
        if (inited)
            return;
        inited = true;
        if (Links != null)
        {
            for (int i = 0; i < Links.Count; ++i)
            {
                UILink ul = Links[i];
                if (ul == null || !ul.LinkObj)
                    continue;
                all_objs[ul.Name] = ul;
                HashMap[ul.HashID] = ul;
            }
        }
    }
}

public class LuaFiles : MonoBehaviour
{
    [System.Serializable]
    public class SingleLuaFile
    {
        public GameObject go;
        public string LuaFile;
    }

    public List<SingleLuaFile> LuaList = new List<SingleLuaFile>();


    public string GetLuaFile(GameObject go)
    {
        for (int i = 0; i < LuaList.Count; i++)
        {
            SingleLuaFile sl = LuaList[i];
            if (sl.go == go)
                return sl.LuaFile;
        }
        return string.Empty;
    }
}