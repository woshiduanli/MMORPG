
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class WorldMapSceneCtrl : GameSceneCtrlbase
{
    /// <summary>
    /// 主角出生点
    /// </summary>
    [SerializeField]
    private Transform m_PlayerBornPos;

    WorldMapEntity CurrWorldMapEntity;



    protected override void OnAwake()
    {
        //if (FingerEvent.Instance != null)
        //{
        //    FingerEvent.Instance.OnFingerDrag += OnFingerDrag;
        //    FingerEvent.Instance.OnZoom += OnZoom;
        //    FingerEvent.Instance.OnPlayerClick += OnPlayerClick;
        //}
    }





    protected override void OnLoadUIMainCityViewComplete(GameObject obj)
    {
        base.OnLoadUIMainCityViewComplete(obj);
        RoleMgr.Instance.InitMainPlayer();
        PlayerCtrl.Instance.SetMainCityRoleData();
        //加载玩家 ,
        if (GlobalInit.Instance.CurrPlayer != null)
        {
            CurrWorldMapEntity = WorldMapDBModel.Instance.Get(SceneMgr.Instance.CurrWorldMapId);
            if (CurrWorldMapEntity != null && CurrWorldMapEntity.RoleBirthPostion != Vector3.zero)
            {
                GlobalInit.Instance.CurrPlayer.Born( CurrWorldMapEntity.RoleBirthPostion); 
                GlobalInit.Instance.CurrPlayer.gameObject.transform.eulerAngles = new Vector3(0, CurrWorldMapEntity.RoleBirthEulerAnglesY, 0);
            }
            else
            {
                GlobalInit.Instance.CurrPlayer.Born(m_PlayerBornPos.position); 
            }

            Debug
                .LogError("1112323");
            if (DelegateDefine.Instance.OnSceneLoadOk != null)
            {
                Debug
              .LogError("111232377");
                DelegateDefine.Instance.OnSceneLoadOk();
            }

        }

        StartCoroutine(InitNPC());


    }

    IEnumerator InitNPC()
    {
        yield return null;

        if (CurrWorldMapEntity == null) yield break;

        for (int i = 0; i < CurrWorldMapEntity.NPCWorldMapList.Count; i++)
        {

            NPCWorldMapData data = CurrWorldMapEntity.NPCWorldMapList[i];
            NPCEntity entity = NPCDBModel.Instance.Get(data.NPCId);

            string prefabName = entity.PrefabName;
            GameObject obj = RoleMgr.Instance.LoadNPC(entity.PrefabName);

            obj.transform.position = data.NPCPostion;
            obj.transform.eulerAngles = new Vector3(0, data.EulerAnglesY, 0);

            NPCCtrl ctrl = obj.GetComponent<NPCCtrl>();
            if (ctrl != null)
                ctrl.Init(data);
        }
    }

    #region OnZoom 摄像机缩放
    /// <summary>
    /// 摄像机缩放
    /// </summary>
    /// <param name="obj"></param>

    #endregion

    #region OnFingerDrag 手指滑动
    /// <summary>
    /// 手指滑动
    /// </summary>
    /// <param name="obj"></param>

    #endregion




    #region OnDestroy 销毁
    /// <summary>
    /// 销毁
    /// </summary>
    protected override void BeforeOnDestroy()
    {

    }


    #endregion
}