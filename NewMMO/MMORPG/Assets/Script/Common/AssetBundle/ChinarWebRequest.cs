using System.Collections;
using UnityEngine;
using System; 
using UnityEngine.Networking;

/// <summary>
/// 网络请求测试
/// </summary>
public class ChinarWebRequest : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(SendRequest());
    }

    /// <summary>
    /// 开启一个协程，发送请求
    /// </summary>
    /// <returns></returns>
    IEnumerator SendRequest()
    {
        //Uri uri = new Uri("http://www.baidu.com"); //Uri 是 System 命名空间下的一个类,注意引用该命名空间
        UnityWebRequest uwr = new UnityWebRequest("http://www.baidu.com");        //创建UnityWebRequest对象
        uwr.timeout = 5;
        UnityWebRequestAsyncOperation y =  uwr.SendWebRequest(); 
        yield return y;                     //等待返回请求的信息
     
        if (uwr.isHttpError || uwr.isNetworkError)             //如果其 请求失败，或是 网络错误
        {
            Debug.LogError(uwr.error); //打印错误原因
        }
        else //请求成功
        {

            Debug.LogError(uwr.responseCode); 

            UnityWebRequest t = y.webRequest;

           ; 
            Debug.Log("请求成功" + t.downloadHandler.text);
        }
    }
}
