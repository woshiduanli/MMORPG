using UnityEngine;
using System.Collections;

public class TimeMgr : SingletonMono<TimeMgr>
{
    /// <summary>
    /// 是否缩放中
    /// </summary>
    private bool m_IsTimeScale;

    /// <summary>
    /// 时间缩放结束时间
    /// </summary>
    private float m_TimeScaleEndTime = 0f;

    /// <summary>
    /// 修改时间缩放
    /// </summary>
    /// <param name="toTimeScale">缩放的值</param>
    /// <param name="continueTime">持续时间</param>
    public void ChangeTimeScale(float toTimeScale, float continueTime)
    {
        m_IsTimeScale = true;
        Time.timeScale = toTimeScale;
        m_TimeScaleEndTime = Time.realtimeSinceStartup + continueTime;
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();

        if (m_IsTimeScale)
        {
            if (Time.realtimeSinceStartup > m_TimeScaleEndTime)
            {
                Time.timeScale = 1;
                m_IsTimeScale = false;
            }
        }
    }
}