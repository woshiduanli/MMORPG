//using UnityEngine;
//using System.Collections;

//public class UIGameLevelFailView : UIWindowViewBase
//{
//    /// <summary>
//    /// ����ί��
//    /// </summary>
//    public System.Action OnResurgence;

//    protected override void OnBtnClick(GameObject go)
//    {
//        base.OnBtnClick(go);

//        switch (go.name)
//        {
//            case "btnReturn":
//                //��Ҹ���
//                GlobalInit.Instance.CurrPlayer.ToResurgence(RoleIdleState.IdleFight);
//                SceneMgr.Instance.LoadToWorldMap(PlayerCtrl.Instance.LastInWorldMapId);
//                break;
//            case "btnResurgence":
//                if (OnResurgence != null) OnResurgence();
//                break;
//        }
//    }
//}
