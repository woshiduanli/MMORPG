
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIGameServerEnterView : UIWindowViewBase
{
    public Text lblDefaultGameServer;

    public void SetUI(string gameServerName)
    {
        lblDefaultGameServer.text = gameServerName;
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnSelectGameServer":
                UIDispatcher.Instance.Dispatch(ConstDefine.UIGmeServerEnterView_btnSelectGameServer);
                break;
            case "btnEnterGame":
                UIDispatcher.Instance.Dispatch(ConstDefine.UIGmeServerEnterView_btnEnterGame);
                break;
        }
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        lblDefaultGameServer = null;
    }
}