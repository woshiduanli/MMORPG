using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Threading;
using System.Collections.Generic;
public class testLoom : MonoBehaviour
{

    public Text mText;
    void Start()
    {
        //GameObject obj = GameObject.Find("test1");
        //for (int i = 0; i < 100; i++)
        //{

        //Instantiate(obj); 
        //}
        //ScaleMesh2();
        // 用Loom的方法调用一个线程
        //Loom.RunAsync(
        //    () =>
        //    {
        //        Thread thread = new Thread(RefreshText);
        //        thread.Start();
        //    }
        //    );
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameObject obj = GameObject.Find("test1");
            MyDebug.debug(System.DateTime.Now);
            for (int i = 0; i < 1000; i++)
            {

                Instantiate(obj);
            }
            MyDebug.debug(System.DateTime.Now);

        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            CText obj = GameObject.Find("test1").GetComponent<CText>();
            List<string> list = new List<string>();
            List<CText> list2 = new List<CText>();
            for (int i = 0; i < 100; i++)
            {
                list.Add(i.ToString());
            }
            ToDat(obj, list, list2);

            //MyDebug.debug("222:" + System.DateTime.Now);

            //Loom.RunAsync(() =>
            //{
            //    for (int i = 0; i < 1000; i++)
            //    {
            //        //MyDebug.debug("sfsf1:   " + System.DateTime.Now);
            //        Thread.Sleep(1);
            //        //MyDebug.debug("sfsf2:   " + System.DateTime.Now);

            //        Loom.QueueOnMainThread(() =>
            //        {
            //            if (i == 999)
            //            {
            //                MyDebug.debug("333:" + System.DateTime.Now);

            //            }
            //            GameObject obj222 = Instantiate(obj);
            //            CText ext = obj222.GetComponent<CText>();




            //        });
            //    }
            //});
            //MyDebug.debug("111:" + System.DateTime.Now);


            //cortine.start( (at){
            //          if ()
            //} )
        }
    }
    public void ToDat2(CText text, List<string> strData, List<CText> listText)
    {
        //for (int i = 0; i < listText.Count; i++)
        //{

        //}
    }

    public void ToDat(CText text, List<string> strData, List<CText> listText)
    {
        int j = 0;
        Loom.RunAsync(() =>
           {
               for (int i = 0; i < strData.Count; i++)
               {
                   Thread.Sleep(1);
                   Loom.QueueOnMainThread(() =>
                   {
                       GameObject obj222 = Instantiate(text.gameObject);
                       CText ext = obj222.GetComponent<CText>();
                       listText.Add(ext);
                       ext.text = strData[j];

                   });
               }
           });
    }

    //这个里面的Unity组件， 在线程里面去执行了
    private void RefreshText()
    {
        Loom.QueueOnMainThread(() => { mText.text = "Hello Loom!"; }, 0);
        // 用Loom的方法在Unity主线程中调用Text组件
        //Loom.QueueOnMainThread(() =>
        //{
        //    mText.text = "Hello Loom!";
        //}, null);
    }

    //Scale a mesh on a second thread
    void ScaleMesh(Mesh mesh, float scale)
    {
        //Get the vertices of a mesh
        var vertices = mesh.vertices;
        //Run the action on a new thread
        Loom.RunAsync(() =>
        {
            //Loop through the vertices
            for (var i = 0; i < vertices.Length; i++)
            {
                //Scale the vertex
                vertices[i] = vertices[i] * scale;
            }
            //Run some code on the main thread
            //to update the mesh
            Loom.QueueOnMainThread(() =>
            {
                //Set the vertices
                mesh.vertices = vertices;
                //Recalculate the bounds
                mesh.RecalculateBounds();
            });

        });
    }
    void ScaleMesh2()
    {


        Loom.RunAsync(() =>
        {
            for (int i = 0; i < 5; i++)
            {
                MyDebug.debug("sfsf1:   " + System.DateTime.Now);
                Thread.Sleep(1000);
                MyDebug.debug("sfsf2:   " + System.DateTime.Now);

                Loom.QueueOnMainThread(() =>
                {
                    MyDebug.debug("sfsf3:   " + System.DateTime.Now);

                });
            }
        });
    }

}