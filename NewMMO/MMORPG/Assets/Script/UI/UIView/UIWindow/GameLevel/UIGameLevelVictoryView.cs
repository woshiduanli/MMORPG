using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIGameLevelVictoryView : UIWindowViewBase
{
    [SerializeField]
    private Text lblPassTime;

    [SerializeField]
    private Text lblExp;

    [SerializeField]
    private Text lblGold;

    [SerializeField]
    private Transform[] m_Stars;

    //[SerializeField]
    //private UIGameLevelRewardView[] m_RewardViews;

    protected override void OnAwake()
    {
        base.OnAwake();

        //for (int i = 0; i < m_Stars.Length; i++)
        //{
        //    m_Stars[i].gameObject.SetActive(false);
        //}

        //for (int i = 0; i < m_RewardViews.Length; i++)
        //{
        //    m_RewardViews[i].gameObject.SetActive(false);
        //}
    }

    protected override void OnStart()
    {
        base.OnStart();
    }
    protected override void OnBtnClick(GameObject go)
    {
        base.OnBtnClick(go);

        switch (go.name)
        {
            case "btnReturn":
                Debug.LogError("点钟了");
                SceneMgr.Instance.LoadToWorldMap(GlobalInit.Instance.LastInWorldMapId);

                break;
        }
    }

    public void SetUI(TransferData data)
    {
        float time = data.GetValue<float>(ConstDefine.GameLevelPassTime);
        //lblPassTime.SetText(string.Format("通关时间：<color=#ff0000>{0}秒</color>", time.ToString("f0")), true, DG.Tweening.ScrambleMode.None);
        //lblExp.SetText(data.GetValue<int>(ConstDefine.GameLevelExp).ToString(), true, DG.Tweening.ScrambleMode.Numerals);
        //lblGold.SetText(data.GetValue<int>(ConstDefine.GameLevelGold).ToString(), true, DG.Tweening.ScrambleMode.Numerals);

        //获得的星级 1-3
        int star = data.GetValue<int>(ConstDefine.GameLevelStar);

        for (int i = 0; i < m_Stars.Length; i++)
        {
            if (i >= star) break;
            m_Stars[i].gameObject.SetActive(true);
        }

        //接收奖励的物品
        List<TransferData> lstReward = data.GetValue<List<TransferData>>(ConstDefine.GameLevelReward);

        if (lstReward.Count > 0)
        {
            for (int i = 0; i < lstReward.Count; i++)
            {
                //m_RewardViews[i].gameObject.SetActive(true);
                //m_RewardViews[i].SetUI(lstReward[i].GetValue<string>(ConstDefine.GoodsName), lstReward[i].GetValue<int>(ConstDefine.GoodsId), lstReward[i].GetValue<GoodsType>(ConstDefine.GoodsType));
            }
        }
    }
}
