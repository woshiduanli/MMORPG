using UnityEngine;
using System.Collections;

public class UIGameLevelFailView : UIWindowViewBase
{
    /// <summary>
    /// 复活委托
    /// </summary>
    public System.Action OnResurgence;

    protected override void OnStart()
    {


       
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);

        switch (go.name)
        {
            case "btnReturn":
                //玩家复活
                //SceneMgr.Instance.LoadToWorldMap(PlayerCtrl.Instance.LastInWorldMapId);
                GlobalInit.Instance.CurrPlayer.Resume();
                SceneMgr.Instance.LoadToWorldMap(GlobalInit.Instance.LastInWorldMapId);

                break;
            case "btnResurgence":


                base.Close();
                GlobalInit.Instance.CurrPlayer.Resume();
                //if (OnResurgence != null) OnResurgence();
                //GlobalInit.Instance.CurrPlayer.ToResurgence(RoleIdleState.IdleFight);
                break;
        }
    }
}
