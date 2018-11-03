//===================================================

using UnityEngine;
using System.Collections;

public class SmallMapHelper : MonoBehaviour
{
    public static SmallMapHelper Instance;
    void Awake()
    {
        Instance = this;
    }
}