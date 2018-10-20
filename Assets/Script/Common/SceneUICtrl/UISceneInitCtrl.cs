
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UISceneInitCtrl : MonoBehaviour 
{
    [SerializeField]
    private Text txt_Load;

    [SerializeField]
    private Slider slider_Load;

    public static UISceneInitCtrl Instance;

    void Awake () 
	{
        Instance = this;
    }

    /// <summary>
    /// 设置进度条
    /// </summary>
    /// <param name="text"></param>
    /// <param name="value"></param>
    public void SetProgress(string text, float value)
    {
        txt_Load.text = text; 
        slider_Load.SetSliderValue(value);
    }
}