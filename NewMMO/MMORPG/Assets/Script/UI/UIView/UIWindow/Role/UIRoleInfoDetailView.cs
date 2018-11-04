
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIRoleInfoDetailView : UISubViewBase
{
    /// <summary>
    /// 元宝
    /// </summary>
    [SerializeField]
    private Text lblMoney;

    /// <summary>
    /// 金币
    /// </summary>
    [SerializeField]
    private Text lblGold;

    /// <summary>
    /// HP
    /// </summary>
    [SerializeField]
    private Slider sliderHP;


    [SerializeField]
    private Text lblHP;

    [SerializeField]
    private Slider sliderMP;

    [SerializeField]
    private Text lblMP;

    [SerializeField]
    private Slider sliderExp;

    [SerializeField]
    private Text lblExp;

    [SerializeField]
    private Text lblAttack;

    [SerializeField]
    private Text lblDefense;

    [SerializeField]
    private Text lblDodge;

    [SerializeField]
    private Text lblHit;

    [SerializeField]
    private Text lblCri;

    [SerializeField]
    private Text lblRes;

    public void SetUI(TransferData data)
    {
        //lblMoney.SetText(data.GetValue<int>(ConstDefine.Money).ToString());
        //lblGold.SetText(data.GetValue<int>(ConstDefine.Gold).ToString());

        //sliderHP.SetSliderValue((float)data.GetValue<int>(ConstDefine.CurrHP) / data.GetValue<int>(ConstDefine.MaxHP));
        //lblHP.SetText(string.Format("{0}/{1}", data.GetValue<int>(ConstDefine.CurrHP), data.GetValue<int>(ConstDefine.MaxHP)));

        //sliderMP.SetSliderValue((float)data.GetValue<int>(ConstDefine.CurrMP) / data.GetValue<int>(ConstDefine.MaxMP));
        //lblMP.SetText(string.Format("{0}/{1}", data.GetValue<int>(ConstDefine.CurrMP), data.GetValue<int>(ConstDefine.MaxMP)));

        //sliderExp.SetSliderValue((float)data.GetValue<int>(ConstDefine.CurrExp) / data.GetValue<int>(ConstDefine.MaxExp));
        //lblExp.SetText(string.Format("{0}/{1}", data.GetValue<int>(ConstDefine.CurrExp), data.GetValue<int>(ConstDefine.MaxExp)));

        //lblAttack.SetText(data.GetValue<int>(ConstDefine.Attack).ToString());
        //lblDefense.SetText(data.GetValue<int>(ConstDefine.Defense).ToString());
        //lblDodge.SetText(data.GetValue<int>(ConstDefine.Dodge).ToString());
        //lblHit.SetText(data.GetValue<int>(ConstDefine.Hit).ToString());
        //lblCri.SetText(data.GetValue<int>(ConstDefine.Cri).ToString());
        //lblRes.SetText(data.GetValue<int>(ConstDefine.Res).ToString());
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();

        lblMoney = null;
        lblGold = null;
        sliderHP = null;
        lblHP = null;
        sliderMP = null;
        lblMP = null;
        sliderExp = null;
        lblExp = null;
        lblAttack = null;
        lblDefense = null;
        lblDodge = null;
        lblHit = null;
        lblCri = null;
        lblRes = null;
    }
}