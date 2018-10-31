
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 职业项
/// </summary>
public class UISelectRoleJobItemView : MonoBehaviour
{
    /// <summary>
    /// 职业编号
    /// </summary>
    [SerializeField]
    private int m_JobID;

    /// <summary>
    /// 旋转的目标角度
    /// </summary>
    [SerializeField]
    public int m_RotateAngle;

    public delegate void OnSelectJobHandler(int jobId, int rotateAngle);

    public OnSelectJobHandler OnSelectJob;

    /// <summary>
    /// 移动的目标点
    /// </summary>
    private Vector3 m_MoveTargetPos;

    private int m_SelectJobId;

    void Start () 
	{
        GetComponent<Button>().onClick.AddListener(OnBtnClick);

        m_MoveTargetPos = transform.localPosition + new Vector3(50, 0, 0);
        transform.DOLocalMove(m_MoveTargetPos, 0.2f).SetAutoKill(false).SetEase(GlobalInit.Instance.UIAnimationCurve).Pause();

        SetSelected(m_SelectJobId);
    }

    void OnDestroy()
    {
        OnSelectJob = null;
    }

    public void SetSelected(int selectJobId)
    {
        m_SelectJobId = selectJobId;
        if (m_JobID == selectJobId)
        {
            //突出显示
            transform.DOPlayForward();
        }
        else
        {
            transform.DOPlayBackwards();
        }
    }

    private void OnBtnClick()
    {
        if (OnSelectJob != null)
        {
            OnSelectJob(m_JobID, m_RotateAngle);
        }
    }
}