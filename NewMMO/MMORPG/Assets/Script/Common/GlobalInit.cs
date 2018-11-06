
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public static class MyDebug
{
#if DEBUG_LOG
    public static System.Action<System.Object> debug = Debug.Log;
#else

    public static System.Action<System.Object> debug = Sysob;
#endif
    public static void Sysob(System.Object obj)
    {

    }

}

public class GlobalInit : MonoBehaviour
{
    #region 常量
    /// <summary>
    /// 昵称KEY
    /// </summary>
    public const string MMO_NICKNAME = "MMO_NICKNAME";

    /// <summary>
    /// 密码KEY
    /// </summary>
    public const string MMO_PWD = "MMO_PWD";

    public Shader MogoSkyboxShader; 
    #endregion

    public static GlobalInit Instance;

    [HideInInspector]
    public RoleInfoMainPlayer MainPlayerInfo;
    /// <summary>
    /// 玩家注册时候的昵称
    /// </summary>
    [HideInInspector]
    public string CurrRoleNickName;


    [HideInInspector]
    public long serverTime = 0;

    [HideInInspector]
    public Dictionary<int, GameObject> JobObjectDic = new Dictionary<int, GameObject>();
    public Shader T4MShader;

    [HideInInspector]
    public RoleCtrl CurrPlayer;

    [HideInInspector]
    public long CurServerTime
    {
        get
        {
            return (long)(serverTime + RealTime.time);
        }
    }

    [HideInInspector]
    public RetAccountEntity CurAccount;

    /// <summary>
    ///  当前选择的区服
    /// </summary>
    [HideInInspector]
    public RetGameServerEntity CurrSelectGameServer;


    private string m_clientDeviceID;
    /// <summary>
    ///  客户端设备id
    /// </summary>
    public string ClientDeviceID
    {
        get
        {
            if (!string.IsNullOrEmpty(m_clientDeviceID))
                return m_clientDeviceID;
            else
            {

            }
            return "";

        }
    }

  

    public string WebAccountUrl = @"http://localhost:8080/";

    /// <summary>
    /// UI动画曲线
    /// </summary>
    public AnimationCurve UIAnimationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        NetWorkHttp.Instance.SendData(WebAccountUrl + "api/time", OnGetTimeCallBack);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PlayerPrefs.DeleteAll();

        }


        //if (Input.GET)
        //{

        //}
    }

    private void OnGetTimeCallBack(NetWorkHttp.CallBackArgs obj)
    {
        if (!obj.HasError)
        {
            serverTime = long.Parse(obj.Value);
        }
    }
}