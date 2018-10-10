#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;
using System.Linq;
using LitJson;
using System.Text.RegularExpressions;

public static class CClientCommon
{

    public static T AddComponent<T>(GameObject target) where T : UnityEngine.Component
    {
        if (target)
        {
            T result = target.GetComponent<T>();
            if (result == null)
                result = target.AddComponent<T>();
            return result;
        }
        else
        {
            return null;
        }
    }

    private static Vector3 ms_KeepPosition = Vector3.zero;
    private static Quaternion ms_KeepRotation = Quaternion.identity;
    private static bool ms_KeepTransform = false;
    private static Vector3 ms_KeepLocalScale = Vector3.one;
    public static void SaveTransform(Transform transform)
    {
        ms_KeepTransform = true;
        ms_KeepPosition = transform.position;
        ms_KeepRotation = transform.rotation;
        ms_KeepLocalScale = transform.localScale;
    }



    public static void RemoveComponent<T>(GameObject go) where T : Component
    {
        if (go)
        {
            Component c = go.GetComponent<T>();
            if (c)
                Object.DestroyImmediate(c, true);
        }
    }

    public static void ZeroTransform(Transform tf)
    {
        if (!tf)
            return;
        tf.localEulerAngles = Vector3.zero;
        tf.localPosition = Vector3.zero;
        tf.localScale = Vector3.one;
    }

    static public void DestroyImmediate(UnityEngine.Object obj)
    {
        if (obj != null)
        {
            if (Application.isEditor)
                UnityEngine.Object.DestroyImmediate(obj, true);
            else
                UnityEngine.Object.Destroy(obj);
        }
        obj = null;
    }


    //---------------------------------------------------------------------
    public static void RevertTransform(Transform transform)
    {
        if (!ms_KeepTransform)
        {
            LOG.LogWarning("You must call BeginTransform first.");
        }

        transform.position = ms_KeepPosition;
        transform.rotation = ms_KeepRotation;
        transform.localScale = ms_KeepLocalScale;
    }

    public static void NormalizeTransform(GameObject gameObject)
    {
        NormalizeTransform(gameObject.transform);
    }

    public static void AttachChild(Transform parent, Transform child, bool inheritLayer)
    {
        if (child == null)
            return;
        child.SetParent(parent);
        if (parent == null)
            return;

        if (inheritLayer && parent.gameObject.layer != child.gameObject.layer)
            ForEach(parent, HanleModifyLayer, false);
    }

    public static bool ForEach(Transform transform, Func<Transform, Transform, bool> functor, bool includeRoot)
    {
        return ForEach(transform, transform, functor, includeRoot);
    }
    private static bool HanleModifyLayer(Transform parent, Transform child)
    {
        child.gameObject.layer = parent.gameObject.layer;
        return false;
    }

    //---------------------------------------------------------------------
    private static bool ForEach(Transform rootTrans, Transform child, Func<Transform, Transform, bool> functor, bool includeRoot)
    {
        if (child == null)
            return false;

        if (includeRoot)
            functor(rootTrans, child);

        for (int i = 0; i < child.childCount; ++i)
        {
            if (ForEach(rootTrans, child.GetChild(i), functor, true))
                return true;
        }

        return false;
    }

