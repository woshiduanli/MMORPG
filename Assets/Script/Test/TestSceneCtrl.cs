//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2015-11-08 23:13:07
//备    注：
//===================================================
using UnityEngine;
using System.Collections;

/// <summary>
/// 
/// </summary>
public class TestSceneCtrl : MonoBehaviour 
{
    /// <summary>
    /// 创建箱子的区域
    /// </summary>
    [SerializeField]
    private Transform transCreateBox;

    /// <summary>
    /// 箱子的父物体
    /// </summary>
    [SerializeField]
    private Transform boxParent;

    /// <summary>
    /// 箱子的预设
    /// </summary>
    private GameObject m_BoxPrefab;

    /// <summary>
    /// 当前箱子的数量
    /// </summary>
    private int m_CurrCount = 0;

    /// <summary>
    /// 最大数量
    /// </summary>
    private int m_MaxCount = 10;

    /// <summary>
    /// 下次克隆时间
    /// </summary>
    private float m_NextCloneTime = 0f;

    private string m_BoxKey = "BoxKey";

    /// <summary>
    /// 上次拾取箱子的数量
    /// </summary>
    private int m_PrevCount;

    /// <summary>
    /// 单例
    /// </summary>
    public static TestSceneCtrl Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        m_BoxPrefab = Resources.Load("RolePrefab/Item/xiangzi") as GameObject;

        m_PrevCount = PlayerPrefs.GetInt(m_BoxKey, 0);
    }

    void Update()
    {

        if (m_CurrCount < m_MaxCount)
        {
            if (Time.time > m_NextCloneTime)
            { 
                m_NextCloneTime = Time.time + 2f;

                //克隆
                GameObject objClone = Instantiate(m_BoxPrefab) as GameObject;

                objClone.transform.parent = boxParent;
                objClone.transform.position = transCreateBox.TransformPoint(new Vector3(Random.Range(-0.5f, 0.5f), 0, Random.Range(-0.5f, 0.5f)));
                BoxCtrl boxCtrl = objClone.GetComponent<BoxCtrl>();
                if (boxCtrl != null)
                {
                    boxCtrl.OnHit = BoxHit;
                    m_CurrCount++;
                }
                
            }
        }
    }

    private void BoxHit(GameObject obj)
    {
        m_CurrCount--;
        m_PrevCount++;
        PlayerPrefs.SetInt(m_BoxKey, m_PrevCount);

        GameObject.Destroy(obj);

        Debug.Log("累计拾取了" + m_PrevCount+"个箱子");
    }
}