//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2016-06-29 21:47:06
//备    注：
//===================================================
using UnityEngine;
using System.Collections;

/// <summary>
/// UI摄像机
/// </summary>
public class UI_Camera : MonoBehaviour
{
    [HideInInspector]
    public Camera Camera;

    public static UI_Camera Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Camera = GetComponent<Camera>();
    }
}