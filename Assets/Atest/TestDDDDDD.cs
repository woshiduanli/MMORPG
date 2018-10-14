using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDDDDDD : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        MyDebug.debug("start");
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.get){

        //}
    }
    void OnDisable()
    {
        MyDebug.debug("OnDisable");
    }
    void OnDestroy()
    {
        MyDebug.debug("OnDestroy");
    }
}
