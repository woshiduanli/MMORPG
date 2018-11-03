
using UnityEngine;
using System.Collections;

/// <summary>
/// UI摄像机
/// </summary>
public class UI_Camera222 : MonoBehaviour
{
    [HideInInspector]
    public new Camera camera;

    public static UI_Camera222 Instance;

    void Awake()
    {

        Instance = this;
    }

    void Start()
    {
        camera = transform.GetComponent<Camera>();
    }
}