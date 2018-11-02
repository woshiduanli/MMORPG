
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIMainCityRoleInfoView : UISubViewBase
{
    /// <summary>
    /// 头像
    /// </summary>
    [SerializeField]
    private Image imgHeadPic;

    /// <summary>
    /// 昵称
    /// </summary>
    [SerializeField]
    private Text lblNickName;

    /// <summary>
    /// 等级
    /// </summary>
    [SerializeField]
    private Text lblLV;

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

    /// <summary>
    /// MP
    /// </summary>
    [SerializeField]
    private Slider sliderMP;

    public static UIMainCityRoleInfoView Instance;

    protected override void OnAwake()
    {
        base.OnAwake();
        Instance = this;
    }

    public void SetUI(string headPic, string nickName, int level, int money, int gold, int currHP, int maxHP, int currMP, int maxMP)
    {

        //AssetBundleMgr.Instance.LoadOrDownload<Texture2D>(string.Format("Download/Source/UISource/HeadImg/{0}.assetbundle", headPic), headPic, (Texture2D obj) =>
        //{
        //    if (obj == null) return;
        //    var iconRect = new Rect(0, 0, obj.width, obj.height);
        //    var iconSprite = Sprite.Create(obj, iconRect, new Vector2(.5f, .5f));

        //    imgHeadPic.overrideSprite = iconSprite;
        //}, type: 1);

        lblNickName.text = nickName;
        lblLV.text = string.Format("LV.{0}", level);
        lblMoney.text = money.ToString();
        lblGold.text = gold.ToString();
        sliderHP.SetSliderValue((float)currHP / maxHP);
        sliderMP.SetSliderValue((float)currMP / maxMP);
    }

    public void SetHP(int currHP, int maxHP)
    {
        sliderHP.SetSliderValue((float)currHP / maxHP);
    }

    public void SetMP(int currMP, int maxMP)
    {
        sliderMP.SetSliderValue((float)currMP / maxMP);
    }

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();

        imgHeadPic = null;
        lblNickName = null;
        lblLV = null;
        lblMoney = null;
        lblGold = null;
        sliderHP = null;
        sliderMP = null;

        Instance = null;
    }
}