
using UnityEngine;
using System.Collections;

public static class GameObjectUtil 
{
    /// <summary>
    /// 获取或创建组建
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T GetOrCreatComponent<T>(this GameObject obj) where T:MonoBehaviour
    {
        T t = obj.GetComponent<T>();
        if (t == null)
        {
            t = obj.AddComponent<T>();
        }
        return t;
    }
}