


using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WWWLoad
{
    private WWW www = null;
    static System.Diagnostics.Stopwatch stopWatch = new System.Diagnostics.Stopwatch();
    /// <summary>
    /// 下载文件
    /// </summary>
    public IEnumerator DownFile(string url, string savePath, Action<WWW> process = null)
    {
        FileInfo file = new FileInfo(savePath);
        stopWatch.Start();
        UnityEngine.Debug.Log("Start:" + Time.realtimeSinceStartup);
        www = new WWW(url);
        while (!www.isDone)
        {
            yield return 0;
            if (process != null)
                process(www);
        }
        yield return www;
        if (www.isDone)
        {
            byte[] bytes = www.bytes;
            CreatFile(savePath, bytes);
        }
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="bytes"></param>
    public void CreatFile(string savePath, byte[] bytes)
    {
        FileStream fs = new FileStream(savePath, FileMode.Append);
        BinaryWriter bw = new BinaryWriter(fs);
        fs.Write(bytes, 0, bytes.Length);
        fs.Flush();     //流会缓冲，此行代码指示流不要缓冲数据，立即写入到文件。
        fs.Close();     //关闭流并释放所有资源，同时将缓冲区的没有写入的数据，写入然后再关闭。
        fs.Dispose();   //释放流
        www.Dispose();

        stopWatch.Stop();
        Debug.Log("下载完成,耗时:" + stopWatch.ElapsedMilliseconds);
        UnityEngine.Debug.Log("End:" + Time.realtimeSinceStartup);


        AssetBundle dd = AssetBundle.LoadFromFile(savePath);
        UnityEngine.Object obj = dd.LoadAsset("role_mainplayer_cike");
        UnityEngine.Object.Instantiate(obj);
    }
}


public class TestCode : MonoBehaviour
{
    Animator a;
    // Use this for initialization
    void Start()
    {
        return;
        a = GetComponent<Animator>();
        a.SetBool("ToRun", true);




        return;
        FingerEvent.Instance.OnFingerDrag += OnFingerDrag;
    }

    private void OnFingerDrag(FingerEvent.FingerDir obj)
    {
        MyDebug.debug(obj.ToString());
        switch (obj)
        {
            case FingerEvent.FingerDir.Left:
                //MyDebug.debug(); 
                //CameraCtrl.Instance.SetCameraRotate(0);
                break;
            case FingerEvent.FingerDir.Right:
                //CameraCtrl.Instance.SetCameraRotate(1);
                break;
            case FingerEvent.FingerDir.Up:
                //CameraCtrl.Instance.SetCameraUpAndDown(1);
                break;
            case FingerEvent.FingerDir.Down:
                //CameraCtrl.Instance.SetCameraUpAndDown(0);
                break;
        }


    }
    Vector3 tar;
    bool isMove;
    bool isclick;
    Vector3 dic;

    bool needzhua = true; 
    float m_RotationSpeed;

    private Quaternion m_TargetQuaternion;
    private void OnPlayerClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hitInfo;

            RaycastHit[] hitArr = Physics.RaycastAll(ray, Mathf.Infinity, 1 << LayerMask.NameToLayer("Role"));

            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.collider.gameObject.tag.Equals("Road", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    isclick = true;
                    //CharacterController c = this.GetComponent<CharacterController>();
                    tar = hitInfo.point;
                    Vector3 dic = tar - transform.position;
                    dic = new Vector3(dic.x, dic.y - 1000f, dic.z);

                }
            }

        }

        CharacterController c = this.GetComponent<CharacterController>();
        if (isclick && Vector3.Distance(tar, transform.position) >= 1f)
        {
            isMove = true;
            dic = tar - transform.position;
            dic = new Vector3(dic.x, 0, dic.z);
            dic = dic.normalized;
        }
        else
        {
            isclick = false;
            isMove = false;
        }

        if (isMove && isclick)
        {
            if (needzhua)
            m_TargetQuaternion = Quaternion.LookRotation(dic);
            if (Quaternion.Angle(transform.rotation, m_TargetQuaternion) > 1)
            {
                if (m_RotationSpeed <= 1)
                {
                    m_RotationSpeed += 10f * Time.deltaTime;
                    if (Quaternion.Angle(transform.rotation, m_TargetQuaternion) > 1)
                    {

                    }
                    m_TargetQuaternion = Quaternion.LookRotation(dic);
                    transform.rotation = Quaternion.Lerp(transform.rotation, m_TargetQuaternion, m_RotationSpeed);

                    if (Quaternion.Angle(transform.rotation, m_TargetQuaternion) < 1)
                    {
                        m_RotationSpeed = 0;
                    }
                }
            }

            c.Move(dic*Time.deltaTime *10);
        }
    }
    // Update is called once per frame
    void Update()
    {
        OnPlayerClick();
        if (Input.GetKeyDown(KeyCode.A))
        {
            MyDebug.debug("sf");

            CharacterController c = this.GetComponent<CharacterController>();
            c.Move(Vector3.one * 20);
            //WWWLoad w = new WWWLoad();
            //string str = @"file://G:\FanFanKeTang\MMORPG2\NewMMO\MMORPG\AssetBundles\Android\Role\role_mainplayer_cike.assetbundle";
            //MyDebug.debug(Application.dataPath);

            //StartCoroutine(w.DownFile(str, Application.dataPath + "/role_mainplayer_cike.assetbundle"));
        }
        //a = GetComponent<Animator>();
        //a.SetBool("ToRun", true);

        return;
        AnimatorStateInfo info = a.GetCurrentAnimatorStateInfo(0);

        if (info.IsName(RoleAnimatorState.Run.ToString()))
        {
            a.SetInteger(ToAnimatorCondition.CurrState.ToString(), (int)RoleState.Run);
        }
        else
        {
            a.SetInteger(ToAnimatorCondition.CurrState.ToString(), 0);
        }
    }
}
