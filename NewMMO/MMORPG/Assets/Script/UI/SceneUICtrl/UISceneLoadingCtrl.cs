//=
using UnityEngine;
using System.Collections;

/// <summary>
/// Loading场景UI控制器
/// </summary>
public class UISceneLoadingCtrl : UISceneViewBase
{
    /// <summary>
    /// 进度条
    /// </summary>
    [SerializeField]
    private UIProgressBar m_Progress;

    /// <summary>
    /// 进度条上的文本
    /// </summary>
    [SerializeField]
    private UILabel m_LblProgress;

    /// <summary>
    /// 发光的图
    /// </summary>
    [SerializeField]
    private UISprite m_SprProgressLight;

    /// <summary>
    /// 设置进度条的值
    /// </summary>
    /// <param name="value"></param>
    public void SetProgressValue(float value)
    {
        if (m_Progress == null || m_LblProgress == null || m_SprProgressLight == null) { return;  }
        m_Progress.value = value;
        m_LblProgress.text = string.Format("{0}%", (int)(value * 100));

        m_SprProgressLight.transform.localPosition = new Vector3(880* value, 0, 0);
    }

    protected override void BeforeOnDestroy()
    {
        m_Progress = null;
        m_LblProgress = null;
        m_SprProgressLight = null; 
    }


}