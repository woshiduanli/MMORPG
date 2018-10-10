using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore;

public class CObjectData : CObject
{
    //public CMainPlayer Mp
    //{
    //    get
    //    {
    //        if (!Mp_)
    //            Mp_ = this.GetSingleT<CMainPlayer>();
    //        return Mp_;
    //    }
    //}
    //private CMainPlayer Mp_;

    //public CWorld World
    //{
    //    get
    //    {
    //        return CWorld.instance;
    //    }
    //}

    protected virtual void RegEvents() { }

    protected virtual void InitData() { }

    public override void Initialize()
    {
        base.Initialize();
        InitData();
        RegEvents();
    }
}
