
using UnityEngine;
using System.Collections;
using System;

public class UIPlayerInfo : MonoBehaviour 
{
    /// <summary>
    /// 昵称
    /// </summary>
    [SerializeField]
    private UILabel lblNickName;

    /// <summary>
    /// HP
    /// </summary>
    [SerializeField]
    private UISprite sprHP;

    public static UIPlayerInfo Instance;

    void Awake()
    {
        Instance = this;
    }

	void Start ()
	{
        if (GlobalInit.Instance.CurrPlayer != null)
        {
            GlobalInit.Instance.CurrPlayer.OnRoleHurt = MainPlayerHurt;
        }
	}

    private void MainPlayerHurt()
    {
        sprHP.fillAmount = (float)GlobalInit.Instance.CurrPlayer.CurrRoleInfo.CurrHP / GlobalInit.Instance.CurrPlayer.CurrRoleInfo.MaxHP;
    }

    public void SetPlayerInfo()
    {
        lblNickName.text = GlobalInit.Instance.CurrPlayer.CurrRoleInfo.NickName;
    }
}