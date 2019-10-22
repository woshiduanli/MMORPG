
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

/// <summary>
/// 主下载器
/// </summary>
public class AssetBundleDownload : SingletonMono<AssetBundleDownload>
{
    private string m_VersionUrl;
    private Action<List<DownloadDataEntity>> m_OnInitVersion;

    //下载器的数组
    private AssetBundleDownloadRoutine[] m_Routine = new AssetBundleDownloadRoutine[DownloadMgr.DownloadRoutineNum];

    //下载器的数组
    private HttpThreadDownLoad[] m_thread = new HttpThreadDownLoad[DownloadMgr.DownloadRoutineNum];


    private int m_RoutineIndex = 0; //下载器索引

    private bool m_IsDownloadOver = false;

    protected override void OnStart()
    {
        base.OnStart();

        //真正的运行
        StartCoroutine(DownloadVersion(m_VersionUrl));
    }

    private float m_Time = 2; //采样时间
    private float m_AlreadyTime = 0; //已经下载的时间

    private float m_NeedTime = 0f;//剩余的时间
    private float m_Speed = 0f;//下载速度

    protected override void OnUpdate()
    {
        base.OnUpdate();

        //如果需要下载的数量大于0 并且还没有下载完成
        if (TotalCount > 0 && !m_IsDownloadOver)
        {
            int totalCompleteCount = CurrCompleteTotalCount();
            totalCompleteCount = totalCompleteCount == 0 ? 1 : totalCompleteCount;

            int totalCompleteSize = CurrCompleteTotalSize();


            m_AlreadyTime += Time.deltaTime;
            if (m_AlreadyTime > m_Time && m_Speed == 0)
            {
                m_Speed = totalCompleteSize / m_Time; //速度
            }

            //计算剩余时间 = (总大小 - 已经下载的大小) / 速度
            if (m_Speed > 0)
            {
                m_NeedTime = (TotalSize - totalCompleteSize) / m_Speed;
            }

            string str = string.Format("正在下载{0}/{1}", totalCompleteCount, TotalCount);
            string strProgress = string.Format("下载进度={0}", totalCompleteSize / (float)TotalSize);

            //UISceneInitCtrl.Instance.SetProgress(str, totalCompleteCount / (float)TotalCount);

            //AppDebug.LogError(str);
            //AppDebug.LogError(strProgress);

            if (m_NeedTime > 0)
            {
                string strNeedTime = string.Format("剩余时间{0}秒", m_NeedTime);
            }
            //AppDebug.LogError(totalCompleteCount + "  "+ TotalCount+ "   "+  "下载2完成--------------------------------------------------： " + System.DateTime.Now.ToString());
            if (totalCompleteCount == TotalCount)
            {
                AppDebug.LogError("下载完成--------------------------------------------------： "+ System.DateTime.Now.ToString());
                m_IsDownloadOver = true;
                //UISceneInitCtrl.Instance.SetProgress("资源更新完毕", 1);
                if (DownloadMgr.Instance.OnInitComplete != null)
                {
                    DownloadMgr.Instance.OnInitComplete();
                }
            }
        }
    }

    /// <summary>
    /// 初始化服务器版本信息
    /// </summary>
    /// <param name="url"></param>
    /// <param name="onInitVersion"></param>
    public void InitServerVersion(string url, Action<List<DownloadDataEntity>> onInitVersion)
    {
        m_VersionUrl = url;
        m_OnInitVersion = onInitVersion;
    }

    private IEnumerator DownloadVersion(string url)
    {
        WWW www = new WWW(url);

        float timeOut = Time.time;
        float progress = www.progress;

        while (www != null && !www.isDone)
        {
            if (progress < www.progress)
            {
                timeOut = Time.time;
                progress = www.progress;
            }

            if ((Time.time - timeOut) > DownloadMgr.DownloadTimeOut)
            {
                AppDebug.Log("下载超时");
                yield break;
            }
        }

        yield return www;

        if (www != null && www.error == null)
        {
            string content = www.text;
            if (m_OnInitVersion != null)
            {
                m_OnInitVersion(DownloadMgr.Instance.PackDownloadData(content));
            }
        }
        else
        {
            AppDebug.LogError("下载失败 原因:" + www.error);
        }
    }

