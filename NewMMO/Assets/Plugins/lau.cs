using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

public interface Itestcodetwo
{
    void StartGame();
}

internal class lau : MonoBehaviour {
    Itestcodetwo ito; 
    // Use this for initialization
    string mdb_path;
    Assembly assembly;
    void Start () {

        mdb_path =   string.Concat(Application.streamingAssetsPath, "/") + "res/Game.dll.mdb"; 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnGUI()
    {
        if (GUI.Button(new Rect(200, 30, 200, 200), "点击2"))
        {
            FileStream stream = File.Open( string.Concat(Application.streamingAssetsPath, "/") + "res/Game.dll", FileMode.Open);

            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);
            stream.Close();

            if (string.IsNullOrEmpty(mdb_path) && File.Exists(mdb_path))
            {
                FileStream mdb_stream = File.Open(mdb_path, FileMode.Open);
                byte[] mdb_buffer = new byte[mdb_stream.Length];
                mdb_stream.Read(mdb_buffer, 0, (int)mdb_stream.Length);
                mdb_stream.Close();
                assembly = System.Reflection.Assembly.Load(buffer, mdb_buffer);
            }
            else
            {
                assembly = System.Reflection.Assembly.Load(buffer);
            }


            Type type = assembly.GetType("testdll.testcodetwo");

            object obj = Activator.CreateInstance(type);
            MethodInfo Add = type.GetMethod("StartGame");  //根据方法名获取MethodInfo对象
            Add.Invoke(obj, new object[1] { 1 });
            

        }
    }
}
