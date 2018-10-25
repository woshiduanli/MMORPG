using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageCtrl : Singleton<MessageCtrl>
{
    GameObject obj;
    public void Show(string title, string message, MessageViewType type = MessageViewType.Ok, System.Action Okaction = null, System.Action cancelAction = null)
    {
        if (obj == null)
        {
            obj = ResourcesMgr.Instance.Load(ResourcesMgr.ResourceType.UIWindow, "pan_Message", true);
            obj.transform.parent = SceneUIMgr.Instance.CurrentUIScene.Container_Center; 
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<RectTransform>().sizeDelta = Vector2.zero; 
            obj.gameObject.SetActive(true);
        }

        UIMessageView view = obj.GetComponent<UIMessageView>();
        if (view != null)
        {
            view.Show(title, message, type, Okaction, cancelAction);
        }
    }
}
