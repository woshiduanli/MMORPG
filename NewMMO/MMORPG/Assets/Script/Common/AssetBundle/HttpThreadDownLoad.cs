using UnityEngine;
using System.Collections;
using System.Threading;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;

/// <summary>
/// 通过http下载资源
/// </summary>
public class HttpThreadDownLoad
{
    //下载进度
    public float progress { get; private set; }
    //涉及子线程要注意,Unity关闭的时候子线程不会关闭，所以要有一个标识
    private bool isStop;
    //子线程负责下载，否则会阻塞主线程，Unity界面会卡主
    private Thread thread;
    //表示下载是否完成
    public bool isDone { get; private set; }

    List<DownloadDataEntity> m_list;

    AssetBundleDownloadRoutine ro;

    public HttpThreadDownLoad(List<DownloadDataEntity> list, AssetBundleDownloadRoutine ro)
    {
        m_list = list;
        this.ro = ro;
    }
    /// <summary>
    /// 下载方法(断点续传)
    /// </summary>
    /// <param name="url">URL下载地址</param>
    /// <param name="savePath">Save path保存路径</param>
    /// <param name="callBack">Call back回调函数</param>
    public void DownLoad()
    {
        isStop = false;
        //开启子线程下载,使用匿名方法
        thread = new Thread(delegate ()
        {

            //Debug.LogError("sfsfsf::::::::::::::3434:::::-----------------------");
            StartDownLoad();
        });
        //开启子线程
        thread.IsBackground = true;
        thread.Start();
    }

    private void StartDownLoad(Action callBack = null)
    {
        if (m_list == null || m_list.Count == 0) return;
        ro.m_CurrDownloadData = m_list[0];

        string dataUrl = DownloadMgr.DownloadUrl + ro.m_CurrDownloadData.FullName; //资源下载路径
     //Debug.LogError("dataUrl=" + dataUrl);

        int lastIndex = ro.m_CurrDownloadData.FullName.LastIndexOf('\\');

        if (lastIndex > -1)
        {
            //短路径 用于创建文件夹
            string path = ro.m_CurrDownloadData.FullName.Substring(0, lastIndex);

            //得到本地路径
            string localFilePath = DownloadMgr.Instance.LocalFilePath + path;

            if (!Directory.Exists(localFilePath))
            {
                Directory.CreateDirectory(localFilePath);
            }
        }
        
        //yield return www;
        //Debug.LogError("ssss:" + DownloadMgr.Instance.LocalFilePath + ro.m_CurrDownloadData.FullName);
        ToDownLoad(dataUrl, DownloadMgr.Instance.LocalFilePath + ro.m_CurrDownloadData.FullName, (isSuccess) =>
        {
            if (isSuccess)
            {
                //Debug.LogError("成功："+ ro.m_CurrDownloadData.FullName);
                DownloadMgr.Instance.ModifyLocalData(ro.m_CurrDownloadData);
            }
        });

        //写入本地文件


        m_list.RemoveAt(0);
        ro.CompleteCount++;

        if (m_list.Count == 0)
        {
            m_list.Clear();
        }
        else
        {
            StartDownLoad(null);
        }
    }

