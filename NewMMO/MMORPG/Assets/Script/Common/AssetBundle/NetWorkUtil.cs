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
            if (uwr.responseCode == 200)//200��ʾ���ܳɹ�
            {
                //Debug.LogError("commonUWRBack text " + uwr.downloadHandler.text);
                deal(uwr);
            }
        }
    };

    private static bool ReportUWRException(UnityWebRequest uwr)
    {
//        if (!string.IsNullOrEmpty(uwr.error) || uwr.isNetworkError || uwr.isHttpError)
//        {
//            Debug.LogError("commonUWRBack error " + uwr.error);
//            Debug.LogError(uwr.error);
//            Debug.LogError(uwr.url);
//            Debug.LogError("uwr.isNetworkError " + uwr.isNetworkError);
//            Debug.LogError("uwr.isHttpError " + uwr.isHttpError);
//            Debug.LogError("uwr.responseCode " + uwr.responseCode);
//            return true;
//        }
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
    /// GET����
    /// </summary>
    /// <param name="url">�����ַ</param>
    /// <param name="actionResult">�������ص�����</param>
    /// <param name="a">���������ص������ί��,�����������</param>
    public void Get(string url, Action<UnityWebRequest, Action<UnityWebRequest>> actionResult = null, Action<UnityWebRequest> a = null)
    {
        StartCoroutine(_Get(url, actionResult, a));
    }

    /// <summary>
    /// �����ļ�
    /// </summary>
    /// <param name="url">�����ַ</param>
    /// <param name="downloadFilePathAndName">�����ļ���·�����ļ��� like 'Application.persistentDataPath+"/unity3d.html"'</param>
    /// <param name="actionResult">���������ص������ί��,�����������</param>
    /// <returns></returns>
    public void DownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult = null)
    {
        StartCoroutine(_DownloadFile(url, downloadFilePathAndName, actionResult));
    }


    /// <summary>
    /// ����ͼƬ
    /// </summary>
    /// <param name="url">ͼƬ��ַ,like 'http://www.my-server.com/image.png '</param>
    /// <param name="action">���������ص������ί��,������������ͼƬ</param>
    /// <returns></returns>
    public void GetTexture(string url, Action<Texture2D> actionResult = null)
    {
        StartCoroutine(_GetTexture(url, actionResult));
    }


    /// <summary>
    /// ����ͼƬ
    /// </summary>
    /// <param name="url">ͼƬ��ַ,like 'http://www.my-server.com/image.png '</param>
    /// <param name="action">���������ص������ί��,������������ͼƬ</param>
    /// <returns></returns>
    public void GetTexture(string url, Func<Texture2D, Image> actionResult = null)
    {
        StartCoroutine(_GetTexture(url, null, actionResult));
    }

    /// <summary>
    /// ����AssetBundle
    /// </summary>
    /// <param name="url">AssetBundle��ַ,like 'http://www.my-server.com/myData.unity3d'</param>
    /// <param name="actionResult">���������ص������ί��,������������AssetBundle</param>
    /// <returns></returns>
    public void GetAssetBundle(string url, Action<AssetBundle> actionResult = null)
    {
        StartCoroutine(_GetAssetBundle(url, actionResult));
    }

    /// <summary>
    /// �����������ַ�ϵ���Ч
    /// </summary>
    /// <param name="url">����Ч��ַ,like 'http://myserver.com/mysound.wav'</param>
    /// <param name="actionResult">���������ص������ί��,������������AudioClip</param>
    /// <param name="audioType">��Ч����</param>
    /// <returns></returns>
    public void GetAudioClip(string url, Action<AudioClip> actionResult = null, AudioType audioType = AudioType.WAV)
    {
        StartCoroutine(_GetAudioClip(url, actionResult, audioType));
    }

    /// <summary>
    /// ��������ύpost����
    /// </summary>
    /// <param name="serverURL">����������Ŀ���ַ,like "http://www.my-server.com/myform"</param>
    /// <param name="wwwform">form������</param>
    /// <param name="actionResult">�������ص�����</param>
    /// <param name="a">�����ؽ����ί��,�����������</param>
    /// <returns></returns>
    public void Post(string serverURL, WWWForm wwwform, Action<UnityWebRequest, Action<UnityWebRequest>> actionResult = null, Action<UnityWebRequest> a = null)
    {
        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));
        StartCoroutine(_Post(serverURL, wwwform, actionResult, a));

    }




    /// <summary>
    /// ��������ύpost���� Ŀǰ��json��������ʹ�����ַ�ʽ
    /// </summary>
    /// <param name="serverURL">����������Ŀ���ַ</param>
    /// <param name="jsonStr">json�ַ���</param>
    /// <param name="actionResult">�������ص�����</param>
    /// <param name="a">�����ؽ����ί��,�����������</param>
    public void Post2(string serverURL, string jsonStr, Action<UnityWebRequest, Action<UnityWebRequest>> actionResult = null, Action<UnityWebRequest> a = null)
    {
        //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));
        StartCoroutine(_Post2(serverURL, jsonStr, actionResult, a));
    }


    /// <summary>
    /// �ϴ�ͼƬ
    /// </summary>
    /// <param name="serverURL">����������Ŀ���ַ</param>
    /// <param name="tex">ͼƬ</param>
    /// <param name="a">�ϴ��ص�</param>
    /// <param name="contentType">����ͷ ��������</param>
    public void UploadPic(string serverURL, Texture2D tex, Action<UnityWebRequest> a = null, string contentType = Const.uploadImgContentType2, Hashtable headerParams = null)
    {
        //byte[] bytes = tex.EncodeToPNG();
        byte[] bytes = tex.EncodeToJPG(85);
        //Debug.LogError(bytes.Length);

        UploadByPut(serverURL, bytes, a, contentType, headerParams);
    }

    /// <summary>
    /// ͨ��PUT��ʽ���ֽ�������������
    /// </summary>
    /// <param name="url">������Ŀ���ַ like 'http://www.my-server.com/upload' </param>
    /// <param name="contentBytes">��Ҫ�ϴ����ֽ���</param>
    /// <param name="resultAction">�����ؽ����ί��</param>
    /// <param name="contentType">����ͷ ��������</param>
    public void UploadByPut(string url, byte[] contentBytes, Action<UnityWebRequest> actionResult = null, string contentType = Const.contentType2, Hashtable headerParams = null)
    {
        StartCoroutine(_UploadByPut(url, contentBytes, actionResult, contentType, headerParams));
    }

    /// <summary>
    /// ͨ��PUT��ʽ���ֽ�������������
    /// </summary>
    /// <param name="url">������Ŀ���ַ like 'http://www.my-server.com/upload' </param>
    /// <param name="contentBytes">��Ҫ�ϴ����ֽ���</param>
    /// <param name="resultAction">�����ؽ����ί��</param>
    /// <param name="contentType">����header�ļ��е�Content-Type����</param>
    /// <param name="headerParams">����ͷ���Ĳ����б�</param>
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
    /// GET����
    /// </summary>
    /// <param name="url">�����ַ,like 'http://www.my-server.com/ '</param>
    /// <param name="actionResult">������� �������ص�����</param>
    /// <param name="a">���������ص������ί��</param>
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
    /// �����ļ�
    /// </summary>
    /// <param name="url">�����ַ</param>
    /// <param name="downloadFilePathAndName">�����ļ���·�����ļ��� like 'Application.persistentDataPath+"/unity3d.html"'</param>
    /// <param name="actionResult">���������ص������ί��,�����������</param>
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
    /// ����ͼƬ
    /// </summary>
    /// <param name="url">ͼƬ��ַ,like 'http://www.my-server.com/image.png '</param>
    /// <param name="action">���������ص������ί��,������������ͼƬ</param>
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
    /// ����AssetBundle
    /// </summary>
    /// <param name="url">AssetBundle��ַ,like 'http://www.my-server.com/myData.unity3d'</param>
    /// <param name="actionResult">���������ص������ί��,������������AssetBundle</param>
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
    /// �����������ַ�ϵ���Ч
    /// </summary>
    /// <param name="url">û����Ч��ַ,like 'http://myserver.com/mysound.wav'</param>
    /// <param name="actionResult">���������ص������ί��,������������AudioClip</param>
    /// <param name="audioType">��Ч����</param>
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
    /// ��������ύpost����
    /// </summary>
    /// <param name="serverURL">����������Ŀ���ַ,like "http://www.my-server.com/myform"</param>
    /// <param name="wwwform">form������</param>
    /// <param name="actionResult">������� �������ص�����</param>
    /// <param name="header">����ͷ</param>
    /// <param name="a">�����ؽ����ί��</param>
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
    /// �ڶ��ַ�ʽ��������ύpost���� Ŀǰ����ֻ�����ַ�ʽ�ύjson�ɹ�
    /// </summary>
    /// <param name="serverURL">����������Ŀ���ַ,like "http://www.my-server.com/myform"</param>
    /// <param name="jsonStr">json�ַ���</param>
    /// <param name="actionResult">������� �������ص�����</param>
    /// <param name="a">�����ؽ����ί��</param>
    /// <param name="header">����ͷ</param>
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

