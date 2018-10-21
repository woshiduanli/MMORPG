
using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public static class StringUtil 
{
    /// <summary>
    /// 扩展方法
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static int ToInt(this string str)
    {
        int temp = 0;
        int.TryParse(str, out temp);
        return temp;
    }
}