
using UnityEngine;
using System.Collections;

/// <summary>
/// 主城UI控制器
/// </summary>
public class UISceneCityCtrl : UISceneViewBase
{
    protected override void OnBtnClick(GameObject go)
    {
        switch (go.name)
        {
            case "btnHead":
                OpenRoleInfo();
                break;
        }
    }

    private void OpenRoleInfo()
    {
        UIViewUtil.Instance.OpenWindow(WindowUIType.RoleInfo);
    }
}