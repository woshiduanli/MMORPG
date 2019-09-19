using UnityEngine;
using System.Collections;

public class UIGameLevelFailView : UIWindowViewBase
{
    /// <summary>
    /// ����ί��
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
                //��Ҹ���
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
