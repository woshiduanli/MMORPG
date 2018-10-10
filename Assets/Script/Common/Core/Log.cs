using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class LOG {
    
    #region format
    public static void Debug(string fmt, params object[] args) {
        if (!DebugSetting.EnableLog || (int)DebugSetting.Level < (int)DebugSetting.DebugLevel.Log)
            return;

        if (args.Length == 0)
            UnityEngine.Debug.Log(fmt);
        else
            UnityEngine.Debug.Log(string.Format(fmt, args));
    }

    public static void Warning(string fmt, params object[] args) {
        if (!DebugSetting.EnableLog || (int)DebugSetting.Level < (int)DebugSetting.DebugLevel.Waring)
            return;

        if (args.Length == 0)
            UnityEngine.Debug.LogWarning(fmt);
        else
            UnityEngine.Debug.LogWarning(string.Format(fmt, args));
    }

    public static void Erro(string fmt, params object[] args) {
        //if (!Application.isEditor)
        //    return;
        //if (!DebugSetting.EnableLog || (int)DebugSetting.Level < (int)DebugSetting.DebugLevel.Error)
        //    return;

        if (args.Length == 0)
            UnityEngine.Debug.LogError(fmt);
        else
            UnityEngine.Debug.LogError(string.Format(fmt, args));
    }
    #endregion


    #region context

    public static void Log(object message, UnityEngine.Object context = null) {
        if (!DebugSetting.EnableLog || (int)DebugSetting.Level < (int)DebugSetting.DebugLevel.Log)
            return;

        UnityEngine.Debug.Log(message, context);
    }

    public static void LogWarning(object message, UnityEngine.Object context = null) {
        if (!DebugSetting.EnableLog || (int)DebugSetting.Level < (int)DebugSetting.DebugLevel.Waring)
            return;

        UnityEngine.Debug.LogWarning(message, context);
    }

    public static void LogError(object message, UnityEngine.Object context = null) {
        //if (!DebugSetting.EnableLog || (int)DebugSetting.Level < (int)DebugSetting.DebugLevel.Error)
        //    return;

        UnityEngine.Debug.LogError(message, context);
    }

    public static void LogException(Exception exception, UnityEngine.Object context = null) {
        //if (!DebugSetting.EnableLog || (int)DebugSetting.Level < (int)DebugSetting.DebugLevel.Exception)
        //    return;

        UnityEngine.Debug.LogException(exception, context);
    }
    #endregion

    public static void Trace(string fmt, params object[] args) {
        Debug(fmt, args);
    }

    public static void TraceRed(string fmt, params object[] args) {
        Erro(fmt, args);
    }

    public static void Output(string fmt, params object[] args) {
        Debug(fmt, args);
    }
    /// <summary>
    /// <para>警告！！！！！！！！！！！！！！！！！！！！</para>
    /// <para>这个函数请谨慎使用，会实时打印出函数调用栈信息，类似 print( Exception.stackTrace )，</para>
    /// <para>调用栈目前只打印了，从调用这个函数开始的函数往下10层，并不是全部的栈信息。</para>
    /// <para>本函数只在windows pc下才可以被正确执行，其余平台等于普通log</para>
    /// </summary>
    /// <param name="fmt"></param>
    /// <param name="args"></param>
    public static void LogStackTrace(string fmt, params object[] args) {
        string str = "";
        if (args.Length == 0)
            str = fmt;
        else
            str = string.Format(fmt, args);
#if UNITY_STANDALONE_WIN
        str += "\n";
        System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(true);
        System.Diagnostics.StackFrame frame = null;
        for (int i = 1; i < 11; i++) {
            frame = st.GetFrame(i);
            str += frame.GetFileName() + " - ( " + frame.GetFileLineNumber() + " , " + frame.GetFileColumnNumber() + " ) : " + frame.GetMethod().Name + " \n";
        }

        Debug(str);
#else
        Debug(str);
#endif
    }

}

//输出设置
public class DebugSetting {
    public enum DebugLevel {
        None = 0,
        Log,
        Waring,
        Error,
        Exception,
        All,
    }

    #region menber
    private static DebugLevel debugLevel = DebugLevel.All;
    private static List<Application.LogCallback> unityLogCallBack = new List<Application.LogCallback>();
    private static DebugSetting _instance = null;
    #endregion

    #region public Method
    public static DebugSetting Instance {
        get {
            if (_instance == null)
                _instance = new DebugSetting();
            return _instance;
        }
    }

    public static bool EnableLog
    {
        get
        {
            //if (ConfigHelper.instance!=null)
            //    return ConfigHelper.instance.debug;

            return false;
        }
    }

    public static bool isRelease {
        get { return !EnableLog; }
    }

    public static DebugLevel Level {
        get {
            return debugLevel;
        }
        set { debugLevel = value; }
    }

    public static void RegisterLogCallback(Application.LogCallback handler) {
        unityLogCallBack.Add(handler);
    }

    public static void UnRegisterLogCallback(Application.LogCallback handler) {
        if (unityLogCallBack.Contains(handler))
            unityLogCallBack.Remove(handler);
    }
    #endregion

   public static void HandlerUnityLog(string condition, string stackTrace, LogType type) {
        for (int i = 0; i < unityLogCallBack.Count; i++) {
            if (unityLogCallBack[i] != null)
                unityLogCallBack[i](condition, stackTrace, type);
        }
    }
   //public static bool debug = true  ;// 项目上线会删除此字段和引用
}