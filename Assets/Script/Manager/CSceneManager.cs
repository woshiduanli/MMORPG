using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZCore;
using LitJson;
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
        RegEvent<CEvent.Scene.LuaLoadLevel>(OnLuaLoadLevel);
    }

    private void OnLuaLoadLevel(CObject sender, CEvent.Scene.LuaLoadLevel e)
    {
        CEvent.Scene.LoadLevel level2 = JsonMapper.ToObject<CEvent.Scene.LoadLevel>(e.json);
        OnLoadLevel(null, level2);
    }

    private bool loading;
    private void OnLoadLevel(CObject sender, CEvent.Scene.LoadLevel e)
    {
        // 如果正在读取场景， 就结束
        if (loading)
            return;


        loading = true;
        // 切换场景之前， 清除ui 
        FireEvent(new CEvent.UI.DisposeEvent(e.sceneName));
        // 发送开始读取场景事件 
        FireEvent(new CEvent.Scene.LoadLevelBegin(e.sceneName));

        // 清除掉progress 
        Progress.Instance.Dispose();

        this.targetLevel = e.sceneName;
        //预先加载的资源名字， 是一个list集合
        this.PreloadAssets = e.Assets;
        // 是否等待配置表加载完成
        this.WaitRef = e.WaitRef;
        if (PreloadAssets == null && !e.WaitRef && SceneName.InUnity(targetLevel))
        {
            // 如果目标场景是login, select ays三个， 预加载为空， 不需要等待配置表加载完成， 那么直接读取这个三个login, asy , selectrole中的一个
            FireEvent(new CEvent.Scene.LoadSceneBegin(this.targetLevel));
            SceneManager.LoadScene(this.targetLevel);
            return;
        }

        //2018-8-10 成娟修改过图紫色
        CAsyncLevelLoaderUI.Create();

        //当前地图--》过图场景--》目标地图， 发送一个过图场景， 加
        FireEvent(new CEvent.Scene.LoadSceneBegin(SceneName.ASYNC_LOADER_SCENE));
        // 否则加载asy场景
        SceneManager.LoadScene(SceneName.ASYNC_LOADER_SCENE);
    }

    //private CSceneAssets CurSceneAssets;
    //private CSceneAssets CacheSceneAssets;

    // 场景加载完成以后， 执行的回调
    public override void OnLevelWasLoaded()
    {
        string ActiveName = SceneManager.GetActiveScene().name;
        //2018-8-10 成娟修改过图错误
        //1.1 如果此时等于目标场景， 说明已经到达了目标场景， 开启读取功能
        if (ActiveName == this.targetLevel)
            loading = false;
        //1.1 关闭进度条
        Progress.Instance.Dispose();

        //2.1 如果此时是asy场景
        if (ActiveName == SceneName.ASYNC_LOADER_SCENE)
        {

            //2.2  释放预先加载的资源
            //DisposeSceneAsset(ref CacheSceneAssets);
            //DisposeSceneAsset(ref CurSceneAssets);

            //2.3 如果是在此时再login selectrole asy中， 
            if (SceneName.InUnity(targetLevel))
            {
                //2.4 并且此时有预先加载的资源， 那么加载这些资源
                if (this.PreloadAssets != null || this.WaitRef)
                {

                    //CurSceneAssets = new CSceneAssets(this);
                }
                else
                    // 2.5 否则加载目标场景
                    SceneManager.LoadScene(targetLevel);
            }
            else
            {
                //CurSceneAssets = new CSceneAssets(this, this.World.Ref);
            }
        }
        FireEvent(new CEvent.Scene.LevelWasLoaded(ActiveName, targetLevel));
    }
    //private void DisposeSceneAsset(ref CSceneAssets asset)
    //{
    //    //2018-8-10 成娟修改过图紫色
    //    if (Camera.main)
    //    {
    //        Camera.main.cullingMask = 0;
    //        Camera.main.clearFlags = CameraClearFlags.SolidColor;
    //        Camera.main.backgroundColor = Color.black;
    //    }
    //    if (asset != null)
    //        asset.Dispose();
    //    asset = null;
    //}
}

//public CSceneAssets(CSceneManager sm)
//{
//    Progress.Instance.SetProgressPercent(96, 4);
//    CCoroutineEngine coroutine = sm.GetSingleT<CCoroutineEngine>();
//    if (sm.PreloadAssets != null)
//    {
//        for (int i = 0; i < sm.PreloadAssets.Count; i++)
//        {
//            CSceneAsset sceneAsset = CResourceFactory.CreateInstance<CSceneAsset>(sm.PreloadAssets[i], null, PLevel.Low);
//            SceneAssets.Add(sceneAsset);
//            coroutine.AddQueueCmd(sceneAsset, true);
//        }
//    }
//    coroutine.AddQueueCmd(new WaitReference(sm.GetSingleT<CReferenceManager>()), true);
//    coroutine.AddQueueCmd(new UnloadUnusedAssets(), true);
//    coroutine.AddQueueCmd(new CScene(sm.targetLevel), true);
//}


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