using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class testCode : MonoBehaviour {

    private void OnGUI()
    {
        if (GUI.Button(new Rect(50, 50, 200, 200),"点击"))
        {
            Debug.Log("被点击了");
        }
    }
}
