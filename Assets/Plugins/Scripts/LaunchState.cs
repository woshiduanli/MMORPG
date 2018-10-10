using UnityEngine;
using System.Collections;

public abstract class LaunchState
{
    public string DownLoad { protected set; get; }
    public float ProgressValue { protected set; get; }
    public string Tips { protected set; get; }
    protected Launcher launcher = null;
    public LaunchState(Launcher launcher) { this.launcher = launcher; }
    public bool pause { get; private set; }
    public void SetPause(bool pause) { this.pause = pause; }
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
    public abstract LaunchState CheckTransition();

    public static LaunchState Create(Launcher launcher) { return new InitState(launcher); }

}

