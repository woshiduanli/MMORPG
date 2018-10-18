using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 进度条
/// </summary>
public class CLoaderUI : MonoBehaviour
{
    private Slider Bar;
    private Text WarmPrompt;
    private float value;
    //private CTexture BgImage;
    private RawImage rawImage;
    private int Speed = 30;
    private int Custom = 70;
    void Awake()
    {
        NGUILink link = this.gameObject.GetComponent(typeof(NGUILink)) as NGUILink;
        Bar = link.GetComponent<Slider>("imageSlider");
        //WarmPrompt = link.GetComponent<Text>("WarmPrompt");
        //rawImage = link.GetComponent<RawImage>("Image");
    }

    public void SetProgressSpeed(int speed,int custom)
    {
        this.Speed = speed;
        this.Custom = custom;
    }

    public void LoadImage(string texname)
    {
        //BgImage = CResourceFactory.CreateInstance<CTexture>(string.Format("res/loading_pic/{0}.tex", texname), null, PLevel.Low, texname);
        //BgImage.SetTexture(rawImage);
    }

    public void Update()
    {
        MyDebug.debug("Progress.Instance.progress:" + Progress.Instance.progress);
        MyDebug.debug("  value:" + value);
        if (Progress.Instance.progress <= this.Custom)
            value += Time.deltaTime * this.Speed;
        if (value < Progress.Instance.progress && Progress.Instance.progress >= this.Custom)
            value = Progress.Instance.progress;
        if (value >= 95)
            value = 95;
        Bar.value = value / 100;
        //WarmPrompt.text = Progress.Instance.WarmPrompt;
    }
}
