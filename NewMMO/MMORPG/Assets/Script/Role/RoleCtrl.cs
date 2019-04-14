
using UnityEngine;
using System.Collections;
using Pathfinding;

/// <summary>
/// 角色控制器
/// </summary>
/// []
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(FunnelModifier))]
public class RoleCtrl : MonoBehaviour
{
    #region 成员变量或属性
    /// <summary>
    /// 昵称挂点
    /// </summary>
    [SerializeField]
    private Transform m_HeadBarPos;

    /// <summary>
    /// 头顶UI条
    /// </summary>
    private GameObject m_HeadBar;

    /// <summary>
    /// 动画
    /// </summary>
    [SerializeField]
    public Animator Animator;

    /// <summary>
    /// 移动的目标点
    /// </summary>
    [HideInInspector]
    public Vector3 TargetPos = Vector3.zero;

    /// <summary>
    /// 控制器
    /// </summary>
    [HideInInspector]
    public CharacterController CharacterController;

    /// <summary>
    /// 移动速度
    /// </summary>
    [SerializeField]
    public float Speed = 10f;

    /// <summary>
    /// 出生点
    /// </summary>
    [HideInInspector]
    public Vector3 BornPoint;

    /// <summary>
    /// 视野范围
    /// </summary>
    public float ViewRange;

    /// <summary>
    /// 巡逻范围
    /// </summary>
    public float PatrolRange;

    /// <summary>
    /// 攻击范围
    /// </summary>
    public float AttackRange;

    /// <summary>
    /// 当前角色类型
    /// </summary>
    public RoleType CurrRoleType = RoleType.None;

    /// <summary>
    /// 当前角色信息
    /// </summary>
    public RoleInfoBase CurrRoleInfo = null;

    /// <summary>
    /// 当前角色AI
    /// </summary>
    public IRoleAI CurrRoleAI = null;

    /// <summary>
    /// 锁定敌人
    /// </summary>
    [HideInInspector]
    public RoleCtrl LockEnemy;

    /// <summary>
    /// 角色受伤委托
    /// </summary>
    public System.Action OnRoleHurt;

    /// <summary>
    /// 角色死亡
    /// </summary>
    public System.Action<RoleCtrl> OnRoleDie;

    /// <summary>
    /// 当前角色有限状态机管理器
    /// </summary>
    public RoleFSMMgr CurrRoleFSMMgr = null;

    private RoleHeadBarView roleHeadBarView = null;

    #endregion


    Seeker m_Seeker;
    [HideInInspector]
    public ABPath AStartPath;

    [HideInInspector]
    public int AstartCurrWayPointIndex = 1;

   
    //public RoleAttack attack;

    //RoleHurt m_hurt;
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="roleType">角色类型</param>
    /// <param name="roleInfo">角色信息</param>
    /// <param name="ai">AI</param>
    public void Init(RoleType roleType, RoleInfoBase roleInfo, IRoleAI ai)
    {
        CurrRoleType = roleType;
        CurrRoleInfo = roleInfo;
        CurrRoleAI = ai;
    }

    void Start()
    {
        CharacterController = GetComponent<CharacterController>();

        m_Seeker = GetComponent<Seeker>();
        if (m_Seeker == null)
        {
            m_Seeker = this.gameObject.AddComponent<Seeker>();
            m_Seeker.drawGizmos = true;
            this.gameObject.AddComponent<FunnelModifier>();
        }


        if (CurrRoleType == RoleType.MainPlayer)
        {
            if (CameraCtrl.Instance != null)
            {
                CameraCtrl.Instance.Init();
            }
        }
        if (transform.GetChild(0).GetComponent<Animator>() != null)
            Animator = transform.GetChild(0).GetComponent<Animator>();

        CurrRoleFSMMgr = new RoleFSMMgr(this);
        //m_hurt = new RoleHurt(CurrRoleFSMMgr);
        //attack.SetFSM(CurrRoleFSMMgr); 

        ToIdle();
        InitHeadBar();
    }

