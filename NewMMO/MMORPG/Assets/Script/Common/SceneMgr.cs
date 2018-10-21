using UnityEngine;
using System.Collections;

/// <summary>
/// 场景管理器
/// </summary>
public class SceneMgr : Singleton<SceneMgr>
{
    /// <summary>
    /// 当前场景类型
    /// </summary>
    public SceneType CurrentSceneType
    {
        get;
        private set;
    }

    public void LoadToLogOn()
    {
        CurrentSceneType = SceneType.LogOn;
        
        Application.LoadLevel("Scene_Loading");
    }

    /// <summary>
    /// 去城镇场景
    /// </summary>
    public void LoadToCity()
    {
        CurrentSceneType = SceneType.City;
        Application.LoadLevel("Scene_Loading");
    }
}