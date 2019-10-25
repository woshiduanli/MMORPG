using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChinarBreakpointRenewal : MonoBehaviour
{
    private bool _isStop; //�Ƿ���ͣ

    public Slider ProgressBar; //������
    public Text SliderValue; //������ֵ
    private Button startBtn; //��ʼ��ť
    private Button pauseBtn; //��ͣ��ť
    public string Url;

    private string strName = "";

    /// <summary>
    /// ��ʼ��UI���漰����ť�󶨷���
    /// </summary>
    void Start()
    {
        strName = "resource.zip";
        Url = "http://a769135040.gnway.cc:40061/resource/" + strName;

        //��ʼ�����������ı���
//        ProgressBar.value = 0;
//        SliderValue.text = "0.0%";
//        startBtn = GameObject.Find("Start Button").GetComponent<Button>();
//        startBtn.onClick.AddListener(OnClickStartDownload);
//        pauseBtn = GameObject.Find("Pause Button").GetComponent<Button>();
//        pauseBtn.onClick.AddListener(OnCli2ckStop);
        OnClickStartDownload();
    }


    /// <summary>
    ///
    /// </summary>
    public void OnClickStartDownload()
    {
        // Application.persistentDataPath
        StartCoroutine(DownloadFile(Url, Application.persistentDataPath + "/" + strName, null));
    }


    IEnumerator _DownloadFile2(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult = null)
    {
        var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
//        uwr.downloadHandler = new DownloadHandlerFile(downloadFilePathAndName);

        uwr.downloadHandler = new DownloadHandlerBuffer();


        uwr.SetRequestHeader("Range", "bytes=" + 10000000 + "-");
        UnityWebRequestAsyncOperation yi = uwr.SendWebRequest();

        while (yi.isDone == false)
        {
            yield return null;
            byte[] dddd = uwr.downloadHandler.data;

            Debug.Log("长度:" + dddd.Length);
        }

        Debug.Log("最后长度：" + uwr.downloadHandler.data.Length);
        Debug.Log("完成");
    }

    public float progross2;
    /// <summary>
    ///  百分比
    /// </summary>
    public string progrossPrecent;
    /// <summary>
    ///
    /// </summary>
    /// <param name="url">������Web��ַ</param>
    /// <param name="filePath">�ļ�����·��</param>
    /// <param name="callBack">�������ɵĻص�����</param>
    /// <returns></returns>
    IEnumerator DownloadFile(string url, string filePath, Action callBack)
    {
        UnityWebRequest huwr = UnityWebRequest.Head(url); //Head�������Ի�ȡ���ļ���ȫ������
        yield return huwr.SendWebRequest();
        if (huwr.isNetworkError || huwr.isHttpError || !string.IsNullOrEmpty(huwr.error)) //��������
        {
            // 判断网络是不是通的  ，说明不通
            Debug.LogError("error net is not connect");
        }
        else
        {
            long totalLength = long.Parse(huwr.GetResponseHeader("Content-Length")); //�����õ��ļ���ȫ������
            string dirPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }


            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                long nowFileLength = fs.Length;
                long downLoadLength = 0;
                Debug.Log(fs.Length);
                if (nowFileLength < totalLength)
                {
                    Debug.Log("进入下载");
                    fs.Seek(nowFileLength, SeekOrigin.Begin); 
                    UnityWebRequest uwr = UnityWebRequest.Get(url);
                    uwr.SetRequestHeader("Range", "bytes=" + nowFileLength + "-");
                    Debug.Log("现在的长度：" + nowFileLength + "  " + totalLength);

                    uwr.SendWebRequest(); //
                    if (uwr.isNetworkError || uwr.isHttpError) 
                    {
                        Debug.Log("报错了");
                    }
                    else
                    {
                        long index = 0; //
                        while (uwr.isDone == false) //
                        {
                            if (_isStop) break;
                            for (int i = 0; i < 5; i++)
                                yield return null;
                            byte[] data = uwr.downloadHandler.data;

                            if (data != null)
                            {
                                long length = data.Length - index;
                                fs.Write(data, (int) index, (int) length); //д���ļ�
                                index += length;
                                nowFileLength += length;
                                progross2 = (float) nowFileLength / totalLength;
                                Debug.Log("下载百分比："+ progross2);
                                progrossPrecent = Math.Floor((float) nowFileLength / totalLength * 100) + "%";
                                if (nowFileLength >= totalLength) //��������������
                                {
//                                    ProgressBar.value = 1; //�ı�Slider��ֵ
//                                    SliderValue.text = 100 + "%";
                                    if (callBack != null)
                                    {
                                        callBack.Invoke();
                                    }

                                    //callBack?.Invoke();
                                }
                            }
                        }

                        if (uwr.isDone && totalLength != nowFileLength)
                        {
                            StartCoroutine(DownloadFile(url, filePath, null));
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// �������ɺ��Ļص�����
    /// </summary>
    void CallBack()
    {
        Debug.Log("��������");
    }

    /// <summary>
    /// ��ͣ����
    /// </summary>
    public void OnClickStop()
    {
        if (_isStop)
        {
            pauseBtn.GetComponentInChildren<Text>().text = "readP";
            Debug.Log("��������");
            _isStop = !_isStop;
            OnClickStartDownload();
        }
        else
        {
            pauseBtn.GetComponentInChildren<Text>().text = "toStar";
            Debug.Log("��ͣ����");
            _isStop = !_isStop;
        }
    }
}