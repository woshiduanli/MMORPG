
using UnityEngine;
using System.Collections;

/// <summary>
/// 登录场景UI控制器
/// </summary>
public class UISceneLogonCtrl : UISceneBase
{
    protected override void OnStart()
    {
        base.OnStart();

        StartCoroutine(OpenLogOnWindow());
    }

    private IEnumerator OpenLogOnWindow()
    {
        yield return new WaitForSeconds(.2f);
        //GameObject obj = UIViewUtil.Instance.OpenWindow(WindowUIType.LogOn);

//         UIViewUtil.Instance.LoadWindow(WindowUIType.LogOn.ToString(), (GameObject obj) =>
//         {
// 
//         });
    }
}