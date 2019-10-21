
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// 下载器
/// </summary>
public class AssetBundleDownloadRoutine : MonoBehaviour
{
    private List<DownloadDataEntity> m_List = new List<DownloadDataEntity>(); //需要下载的文件列表

    public DownloadDataEntity m_CurrDownloadData; //当前正在下载的数据

    /// <summary>
    /// 需要下载的数量
    /// </summary>
    public int NeedDownloadCount
    {
        private set;
        get;
    }

    /// <summary>
    /// 已经下载完成的数量
    /// </summary>
    public int CompleteCount
    {
          set;
        get;
    }

    public int m_DownloadSize; //已经下载好的文件的总大小
    public int m_CurrDownloadSize; //当前下载的文件大小

    /// <summary>
    /// 这个下载器已经下载的大小
    /// </summary>
    public int DownloadSize
    {
        get { return m_DownloadSize + m_CurrDownloadSize; }
    }

    /// <summary>
    /// 是否开始下载
    /// </summary>
    public bool IsStartDownload
    {
        private set;
        get;
    }

    /// <summary>
    /// 添加下载对象
    /// </summary>
    /// <param name="entity"></param>
    public void AddDownload(DownloadDataEntity entity)
    {
        m_List.Add(entity);
    }
    bool isUseThread = true;
    HttpThreadDownLoad threadLoad; 
    /// <summary>
    /// 开始下载
    /// </summary>
    public void StartDownload(bool isUseThread = true)
    {
        this.isUseThread = isUseThread;
        IsStartDownload = true;
        NeedDownloadCount = m_List.Count;


        if (isUseThread)
        {
            threadLoad = new HttpThreadDownLoad(m_List, this );
            threadLoad.DownLoad(); 
        }
    }

    void Update()
    {
        if (IsStartDownload && !isUseThread)
        {
            IsStartDownload = false;
            StartCoroutine(DownloadData());
        }
    }

    private IEnumerator DownloadData()
    {
        if (NeedDownloadCount == 0) yield break;
        m_CurrDownloadData = m_List[0];

        string dataUrl = DownloadMgr.DownloadUrl + m_CurrDownloadData.FullName; //资源下载路径
        AppDebug.Log("dataUrl=" + dataUrl);

        int lastIndex = m_CurrDownloadData.FullName.LastIndexOf('\\');

        if (lastIndex > -1)
        {
            //短路径 用于创建文件夹
            string path = m_CurrDownloadData.FullName.Substring(0, lastIndex);

            //得到本地路径
            string localFilePath = DownloadMgr.Instance.LocalFilePath + path;

            if (!Directory.Exists(localFilePath))
            {
                Directory.CreateDirectory(localFilePath);
            }
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

                m_CurrDownloadSize = (int)(m_CurrDownloadData.Size * progress);
            }

            if ((Time.time - timeout) > DownloadMgr.DownloadTimeOut)
            {
                AppDebug.LogError("下载失败 超时");
                yield break;
            }

            yield return null; //一定要等一帧
        }

        yield return www;

        if (www != null && www.error == null)
        {
            using (FileStream fs = new FileStream(DownloadMgr.Instance.LocalFilePath + m_CurrDownloadData.FullName, FileMode.Create, FileAccess.ReadWrite))
            {
                fs.Write(www.bytes, 0, www.bytes.Length);
            }
        }

        //下载成功
        m_CurrDownloadSize = 0;
        m_DownloadSize += m_CurrDownloadData.Size;

        //写入本地文件
        DownloadMgr.Instance.ModifyLocalData(m_CurrDownloadData);

        m_List.RemoveAt(0);
        CompleteCount++;

        if (m_List.Count == 0)
        {
            m_List.Clear();
        }
        else
        {
            IsStartDownload = true;
        }
    }
}