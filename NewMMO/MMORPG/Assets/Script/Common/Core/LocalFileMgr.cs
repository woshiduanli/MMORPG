
using UnityEngine;
using System.Collections;
using System.IO;

/// <summary>
/// 本地文件管理
/// </summary>
public class LocalFileMgr : Singleton<LocalFileMgr>
{
    //public readonly string LocalFilePath = Application.persistentDataPath + "/";
    public readonly string LocalFilePath = Application.dataPath + "/../AssetBundles/Android/" ;


    /// <summary>
    /// 读取本地文件到byte数组
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public byte[] GetBuffer(string path)
    {
        byte[] buffer = null;

        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            buffer = new byte[fs.Length];
            fs.Read(buffer, 0, buffer.Length);
        }
        return buffer;
    }
}