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

        // ���������
        m_monsterPool = PoolManager.Pools.Create("Monster");
        m_monsterPool.group.parent = null;
        m_monsterPool.group.localPosition = Vector3.zero;

        for (int i = 0; i < m_monsterId.Length; i++)
        {
            PrefabPool prefabPool = null;
            if (RoleMgr.Instance.LoadSprite(m_monsterId[i]) != null)
                prefabPool = new PrefabPool(RoleMgr.Instance.LoadSprite(m_monsterId[i]).transform);
            if (prefabPool != null)
            {
                prefabPool.preloadAmount = 5;
                // �Ƿ����Զ�����
                prefabPool.cullDespawned = true;

                // �Զ����� ���Ǳ���5��������
                prefabPool.cullAbove = 5;

                // �೤ʱ������һ��
                prefabPool.cullDelay = 2;

                // ÿ��������
                prefabPool.cullMaxPerPass = 2;

                m_monsterPool.CreatePrefabPool(prefabPool);
            }
        }

        m_curRegionIndex = 0;
        EnterRegion(m_curRegionIndex);
    }

    // ��ǰ�ֵ�����
    int m_allMonsterCount;

    // ��ǰ�ؿ����Ѷȵȼ�
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

    // ��ʱ�ֵ�id
    int m_MonsterTemp;
    int m_index = 0;
    void CreateMonster()
    {
        // ��ʱ����
        //if (m_curRegionCreateMonsterCount >= 1)
        //{
        //    return;

        //}
        m_index =UnityEngine. Random.Range(0, m_regionMonster.Count);

        int monsterId = m_regionMonster[m_index].SpriteId;

        Transform trans = null;
        if (SpriteDBModel.Instance.Get(monsterId) != null && SpriteDBModel.Instance.Get(monsterId).IsBoss != 1)
            trans = m_monsterPool.Spawn(SpriteDBModel.Instance.Get(monsterId).PrefabName);
        if (trans == null) return;

        Transform monsterBornPos = m_curRegionCtrl.MonsterBornPos[UnityEngine.Random.Range(0, m_curRegionCtrl.MonsterBornPos.Length)];

        trans.position = monsterBornPos.TransformPoint(UnityEngine.Random.Range(-0.5f, 0.5f), 0, UnityEngine.Random.Range(0.5f, 0.5f));

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

            roleMonsterCtrl.ViewRange = entity.Range_View;
        }

        roleMonsterCtrl.Init(RoleType.Monster, monsterInfo, new GameLevel_RoleMonsterAI(roleMonsterCtrl, monsterInfo));
        roleMonsterCtrl.OnRoleDestroy = OnRoleDestroyCallBack;
        roleMonsterCtrl.Born(trans.position);

        m_regionMonster[m_index].SpriteCount--;
        if (m_regionMonster[m_index].SpriteCount <= 0)
            m_regionMonster.RemoveAt(m_index);
        //m_regionMonster[monsterId]--;

        ++m_curRegionCreateMonsterCount;
    }

    private void OnRoleDestroyCallBack(Transform obj)
    {
        // �سز���
        m_monsterPool.Despawn(obj);
    }
}
