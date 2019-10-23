using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json;
using System.Text;


public class NetWorkUtil : MonoBehaviour
{

    static NetWorkUtil instance;
    public static NetWorkUtil Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject mounter = new GameObject("C_UnityWebRequest");
                instance = mounter.AddComponent<NetWorkUtil>();
            }
            return instance;
        }
    }

    public Action<UnityWebRequest, Action<UnityWebRequest>> commonUWRBack = (UnityWebRequest uwr, Action<UnityWebRequest> deal) => {
        if (!ReportUWRException(uwr))
        {
            if (uwr.responseCode == 200)//200表示接受成功
            {
                //Debug.LogError("commonUWRBack text " + uwr.downloadHandler.text);
                deal(uwr);
            }
        }
    };

    private static bool ReportUWRException(UnityWebRequest uwr)
    {
        if (!string.IsNullOrEmpty(uwr.error) || uwr.isNetworkError || uwr.isHttpError)
        {
            Debug.LogError("commonUWRBack error " + uwr.error);
            Debug.LogError(uwr.error);
            Debug.LogError(uwr.url);
            Debug.LogError("uwr.isNetworkError " + uwr.isNetworkError);
            Debug.LogError("uwr.isHttpError " + uwr.isHttpError);
            Debug.LogError("uwr.responseCode " + uwr.responseCode);
            return true;
        }
        return false;
    }


    /*
    UnityWebRequest uwr = new UnityWebRequest();
    uwr.url = "http://www.mysite.com";
    uwr.method = UnityWebRequest.kHttpVerbGET;   // can be set to any custom method, common constants privided

    uwr.useHttpContinue = false;
    uwr.chunkedTransfer = false;
    uwr.redirectLimit = 0;  // disable redirects
    uwr.timeout = 60;       // don't make this small, web requests do take some time 
    */

    /// <summary>
    /// GET请求
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="actionResult">报错对象回调处理</param>
    /// <param name="a">请求发起后处理回调结果的委托,处理请求对象</param>
    public void Get(string url, Action<UnityWebRequest, Action<UnityWebRequest>> actionResult = null, Action<UnityWebRequest> a = null)
    {
        StartCoroutine(_Get(url, actionResult, a));
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="downloadFilePathAndName">储存文件的路径和文件名 like 'Application.persistentDataPath+"/unity3d.html"'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求对象</param>
    /// <returns></returns>
    public void DownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult = null)
    {
        StartCoroutine(_DownloadFile(url, downloadFilePathAndName, actionResult));
    }


    /// <summary>
    /// 请求图片
    /// </summary>
    /// <param name="url">图片地址,like 'http://www.my-server.com/image.png '</param>
    /// <param name="action">请求发起后处理回调结果的委托,处理请求结果的图片</param>
    /// <returns></returns>
    public void GetTexture(string url, Action<Texture2D> actionResult = null)
    {
        StartCoroutine(_GetTexture(url, actionResult));
    }


    /// <summary>
    /// 请求图片
    /// </summary>
    /// <param name="url">图片地址,like 'http://www.my-server.com/image.png '</param>
    /// <param name="action">请求发起后处理回调结果的委托,处理请求结果的图片</param>
    /// <returns></returns>
    public void GetTexture(string url, Func<Texture2D, Image> actionResult = null)
    {
        StartCoroutine(_GetTexture(url, null, actionResult));
    }

    /// <summary>
    /// 请求AssetBundle
    /// </summary>
    /// <param name="url">AssetBundle地址,like 'http://www.my-server.com/myData.unity3d'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AssetBundle</param>
    /// <returns></returns>
    public void GetAssetBundle(string url, Action<AssetBundle> actionResult = null)
    {
        StartCoroutine(_GetAssetBundle(url, actionResult));
    }

    /// <summary>
    /// 请求服务器地址上的音效
    /// </summary>
    /// <param name="url">有音效地址,like 'http://myserver.com/mysound.wav'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AudioClip</param>
    /// <param name="audioType">音效类型</param>
    /// <returns></returns>
    public void GetAudioClip(string url, Action<AudioClip> actionResult = null, AudioType audioType = AudioType.WAV)
    {
        StartCoroutine(_GetAudioClip(url, actionResult, audioType));
    }

    /// <summary>
    /// 向服务器提交post请求
    /// </summary>
    /// <param name="serverURL">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
    /// <param name="wwwform">form表单参数</param>
    /// <param name="actionResult">报错对象回调处理</param>
    /// <param name="a">处理返回结果的委托,处理请求对象</param>
    /// <returns></returns>
    public void Post(string serverURL, WWWForm wwwform, Action<UnityWebRequest, Action<UnityWebRequest>> actionResult = null, Action<UnityWebRequest> a = null)
    {
        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));
        StartCoroutine(_Post(serverURL, wwwform, actionResult, a));

    }




    /// <summary>
    /// 向服务器提交post请求 目前传json给服务器使用这种方式
    /// </summary>
    /// <param name="serverURL">服务器请求目标地址</param>
    /// <param name="jsonStr">json字符串</param>
    /// <param name="actionResult">报错对象回调处理</param>
    /// <param name="a">处理返回结果的委托,处理请求对象</param>
    public void Post2(string serverURL, string jsonStr, Action<UnityWebRequest, Action<UnityWebRequest>> actionResult = null, Action<UnityWebRequest> a = null)
    {
        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));
        StartCoroutine(_Post2(serverURL, jsonStr, actionResult, a));
    }


    /// <summary>
    /// 上传图片
    /// </summary>
    /// <param name="serverURL">服务器请求目标地址</param>
    /// <param name="tex">图片</param>
    /// <param name="a">上传回调</param>
    /// <param name="contentType">请求头 类型设置</param>
    public void UploadPic(string serverURL, Texture2D tex, Action<UnityWebRequest> a = null, string contentType = Const.uploadImgContentType2, Hashtable headerParams = null)
    {
        //byte[] bytes = tex.EncodeToPNG();
        byte[] bytes = tex.EncodeToJPG(85);
        //Debug.LogError(bytes.Length);

        UploadByPut(serverURL, bytes, a, contentType, headerParams);
    }

    /// <summary>
    /// 通过PUT方式将字节流传到服务器
    /// </summary>
    /// <param name="url">服务器目标地址 like 'http://www.my-server.com/upload' </param>
    /// <param name="contentBytes">需要上传的字节流</param>
    /// <param name="resultAction">处理返回结果的委托</param>
    /// <param name="contentType">请求头 类型设置</param>
    public void UploadByPut(string url, byte[] contentBytes, Action<UnityWebRequest> actionResult = null, string contentType = Const.contentType2, Hashtable headerParams = null)
    {
        StartCoroutine(_UploadByPut(url, contentBytes, actionResult, contentType, headerParams));
    }

    /// <summary>
    /// 通过PUT方式将字节流传到服务器
    /// </summary>
    /// <param name="url">服务器目标地址 like 'http://www.my-server.com/upload' </param>
    /// <param name="contentBytes">需要上传的字节流</param>
    /// <param name="resultAction">处理返回结果的委托</param>
    /// <param name="contentType">设置header文件中的Content-Type属性</param>
    /// <param name="headerParams">请求头传的参数列表</param>
    /// <returns></returns>
    IEnumerator _UploadByPut(string url, byte[] contentBytes, Action<UnityWebRequest> actionResult = null, string contentType = Const.contentType2, Hashtable headerParams = null)
    {
        using (UnityWebRequest uwr = UnityWebRequest.Put(url, contentBytes))
        {

            if (headerParams != null)
            {
                foreach (string key in headerParams.Keys)
                {
                    uwr.SetRequestHeader(key, headerParams[key].ToString());
                }
            }
            uwr.SetRequestHeader("extName", "jpeg");


            yield return uwr.SendWebRequest();

            if (!ReportUWRException(uwr))
            {
                actionResult(uwr);
            }
        }
    }


    /// <summary>
    /// GET请求
    /// </summary>
    /// <param name="url">请求地址,like 'http://www.my-server.com/ '</param>
    /// <param name="actionResult">请求发起后 报错对象回调处理</param>
    /// <param name="a">请求发起后处理回调结果的委托</param>
    /// <returns></returns>
    IEnumerator _Get(string url, Action<UnityWebRequest, Action<UnityWebRequest>> actionResult = null, Action<UnityWebRequest> a = null)
    {
        using (UnityWebRequest uwr = UnityWebRequest.Get(url))
        {
            yield return uwr.SendWebRequest();
            //if (url == Const.mainGoJsonServerUrl) {
            //	Debug.LogError("Get Back " + url);
            //}
            if (actionResult != null)
            {
                actionResult(uwr, a);
            }
        }
    }

    /// <summary>
    /// 下载文件
    /// </summary>
    /// <param name="url">请求地址</param>
    /// <param name="downloadFilePathAndName">储存文件的路径和文件名 like 'Application.persistentDataPath+"/unity3d.html"'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求对象</param>
    /// <returns></returns>
    IEnumerator _DownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult = null)
    {
        var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
        uwr.downloadHandler = new DownloadHandlerFile(downloadFilePathAndName);
        yield return uwr.SendWebRequest();
        if (actionResult != null)
        {
            actionResult(uwr);
        }
    }

    /// <summary>
    /// 请求图片
    /// </summary>
    /// <param name="url">图片地址,like 'http://www.my-server.com/image.png '</param>
    /// <param name="action">请求发起后处理回调结果的委托,处理请求结果的图片</param>
    /// <returns></returns>
    IEnumerator _GetTexture(string url, Action<Texture2D> actionResult = null, Func<Texture2D, Image> funcResult = null)
    {
        UnityWebRequest uwr = new UnityWebRequest(url);
        DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
        uwr.downloadHandler = downloadTexture;
        yield return uwr.SendWebRequest();

        Texture2D t = null;
        if (!ReportUWRException(uwr))
        {
            t = downloadTexture.texture;
            if (actionResult != null)
            {
                actionResult(t);
            }
        }

    }

    /// <summary>
    /// 请求AssetBundle
    /// </summary>
    /// <param name="url">AssetBundle地址,like 'http://www.my-server.com/myData.unity3d'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AssetBundle</param>
    /// <returns></returns>
    IEnumerator _GetAssetBundle(string url, Action<AssetBundle> actionResult = null)
    {
        //Debug.LogError("_GetAssetBundle  url  " + url);
        UnityWebRequest www = new UnityWebRequest(url);
        DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(www.url, uint.MaxValue);
        www.downloadHandler = handler;
        yield return www.SendWebRequest();
        AssetBundle bundle = null;
        if (!ReportUWRException(www))
        {
            bundle = handler.assetBundle;
            if (actionResult != null)
            {
                actionResult(bundle);
            }
        }
        else
        {
            ReportUWRException(www);
        }
    }

    /// <summary>
    /// 请求服务器地址上的音效
    /// </summary>
    /// <param name="url">没有音效地址,like 'http://myserver.com/mysound.wav'</param>
    /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AudioClip</param>
    /// <param name="audioType">音效类型</param>
    /// <returns></returns>
    IEnumerator _GetAudioClip(string url, Action<AudioClip> actionResult = null, AudioType audioType = AudioType.WAV)
    {
        using (var uwr = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
        {
            yield return uwr.SendWebRequest();
            if (!ReportUWRException(uwr))
            {
                if (actionResult != null)
                {
                    actionResult(DownloadHandlerAudioClip.GetContent(uwr));
                }
            }
        }
    }

    /// <summary>
    /// 向服务器提交post请求
    /// </summary>
    /// <param name="serverURL">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
    /// <param name="wwwform">form表单参数</param>
    /// <param name="actionResult">请求发起后 报错对象回调处理</param>
    /// <param name="header">请求头</param>
    /// <param name="a">处理返回结果的委托</param>
    /// <returns></returns>
    IEnumerator _Post(string serverURL, WWWForm wwwform, Action<UnityWebRequest, Action<UnityWebRequest>> actionResult = null, Action<UnityWebRequest> a = null, string header = Const.contentType1)
    {
        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));
        UnityWebRequest uwr = UnityWebRequest.Post(serverURL, wwwform);
        //UnityWebRequest uwr = UnityWebRequest.Post(serverURL, );

        uwr.SetRequestHeader("Content-Type", header);
        yield return uwr.SendWebRequest();
        if (actionResult != null)
        {
            actionResult(uwr, a);
        }
    }

    /// <summary>
    /// 第二种方式向服务器提交post请求 目前测试只有这种方式提交json成功
    /// </summary>
    /// <param name="serverURL">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
    /// <param name="jsonStr">json字符串</param>
    /// <param name="actionResult">请求发起后 报错对象回调处理</param>
    /// <param name="a">处理返回结果的委托</param>
    /// <param name="header">请求头</param>
    /// <returns></returns>
    IEnumerator _Post2(string serverURL, string jsonStr, Action<UnityWebRequest, Action<UnityWebRequest>> actionResult = null, Action<UnityWebRequest> a = null, string header = Const.contentType1)
    {
        var request = new UnityWebRequest(serverURL, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonStr);

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", header);

        yield return request.SendWebRequest();

        if (!ReportUWRException(request))
        {

        }
    }





}

