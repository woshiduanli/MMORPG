
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class RoleHeadBarView : MonoBehaviour
{

    /// <summary>
    /// 昵称
    /// </summary>
    [SerializeField]
    private Text lblNickName;

    [SerializeField]
    private UIFollowTarget followTarget;
    [SerializeField]
    private HUDText hudText;

    [SerializeField]
    public Slider sliderHp;

    /// </summary>
    private Transform m_Target;

    RectTransform m_Trans;
    void Start()
    {
        m_Trans = this.GetComponent<RectTransform>();
    }
    public static bool isCameraWithinScreen(Vector3 pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x <= Screen.width && pos.y <= Screen.height && pos.z > 0;
    }

    Vector3 m_screenPos;
    // 让一个ui挂在某个世界空间的点下
    public void WolrdPostionToRectTransfromToWorldPos(Vector3 worldPos, RectTransform rect, Camera uiCamera)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        if (m_screenPos == screenPos) return;
        m_screenPos = screenPos;
        rect.gameObject.transform.position = uiCamera.ScreenToWorldPoint(screenPos);
        Vector3 localPos = rect.gameObject.transform.localPosition;
        localPos.z = 0;
        rect.gameObject.transform.localPosition = localPos;
    }

    void Update()
    {
        if (m_Target != null)
        {
            WolrdPostionToRectTransfromToWorldPos(m_Target.position, m_Trans, UI_Camera222.Instance.camera);
            //m_Target = ctrl.transform.Find("TitleBarPos");
        }
    }
    RoleCtrl ctrl;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <param name="nickName"></param>
    /// <param name="isShowHPBar">是否显示血条</param>
    public void Init(RoleCtrl ctrl, Transform target, string nickName, bool isShowHPBar = false, float SliderValue = 1)
    {
        this.ctrl = ctrl;
        m_Target = target;
        lblNickName.text = nickName;
        if (sliderHp == null) sliderHp = transform.Find("sliderHP").GetComponent<Slider>();
        sliderHp.gameObject.SetActive(isShowHPBar);

        Debug.LogError("fuzhi le ::::::::::::::::::::::");
        sliderHp.value = SliderValue;
    }


    public void SetSliderHp(float SliderValue = 1)
    {
        sliderHp.value = SliderValue;
    }

    /// <summary>
    /// 上弹伤害文字
    /// </summary>
    /// <param name="hurtValue"></param>
    public void Hurt(int hurtValue, float pbHPValue = 0)
    {
        //hudText.Add(string.Format("-{0}", hurtValue), Color.red, 0.1f);
        //pbHP.value = pbHPValue;
    }

    internal void BloodFly()
    {



        //if (followTarget==null) followTarget = transform.Find("follow").GetComponent<UIFollowTarget> ();

        //if (hudText==null)
        //{
        //    hudText=transform.Find("hudtext").GetComponent<HUDText>();
        //}
        //followTarget.target = hudText.transform;
        //followTarget.uiCamera = GameObject.Find("UICamera").GetComponent<Camera> (); 
        //hudText.Add("-5", Color.red, 1f);
    }
}