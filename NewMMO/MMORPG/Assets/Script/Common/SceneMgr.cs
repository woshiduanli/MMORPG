using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement; 

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
        // 初始化场景， 进入scene_loading 
        CurrentSceneType = SceneType.LogOn;

        SceneManager.LoadScene("Scene_Loading"); 
    }

    /// <summary>
    /// 去城镇场景
    /// </summary>
    public void LoadToCity()
    {
        CurrentSceneType = SceneType.City;
        SceneManager.LoadScene("Scene_Loading");
    }
    public void LoadToShaMo()
    {
        CurrentSceneType = SceneType.ShaMo;
        SceneManager.LoadScene("Scene_Loading");
    }
}