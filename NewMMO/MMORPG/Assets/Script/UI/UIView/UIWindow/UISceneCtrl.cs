
using UnityEngine;
using System.Collections;

/// <summary>
/// 场景UI控制器
/// </summary>
public class UISceneCtrl : Singleton<UISceneCtrl>
{

    /// <summary>
    /// 场景UI类型
    /// </summary>
    public enum SceneUIType
    {
        /// <summary>
        /// 未定义
        /// </summary>
        None,
        /// <summary>
        /// 登录
        /// </summary>
        LogOn,
        /// <summary>
        /// 加载
        /// </summary>
        Loading,
        /// <summary>
        /// 选人场景
        /// </summary>
        SelectRole,
        /// <summary>
        /// 主城
        /// </summary>
        MainCity
    }

    /// <summary>
    /// 当前场景UI
    /// </summary>
    public UISceneViewBase CurrentUIScene;

    //public void LoadSceneUI(string path, XLuaCustomExport.OnCreate OnCreate)
    //{
    //    LoadSceneUI(SceneUIType.None, null, OnCreate, path);
    //}

    #region LoadSceneUI 加载场景UI
    /// <summary>
    /// 加载场景UI
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject LoadSceneUI(SceneUIType type)
    {
        GameObject obj = null;
        switch (type)
        {
            case SceneUIType.LogOn:
                obj = ResourcesMgr.Instance.Load(ResourcesMgr.ResourceType.UIScene, "UI Root_LogOnScene");
                CurrentUIScene = obj.GetComponent<UISceneLogonCtrl>();
                break;
            case SceneUIType.Loading:
                break;
            case SceneUIType.MainCity:
                obj = ResourcesMgr.Instance.Load(ResourcesMgr.ResourceType.UIScene, "UI Root_City");
                CurrentUIScene = obj.GetComponent<UISceneCityCtrl>();
                break; 
            case SceneUIType.SelectRole:
                obj = ResourcesMgr.Instance.Load(ResourcesMgr.ResourceType.UIScene, "UI_Root_SelectRole");
                CurrentUIScene = obj.GetComponent<UISceneCityCtrl>();
                break;
        }
        return obj;
    }
    #endregion
}