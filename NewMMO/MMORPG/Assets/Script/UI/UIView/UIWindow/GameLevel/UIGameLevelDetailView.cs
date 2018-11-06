//===================================================

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIGameLevelDetailView : UIWindowViewBase
{
    /// <summary>
    /// 关卡名称
    /// </summary>
    [SerializeField]
    private Text lblGameLevelName;

    [SerializeField]
    private Image imgDetail;

    [SerializeField]
    private UIGameLevelRewardView[] rewards;

    [SerializeField]
    private Text lblGold;

    [SerializeField]
    private Text lblExp;

    [SerializeField]
    private Text lblDescription;

    [SerializeField]
    private Text lblCondition;

    [SerializeField]
    private Text lblCommendFighting;

    /// <summary>
    /// 已经选择的难度等级颜色
    /// </summary>
    [SerializeField]
    private Color selectedGradeColor;

    //默认的颜色
    private Color normalGradeColor;

    /// <summary>
    /// 难度等级按钮数组
    /// </summary>
    [SerializeField]
    private Image[] btnGrades;

    private int m_GameLevelId;

    ////难度等级按钮点击
    public delegate void OnBtnGradeClickHandler(int gameLevelId, GameLevelGrade grade);

    public OnBtnGradeClickHandler OnBtnGradeClick;

    //进入关卡按钮点击
    public delegate void OnBtnEnterClickHandler(int gameLevelId, GameLevelGrade grade);

    public OnBtnEnterClickHandler OnBtnEnterClick;

    /// <summary>
    /// 当前选择的难度等级
    /// </summary>
    private GameLevelGrade m_CurrSelectGrade;

    protected override void OnStart()
    {
        base.OnStart();

        //默认普通的被选中
        if (btnGrades.Length > 0)
        {
            normalGradeColor = btnGrades[0].color;

            btnGrades[0].color = selectedGradeColor;
        }
    }

    /// <summary>
    /// 重置难度等级按钮颜色
    /// </summary>
    private void ResetBtnGradeColor()
    {
        for (int i = 0; i < btnGrades.Length; i++)
        {
            btnGrades[i].color = normalGradeColor;
        }
    }

    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);

        switch (go.name)
        {
            case "btnNormal":
                BtnGradeCilck(GameLevelGrade.Normal);
                break;
            case "btnHard":
                BtnGradeCilck(GameLevelGrade.Hard);
                break;
            case "btnHell":
                BtnGradeCilck(GameLevelGrade.Hell);
                break;
            case "btnEnter":
                if (OnBtnEnterClick != null) OnBtnEnterClick(m_GameLevelId, m_CurrSelectGrade);
                break;
        }
    }

    private void BtnGradeCilck(GameLevelGrade grade)
    {
        if (m_CurrSelectGrade == grade) return;

        m_CurrSelectGrade = grade;

        //重置按钮颜色
        ResetBtnGradeColor();

        if (OnBtnGradeClick != null)
        {
            OnBtnGradeClick(m_GameLevelId, grade);
        }

        if (btnGrades.Length == 3)
        {
            btnGrades[(int)grade].color = selectedGradeColor;
        }
    }

    public void SetUI(TransferData data)
    {
        m_GameLevelId = data.GetValue<int>(ConstDefine.GameLevelId);
        imgDetail.overrideSprite = GameUtil.LoadGameLevelDetailPic(data.GetValue<string>(ConstDefine.GameLevelDlgPic));
        //AssetBundleMgr.Instance.LoadOrDownload<Sprite>(string.Format("Download/Source/UISource/GameLevel/GameLevelDetail/{0}.assetbundle", data.GetValue<string>(ConstDefine.GameLevelDlgPic)), data.GetValue<string>(ConstDefine.GameLevelDlgPic), (Sprite obj) =>
        //{
        //    imgDetail.overrideSprite = obj;
        //}, type: 1);
        lblGameLevelName.SetText(data.GetValue<string>(ConstDefine.GameLevelName));

        lblExp.SetText(data.GetValue<int>(ConstDefine.GameLevelExp).ToString());
        lblGold.SetText(data.GetValue<int>(ConstDefine.GameLevelGold).ToString());
        lblDescription.SetText(data.GetValue<string>(ConstDefine.GameLevelDesc));
        lblCondition.SetText(data.GetValue<string>(ConstDefine.GameLevelConditionDesc));
        //lblCommendFighting.SetText(data.GetValue<int>(ConstDefine.GameLevelCommendFighting).ToString(), true, DG.Tweening.ScrambleMode.Numerals);
        lblCommendFighting.SetText(data.GetValue<int>(ConstDefine.GameLevelCommendFighting).ToString());


        //接收奖励的物品
        List<TransferData> lstReward = data.GetValue<List<TransferData>>(ConstDefine.GameLevelReward);

        if (rewards != null)
            for (int i = 0; i < rewards.Length; i++)
            {
                rewards[i].gameObject.SetActive(false);
            }

        if (lstReward != null && lstReward.Count > 0)
        {
            for (int i = 0; i < lstReward.Count; i++)
            {
                rewards[i].gameObject.SetActive(true);

                rewards[i].SetUI(lstReward[i].GetValue<string>(ConstDefine.GoodsName), lstReward[i].GetValue<int>(ConstDefine.GoodsId), lstReward[i].GetValue<GoodsType>(ConstDefine.GoodsType));
            }
        }
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        lblGameLevelName = null;
        imgDetail = null;
        rewards.SetNull();
        lblGold = null;
        lblExp = null;
        lblDescription = null;
        lblCondition = null;
        lblCommendFighting = null;
    }
}