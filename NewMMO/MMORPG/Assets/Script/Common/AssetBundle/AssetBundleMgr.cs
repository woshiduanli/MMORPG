
using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
//using XLua;


public class AssetBundleMgr : Singleton<AssetBundleMgr>
{
    private AssetBundleManifest m_Manifest; //依赖文件配置

    /// <summary>
    /// 加载依赖文件配置
    /// </summary>
    private void LoadManifestBundle()
    {
        if (m_Manifest != null)
        {
            return;
        }

        string assetName = string.Empty;
#if UNITY_STANDALONE_WIN
        assetName = "Windows";
#elif UNITY_ANDROID
        assetName = "Android";
#elif UNITY_IPHONE
        assetName = "iOS";
#endif

        using (AssetBundleLoader loader = new AssetBundleLoader(assetName))
        {
            m_Manifest = loader.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //AppDebug.Log("加载依赖文件配置 完毕");
    }


    //public GameObject Load2(string path, string name )
    //{
    //    using (AssetBundleLoader loadiing = new AssetBundleLoader(path))
    //    {

    //    }
    //}

    /// <summary>
    /// 加载镜像
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject Load(string path, string name)
    {
#if DISABLE_ASSETBUNDLE && UNITY_EDITOR
        return UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(string.Format("Assets/{0}", path.Replace("assetbundle", "prefab")));
#else
        using (AssetBundleLoader loader = new AssetBundleLoader(path))
        {
            return loader.LoadAsset<GameObject>(name);
        }
#endif
    }

    /// <summary>
    /// 所有加载的Asset资源镜像
    /// </summary>
    private Dictionary<string, Object> m_AssetDic = new Dictionary<string, Object>();

    /// <summary>
    /// 依赖项的列表
    /// </summary>
    private Dictionary<string, AssetBundleLoader> m_AssetBundleDic = new Dictionary<string, AssetBundleLoader>();

    //public void LoadOrDownloadForLua(string path, string name, XLuaCustomExport.OnCreate OnCreate)
    //{
    //    LoadOrDownload<GameObject>(path, name, null, OnCreate: OnCreate, type: 0);
    //}

    public void LoadOrDownload<T>(string path, string name, System.Action<T> onComplete, System.Action<System.Object> OnCreate = null, byte type = 0) where T : Object
    {
        lock (this)
        {
#if DISABLE_ASSETBUNDLE

            string newPath = string.Empty;
            switch (type)
            {
                case 0:
                    newPath = string.Format("Assets/{0}", path.Replace("assetbundle", "prefab"));
                    break;
                case 1:
                    newPath = string.Format("Assets/{0}", path.Replace("assetbundle", "png"));
                    break;
            }

            if (onComplete != null)
            {
                Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(newPath);
                onComplete(obj as T);
            }
            if (OnCreate != null)
            {
                Object obj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(newPath);
                OnCreate(obj as GameObject);
            }
#else
            //1.加载依赖文件配置
            LoadManifestBundle();

            //2.加载依赖项开始
            string[] arrDps = m_Manifest.GetAllDependencies(path);
            //先检查所有依赖项 是否已经下载 没下载的就下载
            CheckDps(0, arrDps, () =>
            {
                // 执行到这里， 说明依赖文件已经加载完成
                //=============下载主资源开始===================
                string fullPath = (LocalFileMgr.Instance.LocalFilePath + path).ToLower();

                //AppDebug.Log("fullPath=" + fullPath);

                #region 下载或者加载主资源
                if (!File.Exists(fullPath))
                {
                    #region 如果文件不存在 需要下载
                    DownloadDataEntity entity = DownloadMgr.Instance.GetServerData(path);
                    if (entity != null)
                    {
                        AssetBundleDownload.Instance.StartCoroutine(AssetBundleDownload.Instance.DownloadData(entity,
                            (bool isSuccess) =>
                            {
                                if (isSuccess)
                                {
                                    //主资源的ab包下载完成， 开始读取主资源的对象
                                    ToLoadObj(name, onComplete, arrDps, fullPath);
                                }
                            }));
                    }
                    #endregion
                }
                else
                {
                    ToLoadObj(name, onComplete, arrDps, fullPath);
                }
                #endregion

                //=============下载主资源结束===================
            });
#endif
        }
    }

    private void ToLoadObj<T>(string name, System.Action<T> onComplete, string[] arrDps, string fullPath) where T : Object
    {
        if (!m_AssetDic.ContainsKey(fullPath))
        {
            for (int i = 0; i < arrDps.Length; i++)
            {
                if (!m_AssetDic.ContainsKey((LocalFileMgr.Instance.LocalFilePath + arrDps[i]).ToLower()))
                {
                    AssetBundleLoader loader = new AssetBundleLoader(arrDps[i]);
                    Object obj = loader.LoadAsset(GameUtil.GetFileName(arrDps[i]));
                    m_AssetBundleDic[(LocalFileMgr.Instance.LocalFilePath + arrDps[i]).ToLower()] = loader;
                    m_AssetDic[(LocalFileMgr.Instance.LocalFilePath + arrDps[i]).ToLower()] = obj;
                }
            }

            //直接加载
            using (AssetBundleLoader loader = new AssetBundleLoader(fullPath, isFullPath: true))
            {
                if (onComplete != null)
                {
                    Object obj = loader.LoadAsset<T>(name);
                    m_AssetDic[fullPath] = obj;
                    //进行回调
                    onComplete(obj as T);
                }

                //todu 进行xlua的回调
            }
        }
        // 执行到这里说明主资源的ab包下载， 完成， 然后加载对象
        else
        {
            if (onComplete != null)
            {
                onComplete(m_AssetDic[fullPath] as T);
            }
        }
    }

    /// <summary>
    /// 检查依赖项
    /// </summary>
    /// <param name="index"></param>
    /// <param name="arrDps"></param>
    /// <param name="onComplete"></param>
    private void CheckDps(int index, string[] arrDps, System.Action onComplete)
    {
        lock (this)
        {
            if (arrDps == null || arrDps.Length == 0)
            {
                if (onComplete != null) onComplete();
                return;
            }

            string fullPath = LocalFileMgr.Instance.LocalFilePath + arrDps[index];

            if (!File.Exists(fullPath))
            {
                //如果文件不存在 需要下载, 下载完成以后， 又递归， 下载下一个
                index = ToDownLoad(index, arrDps, onComplete);
            }
            else
            {
                // 文件已经存在， 那么就不需要下载， 执行检查下一个依赖项
                index++;
                ToCheckDps(index, arrDps, onComplete);
            }
        }
    }

    private void ToCheckDps(int index, string[] arrDps, System.Action onComplete)
    {
        if (index < arrDps.Length)
        {
            CheckDps(index, arrDps, onComplete);
        }
        else
        {
            if (onComplete != null) onComplete();
        }
    }

    private int ToDownLoad(int index, string[] arrDps, System.Action onComplete)
    {
        DownloadDataEntity entity = DownloadMgr.Instance.GetServerData(arrDps[index]);
        if (entity != null)
        {
            AssetBundleDownload.Instance.StartCoroutine(AssetBundleDownload.Instance.DownloadData(entity,
                (bool isSuccess) =>
                {
                    index++;
                    ToCheckDps(index, arrDps, onComplete);
                }));
        }

        return index;
    }

    /// <summary>
    /// 加载或者下载资源
    /// </summary>
    /// <param name="path">短路径</param>
    /// <param name="name"></param>
    /// <param name="onComplete"></param>
    public void LoadOrDownload(string path, string name, System.Action<GameObject> onComplete)
    {
        LoadOrDownload<GameObject>(path, name, onComplete, type: 0);
    }

    /// <summary>
    /// 卸载依赖项
    /// </summary>
    public void UnloadDpsAssetBundle()
    {
        foreach (var item in m_AssetBundleDic)
        {
            item.Value.Dispose();
        }
        m_AssetBundleDic.Clear();

        m_AssetDic.Clear();
    }

    /// <summary>
    /// 加载克隆
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public GameObject LoadClone(string path, string name)
    {
#if DISABLE_ASSETBUNDLE
        GameObject obj = UnityEditor.AssetDatabase.LoadAssetAtPath<GameObject>(string.Format("Assets/{0}", path.Replace("assetbundle", "prefab")));
        return Object.Instantiate(obj);
#else
        using (AssetBundleLoader loader = new AssetBundleLoader(path))
        {
            GameObject obj = loader.LoadAsset<GameObject>(name);
            return Object.Instantiate(obj);
        }
#endif
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="path"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public AssetBundleLoaderAsync LoadAsync(string path, string name)
    {
        GameObject obj = new GameObject("AssetBundleLoadAsync");
        AssetBundleLoaderAsync async = obj.GetOrCreatComponent<AssetBundleLoaderAsync>();
        async.Init(path, name);
        return async;
    }
}