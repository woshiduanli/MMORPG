
using UnityEngine;
using System.Collections;

public class GlobalInit : MonoBehaviour 
{
    #region 常量
    /// <summary>
    /// 昵称KEY
    /// </summary>
    public const string MMO_NICKNAME = "MMO_NICKNAME";

    /// <summary>
    /// 密码KEY
    /// </summary>
    public const string MMO_PWD = "MMO_PWD";

    #endregion

    public static GlobalInit Instance;

    /// <summary>
    /// 玩家注册时候的昵称
    /// </summary>
    [HideInInspector]
    public string CurrRoleNickName;

    /// <summary>
    /// 当前玩家
    /// </summary>
    [HideInInspector]
    public RoleCtrl CurrPlayer;

    /// <summary>
    /// UI动画曲线
    /// </summary>
    public AnimationCurve UIAnimationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

	void Start ()
	{
	
	}
}