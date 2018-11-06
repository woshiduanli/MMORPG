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

    private int m_CurrWorldMapId;

    public int CurrWorldMapId
    {
        get { return m_CurrWorldMapId; }
        private set { }
    }
    /// <summary>
    /// 去城镇场景
    /// </summary>
    public void LoadToWorldMap(int WorldMapId = 1)
    {
        m_CurrWorldMapId = WorldMapId;
        CurrentSceneType = SceneType.WorldMap;
        SceneManager.LoadScene("Scene_Loading");
    }

    public void LoadToSelectRole()
    {
        CurrentSceneType = SceneType.SelectRole;
        SceneManager.LoadScene("Scene_Loading");
    }

    public void LoadToShaMo()
    {
        CurrentSceneType = SceneType.ShaMo;
        SceneManager.LoadScene("Scene_Loading");
    }


    private int m_gameLevelId;

    public int CurGameLevelId
    {
        get { return m_gameLevelId; }
        private set { }
    }

    private GameLevelGrade m_CurrGameLevelGrade;

    public GameLevelGrade CurrGameLevelGrade
    {
        get { return m_CurrGameLevelGrade; }
        private set { }
    }

    // 去游戏关卡
    public void LoadToGameLevel(int gameLevelId, GameLevelGrade grade)
    {
        this.m_CurrGameLevelGrade = grade;
        this.m_gameLevelId = gameLevelId;

        // 初始化场景， 进入scene_loading 
        CurrentSceneType = SceneType.GameLevel;
        SceneManager.LoadScene("Scene_Loading");
    }
}