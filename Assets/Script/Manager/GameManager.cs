using UnityEngine;
using System.Collections;

public class CGameManager : CLoopObject
{
    protected override void InitData()
    {
        CreateSingleT<CCoroutineEngine>();
        CreateSingleT<XLuaManager>();
        CreateSingleT<CSceneManager>();
      
        CreateSingleT<CResourceFactory>(); 
        CreateSingleT<CUIManager>();
        CreateSingleT<CGameState_Init>();
        Global.InitData(GetSingleT<CUIManager>()); 
    }
}
