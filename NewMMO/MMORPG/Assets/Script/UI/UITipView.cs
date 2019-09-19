using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PathologicalGames;

public class UITipView : UISubViewBase
{
    public static UITipView Instance;

    /// <summary>
    /// 提示队列
    /// </summary>
    private Queue<TipEntity> m_TipQueue;

    /// <summary>
    /// 上次提示时间
    /// </summary>
    private float m_PrevTipTime = 0;

    /// <summary>
    /// Tip池
    /// </summary>
    private SpawnPool m_TipPool;

    private Transform m_TipItem;

    protected override void OnAwake()
    {
        base.OnAwake();
        Instance = this;
        m_TipQueue = new Queue<TipEntity>();
    }

    protected override void OnStart()
    {
        base.OnStart();
        //return;
        //Transform m_TipItem;
        m_TipItem = ResourcesMgr.Instance.Load(ResourcesMgr.ResourceType.UIOther, "UITipItem", true).transform;
        //m_TipItem = obj.transform;

        m_TipPool = PoolManager.Pools.Create("TipPool");
        m_TipPool.group.parent = transform;
        m_TipPool.group.localPosition = Vector3.zero;

        PrefabPool prefabPool = new PrefabPool(m_TipItem);
        prefabPool.preloadAmount = 5; //预加载数量

        prefabPool.cullDespawned = true; //是否开启缓存池自动清理模式
        prefabPool.cullAbove = 10;// 缓存池自动清理 但是始终保留几个对象不清理
        prefabPool.cullDelay = 2;//多长时间清理一次 单位是秒
        prefabPool.cullMaxPerPass = 2; //每次清理几个

        m_TipPool.CreatePrefabPool(prefabPool);


        //m_TipItem = ResourcesMgr.Instance.Load(ResourcesMgr.ResourceType.UIOther, "UITipItem", cache: true, returnClone: false).transform;

    }

    public void ShowTip(int type, string text)
    {
        //加入队列
        m_TipQueue.Enqueue(new TipEntity() { Type = type, Text = text });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ShowTip(1, "+200");
        }
        //显示提示
        if (Time.time > m_PrevTipTime + 0.5f)
        {
            m_PrevTipTime = Time.time;

            if (m_TipQueue.Count > 0)
            {
                //从队列中取数据
                TipEntity entity = m_TipQueue.Dequeue();

                Transform trans = m_TipPool.Spawn(m_TipItem);
                //Transform trans = null;

                //trans = m_TipPool.Spawn();

                trans.GetComponent<UITipItemView>().SetUI(entity.Type, entity.Text);
                trans.SetParent(transform);
                trans.localPosition = Vector3.zero;
                trans.localScale = Vector3.one;

                trans.DOLocalMoveY(150, 0.5f).OnComplete(() =>
                {
                    //Destroy(trans.gameObject);
                    //trans.gameObject.SetActive(false);
                    m_TipPool.Despawn(trans);
                });
            }
        }
    }
}