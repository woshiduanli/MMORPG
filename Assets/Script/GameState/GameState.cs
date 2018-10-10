using System;
using System.Collections.Generic;
using ZCore; 

public enum GameState
{
    INIT,
    CHECK,//MSDK验证流程
    LOGIN,
    SELECT_ROLE,
    GAME,
    AutoConnect,//断线重连
}

/// <summary>
/// 游戏状态管理
/// </summary>
public abstract class CGameState : CLoopObject
{
    public static CGameState Current = null;

    protected List<IDisposable> Disposabledic = new List<IDisposable>();
    public CGameState()
    {
        //if (Current != null)
        //    Current.Dispose();
        //Current = this;
    }

    protected virtual void ChangeState(GameState state, ObjArgs objs)
    {
        //for (int i = 0; i < Disposabledic.Count; i++)
        //    Disposabledic[i].Dispose();
        //Disposabledic.Clear();
        //if (Current)
        //    Current.Dispose();
    }
}