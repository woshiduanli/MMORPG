using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace testdll
{


    public class testcodetwo
    {


        public int addd(int i, int j)
        {
            return i + j;
        }

        public void StartGame(int ddddd )
        {
            Debug.Log("开始了游戏"+ ddddd);

            GameObject.Find("Main Camera").AddComponent<testCode>();

        }
    }
}
