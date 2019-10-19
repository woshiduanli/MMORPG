
using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class Menu
{
    [MenuItem("悠游工具/设置")]
    public static void Settings()
    {
        SettingsWindow win = (SettingsWindow)EditorWindow.GetWindow(typeof(SettingsWindow));
        win.titleContent = new GUIContent("全局设置");
        win.Show();
    }

    [MenuItem("悠游工具/资源管理/资源打包管理")]
    public static void AssetBundleCreate()
    {

        Debug.LogError(Application.platform ); 
        AssetBundleWindow win = EditorWindow.GetWindow<AssetBundleWindow>();
        win.titleContent = new GUIContent("资源打包");
        win.Show();
    }

    [MenuItem("悠游工具/资源管理/初始资源拷贝到StreamingAsstes")]
    public static void AssetBundleCopyToStreamingAsstes()
    {
        string toPath = Application.streamingAssetsPath + "/AssetBundles/";

        if (Directory.Exists(toPath))
        {
            Directory.Delete(toPath, true);
        }
        Directory.CreateDirectory(toPath);

        IOUtil.CopyDirectory(Application.persistentDataPath, toPath);
        AssetDatabase.Refresh();
        Debug.Log("拷贝完毕");
    }
}