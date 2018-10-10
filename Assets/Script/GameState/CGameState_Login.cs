using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class CGameState_Login : CGameState
{

    protected override void InitData()
    {
        if (SceneManager.GetActiveScene().name == SceneName.LOGIN_SCENE)
            return;

        //

        FireEvent(new CEvent.Scene.LoadLevel(SceneName.LOGIN_SCENE));
    }
}
