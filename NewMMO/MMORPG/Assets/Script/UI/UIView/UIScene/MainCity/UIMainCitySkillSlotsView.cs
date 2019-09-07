using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

/// <summary>
/// 技能槽视图
/// </summary>
public class UIMainCitySkillSlotsView : UISubViewBase
{
    public int SlotsNo;

    [HideInInspector]
    public int SkillId;

    [SerializeField]
    private Image SkillImg;

    /// <summary>
    /// CD图片
    /// </summary>
    [SerializeField]
    private Image CDImg;

    /// <summary>
    /// 冷却时间
    /// </summary>
    private float m_CDTime = 0f;

    /// <summary>
    /// 技能是否冷却中
    /// </summary>
    private bool m_IsCD = false;

    /// <summary>
    /// 技能开始冷却的时间
    /// </summary>
    private float m_BeginCDTime = 0;

    /// <summary>
    /// 旋转的百分比
    /// </summary>
    private float m_CurrFillAmount = 0f;

    private Action<int> OnSkillClick;

    protected override void OnAwake()
    {
        base.OnAwake();
        SkillImg.gameObject.SetActive(false);
        CDImg.transform.parent.gameObject.SetActive(false);
    }

    protected override void OnStart()
    {
        base.OnStart();
        EventTriggerListener.Get(gameObject).onClick += onClick;
    }

    /// <summary>
    /// 技能槽按钮点击
    /// </summary>
    /// <param name="go"></param>
    private void onClick(GameObject go)
    {


        //Debug.Log("SkillId=" + SkillId);
        //如果没有装配技能 直接返回
        if (SkillId < 1)
        {
            //OnSkillClick(109);
            // 强制改变数据

            return;
        }
        if (m_IsCD) return;

        if (OnSkillClick != null)
        {
            Debug.LogError("111111111111111:"+ SkillId);

            OnSkillClick(SkillId);
        }
    }

    public void SetUI(int skillId, int skillLevel, string skillPic, float cdTime, Action<int> onSkillClick)
    {
        if (skillId == 0) return;

        SkillId = skillId;
        m_CDTime = cdTime; //冷却时间
        SkillImg.gameObject.SetActive(true);
        SkillImg.SetImage(RoleMgr.Instance.LoadSkillPic(skillPic));
        OnSkillClick = onSkillClick;
    }

    /// <summary>
    /// 开始冷却
    /// </summary>
    public void BeginCD()
    {
        CDImg.transform.parent.gameObject.SetActive(true);
        m_IsCD = true;
        m_BeginCDTime = Time.time;
        m_CurrFillAmount = 1;
    }

    void Update()
    {
        if (m_IsCD)
        {
            //百分比
            m_CurrFillAmount = Mathf.Lerp(1, 0, (Time.time - m_BeginCDTime) / m_CDTime);
            CDImg.fillAmount = m_CurrFillAmount;

            if (Time.time > m_BeginCDTime + m_CDTime)
            {
                m_IsCD = false;
            }
        }
    }
}