
using UnityEngine;
using System.Collections;

public class MogoSkybox : MonoBehaviour
{
    void Start()
    {
        if (GlobalInit.Instance == null) return;

        Renderer[] arr = GetComponentsInChildren<Renderer>(true);

        if (arr != null && arr.Length > 0)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i].material.shader = GlobalInit.Instance.MogoSkyboxShader;
            }
        }
        Destroy(this);
    }
}