
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using DG.Tweening;
using LitJson;
//using DG;
//MyDebug.Debug = UnityEngine.de



/// <summary>
/// Http通讯管理
/// 
/// </summary>
public class NetWorkHttp : SingletonMono<NetWorkHttp>
{
    #region 属性
    /// <summary>
    /// Web请求回调
    /// </summary>
    //private XLuaCustomExport.NetWorkSendDataCallBack m_CallBack;
    System.Action<CallBackArgs> m_CallBack;
    /// <summary>
    /// Web请求回调数据
    /// </summary>
    private CallBackArgs m_CallBackArgs;

    /// <summary>
    /// 是否繁忙
    /// </summary>
    private bool m_IsBusy = false;

    /// <summary>
    /// 是否繁忙
    /// </summary>
    public bool IsBusy
    {
        get { return m_IsBusy; }
    }
    #endregion


    string url = "http://localhost:8081/api/account";
    protected override void OnStart()
    {
        //Debug.Log 
        //m_CallBack = CallBack;

        m_CallBackArgs = new CallBackArgs();
    }

    #region SendData 发送web数据
    /// <summary>
    /// 发送web数据
    /// </summary>
    /// <param name="url"></param>
    /// <param name="callBack"></param>
    /// <param name="isPost"></param>
    /// <param name="json"></param>
    public void SendData(string url, System.Action<CallBackArgs> m_CallBack = null, bool isPost = false, string json = null)
    {
        if (m_IsBusy) return;

        m_IsBusy = true;
        this.m_CallBack = m_CallBack;

        if (!isPost)
        {
            // get请求
            GetUrl(url);
        }
        else
        {
            JsonData dic = JsonMapper.ToObject(json);
            //web加密
            if (dic != null)
            {
                //客户端标识符
                dic["deviceIdentifier"] = DeviceUtil.DeviceIdentifier;


                //设备型号
                dic["deviceModel"] = DeviceUtil.DeviceModel;
                long t = GlobalInit.Instance.CurServerTime;
                // 服务器时间
                //签名
                dic["sign"] = EncryptUtil.Md5(string.Format("{0}:{1}", t, DeviceUtil.DeviceIdentifier));

                //时间戳
                dic["t"] = t;
            }

            PostUrl(url, dic == null ? "" : JsonMapper.ToJson(dic));
        }
    }
    #endregion

    #region GetUrl Get请求
    /// <summary>
    /// Get请求
    /// </summary>
    /// <param name="url"></param>
    private void GetUrl(string url)
    {
        WWW data = new WWW(url);
        StartCoroutine(Request(data));
    }
    #endregion

    #region PostUrl Post请求
    /// <summary>
    /// Post请求
    /// </summary>
    /// <param name="url"></param>
    /// <param name="json"></param>
    private void PostUrl(string url, string json)
    {
        //定义一个表单
        WWWForm form = new WWWForm();

        //给表单添加值
        form.AddField("", json);

        WWW data = new WWW(url, form);
        StartCoroutine(Request(data));
    }
    #endregion

    #region Request 请求服务器
    /// <summary>
    /// 请求服务器
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    private IEnumerator Request(WWW data)
    {
        yield return data;

        m_IsBusy = false;
        if (string.IsNullOrEmpty(data.error))
        {
            if (data.text == "null")
            {
                if (m_CallBack != null)
                {
                    m_CallBackArgs.HasError = true;
                    m_CallBackArgs.ErrorMsg = "未请求到数据";
                    m_CallBack(m_CallBackArgs);
                }
            }
            else
            {
                if (m_CallBack != null)
                {
                    m_CallBackArgs.HasError = false;
                    m_CallBackArgs.Value = data.text;
                    m_CallBack(m_CallBackArgs);
                }
            }
        }
        else
        {
            Debug.Log("data.error=" + data.error);
            if (m_CallBack != null)
            {
                m_CallBackArgs.HasError = true;
                m_CallBackArgs.ErrorMsg = data.error;
                m_CallBack(m_CallBackArgs);
            }
        }
    }
    #endregion

    #region CallBackArgs Web请求回调数据
    /// <summary>
    /// Web请求回调数据
    /// </summary>
    public class CallBackArgs : EventArgs
    {
        /// <summary>
        /// 是否有错
        /// </summary>
        public bool HasError;

        /// <summary>
        /// 错误原因
        /// </summary>
        public string ErrorMsg;

        /// <summary>
        /// 返回值
        /// </summary>
        public string Value;

        public string Type;

    }
    #endregion
}