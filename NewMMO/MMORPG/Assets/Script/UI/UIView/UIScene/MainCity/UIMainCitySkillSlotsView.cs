using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

/// <summary>
/// ���ܲ���ͼ
/// </summary>
public class UIMainCitySkillSlotsView : UISubViewBase
{
    public int SlotsNo;

    [HideInInspector]
    public int SkillId;

    [SerializeField]
    private Image SkillImg;

    /// <summary>
    /// CDͼƬ
    /// </summary>
    [SerializeField]
    private Image CDImg;

    /// <summary>
    /// ��ȴʱ��
    /// </summary>
    private float m_CDTime = 0f;

    /// <summary>
    /// �����Ƿ���ȴ��
    /// </summary>
    private bool m_IsCD = false;

    /// <summary>
    /// ���ܿ�ʼ��ȴ��ʱ��
    /// </summary>
    private float m_BeginCDTime = 0;

    /// <summary>
    /// ��ת�İٷֱ�
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
    /// ���ܲ۰�ť���
    /// </summary>
    /// <param name="go"></param>
    private void onClick(GameObject go)
    {


        //Debug.Log("SkillId=" + SkillId);
        //���û��װ�似�� ֱ�ӷ���
        if (SkillId < 1)
        {
            //OnSkillClick(109);
            // ǿ�Ƹı�����

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
        m_CDTime = cdTime; //��ȴʱ��
        SkillImg.gameObject.SetActive(true);
        SkillImg.SetImage(RoleMgr.Instance.LoadSkillPic(skillPic));
        OnSkillClick = onSkillClick;
    }

    /// <summary>
    /// ��ʼ��ȴ
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
            //�ٷֱ�
            m_CurrFillAmount = Mathf.Lerp(1, 0, (Time.time - m_BeginCDTime) / m_CDTime);
            CDImg.fillAmount = m_CurrFillAmount;

            if (Time.time > m_BeginCDTime + m_CDTime)
            {
                m_IsCD = false;
            }
        }
    }
}