    void Update()
    {
        if (CurrRoleFSMMgr != null)
            CurrRoleFSMMgr.OnUpdate();

        //如果角色没有AI 直接返回
        if (CurrRoleAI == null) return;
        CurrRoleAI.DoAI();



        if (CharacterController == null) return;

        //让角色贴着地面
        if (!CharacterController.isGrounded)
        {
            CharacterController.Move((transform.position + new Vector3(0, -1000, 0)) - transform.position);
        }

        if (Input.GetMouseButtonUp(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Item")))
            {
                BoxCtrl boxCtrl = hit.collider.GetComponent<BoxCtrl>();
                if (boxCtrl != null)
                {
                    boxCtrl.Hit();
                }
            }
        }

        //让角色贴着地面
        if (!CharacterController.isGrounded)
        {
            CharacterController.Move((transform.position + new Vector3(0, -1000, 0)) - transform.position);
        }

        if (CurrRoleType == RoleType.MainPlayer)
        {
            CameraAutoFollow();
        }

        // 小地图
        AutoSmallMap();
    }

    void AutoSmallMap()
    {
        //if (SmallMapHelper.Instance == null && UIMainCitySmallMapView.Instance == null) return;
        //SmallMapHelper.Instance.gameObject.transform.position = transform.position;

        //UIMainCitySmallMapView.Instance.transform.localPosition = new Vector3(SmallMapHelper.Instance.gameObject.transform.localPosition.x * -512, SmallMapHelper.Instance.gameObject.transform.localPosition.z * -512, 1);

        //UIMainCitySmallMapView.Instance.SmallMapArr.transform.localEulerAngles = new Vector3(0,0, 360-transform.localEulerAngles.y);
    }

    /// <summary>
    /// 初始化头顶UI条
    /// </summary>
    private void InitHeadBar()
    {
        if (RoleHeadBarRoot.Instance == null) return;
        if (CurrRoleInfo == null) return;
        if (m_HeadBarPos == null) return;

        //克隆预设
        m_HeadBar = ResourcesMgr.Instance.Load(ResourcesMgr.ResourceType.UIOther, "RoleHeadBar");
        m_HeadBar.transform.parent = RoleHeadBarRoot.Instance.gameObject.transform;
        m_HeadBar.transform.localScale = Vector3.one;

        //CharacterController d;
        //d.Move
        roleHeadBarView = m_HeadBar.GetComponent<RoleHeadBarView>();

        //给预设赋值
        roleHeadBarView.Init(m_HeadBarPos, CurrRoleInfo.RoleNickName, CurrRoleType != RoleType.MainPlayer);
    }


    #region 控制角色方法

    public void ToIdle(RoleIdleState state = RoleIdleState.IdelNormal)
    {
        //CurrRoleFSMMgr.to state 
        if (CurrRoleFSMMgr != null)
        {
            CurrRoleFSMMgr.ToIdelState = state;
            CurrRoleFSMMgr.ChangeState(RoleState.Idle);
        }
    }

    public void MoveTo(Vector3 targetPos)
    {
        //如果目标点不是原点 进行移动
        if (targetPos == Vector3.zero) return;
        TargetPos = targetPos;

        m_Seeker.StartPath(transform.position, TargetPos, (path) =>
        {
            if (!path.error)
            {
                AStartPath = (ABPath)path;
                if (Vector3.Distance(AStartPath.endPoint, new Vector3(AStartPath.originalEndPoint.x, AStartPath.endPoint.y, AStartPath.originalEndPoint.z)) > 0.5f)
                {
                    MyDebug.debug("不能达到目标点");
                    AStartPath = null;
                    return;
                }
                AstartCurrWayPointIndex = 1;
                CurrRoleFSMMgr.ChangeState(RoleState.Run);
            }
            else
            {
                MyDebug.debug("寻路有错");
                AStartPath = null;
            }

        });

        CurrRoleFSMMgr.ChangeState(RoleState.Run);
    }

    public void ToAttack(RoleAttackType type = RoleAttackType.PhyAttack, int index=0)
    {
        //attack.ToAttack(type, index); 
    }

    // 临时测试用
    public void ToRun()
    {

        CurrRoleFSMMgr.ChangeState(RoleState.Run);

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="attackValue">受到的攻击力</param>
    /// <param name="delay">延迟时间</param>
    public void ToHurt(int attackValue, float delay)
    {
        StartCoroutine(ToHurtCoroutine(attackValue, delay));
    }

    public void TestToHurt()
    {
        CurrRoleFSMMgr.ChangeState(RoleState.Hurt);
    }

    public void TestToDies()
    {
        CurrRoleFSMMgr.ChangeState(RoleState.Die);
    }

    private IEnumerator ToHurtCoroutine(int attackValue, float delay)
    {
        yield return new WaitForSeconds(delay);

#if DEBUG_ROLESTATE
        //m_hurt.ToHurt(attackValue);
        CurrRoleFSMMgr.ChangeState(RoleState.Hurt);
#else 
        ////计算得出伤害数值
        int hurt = (int)(attackValue * Random.Range(0.5f, 1f));

        if (OnRoleHurt != null)
        {
            OnRoleHurt();
        }


        CurrRoleInfo.CurrHP -= hurt;

        //roleHeadBarCtrl.Hurt(hurt, (float)CurrRoleInfo.CurrHP / CurrRoleInfo.MaxHP);

        if (CurrRoleInfo.CurrHP <= 0)
        {
            CurrRoleFSMMgr.ChangeState(RoleState.Die);
        }
        else
        {
            CurrRoleFSMMgr.ChangeState(RoleState.Hurt);
        }
#endif
    }

    public void ToDie()
    {
        CurrRoleFSMMgr.ChangeState(RoleState.Die);
    }

    public void ToSelect()
    {
        CurrRoleFSMMgr.ChangeState(RoleState.Select);
    }

    #endregion

    #region OnDestroy 销毁
    /// <summary>
    /// 销毁
    /// </summary>
    void OnDestroy()
    {
        if (m_HeadBar != null)
        {
            Destroy(m_HeadBar);
        }
    }
    #endregion

    #region CameraAutoFollow 摄像机自动跟随
    /// <summary>
    /// 摄像机自动跟随
    /// </summary>
    private void CameraAutoFollow()
    {
        if (CameraCtrl.Instance == null) return;

        CameraCtrl.Instance.transform.position = gameObject.transform.position;
        CameraCtrl.Instance.AutoLookAt(gameObject.transform.position);
    }
    #endregion
}