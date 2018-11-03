
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class RoleHeadBarView : MonoBehaviour
{
    /// <summary>
    /// 昵称
    /// </summary>
    [SerializeField]
    private Text lblNickName;

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

    // 让一个ui挂在某个世界空间的点下
    public void WolrdPostionToRectTransfromToWorldPos(Vector3 worldPos, RectTransform rect, Camera uiCamera)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        bool isCanSee = screenPos.x >= 0 && screenPos.y >= 0 && screenPos.x <= Screen.width && screenPos.y <= Screen.height && screenPos.z > 0;
        if (isCanSee)
        {
            rect.gameObject.transform.position = uiCamera.ScreenToWorldPoint(screenPos);
            Vector3 localPos = rect.gameObject.transform.localPosition;
            localPos.z = 0;
            rect.gameObject.transform.localPosition = localPos;
        }
    }

    void Update()
    {
        WolrdPostionToRectTransfromToWorldPos(m_Target.position, m_Trans, UI_Camera222.Instance.camera);
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

        //NGUITools.SetActive(pbHP.gameObject, isShowHPBar);
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
}