using UnityEngine;
using System.Collections;

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
    }
}
