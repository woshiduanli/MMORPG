using UnityEngine;
using System.Collections;

public class TimeMgr : SingletonMono<TimeMgr>
{
    /// <summary>
    /// �Ƿ�������
    /// </summary>
    private bool m_IsTimeScale;

    /// <summary>
    /// ʱ�����Ž���ʱ��
    /// </summary>
    private float m_TimeScaleEndTime = 0f;

    /// <summary>
    /// �޸�ʱ������
    /// </summary>
    /// <param name="toTimeScale">���ŵ�ֵ</param>
    /// <param name="continueTime">����ʱ��</param>
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