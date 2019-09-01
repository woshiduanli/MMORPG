
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneLoadingCtrl : MonoBehaviour
{
    /// <summary>
    /// UI场景控制器
    /// </summary>
    [SerializeField]
    private UISceneLoadingCtrl m_UILoadingCtrl;

    private AsyncOperation m_Async = null;

    /// <summary>
    /// 当前的进度
    /// </summary>
    private int m_CurrProgress = 0;

    void Start()
    {
        // 第一步进入loading场景， 第二步， 在loading条里面， 进行真正的场景的读取。此时， 进行的是异步的读取
        DelegateDefine.Instance.OnSceneLoadOk += OnSceneLoadOk;
        // 然后在loading条里面， 进行尝尽的切换
        LayerUIMgr.Instance.Reset();
        StartCoroutine(LoadingScene());

        UIViewUtil.Instance.CloseAll();
    }

    void OnSceneLoadOk()
    {
        Debug.LogError("OnSceneLoadOk1");




        if (m_UILoadingCtrl != null)
        {
            Debug.LogError("OnSceneLoadOk31");
            Object.Destroy(m_UILoadingCtrl.gameObject);
        }
    }

    private void OnDestroy()
    {
        DelegateDefine.Instance.OnSceneLoadOk -= OnSceneLoadOk;
    }

    private IEnumerator LoadingScene()
    {
        string strSceneName = string.Empty;
        switch (SceneMgr.Instance.CurrentSceneType)
        {
            case SceneType.LogOn:
                strSceneName = "Scene_LogOn";
                break;
            case SceneType.SelectRole:
                strSceneName = "Scene_SelectRole";
                break;
            case SceneType.WorldMap:

                WorldMapEntity entity = WorldMapDBModel.Instance.Get(SceneMgr.Instance.CurrWorldMapId);
                if (entity != null)
                    strSceneName = entity.SceneName;
                break;
            case SceneType.GameLevel:

                GameLevelEntity gameLevelEntity = GameLevelDBModel.Instance.Get(SceneMgr.Instance.CurGameLevelId);
                if (gameLevelEntity != null)
                    strSceneName = gameLevelEntity.SceneName;

                break;
        }
        if (string.IsNullOrEmpty(strSceneName)) yield break;
        //if (SceneMgr.Instance.CurrentSceneType == SceneType.City  )
        //{
        //    // 加载渲染场景的时候，本来是加载ab包的
        //    AssetBundleMgr.Instance.LoadAsync("scene/gamescene_cunzhuang.unity3d", "Scene_SelectRole").OnLoadComplete = (AssetObj) =>
        //    {
        //        m_Async = SceneManager.LoadSceneAsync(strSceneName, LoadSceneMode.Additive);
        //        m_Async.allowSceneActivation = false;
        //    };
        //}
        //else
        //{
        MyDebug.debug("要去的场景：" + strSceneName);
        m_Async = SceneManager.LoadSceneAsync(strSceneName, LoadSceneMode.Additive);
        m_Async.allowSceneActivation = false;
        yield return m_Async;
        //}

    }

    void Update()
    {
        if (m_Async == null) return;
        int toProgress = 0;

        if (m_Async.progress < 0.9f)
        {
            toProgress = Mathf.Clamp((int)m_Async.progress * 100, 1, 100);
        }
        else
        {
            toProgress = 100;
        }

        // 如果此时大于， 这个， 就加一个位置
        if (m_CurrProgress < toProgress)
        {
            m_CurrProgress++;
        }
        else
        {
            m_Async.allowSceneActivation = true;
        }

        m_UILoadingCtrl.SetProgressValue(m_CurrProgress * 0.01f);
    }
}