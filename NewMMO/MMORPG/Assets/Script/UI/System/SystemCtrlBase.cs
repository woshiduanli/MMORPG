using UnityEngine;
using System.Collections;
using System;

public class SystemCtrlBase<T> : IDisposable where T : new()
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new T();
            }
            return instance;
        }
    }

    public virtual void Dispose()
    {

    }


    protected virtual void ShowMessage(string title, string message, MessageViewType type = MessageViewType.Ok, System.Action Okaction = null, System.Action cancelAction = null)
    {
        MessageCtrl.Instance.Show(title, message, type, Okaction, cancelAction);
    }

    public void AddEventListener(string key, UIDispatcher.OnActionHandler handler)
    {
        UIDispatcher.Instance.AddEventListener(key, handler);
    }

    public void RemoveEventListener(string key, UIDispatcher.OnActionHandler handler)
    {
        UIDispatcher.Instance.RemoveEventListener(key, handler);
    }
}