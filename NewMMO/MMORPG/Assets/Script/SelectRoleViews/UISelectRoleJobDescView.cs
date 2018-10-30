
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class UISelectRoleJobDescView : MonoBehaviour
{
    /// <summary>
    /// 职业名称
    /// </summary>
    [SerializeField]
    private Text lblJobName;

    /// <summary>
    /// 职业描述
    /// </summary>
    [SerializeField]
    private Text lblJobDesc;

    /// <summary>
    /// 移动的目标点
    /// </summary>
    private Vector3 m_MoveTargetPos;

    /// <summary>
    /// 是否已经显示
    /// </summary>
    private bool m_IsShow = false;

    void Start()
    {
        m_MoveTargetPos = transform.localPosition;

        Vector3 from = m_MoveTargetPos + new Vector3(0, 500, 0);

        transform.localPosition = from;
        transform.DOLocalMove(m_MoveTargetPos, 0.2f).SetAutoKill(false).SetEase(GlobalInit.Instance.UIAnimationCurve).Pause().OnComplete(() =>
        {
            m_IsShow = true;
        }).OnRewind(() =>
        {
            transform.DOPlayForward();
        });

        DoAnim();
    }

    public void SetUI(string jobName, string jobDesc)
    {
        lblJobName.text = jobName;
        lblJobDesc.text = jobDesc;

        DoAnim();
    }

    private void DoAnim()
    {
        if (!m_IsShow)
        {
            transform.DOPlayForward();
        }
        else
        {
            transform.DOPlayBackwards();
        }
    }

    void OnDestroy()
    {
        lblJobName = null;
        lblJobDesc = null;
    }
}