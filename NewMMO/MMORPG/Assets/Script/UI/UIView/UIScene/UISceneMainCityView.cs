
using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// 主城View
/// </summary>
public class UISceneMainCityView : UISceneViewBase
{
    /// <summary>
    /// 自动战斗容器
    /// </summary>
    [SerializeField]
    private GameObject AutoFightContainer;

    [SerializeField]
    private GameObject BtnAutoFight;

    [SerializeField]
    private GameObject BtnCancelAutoFight;

    protected override void OnAwake()
    {
        base.OnAwake();
    }

    protected override void OnStart()
    {
        base.OnStart();

        //if (OnLoadComplete != null)
        //{
        //    OnLoadComplete();
        //}

        //AutoFightContainer.SetActive(SceneMgr.Instance.CurrSceneType == SceneType.GameLevel);
    }

    protected override void OnBtnClick(GameObject go)
    {
        //switch (go.name)
        //{
        //    case "btnTopMenu":
        //        ChangeMenuState(go);
        //        break;
        //    case "btnMenu_Role":
        //        UIViewMgr.Instance.OpenWindow(WindowUIType.RoleInfo);
        //        break;
        //    case "btnMenu_GameLevel":
        //        UIViewMgr.Instance.OpenWindow(WindowUIType.GameLevelMap);
        //        break;
        //    case "btnMenu_WorldMap":
        //        UIViewMgr.Instance.OpenWindow(WindowUIType.WorldMap);
        //        break;
        //    case "btnAutoFight":
        //        AutoFight(true);
        //        break;
        //    case "btnCancelAutoFight":
        //        AutoFight(false);
        //        break;
        //}
    }

    /// <summary>
    /// 设置自动战斗
    /// </summary>
    /// <param name="isAutoFight"></param>
    private void AutoFight(bool isAutoFight)
    {
        BtnAutoFight.SetActive(!isAutoFight);
        BtnCancelAutoFight.SetActive(isAutoFight);

        //设置主角自动战斗状态
        //GlobalInit.Instance.CurrPlayer.Attack.IsAutoFight = isAutoFight;
    }

    /// <summary>
    /// 切换菜单显示
    /// </summary>
    /// <param name="go"></param>
    private void ChangeMenuState(GameObject go)
    {
        //UIMainCityMenusView.Instance.ChangeState(() =>
        //{
        //    //回调
        //    go.transform.localScale = new Vector3(go.transform.localScale.x, go.transform.localScale.y * -1, go.transform.localScale.z);
        //});
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
    }
}