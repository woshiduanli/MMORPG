
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
    }

    void OnSceneLoadOk()
    {
        if (m_UILoadingCtrl != null)
        {
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
                m_Async = SceneManager.LoadSceneAsync(strSceneName, LoadSceneMode.Single);
                break;
            case SceneType.City:
                strSceneName = "GameScene_CunZhuang";
                m_Async = SceneManager.LoadSceneAsync(strSceneName, LoadSceneMode.Additive);
                break;
        }

        //m_Async = SceneManager/.LoadSceneAsync(strSceneName, LoadSceneMode.Additive);
        m_Async.allowSceneActivation = false;
        yield return m_Async;
    }

    void Update()
    {
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