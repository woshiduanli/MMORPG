using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class UIMainCitySkillView : UISubViewBase
{
    public static UIMainCitySkillView Instance;

    protected override void OnAwake()
    {
        base.OnAwake();
        m_Dic = new Dictionary<int, UIMainCitySkillSlotsView>();
        Instance = this;
    }

    [SerializeField]
    private UIMainCitySkillSlotsView Btn_Skill1;

    [SerializeField]
    private UIMainCitySkillSlotsView Btn_Skill2;

    [SerializeField]
    private UIMainCitySkillSlotsView Btn_Skill3;

    [SerializeField]
    private UIMainCitySkillSlotsView Btn_AddHP;

    private Dictionary<int, UIMainCitySkillSlotsView> m_Dic;

    public void SetUI(List<TransferData> lst, Action<int> onSkillClick)
    {
        for (int i = 0; i < lst.Count; i++)
        {
            int skillSlotsNo = lst[i].GetValue<byte>(ConstDefine.SkillSlotsNo);
            int skillId = lst[i].GetValue<int>(ConstDefine.SkillId);
            int skillLevel = lst[i].GetValue<int>(ConstDefine.SkillLevel);
            string skillPic = lst[i].GetValue<string>(ConstDefine.SkillPic);
            float skillCDTime = lst[i].GetValue<float>(ConstDefine.SkillCDTime);

            switch (skillSlotsNo)
            {
                case 1:
                    Btn_Skill1.SetUI(skillId, skillLevel, skillPic, skillCDTime, onSkillClick);
                    m_Dic[skillId] = Btn_Skill1;
                    break;
                case 2:
                    Btn_Skill2.SetUI(skillId, skillLevel, skillPic, skillCDTime, onSkillClick);
                    m_Dic[skillId] = Btn_Skill2;
                    break;
                case 3:
                    Btn_Skill3.SetUI(skillId, skillLevel, skillPic, skillCDTime, onSkillClick);
                    m_Dic[skillId] = Btn_Skill3;
                    break;
            }
        }
    }

    /// <summary>
    /// ¿ªÊ¼ÀäÈ´
    /// </summary>
    /// <param name="skillId"></param>
    public void BeginCD(int skillId)
    {
        if (m_Dic.ContainsKey(skillId))
        {
            UIMainCitySkillSlotsView view = m_Dic[skillId];
            view.BeginCD();
        }
    }
}
