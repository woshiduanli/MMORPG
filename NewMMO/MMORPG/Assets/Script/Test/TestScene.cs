
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 
/// </summary>
public class TestScene : MonoBehaviour 
{
    public static TestScene Instance;

    //Start之前执行
    void Awake()
    {
        Instance = this;
        //Debug.Log("Awake" + gameObject.name);
    }

    private int m_Ret = 0;

    //开始
    void Start()
    {
        //Debug.Log("Start" + gameObject.name);

        StartCoroutine(Test1(5, 8));
    }

    private IEnumerator Test1(int x, int y)
    {
        yield return new WaitForSeconds(2f);
        m_Ret = x * y;
        Debug.Log("Test1协程执行完毕");
    }

    //每帧执行
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("m_Ret=" + m_Ret);
        }
        else if (Input.GetKeyUp(KeyCode.B))
        {
        }
    }

    //Update之后执行
    void LateUpdate()
    {
        //Debug.Log("LateUpdate");
    }

    //固定时间间隔执行
    void FixedUpdate()
    {
        //Debug.Log("FixedUpdate");
    }

    //销毁的时候执行
    void OnDestroy()
    {
        //Debug.Log("OnDestroy");
    }

    //脚本可用的时候执行 在Start之前
    void OnEnable()
    {
        //Debug.Log("OnEnable");
    }

    //禁用的时候执行
    void OnDisable()
    {
        //Debug.Log("OnDisable");
    }
}