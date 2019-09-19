using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITipItemView : UISubViewBase {

    [SerializeField]
    private Text lblText;

    [SerializeField]
    private Image imgIco;

    [SerializeField]
    private Sprite[] sprType;

    protected override void OnStart()
    {
        base.OnStart();
    }

    public void SetUI(int type, string text)
    {
        if (type >= 0 && type < sprType.Length)
        {
            imgIco.overrideSprite = sprType[type];
            imgIco.SetNativeSize();
        }
        lblText.SetText(text);
    }
}
