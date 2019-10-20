
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

/// <summary>
/// 下载管理器
/// </summary>
public class DownloadMgr : Singleton<DownloadMgr>
{
    public const int DownloadTimeOut = 5; //超时时间
    public static string DownloadBaseUrl = "http://192.168.0.103:81/resource/"; // 这个地址，以后应该改成从服务器读取
    public const int DownloadRoutineNum = 1; //下载器的数量


    public static string DownloadUrl = DownloadBaseUrl + "";


    public string LocalFilePath = Application.persistentDataPath + "/"; //本地资源路径

    private List<DownloadDataEntity> m_NeedDownloadDataList = new List<DownloadDataEntity>(); //需要下载的数据列表

    private List<DownloadDataEntity> m_LocalDataList = new List<DownloadDataEntity>(); //本地数据列表

    private List<DownloadDataEntity> m_ServerDataList; //服务器端的数据列表

    private string m_LoaclVersionPath;// 本地版本文件路径

    private const string m_VersionFileName = "VersionFile.txt"; //版本文件名称

    private string m_StreamingAssetsPath; //资源初始化时候 原始路径

    public Action OnInitComplete; //初始化完毕


    /// <summary>
    /// 第一步：初始化资源
    /// </summary>
    public void InitStreamingAssets(Action onInitComplete)
    {

        SetDownLoadPath();
        Debug.LogError("进来初始化了"); 
        OnInitComplete = onInitComplete;

        m_LoaclVersionPath = LocalFilePath + m_VersionFileName;


        //判断本地是否已经有资源
        if (File.Exists(m_LoaclVersionPath))
        {
            //如果有资源 则检查更新
            InitCheckVersion();
        }
        else
        {
            // 没有     如果没有资源 执行初始化 然后再检查更新
            m_StreamingAssetsPath = "file:///" + Application.streamingAssetsPath + "/AssetBundles/";
#if UNITY_ANDROID && !UNITY_EDITOR
            m_StreamingAssetsPath=Application.streamingAssetsPath + "/AssetBundles/";
#endif

            string versionFileUrl = m_StreamingAssetsPath + m_VersionFileName;

            //
            GlobalInit.Instance.StartCoroutine(ReadStreamingAssetVersionFile(versionFileUrl, OnReadStreamingAssetOver));
        }
    }

