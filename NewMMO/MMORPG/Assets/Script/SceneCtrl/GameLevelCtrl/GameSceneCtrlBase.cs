using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameSceneCtrlbase : MonoBehaviour
{

    public System.Action OnShow;
    protected UISceneMainCityView m_MainCityView;
    void Awake()
    {
        if (FingerEvent.Instance != null)
        {
            FingerEvent.Instance.OnFingerDrag += OnFingerDrag;
            FingerEvent.Instance.OnZoom += OnZoom;
            FingerEvent.Instance.OnPlayerClick += OnPlayerClick;
        }

        OnAwake();
    }

    private void OnFingerDrag(FingerEvent.FingerDir obj)
    {
        switch (obj)
        {
            case FingerEvent.FingerDir.Left:
                CameraCtrl.Instance.SetCameraRotate(0);
                break;
            case FingerEvent.FingerDir.Right:
                CameraCtrl.Instance.SetCameraRotate(1);
                break;
            case FingerEvent.FingerDir.Up:
                CameraCtrl.Instance.SetCameraUpAndDown(1);
                break;
            case FingerEvent.FingerDir.Down:
                CameraCtrl.Instance.SetCameraUpAndDown(0);
                break;
        }
    }

    private void OnZoom(FingerEvent.ZoomType obj)
    {
        switch (obj)
        {
            case FingerEvent.ZoomType.In:
                CameraCtrl.Instance.SetCameraZoom(0);
                break;
            case FingerEvent.ZoomType.Out:
                CameraCtrl.Instance.SetCameraZoom(1);
                break;
        }
    }



    /// <summary>
    /// 玩家点击
    /// </summary>
    private void OnPlayerClick()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);



        RaycastHit[] hitArr = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Role"));
        if (hitArr.Length > 0)
        {
            RoleCtrl hitRole = hitArr[0].collider.gameObject.GetComponent<RoleCtrl>();
            if (hitRole.CurrRoleType == RoleType.Monster)
            {
                GlobalInit.Instance.CurrPlayer.LockEnemy = hitRole;
                return;
            }
        }

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, 1000, 1 << LayerMask.NameToLayer("Ground")))
        {
            if (GlobalInit.Instance.CurrPlayer != null && !GlobalInit.Instance.CurrPlayer.IsRigidity)
            {
                GlobalInit.Instance.CurrPlayer.LockEnemy = null;
                GlobalInit.Instance.CurrPlayer.MoveTo(hitInfo.point);
            }
        }

    }

    /// <summary>
    /// 玩家点击
    /// </summary>


    void Start()
    {
        m_MainCityView = UISceneCtrl.Instance.LoadSceneUI(UISceneCtrl.SceneUIType.MainCity, OnLoadUIMainCityViewComplete).GetComponent<UISceneMainCityView>();
        m_MainCityView.OnSkillClick = OnSkillClick;
        m_MainCityView.OnAddHpClick = OnAddHpClick; 

        OnStart();

        EffectMgr.Instance.Init(this ); 

        //Button[] btnArr = GetComponentsInChildren<Button>(true);
        //for (int i = 0; i < btnArr.Length; i++)
        //{
        //    EventTriggerListener.Get(btnArr[i].gameObject).onClick += BtnClick;
        //}
        //OnStart();
        //if (OnShow != null) OnShow();
    }

    private void OnSkillClick(int index)
    {
        GlobalInit.Instance.CurrPlayer.ToAttackBySkilId(RoleAttackType.SkillAttack, index); 
    }

    private void OnAddHpClick(int obj)
    {
    }

    void OnDestroy()
    {
        if (FingerEvent.Instance != null)
        {
            FingerEvent.Instance.OnFingerDrag -= OnFingerDrag;
            FingerEvent.Instance.OnZoom -= OnZoom;
            FingerEvent.Instance.OnPlayerClick -= OnPlayerClick;
        }
        EffectMgr.Instance.Clear(); 
        BeforeOnDestroy();
    }

    private void BtnClick(GameObject go)
    {
        OnBtnClick(go);
    }

    private void Update()
    {
        OnUpdate(); 
    }

    protected virtual void OnLoadUIMainCityViewComplete(GameObject obj)
    {
        RoleMgr.Instance.InitMainPlayer();
        PlayerCtrl.Instance.SetMainCityRoleData();

    }
    protected virtual void OnUpdate() { }
    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    protected virtual void BeforeOnDestroy() { }
    protected virtual void OnBtnClick(GameObject go) { }
}
