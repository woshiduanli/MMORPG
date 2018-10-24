
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UILogOnView : UIWindowViewBase
{
    public InputField txtUserName;
    public InputField txtPwd;

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);
        switch (go.name)
        {
            case "btnLogOn":
                UIDispatcher.Instance.Dispatch(ConstDefine.UILogOnView_btnLogOn);
                break;
            case "btnToReg":
                UIDispatcher.Instance.Dispatch(ConstDefine.UILogOnView_btnToReg);
                break;
        }
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        txtUserName = null;
        txtPwd = null;
    }
}