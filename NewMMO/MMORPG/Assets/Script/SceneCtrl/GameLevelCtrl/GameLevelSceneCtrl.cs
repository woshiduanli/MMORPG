using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//using PathologicalGames;

public class GameLevelSceneCtrl : GameSceneCtrlbase
{
    [SerializeField]
    GameLevelRegionCtrl[] AllRegion;

    List<GameLevelRegionEntity> m_regionList;

    int m_curRegionIndex;

    int m_CurrGameLevelId;

    //SpawnPool m_monsterPool;




    protected override void OnLoadUIMainCityViewComplete(GameObject obj)
    {
        //base.OnLoadUIMainCityViewComplete(obj);
        //m_CurrGameLevelId = SceneMgr.Instance.CurGameLevelId;


        //m_regionList = GameLevelRegionDBModel.Instance.GetListByGameLevelId(m_CurrGameLevelId);


        //m_allMonsterCount = GameLevelMonsterDBModel.Instance.GetGameLevelMonsterCount(m_CurrGameLevelId, m_curGrade);
        //m_monsterId = GameLevelMonsterDBModel.Instance.GetGameLevelMonsterId(m_CurrGameLevelId, m_curGrade);

        //// 创建怪物池
        //m_monsterPool = PoolManager.Pools.Create("Monster");
        //m_monsterPool.group.parent = null;
        //m_monsterPool.group.localPosition = Vector3.zero;

        //for (int i = 0; i < m_monsterId.Length; i++)
        //{
        //    PrefabPool prefabPool = null;
        //    if (RoleMgr.Instance.LoadSprite(m_monsterId[i]) != null)
        //        prefabPool = new PrefabPool(RoleMgr.Instance.LoadSprite(m_monsterId[i]).transform);
        //    if (prefabPool != null)
        //    {
        //        prefabPool.preloadAmount = 5;
        //        // 是否开启自动清理
        //        prefabPool.cullDespawned = true;

        //        // 自动清理， 但是保存5个不清理
        //        prefabPool.cullAbove = 5;

        //        // 多长时间清理一次
        //        prefabPool.cullDelay = 2;

        //        // 每次清理几个
        //        prefabPool.cullMaxPerPass = 2;

        //        m_monsterPool.CreatePrefabPool(prefabPool);
        //    }
        //}

        m_curRegionIndex = 0;
        EnterRegion(m_curRegionIndex);
    }

    // 当前怪的数量
    int m_allMonsterCount;

    // 当前关卡的难度等级
    GameLevelGrade m_curGrade;

    int m_curRegionMonsterCount;

    int[] m_monsterId;

    GameLevelRegionCtrl m_curRegionCtrl;

    List<GameLevelMonsterEntity> m_regionMonster = new List<GameLevelMonsterEntity>();

    void EnterRegion(int regionIndex)
    {
        // 
        GameLevelRegionEntity entity = GetGameLevelRegionEntityByIndex(regionIndex);
        if (entity != null)
            m_curRegionCtrl = GetRegionCtrlByRegionId(entity.RegionId);
        if (m_curRegionCtrl == null) return;

        m_curRegionCreateMonsterCount = 0;

        if (regionIndex == 0)
            if (AllRegion != null && GlobalInit.Instance.CurrPlayer != null) GlobalInit.Instance.CurrPlayer.transform.position = m_curRegionCtrl.RoleBornPos.position;

        if (DelegateDefine.Instance.OnSceneLoadOk != null)
            DelegateDefine.Instance.OnSceneLoadOk();


        m_curRegionMonsterCount = GameLevelMonsterDBModel.Instance.GetGameLevelMonsterCount(m_CurrGameLevelId, m_curGrade, regionIndex + 1);

        m_regionMonster = GameLevelMonsterDBModel.Instance.GetGameLevelMonster(m_CurrGameLevelId, m_curGrade, regionIndex + 1);

    }

    GameLevelRegionCtrl GetRegionCtrlByRegionId(int regionId)
    {
        if (AllRegion != null)
            for (int i = 0; i < AllRegion.Length; i++)
            {
                if (AllRegion[i].RegionId == regionId)
                {
                    return AllRegion[i];
                }
            }
        return null;
    }

    GameLevelRegionEntity GetGameLevelRegionEntityByIndex(int index)
    {
        if (m_regionList != null && m_regionList.Count > index)
            return m_regionList[index];
        return null;
    }

    float m_nextCreateMonterTime;
    int m_curRegionCreateMonsterCount;
    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (m_curRegionCreateMonsterCount < m_curRegionMonsterCount)
        {
            if (Time.time > m_nextCreateMonterTime)
            {
                m_nextCreateMonterTime = Time.time + 1;
                CreateMonster();
            }
        }
    }

    public bool Getlevel(int trans)
    {
        if (ListCount.Count < trans) return false;

        hecheng(1, trans);
        return ListCount[trans - 1] >= 3;
    }


    
    List<int> ListCount = new List<int>();
    public void hecheng(int level, int trans)
    {
        if (level == trans || level == ListCount.Count)
            return;

        int index = level - 1;
        if (ListCount[index] >= 3)
        {
            int nextNum = ListCount[index] / 3;
            ListCount[index + 1] += nextNum;
        }
        level++;
        hecheng(level, trans);
    }


    int m_index = 0;
    void CreateMonster()
    {

        m_index = Random.Range(0, m_regionMonster.Count);

        int monsterId = m_regionMonster[m_index].SpriteId;

        Transform trans = null;
        if (SpriteDBModel.Instance.Get(monsterId) != null)
            //trans = m_monsterPool.Spawn(SpriteDBModel.Instance.Get(monsterId).PrefabName);
        if (trans == null) return;

        Transform monsterBornPos = m_curRegionCtrl.MonsterBornPos[Random.Range(0, m_curRegionCtrl.MonsterBornPos.Length)];

        trans.transform.position = monsterBornPos.TransformPoint(Random.Range(-0.5f, 0.5f), 0, Random.Range(0.5f, 0.5f));

        m_regionMonster[m_index].SpriteCount--;
        if (m_regionMonster[m_index].SpriteCount <= 0)
            m_regionMonster.RemoveAt(m_index);
        //m_regionMonster[monsterId]--;

        ++m_curRegionCreateMonsterCount;
    }
}
