
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class NPCHeadBarView : MonoBehaviour
{
    /// <summary>
    /// 昵称
    /// </summary>
    [SerializeField]
    private Text lblNickName;

    [SerializeField]
    Image imgTalkBG;
    [SerializeField]
    Text lblTalkText;

    Tweener m_ScaleTween;
    //Tweener m_

    /// </summary>
    private Transform m_Target;

    RectTransform m_Trans;
    void Start()
    {
        m_Trans = this.GetComponent<RectTransform>();
        imgTalkBG.transform.localScale = Vector3.one;
        m_ScaleTween = imgTalkBG.transform.DOScale(Vector3.one, 0.2f).SetAutoKill(false).Pause().OnComplete(() =>
        {

        }
        ).OnRewind(() =>
        {
            imgTalkBG.gameObject.SetActive(false);
        });

    }

    bool m_isTalk;
    float m_TalkStopTime;
    public void Talk(string text, float time)
    {
        m_isTalk = true;
        m_TalkStopTime = time;
        lblTalkText.text = text;
        imgTalkBG.gameObject.SetActive(true);
        m_ScaleTween.PlayForward();
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
        WolrdPostionToRectTransfromToWorldPos(m_Target.position, m_Trans, UI_Camera222.Instance.camera);

        if (Time.time % 10 >= 1 && Time.time % 10 <= 3)
        {
            if (!imgTalkBG.gameObject.activeSelf)
                imgTalkBG.gameObject.SetActive(true);
        }
        else
        {
            if (imgTalkBG.gameObject.activeSelf)
                imgTalkBG.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <param name="nickName"></param>
    /// <param name="isShowHPBar">是否显示血条</param>
    public void Init(Transform target, string nickName, bool isShowHPBar = false)
    {
        m_Target = target;
        lblNickName.text = nickName;
    }



}