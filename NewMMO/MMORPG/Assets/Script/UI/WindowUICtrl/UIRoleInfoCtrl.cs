//=
using UnityEngine;
using System.Collections;

/// <summary>
/// 角色信息窗口控制器
/// </summary>
public class UIRoleInfoCtrl : UIWindowViewBase 
{
    protected override void OnBtnClick(GameObject go)
    {
        switch (go.gameObject.name)
        {
            case "btnClose":
                Close();
                break;
        }
    }
}