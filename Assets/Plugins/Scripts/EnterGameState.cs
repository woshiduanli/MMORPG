using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;


public class EnterGameState : LaunchState
{
    public EnterGameState(Launcher launcher) : base(launcher) { }
    public override void Enter()
    {
        InitEventSystem() ;
        LoadGameData(); 
    }
    private void InitEventSystem()
    {
        GameObject eventsystem = new GameObject("EventSystem");
        Object.DontDestroyOnLoad(eventsystem);
        eventsystem.AddComponent<EventSystem>();
        StandaloneInputModule sim = eventsystem.AddComponent<StandaloneInputModule>();
        sim.forceModuleActive = true;
    }

    protected virtual void LoadGameData()
    {
        string path = CDirectory.MakeCachePath(ConstCopyFilePath.language);
        if (!File.Exists(path)) File.Create(path); 
        byte[] bytes = File.ReadAllBytes(path);
        Localization.language = "language";
        Localization.LoadCSV(bytes);
#if !UNITY_IPHONE
        //if (Application.isEditor)
        //    PCLoadAssembly();
        //else
        //    MobileLoadAssembly();
#endif
       
    }


    public override void Exit()
    {
    }

    public override void Update()
    {
    }

    public override LaunchState CheckTransition()
    {
        return null;
    }
}
