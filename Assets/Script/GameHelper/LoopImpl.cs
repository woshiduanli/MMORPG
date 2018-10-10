using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using ZCore;
/*! CLoopImpl
\brief
   Loop驱动的实现，提供仿unity的接口Update、LateUpdate、OnLevelWasLoaded
*/
public class CLoopImpl : MonoBehaviour
{
    public ILoopInterface loop;

    void Awake()
    {
       SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void SetRepeat(float time, float repeatRate)
    {
        this.InvokeRepeating("RateUpdate", time, repeatRate);
    }

    void Update()
    {
        this.loop.Update();
    }

    void LateUpdate()
    {
        this.loop.LateUpdate();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Single)
            this.loop.OnLevelWasLoaded();
        else
            this.loop.OnLevelWasAddLoaded(scene);
    }

    void OnApplicationQuit()
    {
        this.loop.OnApplicationQuit();
    }

    void OnApplicationPause(bool pause)
    {
        this.loop.OnApplicationPause(pause);
    }

    void OnDestroy()
    {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void RateUpdate()
    {
        this.loop.RateUpdate();
    }
}


public interface ILoopInterface
{
    void RateUpdate();
    void Update();
    void LateUpdate();
    void OnLevelWasLoaded();
    void OnLevelWasAddLoaded(Scene scene);
    void OnApplicationQuit();
    void OnApplicationPause(bool pause);

}


/*! CLoopObject
\brief
  Loop驱动对象基类
*/
public abstract class CLoopObject : CObject, ILoopInterface
{
    public bool isInGame { private set; get; }
    private TimerEvent timer;
    private CLoopImpl impl;
    public CLoopObject()
    {
        GameObject go = new GameObject(CString.Concat(GetType().Name, "Loop"));
        UnityEngine.Object.DontDestroyOnLoad(go);
        this.impl = go.AddComponent(typeof(CLoopImpl)) as CLoopImpl;
        this.impl.loop = this;

        CategorySettings.Attach(go.transform, "_loops/", false);
    }

    protected void SetRepeat(float time, float repeatRate)
    {
        if (this.impl)
            this.impl.SetRepeat(time, repeatRate);
    }


    public override void Initialize()
    {
        base.Initialize();
        InitData();
        RegEvents();
    }

    protected virtual void RegEvents() { }

    protected GameObject GetGameObject()
    {
        if (this.impl == null)
            return null;
        else
            return this.impl.gameObject;
    }
    protected void CheckGameState()
    {
        this.isInGame = false;
        //CGameState_Game game = GetSingleT<CGameState_Game>();
        //if (!game)
        return;
        string ActiveName = SceneManager.GetActiveScene().name;
        this.isInGame = ActiveName != SceneName.lAUNCHER &&
                        ActiveName != SceneName.LOGIN_SCENE &&
                        ActiveName != SceneName.ASYNC_LOADER_SCENE &&
                        ActiveName != SceneName.ROLE_SELECT_SCENE;
    }

    public void StopAllCoroutines()
    {
        if (this.impl != null)
            this.impl.StopAllCoroutines();

    }

    public void StopCoroutine(IEnumerator routine)
    {
        if (this.impl != null)
            this.impl.StopCoroutine(routine);
    }

    public void StopCoroutine(Coroutine routine)
    {
        if (this.impl != null)
            this.impl.StopCoroutine(routine);
    }

    protected override void Dispose(bool whatever)
    {
        base.Dispose();
        if (this.impl != null)
        {
            UnityEngine.Object.Destroy(this.impl.gameObject);
            this.impl = null;
        }
        if (timer != null)
        {
            timer.Clear();
            timer = null;
        }
    }



    public virtual void RateUpdate() { }
    protected virtual void OnUpdate() { }
    public virtual void LateUpdate() { }
    public virtual void OnLevelWasLoaded() { }
    protected virtual void InitData() { }
    public virtual void OnApplicationPause(bool pause) { }


    ////--------------------------------------------------------------------
    public TimerEventObject AddTimer(TimerEventObject.TimerProc proc, object obj, int p1, int p2)
    {
        return AddTimer(1, 1, proc, obj, p1, p2);
    }

    public TimerEventObject AddTimer(float time, TimerEventObject.TimerProc proc, object obj, int p1, int p2)
    {
        int frame = Convert.ToInt32(Application.targetFrameRate * time);
        return AddTimer(frame, frame, proc, obj, p1, p2);
    }

    private TimerEventObject AddTimer(int start, int interval, TimerEventObject.TimerProc proc, object obj, int p1, int p2)
    {
        if (timer == null)
            timer = new TimerEvent(1);
        return timer.Add(start, interval, proc, obj, p1, p2);
    }

    public void RemoveTimer(TimerEventObject proc)
    {
        if (timer == null)
            return;
        timer.Remove(proc);
    }


    protected bool NeedUpdate = true;
    public void Update()
    {
        if (timer != null)
            timer.Process();

        if (NeedUpdate)
            OnUpdate();
    }

    public void OnLevelWasAddLoaded(Scene scene)
    {
    }

    public void OnApplicationQuit()
    {
    }

    public Coroutine StartCoroutine(IEnumerator routine)
    {
        if (this.impl != null)
            return this.impl.StartCoroutine(routine);
        else
            return null;
    }

}

