using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PathologicalGames;
using System;

public class GameLevelSceneCtrl : GameSceneCtrlbase
{
    [SerializeField]
    GameLevelRegionCtrl[] AllRegion;

    List<GameLevelRegionEntity> m_regionList;

    int m_curRegionIndex;

    int m_CurrGameLevelId;

    SpawnPool m_monsterPool;




    protected override void OnLoadUIMainCityViewComplete(GameObject obj)
    {
        base.OnLoadUIMainCityViewComplete(obj);
        m_CurrGameLevelId = SceneMgr.Instance.CurGameLevelId;


        m_regionList = GameLevelRegionDBModel.Instance.GetListByGameLevelId(m_CurrGameLevelId);


        m_allMonsterCount = GameLevelMonsterDBModel.Instance.GetGameLevelMonsterCount(m_CurrGameLevelId, m_curGrade);
        m_monsterId = GameLevelMonsterDBModel.Instance.GetGameLevelMonsterId(m_CurrGameLevelId, m_curGrade);

        // 创建怪物池
        m_monsterPool = PoolManager.Pools.Create("Monster");
        m_monsterPool.group.parent = null;
        m_monsterPool.group.localPosition = Vector3.zero;

        for (int i = 0; i < m_monsterId.Length; i++)
        {
            PrefabPool prefabPool = null;
            //m_monsterId[i] = 1003;
            if (RoleMgr.Instance.LoadSprite(m_monsterId[i]) != null)
                //if (RoleMgr.Instance.LoadSprite(1003) != null)
                prefabPool = new PrefabPool(RoleMgr.Instance.LoadSprite(m_monsterId[i]).transform);
            if (prefabPool != null)
            {
                prefabPool.preloadAmount = 5;
                // 是否开启自动清理
                prefabPool.cullDespawned = true;

                // 自动清理， 但是保存5个不清理
                prefabPool.cullAbove = 5;

                // 多长时间清理一次
                prefabPool.cullDelay = 2;

                // 每次清理几个
                prefabPool.cullMaxPerPass = 2;

                m_monsterPool.CreatePrefabPool(prefabPool);
            }
        }

        m_curRegionIndex = 0;
        EnterRegion(m_curRegionIndex);
    }

    // 当前怪的数量
    int m_allMonsterCount;


    // 当前关卡的难度等级
    GameLevelGrade m_curGrade;

    int m_curRegionKillMonsterCount;

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
        m_curRegionKillMonsterCount = 0;
        if (regionIndex == 0)
        {
            GlobalInit.Instance.CurrPlayer.Born(m_curRegionCtrl.RoleBornPos.position);
            GlobalInit.Instance.CurrPlayer.ToIdle(RoleIdleState.IdelFight);
        }

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

    // 临时怪的id
    int m_MonsterTemp;
    int m_index = 0;
    bool isfrat = false;
    void CreateMonster()
    {
        // 临时测试
        //if (m_curRegionCreateMonsterCount >= 1)
        //{
        //    return;

        //}
        //if (isfrat) return; isfrat = true; 
        m_index = UnityEngine.Random.Range(0, m_regionMonster.Count);

        //int monsterId = 1003;
        int monsterId = UnityEngine.Random.Range(1001, 1004);

        //if (isfrat) return; isfrat = true;
        //int monsterId = m_regionMonster[m_index].SpriteId;

        List<SpriteEntity> list = SpriteDBModel.Instance.GetList();
        //for (int i = 0; i < list.Count; i++)
        //{

        //}



        Transform trans = null;
        if (SpriteDBModel.Instance.Get(monsterId) != null)
            trans = m_monsterPool.Spawn(SpriteDBModel.Instance.Get(monsterId).PrefabName);


        if (trans == null) return;
        if (m_curRegionCtrl.MonsterBornPos.Length == 0)
        {
            trans.position = new Vector3(0, -19, 7);
        }
        else
        {
            Transform monsterBornPos = m_curRegionCtrl.MonsterBornPos[UnityEngine.Random.Range(0, m_curRegionCtrl.MonsterBornPos.Length)];
            trans.position = monsterBornPos.TransformPoint(UnityEngine.Random.Range(-0.5f, 0.5f), 0, UnityEngine.Random.Range(0.5f, 0.5f));
        }


        RoleCtrl roleMonsterCtrl = trans.GetComponent<RoleCtrl>();
        RoleInfoMonster monsterInfo = new RoleInfoMonster();

        SpriteEntity entity = SpriteDBModel.Instance.Get(monsterId);
        Debug.LogError("monsterId:" + monsterId);
        if (entity != null)
        {
            monsterInfo.SpriteEntity = entity;
            monsterInfo.RoldId = ++m_MonsterTemp;
            monsterInfo.RoleNickName = entity.Name;
            monsterInfo.Level = entity.Level;
            monsterInfo.MaxHP = monsterInfo.CurrHP = entity.HP;
            //Debug.LogError("dangqian  hp :::::::::::::::::::::::::::" + monsterInfo.CurrHP);
            monsterInfo.MaxMP = monsterInfo.CurrHP = entity.MP;


            monsterInfo.Attack = entity.Attack;
            monsterInfo.Defense = entity.Defense;
            monsterInfo.Hit = entity.Hit;
            monsterInfo.Dodge = entity.Dodge;
            monsterInfo.Cri = entity.Cri;
            monsterInfo.Res = entity.Res;
            monsterInfo.Fighting = entity.Fighting;


            roleMonsterCtrl.Speed = entity.MoveSpeed;
            roleMonsterCtrl.ViewRange = entity.Range_View;
        }

        roleMonsterCtrl.Init(RoleType.Monster, monsterInfo, new GameLevel_RoleMonsterAI(roleMonsterCtrl, monsterInfo));

        Debug.LogError("怪物id ::::::::::" + monsterInfo.SpriteEntity.Id);
        roleMonsterCtrl.OnRoleDestroy = OnRoleDestroyCallBack;
        roleMonsterCtrl.OnRoleDie = OnRoleDieCallBack;
        roleMonsterCtrl.Born(trans.position);

        m_regionMonster[m_index].SpriteCount--;
        if (m_regionMonster[m_index].SpriteCount <= 0)
            m_regionMonster.RemoveAt(m_index);
        //m_regionMonster[monsterId]--;

        ++m_curRegionCreateMonsterCount;
    }

    void OnRoleDieCallBack(RoleCtrl roleId)
    {
        m_curRegionKillMonsterCount++;
        Debug.LogError("  " + m_curRegionKillMonsterCount + "   " + m_curRegionMonsterCount);
        //if (m_curRegionKillMonsterCount >= m_curRegionMonsterCount-1)
        //{
        m_curRegionIndex++;
        Debug.LogError("  " + m_curRegionKillMonsterCount + "   :331333");

        if (m_curRegionIndex >= m_regionList.Count)
        {
            // TO DO 弹出胜利界面

            Debug.LogError("弹出胜利界面   ---------------------------------------------------------------");
            return;
        }
        // 能进入下一个区域
        Debug.LogError("  " + m_curRegionKillMonsterCount + "   :333");
        //m_curRegionKillMonsterCount = 0
        EnterRegion(m_curRegionIndex);
        //}
    }

    private void OnRoleDestroyCallBack(Transform obj)
    {
        // 回池操作
        m_monsterPool.Despawn(obj);
    }
}
