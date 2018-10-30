
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class UISelectRoleRoleItemView : MonoBehaviour
{
    /// <summary>
    /// 角色编号
    /// </summary>
    private int m_RoleId;

    /// <summary>
    /// 昵称
    /// </summary>
    [SerializeField]
    private Text m_LblNickName;

    /// <summary>
    /// 等级
    /// </summary>
    [SerializeField]
    private Text m_LblLevel;

    /// <summary>
    /// 职业名称
    /// </summary>
    [SerializeField]
    private Text m_LblJobName;

    /// <summary>
    /// 头像
    /// </summary>
    [SerializeField]
    private Image m_ImgRoleHeadPic;

    /// <summary>
    /// 选择已有角色
    /// </summary>
    private Action<int> OnSelectRole;

    /// <summary>
    /// 移动的目标点
    /// </summary>
    private Vector3 m_MoveTargetPos;

    private int m_SelectRoleId;


    void Start () 
	{
        GetComponent<Button>().onClick.AddListener(RoleItemClick);

        m_MoveTargetPos = transform.localPosition + new Vector3(-50, 0, 0);
        transform.DOLocalMove(m_MoveTargetPos, 0.2f).SetAutoKill(false).SetEase(GlobalInit.Instance.UIAnimationCurve).Pause();

        SetSelected(m_SelectRoleId);
    }

    void OnDestroy()
    {
        m_LblNickName = null;
        m_LblLevel = null;
        m_LblJobName = null;
        m_ImgRoleHeadPic = null;
        OnSelectRole = null;
    }

    /// <summary>
    /// 设置选择状态
    /// </summary>
    /// <param name="selectRoleId"></param>
    public void SetSelected(int selectRoleId)
    {
        m_SelectRoleId = selectRoleId;
        if (m_RoleId == selectRoleId)
        {
            //突出显示
            transform.DOPlayForward();
        }
        else
        {
            transform.DOPlayBackwards();
        }
    }

    private void RoleItemClick()
    {
        if (OnSelectRole != null)
        {
            OnSelectRole(m_RoleId);
        }
    }

    public void SetUI(int roleId, string nickName, int level, int jobId, Sprite headPic, Action<int> onSelectRole)
    {
        m_RoleId = roleId;
        m_LblNickName.text = nickName;
        m_LblLevel.text = string.Format("Lv {0}", level);
        m_LblJobName.text = JobDBModel.Instance.Get(jobId).Name;
        m_ImgRoleHeadPic.overrideSprite = headPic;
        OnSelectRole = onSelectRole;
    }
}