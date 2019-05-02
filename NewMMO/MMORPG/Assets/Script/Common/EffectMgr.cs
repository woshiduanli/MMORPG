using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PathologicalGames;


public class EffectMgr : Singleton<EffectMgr>
{
    MonoBehaviour m_Mono;
    public GameObject LoadEffect(string effectName)
    {

        return AssetBundleMgr.Instance.Load("Effect/" + effectName.ToLower() + ".assetbundle", effectName);
    }

    private SpawnPool m_effectPool;

    Dictionary<string, Transform> m_EffectDic = new Dictionary<string, Transform>();

    public void Init(MonoBehaviour mono)
    {
        m_Mono = mono;
        m_effectPool = PoolManager.Pools.Create("Effect");
    }

    public Transform PlayEffect(string effectName)
    {
        if (!m_EffectDic.ContainsKey(effectName))
        {
            m_EffectDic[effectName] = LoadEffect(effectName).transform;
            PrefabPool prefabPool = new PrefabPool(m_EffectDic[effectName]);

            prefabPool.preloadAmount = 0;


            prefabPool.cullDespawned = true;

            prefabPool.cullAbove = 5;
            prefabPool.cullDelay = 2;
            prefabPool.cullMaxPerPass = 2;
            m_effectPool.CreatePrefabPool(prefabPool);
        }

        Transform transMonster = m_effectPool.Spawn(m_EffectDic[effectName]);

        return transMonster;

    }

    public void DestroyEffect(Transform effect, float delay)
    {
        m_Mono.StartCoroutine(DestroyEffectC(effect, delay));
    }

    IEnumerator DestroyEffectC(Transform effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        m_effectPool.Despawn(effect);
    }


    public void Clear()
    {
        m_EffectDic.Clear();
        m_effectPool = null;
    }


}
