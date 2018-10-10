using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ZCore; 

public class CGame : IGame
{
    public CObjectManager obj_mgr { private set; get; }
    public void StartGame()
    {
        Progress.Instance.Dispose();
        CategorySettings.Initialize(new List<string>());
        //OutLog.Create();
        //FPS.Create();
        obj_mgr = new CObjectManager();
        obj_mgr.CreateSingleT<CGameManager>();
    }
}

