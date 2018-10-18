using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZCore;

public class CSceneManager : CLoopObject
{

    protected override void Dispose(bool disposed)
    {
        base.Dispose(disposed);
    }
    /// <summary>
    /// 目标场景
    /// </summary>
    public string targetLevel;
    /// <summary>
    /// 预加载下个场景的资源列表
    /// </summary>
    public List<string> PreloadAssets;
    /// <summary>
    /// 是否等待配置表加载完成
    /// </summary>
    public bool WaitRef;


    protected override void RegEvents()
    {
        RegEvent<CEvent.Scene.LoadLevel>(OnLoadLevel);
    }

    private bool loading;
    private void OnLoadLevel(CObject sender, CEvent.Scene.LoadLevel e)
    {
        if (loading)
            return;

        loading = true;
        // 切换场景， 清理ui 
        FireEvent(new CEvent.UI.DisposeEvent(e.sceneName));
        FireEvent(new CEvent.Scene.LoadLevelBegin(e.sceneName));

        //Progress.Instance.Dispose();
        this.targetLevel = e.sceneName;
        this.PreloadAssets = e.Assets;
        this.WaitRef = e.WaitRef;
        if (PreloadAssets == null && !e.WaitRef && SceneName.InUnity(targetLevel))
        {
            FireEvent(new CEvent.Scene.LoadSceneBegin(this.targetLevel));
            SceneManager.LoadScene(this.targetLevel);
            return;
        }

        //2018-8-10 成娟修改过图紫色
        CAsyncLevelLoaderUI.Create();

        //当前地图--》过图场景--》目标地图
        FireEvent(new CEvent.Scene.LoadSceneBegin(SceneName.ASYNC_LOADER_SCENE));
        SceneManager.LoadScene(SceneName.ASYNC_LOADER_SCENE);
    }

    // 场景加载完成以后， 执行的回调
    public override void OnLevelWasLoaded()
    {
        string ActiveName = SceneManager.GetActiveScene().name;
        //2018-8-10 成娟修改过图错误
        if (ActiveName == this.targetLevel)
            loading = false;
        Progress.Instance.Dispose();
        if (ActiveName == SceneName.ASYNC_LOADER_SCENE)
        {

            SceneManager.LoadScene(targetLevel);
            //      
            //DisposeSceneAsset(ref CacheSceneAssets);
            //DisposeSceneAsset(ref CurSceneAssets);
            //if (SceneName.InUnity(targetLevel))
            //{
            //    if (this.PreloadAssets != null || this.WaitRef)
            //        CurSceneAssets = new CSceneAssets(this);
            //    else
            //        SceneManager.LoadScene(targetLevel);
            //}
            //else
            //{
            //    CurSceneAssets = new CSceneAssets(this, this.World.Ref);
            //}
        }
        FireEvent(new CEvent.Scene.LevelWasLoaded(ActiveName, targetLevel));
    }
}


/// <summary>
/// 场景名称
/// </summary>
public class SceneName
{
    public const string lAUNCHER = "launcher";
    /// <summary>
    /// 登陆
    /// </summary>
    public const string LOGIN_SCENE = "Login";

    /// <summary>
    /// 异步加载场景名称
    /// </summary>
    public const string ASYNC_LOADER_SCENE = "AsyncLevelLoader";

    /// <summary>
    /// 选择角色界面
    /// </summary>
    public static string ROLE_SELECT_SCENE = "SelectRole";
    /// <summary>
    /// 选择角色-法师
    /// </summary>
    //public const string ROLE_SELECT_SCENE_MAGE = "Mage";
    /// <summary>
    /// 选择角色-血族
    /// </summary>
    //public const string ROLE_SELECT_SCENE_VAMPIRE = "Vampire";
    /// <summary>
    /// 选择角色-战士
    /// </summary>
    //public const string ROLE_SELECT_SCENE_WARRIOR = "Warrior";

    public static bool InUnity(string scene)
    {
        return scene == LOGIN_SCENE || scene == ASYNC_LOADER_SCENE || scene == ROLE_SELECT_SCENE;
    }
}