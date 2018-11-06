
//using UnityEngine;
//using System.Collections;
//using UnityEngine.EventSystems;

//public class WorldMapSceneCtrl : MonoBehaviour
//{
//    /// <summary>
//    /// 主角出生点
//    /// </summary>
//    [SerializeField]
//    private Transform m_PlayerBornPos;

//    WorldMapEntity CurrWorldMapEntity;

//    UISceneMainCityView m_MainCityView;

//    void Awake()
//    {
//        if (FingerEvent.Instance != null)
//        {
//            FingerEvent.Instance.OnFingerDrag += OnFingerDrag;
//            FingerEvent.Instance.OnZoom += OnZoom;
//            FingerEvent.Instance.OnPlayerClick += OnPlayerClick;
//        }
//    }

//    void Start()
//    {
//        m_MainCityView = UISceneCtrl.Instance.LoadSceneUI(UISceneCtrl.SceneUIType.MainCity, OnLoadUIMainCityViewComplete).GetComponent<UISceneMainCityView>();
//    }

//    private void Update()
//    {
//        // 改变ui摄像机
//        //SetCamera();
//    }

//    private void SetCamera()
//    {
//        if (m_MainCityView != null && m_MainCityView.transform.parent != null)
//        {
//            m_MainCityView.transform.parent = null;
//            m_MainCityView.transform.localScale = Vector3.one;
//            if (m_MainCityView.transform.Find("UICamera") != null)
//                m_MainCityView.transform.Find("UICamera").GetComponent<Camera>().cullingMask = 1 << 5;
//        }
//    }

//    void OnLoadUIMainCityViewComplete(GameObject obj)
//    {
//        if (DelegateDefine.Instance.OnSceneLoadOk != null)
//        {
//            DelegateDefine.Instance.OnSceneLoadOk();
//        }

//        //加载玩家 ,
//        RoleMgr.Instance.InitMainPlayer();
//        if (GlobalInit.Instance.CurrPlayer != null)
//        {
//            CurrWorldMapEntity = WorldMapDBModel.Instance.Get(SceneMgr.Instance.CurrWorldMapId);
//            if (CurrWorldMapEntity != null && CurrWorldMapEntity.RoleBirthPostion != Vector3.zero)
//            {
//                MyDebug.debug(CurrWorldMapEntity.RoleBirthPostion);
//                GlobalInit.Instance.CurrPlayer.gameObject.transform.position = CurrWorldMapEntity.RoleBirthPostion;
//                GlobalInit.Instance.CurrPlayer.gameObject.transform.eulerAngles = new Vector3(0, CurrWorldMapEntity.RoleBirthEulerAnglesY, 0);
//            }
//            else
//            {
//                GlobalInit.Instance.CurrPlayer.gameObject.transform.position = m_PlayerBornPos.position;
//            }
//            PlayerCtrl.Instance.SetMainCityRoleInfo();
//        }

//        StartCoroutine(InitNPC());


//    }

//    IEnumerator InitNPC()
//    {
//        yield return null;

//        if (CurrWorldMapEntity == null) yield break;

//        for (int i = 0; i < CurrWorldMapEntity.NPCWorldMapList.Count; i++)
//        {

//            NPCWorldMapData data = CurrWorldMapEntity.NPCWorldMapList[i];
//            NPCEntity entity = NPCDBModel.Instance.Get(data.NPCId);

//            string prefabName = entity.PrefabName;
//            GameObject obj = RoleMgr.Instance.LoadNPC(entity.PrefabName);

//            obj.transform.position = data.NPCPostion;
//            obj.transform.eulerAngles = new Vector3(0, data.EulerAnglesY, 0);

//            NPCCtrl ctrl = obj.GetComponent<NPCCtrl>();
//            if (ctrl != null)
//                ctrl.Init(data);
//        }
//    }

//    #region OnZoom 摄像机缩放
//    /// <summary>
//    /// 摄像机缩放
//    /// </summary>
//    /// <param name="obj"></param>
//    private void OnZoom(FingerEvent.ZoomType obj)
//    {
//        switch (obj)
//        {
//            case FingerEvent.ZoomType.In:
//                CameraCtrl.Instance.SetCameraZoom(0);
//                break;
//            case FingerEvent.ZoomType.Out:
//                CameraCtrl.Instance.SetCameraZoom(1);
//                break;
//        }
//    }
//    #endregion

//    #region OnPlayerClickGround 玩家点击
//    /// <summary>
//    /// 玩家点击
//    /// </summary>
//    private void OnPlayerClick()
//    {
//        if (EventSystem.current.IsPointerOverGameObject()) return;

//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//        RaycastHit hitInfo;

//        RaycastHit[] hitArr = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Role"));
//        if (hitArr.Length > 0)
//        {
//            RoleCtrl hitRole = hitArr[0].collider.gameObject.GetComponent<RoleCtrl>();
//            if (hitRole.CurrRoleType == RoleType.Monster)
//            {
//                GlobalInit.Instance.CurrPlayer.LockEnemy = hitRole;
//            }
//        }
//        else
//        {
//            if (Physics.Raycast(ray, out hitInfo))
//            {
//                if (hitInfo.collider.gameObject.tag.Equals("Road", System.StringComparison.CurrentCultureIgnoreCase))
//                {
//                    if (GlobalInit.Instance.CurrPlayer != null)
//                    {
//                        GlobalInit.Instance.CurrPlayer.LockEnemy = null;
//                        GlobalInit.Instance.CurrPlayer.MoveTo(hitInfo.point);
//                    }
//                }
//            }
//        }
//    }
//    #endregion

//    #region OnFingerDrag 手指滑动
//    /// <summary>
//    /// 手指滑动
//    /// </summary>
//    /// <param name="obj"></param>
//    private void OnFingerDrag(FingerEvent.FingerDir obj)
//    {
//        switch (obj)
//        {
//            case FingerEvent.FingerDir.Left:
//                CameraCtrl.Instance.SetCameraRotate(0);
//                break;
//            case FingerEvent.FingerDir.Right:
//                CameraCtrl.Instance.SetCameraRotate(1);
//                break;
//            case FingerEvent.FingerDir.Up:
//                CameraCtrl.Instance.SetCameraUpAndDown(1);
//                break;
//            case FingerEvent.FingerDir.Down:
//                CameraCtrl.Instance.SetCameraUpAndDown(0);
//                break;
//        }
//    }
//    #endregion

//    #region OnDestroy 销毁
//    /// <summary>
//    /// 销毁
//    /// </summary>
//    void OnDestroy()
//    {
//        if (FingerEvent.Instance != null)
//        {
//            FingerEvent.Instance.OnFingerDrag -= OnFingerDrag;
//            FingerEvent.Instance.OnZoom -= OnZoom;
//            FingerEvent.Instance.OnPlayerClick -= OnPlayerClick;
//        }
//    }
//    #endregion
//}