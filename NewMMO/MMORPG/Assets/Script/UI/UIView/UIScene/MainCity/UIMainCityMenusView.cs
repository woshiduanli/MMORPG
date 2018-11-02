////===================================================
////作    者：边涯  http://www.u3dol.com  QQ群：87481002
////创建时间：2016-06-30 21:13:28
////备    注：
////===================================================
//using UnityEngine;
//using System.Collections;
//using DG.Tweening;
//using System;

//public class UIMainCityMenusView : UISubViewBase
//{
//    public static UIMainCityMenusView Instance;

//    /// <summary>
//    /// 移动的目标点
//    /// </summary>
//    private Vector3 m_MoveTargetPos;

//    /// <summary>
//    /// 是否显示
//    /// </summary>
//    private bool m_IsShow = false;
//    private bool m_IsBusy = false;

//    private Action m_OnChangeSuccess;

//    protected override void OnAwake()
//    {
//        base.OnAwake();
//        Instance = this;
//    }

//    protected override void OnStart()
//    {
//        base.OnStart();
//        if (GlobalInit.Instance == null) return;

//        m_IsShow = true;
//        m_MoveTargetPos = transform.localPosition + new Vector3(0, 135, 0);

//        transform.DOLocalMove(m_MoveTargetPos, 0.2f).SetAutoKill(false).SetEase(GlobalInit.Instance.UIAnimationCurve).Pause().OnComplete(() =>
//        {
//            if (m_OnChangeSuccess != null) m_OnChangeSuccess();
//            m_IsBusy = false;
//        }).OnRewind(() =>
//        {
//            if (m_OnChangeSuccess != null) m_OnChangeSuccess();
//            m_IsBusy = false;
//        });
//    }

//    public void ChangeState(Action OnChangeSuccess)
//    {
//        if (m_IsBusy) return;
//        m_IsBusy = true;

//        m_OnChangeSuccess = OnChangeSuccess;
//        if (m_IsShow)
//        {
//            transform.DOPlayForward();
//        }
//        else
//        {
//            transform.DOPlayBackwards();
//        }

//        m_IsShow = !m_IsShow;
//    }

//    protected override void BeforeOnDestroy()
//    {
//        base.BeforeOnDestroy();

//        Instance = null;
//        m_OnChangeSuccess = null;
//    }
//}