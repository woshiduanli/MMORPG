using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChinarBreakpointRenewal : MonoBehaviour
{
    private bool _isStop;       //�Ƿ���ͣ

    public Slider ProgressBar; //������
    public Text SliderValue; //������ֵ
    private Button startBtn;    //��ʼ��ť
    private Button pauseBtn;    //��ͣ��ť
    public string Url = "http://www.linxinfa.test.mp4";

    /// <summary>
    /// ��ʼ��UI���漰����ť�󶨷���
    /// </summary>
    void Start()
    {
        //��ʼ�����������ı���
        ProgressBar.value = 0;
        SliderValue.text = "0.0%";
        startBtn = GameObject.Find("Start Button").GetComponent<Button>();
        startBtn.onClick.AddListener(OnClickStartDownload);
        pauseBtn = GameObject.Find("Pause Button").GetComponent<Button>();
        pauseBtn.onClick.AddListener(OnClickStop);
    }


    /// <summary>
    /// �ص���������ʼ����
    /// </summary>
    public void OnClickStartDownload()
    {
        // ע��������Ҫ��Application.persistentDataPath
        StartCoroutine(DownloadFile(Url, Application.streamingAssetsPath + "/MP4/test.mp4", CallBack));
    }


    /// <summary>
    /// Э�̣������ļ�
    /// </summary>
    /// <param name="url">������Web��ַ</param>
    /// <param name="filePath">�ļ�����·��</param>
    /// <param name="callBack">�������ɵĻص�����</param>
    /// <returns></returns>
    IEnumerator DownloadFile(string url, string filePath, Action callBack)
    {
        UnityWebRequest huwr = UnityWebRequest.Head(url); //Head�������Ի�ȡ���ļ���ȫ������
        yield return huwr.SendWebRequest();
        if (huwr.isNetworkError || huwr.isHttpError) //��������
        {
            Debug.Log(huwr.error); //���� ������Ϣ
        }
        else
        {
            long totalLength = long.Parse(huwr.GetResponseHeader("Content-Length")); //�����õ��ļ���ȫ������
            string dirPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dirPath)) //�ж�·���Ƿ�����
            {
                Directory.CreateDirectory(dirPath);
            }

            //����һ���ļ�����ָ��·��ΪfilePath,ģʽΪ�򿪻򴴽�������Ϊд��
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                long nowFileLength = fs.Length; //��ǰ�ļ�����
                Debug.Log(fs.Length);
                if (nowFileLength < totalLength)
                {
                    Debug.Log("��û��������");
                    fs.Seek(nowFileLength, SeekOrigin.Begin);       //��ͷ��ʼ����������Ϊ��ǰ�ļ�����
                    UnityWebRequest uwr = UnityWebRequest.Get(url); //����UnityWebRequest���󣬽�Url����
                    uwr.SetRequestHeader("Range", "bytes=" + nowFileLength + "-" + totalLength);
                    uwr.SendWebRequest();                      //��ʼ����
                    if (uwr.isNetworkError || uwr.isHttpError) //��������
                    {
                        Debug.Log(uwr.error); //���� ������Ϣ
                    }
                    else
                    {
                        long index = 0;     //�Ӹ���������������
                        while (!uwr.isDone) //ֻҪ����û�����ɣ�һֱִ�д�ѭ��
                        {
                            if (_isStop) break;
                            yield return null;
                            byte[] data = uwr.downloadHandler.data;
                            if (data != null)
                            {
                                long length = data.Length - index;
                                fs.Write(data, (int)index, (int)length); //д���ļ�
                                index += length;
                                nowFileLength += length;
                                ProgressBar.value = (float)nowFileLength / totalLength;
                                SliderValue.text = Math.Floor((float)nowFileLength / totalLength * 100) + "%";
                                if (nowFileLength >= totalLength) //��������������
                                {
                                    ProgressBar.value = 1; //�ı�Slider��ֵ
                                    SliderValue.text = 100 + "%";
                                    if (callBack!=null)
                                    {
                                        callBack.Invoke(); 
                                    }
                                    //callBack?.Invoke();
                                }
                            }
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
            pauseBtn.GetComponentInChildren<Text>().text = "��ͣ����";
            Debug.Log("��������");
            _isStop = !_isStop;
            OnClickStartDownload();
        }
        else
        {
            pauseBtn.GetComponentInChildren<Text>().text = "��������";
            Debug.Log("��ͣ����");
            _isStop = !_isStop;
        }
    }
}