    private IEnumerator DownloadData()
    {
        yield break; 
        //if (NeedDownloadCount == 0) yield break;
        //m_CurrDownloadData = m_List[0];

        //string dataUrl = DownloadMgr.DownloadUrl + m_CurrDownloadData.FullName; //资源下载路径
        //AppDebug.Log("dataUrl=" + dataUrl);

        //int lastIndex = m_CurrDownloadData.FullName.LastIndexOf('\\');

        //if (lastIndex > -1)
        //{
        //    //短路径 用于创建文件夹
        //    string path = m_CurrDownloadData.FullName.Substring(0, lastIndex);

        //    //得到本地路径
        //    string localFilePath = DownloadMgr.Instance.LocalFilePath + path;

        //    if (!Directory.Exists(localFilePath))
        //    {
        //        Directory.CreateDirectory(localFilePath);
        //    }
        //}


        //WWW www = new WWW(dataUrl);

        //float timeout = Time.time;
        //float progress = www.progress;

        //while (www != null && !www.isDone)
        //{
        //    if (progress < www.progress)
        //    {
        //        timeout = Time.time;
        //        progress = www.progress;

        //        m_CurrDownloadSize = (int)(m_CurrDownloadData.Size * progress);
        //    }

        //    if ((Time.time - timeout) > DownloadMgr.DownloadTimeOut)
        //    {
        //        AppDebug.LogError("下载失败 超时");
        //        yield break;
        //    }

        //    yield return null; //一定要等一帧
        //}

        //yield return www;

        //if (www != null && www.error == null)
        //{
        //    using (FileStream fs = new FileStream(DownloadMgr.Instance.LocalFilePath + m_CurrDownloadData.FullName, FileMode.Create, FileAccess.ReadWrite))
        //    {
        //        fs.Write(www.bytes, 0, www.bytes.Length);
        //    }
        //}

        ////下载成功
        //m_CurrDownloadSize = 0;
        //m_DownloadSize += m_CurrDownloadData.Size;

        ////写入本地文件
        //DownloadMgr.Instance.ModifyLocalData(m_CurrDownloadData);

        //m_List.RemoveAt(0);
        //CompleteCount++;

        //if (m_List.Count == 0)
        //{
        //    m_List.Clear();
        //}
        //else
        //{
        //    IsStartDownload = true;
        //}
    }

    private void ToDownLoad(string url, string savePath, Action<bool> callBack)
    {
        //判断保存路径是否存在
        //if (!Directory.Exists(savePath))
        //{
        //    Directory.CreateDirectory(savePath);
        //}
        //这是要下载的文件名，比如从服务器下载a.zip到D盘，保存的文件名是test
        string filePath = savePath;

        //使用流操作文件
        FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
        //获取文件现在的长度
        long fileLength = fs.Length;
        ro.m_CurrDownloadSize = (int)fileLength;
        //获取下载文件的总长度
        //UnityEngine.Debug.Log(111);
        long totalLength = GetLength(url);
        //UnityEngine.Debug.Log(222);


        //如果没下载完
        if (fileLength < totalLength)
        {

            //断点续传核心，设置本地文件流的起始位置
            fs.Seek(fileLength, SeekOrigin.Begin);

            HttpWebRequest request = HttpWebRequest.Create(url) as HttpWebRequest;

            //断点续传核心，设置远程访问文件流的起始位置
            request.AddRange((int)fileLength);
            Stream stream = request.GetResponse().GetResponseStream();

            byte[] buffer = new byte[1024];
            //使用流读取内容到buffer中
            //注意方法返回值代表读取的实际长度,并不是buffer有多大，stream就会读进去多少
            int length = stream.Read(buffer, 0, buffer.Length);
            while (length > 0)
            {
                //如果Unity客户端关闭，停止下载
                if (isStop) break;
                //将内容再写入本地文件中
                fs.Write(buffer, 0, length);
                //计算进度
                fileLength += length;
                progress = (float)fileLength / (float)totalLength;
                ro.m_CurrDownloadSize = (int)fileLength;
                //UnityEngine.Debug.Log(progress);
                //类似尾递归
                length = stream.Read(buffer, 0, buffer.Length);
            }

            ro.m_DownloadSize += ro.m_CurrDownloadData.Size;
            stream.Close();
            stream.Dispose();

        }
        else
        {
            progress = 1;
        }
        ro.m_CurrDownloadSize = 0;

        fs.Close();
        fs.Dispose();
        //如果下载完毕，执行回调
        if (progress == 1)
        {
            
            isDone = true;
            if (callBack != null) callBack(true);
        }
        //UnityEngine.Debug.LogError(1111);
    }


    /// <summary>
    /// 获取下载文件的大小
    /// </summary>
    /// <returns>The length.</returns>
    /// <param name="url">URL.</param>
    long GetLength(string url)
    {
        //UnityEngine.Debug.Log(url);

        HttpWebRequest requet = HttpWebRequest.Create(url) as HttpWebRequest;
        requet.Method = "HEAD";
        HttpWebResponse response = requet.GetResponse() as HttpWebResponse;
        return response.ContentLength;
    }

    public void Close()
    {
        isStop = true;
    }

}