    void SetDownLoadPath()
    {
        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            DownloadUrl = DownloadBaseUrl + "iOS/";
        }
        else if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            DownloadUrl = DownloadBaseUrl + "Windows/";
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            DownloadUrl = DownloadBaseUrl + "Android/";

        }
    }

    /// <summary>
    /// 读取初始资源目录的版本文件
    /// </summary>
    /// <param name="fileUrl"></param>
    /// <param name="onReadStreamingAssetOver"></param>
    /// <returns></returns>
    private IEnumerator ReadStreamingAssetVersionFile(string fileUrl, Action<string> onReadStreamingAssetOver)
    {
        //UISceneInitCtrl.Instance.SetProgress("正在准备进行资源初始化", 0);

        using (WWW www = new WWW(fileUrl))
        {
            yield return www;
            if (www.error == null)
            {
                if (onReadStreamingAssetOver != null)
                {
                    onReadStreamingAssetOver(Encoding.UTF8.GetString(www.bytes));
                }
            }
            else
            {
                onReadStreamingAssetOver("");
            }
        }
    }

    /// <summary>
    /// 读取版本文件完毕
    /// </summary>
    /// <param name="obj"></param>
    private void OnReadStreamingAssetOver(string content)
    {
        GlobalInit.Instance.StartCoroutine(InitStreamingAssetList(content));
    }


    /// <summary>
    /// 初始化资源清单
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    private IEnumerator InitStreamingAssetList(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            InitCheckVersion();
            yield break;
        }

        string[] arr = content.Split('\n');

        //循环解压
        for (int i = 0; i < arr.Length; i++)
        {
            string[] arrInfo = arr[i].Split(' ');

            string fileUrl = arrInfo[0]; //短路径

            yield return GlobalInit.Instance.StartCoroutine(AssetLoadToLocal(m_StreamingAssetsPath + fileUrl, LocalFilePath + fileUrl));

            float value = (i + 1) / (float)arr.Length;
            //UISceneInitCtrl.Instance.SetProgress(string.Format("初始化资源不消耗流量 {0}/{1}", i + 1, arr.Length), value);
        }

        //解压版本文件
        yield return GlobalInit.Instance.StartCoroutine(AssetLoadToLocal(m_StreamingAssetsPath + m_VersionFileName, LocalFilePath + m_VersionFileName));

        //Debug.LogError("技术了-------------------");
        //yield break; 
        //检查跟新
        InitCheckVersion();
    }

    /// <summary>
    /// 解压某个文件到本地
    /// </summary>
    /// <param name="fileUrl"></param>
    /// <param name="toPath"></param>
    /// <returns></returns>
    private IEnumerator AssetLoadToLocal(string fileUrl, string toPath)
    {
        using (WWW www = new WWW(fileUrl))
        {
            yield return www;
            if (www.error == null)
            {
                int lastIndexOf = toPath.LastIndexOf('\\');
                if (lastIndexOf != -1)
                {
                    string localPath = toPath.Substring(0, lastIndexOf); //除去文件名以外的路径

                    if (!Directory.Exists(localPath))
                    {
                        Directory.CreateDirectory(localPath);
                    }
                }

                using (FileStream fs = File.Create(toPath, www.bytes.Length))
                {
                    fs.Write(www.bytes, 0, www.bytes.Length);
                    fs.Close();
                }
            }
        }
    }


    /// <summary>
    /// 第二步：检查版本文件
    /// </summary>
    public void InitCheckVersion()
    {
        m_LoaclVersionPath = LocalFilePath + m_VersionFileName;
        //UISceneInitCtrl.Instance.SetProgress("正在检查版本更新", 0);
        SetDownLoadPath();
        string strVersionUrl = DownloadUrl + m_VersionFileName; //版本文件路径

        //读取这个文件
        AssetBundleDownload.Instance.InitServerVersion(strVersionUrl, OnInitVersionCallBack);
    }

    /// <summary>
    /// 初始化版本文件回调
    /// </summary>
    /// <param name="obj"></param>
    private void OnInitVersionCallBack(List<DownloadDataEntity> serverDownloadData)
    {
        m_ServerDataList = serverDownloadData;

        if (File.Exists(m_LoaclVersionPath))
        {
            //如果本地存在版本文件 则和服务器端的进行对比

            //服务器端数据
            Dictionary<string, string> serverDic = PackDownloadDataDic(serverDownloadData);


            //本地数据
            string content = IOUtil.GetFileText(m_LoaclVersionPath);
            Dictionary<string, string> clientDic = PackDownloadDataDic(content);
            m_LocalDataList = PackDownloadData(content);


            //1.新加的初始资源
            for (int i = 0; i < serverDownloadData.Count; i++)
            {
                if (serverDownloadData[i].IsFirstData && !clientDic.ContainsKey(serverDownloadData[i].FullName))
                {
                    m_NeedDownloadDataList.Add(serverDownloadData[i]); //加入下载列表
                }
            }

            //2.对比已经下载过的，但是有更新的资源
            foreach (var item in clientDic)
            {
                //如果MD5不一致
                if (serverDic.ContainsKey(item.Key) && serverDic[item.Key] != item.Value)
                {
                    //
                    DownloadDataEntity entity = GetDownloadData(item.Key, serverDownloadData);
                    if (entity != null)
                    {
                        m_NeedDownloadDataList.Add(entity);
                    }
                }
            }
        }
        else
        {
            //如果本地没有版本文件 则初始资源就是需要下载的文件
            for (int i = 0; i < serverDownloadData.Count; i++)
            {
                if (serverDownloadData[i].IsFirstData)
                {
                    m_NeedDownloadDataList.Add(serverDownloadData[i]);
                }
            }
        }

        //拿到下载列表 m_NeedDownloadDataList 进行下载

        if (m_NeedDownloadDataList.Count == 0)
        {
            //UISceneInitCtrl.Instance.SetProgress("资源更新完毕", 1);
            if (OnInitComplete != null)
            {
                OnInitComplete();
            }
            return;
        }

        //进行下载 资源服务器和客户端比较， 如果客户单没有，那么就下载，如果， 有，但是mds不一样， 也要下载
        AssetBundleDownload.Instance.DownloadFiles(m_NeedDownloadDataList);
    }

    /// <summary>
    /// 根据资源名称 获取资源实体
    /// </summary>
    /// <param name="fullName"></param>
    /// <param name="lst"></param>
    /// <returns></returns>
    private DownloadDataEntity GetDownloadData(string fullName, List<DownloadDataEntity> lst)
    {
        for (int i = 0; i < lst.Count; i++)
        {
            if (lst[i].FullName.Equals(fullName, StringComparison.CurrentCultureIgnoreCase))
            {
                return lst[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 根据文本内容 封装下载数据列表
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public List<DownloadDataEntity> PackDownloadData(string content)
    {
        // 把版本文件， 改成对象
        List<DownloadDataEntity> lst = new List<DownloadDataEntity>();

        string[] arrLines = content.Split('\n');
        for (int i = 0; i < arrLines.Length; i++)
        {
            string[] arrData = arrLines[i].Split(' ');
            if (arrData.Length == 4)
            {
                DownloadDataEntity entity = new DownloadDataEntity();
                entity.FullName = arrData[0];
                entity.MD5 = arrData[1];
                entity.Size = arrData[2].ToInt();
                entity.IsFirstData = arrData[3].ToInt() == 1;
                lst.Add(entity);
            }
        }

        return lst;
    }

    /// <summary>
    /// 封装字典
    /// </summary>
    /// <param name="lst"></param>
    /// <returns></returns>
    public Dictionary<string, string> PackDownloadDataDic(List<DownloadDataEntity> lst)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();

        for (int i = 0; i < lst.Count; i++)
        {
            dic[lst[i].FullName] = lst[i].MD5;
        }

        return dic;
    }

    /// <summary>
    /// 封装字典
    /// </summary>
    /// <param name="content"></param>
    /// <returns></returns>
    public Dictionary<string, string> PackDownloadDataDic(string content)
    {
        Dictionary<string, string> dic = new Dictionary<string, string>();

        string[] arrLines = content.Split('\n');
        for (int i = 0; i < arrLines.Length; i++)
        {
            string[] arrData = arrLines[i].Split(' ');
            if (arrData.Length == 4)
            {
                dic[arrData[0]] = arrData[1];
            }
        }
        return dic;
    }

    /// <summary>
    /// 修改本地文件
    /// </summary>
    /// <param name="entity"></param>
    public void ModifyLocalData(DownloadDataEntity entity)
    {
        if (m_LocalDataList == null) return;
        bool isExists = false;

        for (int i = 0; i < m_LocalDataList.Count; i++)
        {
            if (m_LocalDataList[i].FullName.Equals(entity.FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                m_LocalDataList[i].MD5 = entity.MD5;
                m_LocalDataList[i].Size = entity.Size;
                m_LocalDataList[i].IsFirstData = entity.IsFirstData;
                isExists = true;
                break;
            }
        }

        if (!isExists)
        {
            m_LocalDataList.Add(entity);
        }

        SavaLoaclVersion();
    }

    /// <summary>
    /// 保存本地版本文件
    /// </summary>
    private void SavaLoaclVersion()
    {
        StringBuilder sbContent = new StringBuilder();

        for (int i = 0; i < m_LocalDataList.Count; i++)
        {
            sbContent.AppendLine(string.Format("{0} {1} {2} {3}", m_LocalDataList[i].FullName, m_LocalDataList[i].MD5, m_LocalDataList[i].Size, m_LocalDataList[i].IsFirstData ? 1 : 0));
        }

        IOUtil.CreateTextFile(m_LoaclVersionPath, sbContent.ToString());
    }

    /// <summary>
    /// 根据路径获取服务器端数据
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public DownloadDataEntity GetServerData(string path)
    {
        if (m_ServerDataList == null) return null;

        for (int i = 0; i < m_ServerDataList.Count; i++)
        {
            //AppDebug.Log("FullName="+ m_ServerDataList[i].FullName);
            if (path.Replace("/", "\\").Equals(m_ServerDataList[i].FullName, StringComparison.CurrentCultureIgnoreCase))
            {
                return m_ServerDataList[i];
            }
        }

        return null;
    }
}