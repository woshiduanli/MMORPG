using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSceneCtrlMgr : MonoBehaviour
{
    [SerializeField]
    GameLevelSceneCtrl gameLevelSceneCtrl;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    WorldMapSceneCtrl worldMapSceneCtrl;

    Dictionary<SceneType, GameObject> m_dic = new Dictionary<SceneType, GameObject>();

    private void Awake()
    {
        if (gameLevelSceneCtrl != null)
            m_dic[SceneType.GameLevel] = gameLevelSceneCtrl.gameObject;

        if (worldMapSceneCtrl != null)
            m_dic[SceneType.WorldMap] = worldMapSceneCtrl.gameObject;


        foreach (var item in m_dic.Keys)
        {
            if (item != SceneMgr.Instance.CurrentSceneType)
                Object.Destroy(m_dic[item]);
        }

        if (m_dic.ContainsKey(SceneMgr.Instance.CurrentSceneType))
            m_dic[SceneMgr.Instance.CurrentSceneType].gameObject.SetActive(true);
    }

    // Use this for initialization
    void Start()
    {

    }

    // 返回 用center用圆心， radius为半径的圆上的随机点 
    private Vector3 OnPostRender(Vector3 center, float radius)
    {
        Vector3 v = new Vector3(0, 0, 1);
        v = Quaternion.Euler(0, Random.Range(0, 360), 0) * v;
        Vector3 pos = v * radius * Random.Range(0.8f, 1);
        Vector3 newPos = center + pos;
        return newPos;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
