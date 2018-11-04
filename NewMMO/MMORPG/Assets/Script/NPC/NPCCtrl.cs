using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCtrl : MonoBehaviour
{


    [SerializeField]
    Transform m_HeadBarPos;

    NPCHeadBarView m_npcHeadBarView;

    float m_nextTalkTime;


    private string[] m_npcTalk; 
    NPCEntity m_CurrNPCEntity;
    GameObject m_HeadBar;
    private void Start()
    {



        InitHeadBar();
    }
    public void Init(NPCWorldMapData npcData)
    {
        m_CurrNPCEntity = NPCDBModel.Instance.Get(npcData.NPCId);
        MyDebug.debug("mingzi shi :" + m_CurrNPCEntity.Name);

        m_npcTalk = m_CurrNPCEntity.Talk.Split('|'); 

    }

    private void Update()
    {
        if (Time.time > m_nextTalkTime)
        {
            m_nextTalkTime = Time.time + 10;
            if (m_npcHeadBarView != null && m_npcTalk !=null && m_npcTalk.Length >0)
            {
                m_npcHeadBarView.Talk(m_npcTalk[UnityEngine.Random.Range(0, m_npcTalk.Length)], 3);
            }
        }
    }

    private void InitHeadBar()
    {

        if (RoleHeadBarRoot.Instance == null) return;
        if (m_CurrNPCEntity == null) return;
        if (m_HeadBarPos == null) return;

        //克隆预设
        m_HeadBar = ResourcesMgr.Instance.Load(ResourcesMgr.ResourceType.UIOther, "NPCHeadBar");
        m_HeadBar.transform.parent = RoleHeadBarRoot.Instance.gameObject.transform;
        m_HeadBar.transform.localScale = Vector3.one;

        m_npcHeadBarView = m_HeadBar.GetComponent<NPCHeadBarView>();

        ////给预设赋值
        m_npcHeadBarView.Init(m_HeadBarPos, m_CurrNPCEntity.Name);
    }



}
