using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CGameManager : CLoopObject
{
    protected override void InitData()
    {
        CreateSingleT<CCoroutineEngine>();
        CreateSingleT<CUIManager>();
        Global.InitData(GetSingleT<CUIManager>());
        CreateSingleT<XLuaManager>();
        CreateSingleT<CSceneManager>();
        CreateSingleT<CResourceFactory>();
        CreateSingleT<CGameState_Init>();
        //List<JobEntity> list =  JobDBModel.Instance.GetList();
        //for (int i = 0; i < list.Count; i++)
        //{
        //    MyDebug.debug("等级："+ list[i].Name);
        //}
    }
}