    //---------------------------------------------------------------------
    public static void NormalizeTransform(Transform transform)
    {
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    static public GameObject AddChild(GameObject parent)
    {
        GameObject go = new GameObject();
        if (parent != null)
        {
            Transform t = go.transform;
            t.parent = parent.transform;
            t.localPosition = Vector3.zero;
            t.localRotation = Quaternion.identity;
            t.localScale = Vector3.one;
            go.layer = parent.layer;
        }
        return go;
    }


    public static void SetUIActive(UnityEngine.GameObject UIobj, bool isShow)
    {
        if (UIobj == null) return;
     
        if ((UIobj.transform.localScale == Vector3.one) != isShow)
        {
            UIobj.transform.localScale = isShow ? Vector3.one : Vector3.zero;
        }
    }

    static public GameObject AddChild(GameObject parent, GameObject child)
    {
        if (!child)
            return null;
        GameObject go = Object.Instantiate(child);
        if (parent != null)
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

    public static void SetActiveOverload(UnityEngine.GameObject obj, bool isShow)
    {
        if (obj != null && isShow != obj.activeSelf)
        {
            obj.SetActive(isShow); ;
        }
    }
}



/// <summary>
/// 手机文本的存取和读取
/// </summary>
public class JsonIO
{
    /// <summary>
    /// 使用json格式写入缓存文件中
    /// </summary>
    /// <param name="filename"></param>
    /// <param name="obj"></param>
    public static void JsonWrite(string filename, object obj, bool absolute_path = false)
    {
        if (!absolute_path)
            filename = CDirectory.MakeCachePath(filename);
        FileInfo t = new FileInfo(filename);
        StreamWriter sw = t.CreateText();
        if (sw != null)
        {
            JsonWriter jw = new JsonWriter();
            jw.PrettyPrint = true;
            jw.Validate = true;
            string json_data = JsonMapper.ToJson(obj);
            sw.Write(json_data);
            sw.Close();
            // 解锁
            sw.Dispose();
        }
    }

    public static void WriteStrToFile(string filePath, string content)
    {
        if (!File.Exists(filePath))
        {

            FileStream f = File.Create(filePath);
            f.Close();
            f.Dispose();
        }
        ClearTextContent(filePath);
        StreamWriter write = File.CreateText(filePath);
        write.Write(content);
        write.Close();
        write.Dispose();
    }

    public static void ClearTextContent(string assetPath)
    {
        FileStream stream2 = File.Open(assetPath, FileMode.OpenOrCreate, FileAccess.Write);
        stream2.Seek(0, SeekOrigin.Begin);
        stream2.SetLength(0); //清空txt文件
        stream2.Close();
        stream2.Dispose();
    }

    public static JsonData ReadJsonData<T>(string filename)
    {
        //filename =/* CDirectory.MakeCachePath(filename);*/
        JsonData result = null;
        StreamReader sr = GetStreamReader(filename);

        if (sr == null)
            return result;
        try
        {
            result = JsonMapper.ToObject(sr);
            if (sr != null && result == null)
            {
                LOG.Erro("解析data有错");
            }
        }
        catch (System.Exception ex)
        {
            if (ex != null)
            {
                sr.Close();
                sr.Dispose();
                //DeleteFile(filename);
                return result;
            }
        }
        sr.Close();
        sr.Dispose();
        return result;
    }




    /// <summary>
    /// 用json格式读取缓存文件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static object JsonRead<T>(string filename)
    {
        filename = CDirectory.MakeCachePath(filename);
        object result = null;
        StreamReader sr = GetStreamReader(filename);
        if (sr == null)
            return result;
        try
        {
            result = JsonMapper.ToObject<T>(sr);
        }
        catch (System.Exception ex)
        {
            if (ex != null)
            {
                sr.Close();
                sr.Dispose();
                DeleteFile(filename);
                return result;
            }
        }
        sr.Close();
        sr.Dispose();
        return result;
    }

    /// <summary>
    /// 使用流读取指定目录下面的文本生成ArrayList
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static ArrayList LoadFile(string filename)
    {
        StreamReader sr = GetStreamReader(CDirectory.MakeCachePath(filename));
        if (sr == null)
        {
            //GameMessgeUI.ShowPrompt( CString.Format( "{0}不存在", path ) );
            return null;
        }
        string line;
        ArrayList arrlist = new ArrayList();
        while ((line = sr.ReadLine()) != null)
        {
            arrlist.Add(line);
        }
        sr.Close();
        sr.Dispose();
        return arrlist;
    }

    /// <summary>
    /// 读取指定目录下的文本生成流
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    static StreamReader GetStreamReader(string path)
    {
        //BufferedStream
        //TextReader
        //MemoryStream str = new MemoryStream();
        //Stream
        //FileStream
        FileInfo t = new FileInfo(path);

        if (!t.Exists)
        {
            return null;
        }
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path);
        }
        catch (Exception /*e*/ )
        {
            //GameMessgeUI.ShowPrompt( e.StackTrace );
            return null;
        }
        return sr;
    }

    /// <summary>
    /// 删除指定目录下的文本
    /// </summary>
    /// <param name="Filename"></param>
    static void DeleteFile(string filename)
    {
        if (File.Exists(filename))
        {
            File.Delete(filename);
        }
    }

}



public class UniqueIndex
{
    private int[] indexes_ = null;
    private int alloc_index_ = 0;
    private int count_ = 0;

    public UniqueIndex(int count)
    {
        this.indexes_ = new int[count];
        this.count_ = count;
        this.alloc_index_ = count_;

        for (int i = 0; i < count_; i++)
            this.indexes_[i] = count_ - 1 - i; //小的放后面,从0开始分配
    }

    public void Grow(int n)
    {
        Array.Resize(ref indexes_, count_ + n);
        for (int i = 0; i < n; i++)
            indexes_[alloc_index_ + i] = count_ + i;
        count_ += n;
        alloc_index_ += n;
    }

    public void RemoveAll()
    {
        alloc_index_ = 0;
    }

    public int Alloc()
    {
        if (alloc_index_ > 0)
            return indexes_[--alloc_index_];
        else
            return Def.INVALID_ID;
    }

    public void Free(int index)
    {
        if (alloc_index_ < indexes_.Length)
            indexes_[alloc_index_++] = index;
    }

    public bool CanAlloc()
    {
        return alloc_index_ > 0;
    }

    public int CanAllocCount()
    {
        return alloc_index_;
    }

    public void Print()
    {
        Console.WriteLine("alloc_index:{0} ", alloc_index_);
        for (int i = 0; i < alloc_index_; i++)
            Console.WriteLine("  {0}", indexes_[i]);
    }

    public int Count
    {
        get { return count_; }
    }
}
