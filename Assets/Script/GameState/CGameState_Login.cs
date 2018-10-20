using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ZCore;

public class CGameState_Login : CGameState
{

    protected override void InitData()
    {
        MyDebug.debug("sssssssssssssssssssssssssssssssssssssssssssss");
        if (SceneManager.GetActiveScene().name == SceneName.LOGIN_SCENE)
            return;
        MyDebug.debug("ssssssssssssssssss4545sssssssssssssssssssssssssssss");

        FireEvent(new CEvent.Scene.LoadLevel(SceneName.LOGIN_SCENE));
        EventDispatcher.Instance.RegProto<RoleOperation_LogOnGameServerReturnProto>(10002, TestEvent);

    }

    RoleOperation_LogOnGameServerReturnProto test;
    private void TestEvent(RoleOperation_LogOnGameServerReturnProto test)
    {
        this.test = test;
     FireEvent(new CEvent.Scene.LoadLevel("SelectRole"));
        //ChangeState(GameState.SELECT_ROLE, test);
    }


}
