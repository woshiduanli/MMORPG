//===================================================

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// 关卡地图上的连线点
/// </summary>
public class UIGameLevelMapPointView : UISubViewBase
{
    [SerializeField]
    private Image imgPass;

    [SerializeField]
    private Image imgUnPass;

    public void SetUI(bool isPass)
    {
        if (isPass)
        {
            imgPass.gameObject.SetActive(true);
        }
        else
        {
            imgUnPass.gameObject.SetActive(true);
        }
    }
}