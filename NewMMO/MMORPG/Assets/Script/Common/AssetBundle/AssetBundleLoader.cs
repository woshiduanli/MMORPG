
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 同步加载资源包
/// </summary>
public class AssetBundleLoader : IDisposable
{
    private AssetBundle bundle;

    public AssetBundleLoader(string assetBundlePath, bool isFullPath = false)
    {
        string fullPath = isFullPath ? assetBundlePath : LocalFileMgr.Instance.LocalFilePath + assetBundlePath;
        bundle = AssetBundle.LoadFromMemory(LocalFileMgr.Instance.GetBuffer(fullPath));
    }

    public T LoadAsset<T>(string name) where T : UnityEngine.Object
    {
        if (bundle == null) return default(T);

        //AssetBundleRequest d = bundle.LoadAssetAsync(name);


        return bundle.LoadAsset(name) as T;
    }

    public UnityEngine.Object LoadAsset(string name)
    {
        return bundle.LoadAsset(name);
    }

    public UnityEngine.Object[] LoadAllAssets()
    {
        return bundle.LoadAllAssets();
    }

    public void Dispose()
    {
        if (bundle != null) bundle.Unload(false);
    }
}