    /// <summary>
    /// 总大小
    /// </summary>
    public int TotalSize
    {
        get;
        private set;
    }

    /// <summary>
    /// 当前已经下载的文件总大小
    /// </summary>
    /// <returns></returns>
    public int CurrCompleteTotalSize()
    {
        int completeTotalSize = 0;

        for (int i = 0; i < m_Routine.Length; i++)
        {
            if (m_Routine[i] == null) continue;
            completeTotalSize += m_Routine[i].DownloadSize;
        }

        return completeTotalSize;
    }

    /// <summary>
    /// 总数量
    /// </summary>
    public int TotalCount
    {
        get;
        private set;
    }

    /// <summary>
    /// 当前已经下载的文件总数量
    /// </summary>
    /// <returns></returns>
    public int CurrCompleteTotalCount()
    {
        int completeTotalCount = 0;

        for (int i = 0; i < m_Routine.Length; i++)
        {
            if (m_Routine[i] == null) continue;
            completeTotalCount += m_Routine[i].CompleteCount;
        }

        return completeTotalCount;
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="downloadList"></param>
    public void DownloadFiles(List<DownloadDataEntity> downloadList, bool isUseThread = true )
    {
        TotalSize = 0;
        TotalCount = 0;

        //初始化下载器
        for (int i = 0; i < m_Routine.Length; i++)
        {
            if (m_Routine[i] == null)
            {
                m_Routine[i] = gameObject.AddComponent<AssetBundleDownloadRoutine>();
            }
        }

        //循环的给下载器分配下载任务
        for (int i = 0; i < downloadList.Count; i++)
        {
            m_RoutineIndex = m_RoutineIndex % m_Routine.Length; //0-4
            Debug.LogError("dddddddddddddddddddddddddddddddd:"+ m_RoutineIndex);

            //其中的一个下载器 给他分配一个文件
            m_Routine[m_RoutineIndex].AddDownload(downloadList[i]);

            m_RoutineIndex++;
            TotalSize += downloadList[i].Size;
            TotalCount++;
        }
        Debug.LogError("下载器的长度：" + m_Routine.Length);
        //让下载器开始下载
        for (int i = 0; i < m_Routine.Length; i++)
        {
            if (m_Routine[i] == null) continue;

            m_Routine[i].StartDownload(isUseThread);
        }
    }

  



    public IEnumerator DownloadData(DownloadDataEntity currDownloadData, Action<bool> onComplete)
    {
        string dataUrl = DownloadMgr.DownloadUrl + currDownloadData.FullName; //资源下载路径
        AppDebug.Log("dataUrl=" + dataUrl);

        //短路径 用于创建文件夹
        string path = currDownloadData.FullName.Substring(0, currDownloadData.FullName.LastIndexOf('\\'));

        //得到本地路径
        string localFilePath = DownloadMgr.Instance.LocalFilePath + path;

        if (!Directory.Exists(localFilePath))
        {
            Directory.CreateDirectory(localFilePath);
        }

        WWW www = new WWW(dataUrl);

        float timeout = Time.time;
        float progress = www.progress;

        while (www != null && !www.isDone)
        {
            if (progress < www.progress)
            {
                timeout = Time.time;
                progress = www.progress;
            }

            if ((Time.time - timeout) > DownloadMgr.DownloadTimeOut)
            {
                AppDebug.LogError("下载失败 超时");
                if (onComplete != null) onComplete(false);
                yield break;
            }

            yield return null; //一定要等一帧
        }

        yield return www;

        if (www != null && www.error == null)
        {
            using (FileStream fs = new FileStream(DownloadMgr.Instance.LocalFilePath + currDownloadData.FullName, FileMode.Create, FileAccess.ReadWrite))
            {
                fs.Write(www.bytes, 0, www.bytes.Length);
            }
        }

        //写入本地文件
        DownloadMgr.Instance.ModifyLocalData(currDownloadData);

        if (onComplete != null) onComplete(true);
    }
}