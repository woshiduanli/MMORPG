
using UnityEngine;
using System.Collections;

public class UISceneLogOnView : UISceneViewBase
{
    protected override void OnStart()
    {
        base.OnStart();
        StartCoroutine(OpenLogOnWindow());
    }

    private IEnumerator OpenLogOnWindow()
    {
        yield return new WaitForSeconds(.2f);
        AccountCtrl.Instance.QuickLogOn(); 
        //UIViewMgr.Instance.OpenWindow(WindowUIType.Reg); 
    }
}