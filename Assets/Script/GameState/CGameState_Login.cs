using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZCore;

public class CGameState_Login : CGameState
{

    protected override void InitData()
    {
        if (SceneManager.GetActiveScene().name == SceneName.LOGIN_SCENE)
            return;
        FireEvent(new CEvent.Scene.LoadLevel(SceneName.LOGIN_SCENE));
        //EventDispatcher.Instance.RegProto<RoleOperation_LogOnGameServerReturnProto>(10002, TestEvent);

    }

    private void TestEvent(RoleOperation_LogOnGameServerReturnProto test)
    {
        ChangeState(GameState.SELECT_ROLE, test);
    }

    protected override void ChangeState(GameState state, params object[] args)
    {
        if (state != GameState.SELECT_ROLE)
            return;
        base.ChangeState(state, args);
        CreateSingleT<CGameState_Select>(args);
    }
}
