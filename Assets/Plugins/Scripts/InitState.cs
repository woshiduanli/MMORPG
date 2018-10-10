using UnityEngine;
using System.Collections;

internal class InitState : LaunchState
{
    public override void Enter()
    {



    }
    public InitState(Launcher launcher) : base(launcher) { }
    public override void Exit()
    {
    }

    public override void Update()
    {
    }

    public override LaunchState CheckTransition()
    {
        return new EnterGameState(this.launcher);
    }
}
