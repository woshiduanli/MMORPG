
using UnityEngine;
using System.Collections;

public class RoleHeadBarCtrl : MonoBehaviour
{
    /// <summary>
    /// 昵称
    /// </summary>
    [SerializeField]
    private UILabel lblNickName;

    /// <summary>
    /// 飘血显示
    /// </summary>
    [SerializeField]
    private HUDText hudText;

    /// <summary>
    /// 血条
    /// </summary>
    [SerializeField]
    private UISlider pbHP;

    /// <summary>
    /// 对齐的目标点
    /// </summary>
    private Transform m_Target;

    void Start()
    {

    }

    void Update()
    {
        if (Camera.main == null || UICamera.mainCamera == null || m_Target == null) return;

        //世界左边点 转换成视口坐标
        Vector3 pos = Camera.main.WorldToViewportPoint(m_Target.position);

        //转换成UI摄像机的世界坐标
        Vector3 uiPos = UICamera.mainCamera.ViewportToWorldPoint(pos);

        gameObject.transform.position = uiPos;
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

        NGUITools.SetActive(pbHP.gameObject, isShowHPBar);
    }

    /// <summary>
    /// 上弹伤害文字
    /// </summary>
    /// <param name="hurtValue"></param>
    public void Hurt(int hurtValue, float pbHPValue = 0)
    {
        hudText.Add(string.Format("-{0}", hurtValue), Color.red, 0.1f);
        pbHP.value = pbHPValue;
    }
}