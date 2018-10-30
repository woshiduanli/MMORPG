using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRoleSceneCtrl : MonoBehaviour
{
    List<JobEntity> m_JobList;
    /// <summary>
    ///  职业镜像字典
    /// </summary>
    private Dictionary<int, GameObject> m_JobObjectDic = new Dictionary<int, GameObject>();
    private Dictionary<int, RoleCtrl> m_JobRoleCtrl = new Dictionary<int, RoleCtrl>();

    UISceneSelectRoleView sceneSelectRoleView;
    public Transform[] roleContainer;
    private void Awake()
    {
        sceneSelectRoleView = UISceneCtrl.Instance.LoadSceneUI(UISceneCtrl.SceneUIType.SelectRole).GetComponent<UISceneSelectRoleView>();



    }
    // Use this for initialization
    void Start()
    {
        if (DelegateDefine.Instance.OnSceneLoadOk != null)
        {
            DelegateDefine.Instance.OnSceneLoadOk();
        }

        if (sceneSelectRoleView!= null)
        {
            sceneSelectRoleView.SelectRoleDragView.OnSelectRoleDrag = OnSelectRoleDrag; 
        }

        // 监听协议
        EventDispatcher.Instance.RegProto<RoleOperation_LogOnGameServerReturnProto>(ProtoCodeDef.RoleOperation_LogOnGameServerReturn, OnLogOnGameServerReturn);

        LoadToObject();

        LogonGameServer();

        CloneCreateRole();
    }

    private Transform dragTarget;
    private float m_rorate = 90; 

    private void OnSelectRoleDrag(int obj)
    {
        MyDebug.debug(obj);
    }

    private void LoadToObject()
    {
        m_JobList = JobDBModel.Instance.GetList();
        for (int i = 0; i < m_JobList.Count; i++)
        {
            GameObject obj = AssetBundleMgr.Instance.Load("role/" + m_JobList[i].PrefabName.ToLower() + ".assetbundle", m_JobList[i].PrefabName);
            if (obj != null)
            {
                m_JobObjectDic[m_JobList[i].Id] = obj;
            }
        }
    }

    private void CloneCreateRole()
    {
        if (roleContainer == null || roleContainer.Length < 4) return;
        for (int i = 0; i < m_JobList.Count; i++)
        {
            GameObject obj = Instantiate(m_JobObjectDic[m_JobList[i].Id]);
            obj.gameObject.SetActive(true);
            //obj.transform.localScale = Vector3.one*10;

            obj.transform.parent = roleContainer[i].transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.transform.localRotation = Quaternion.Euler(Vector3.zero);

            RoleCtrl rolec = obj.GetComponent<RoleCtrl>();
            if (rolec != null)
            {
                m_JobRoleCtrl[m_JobList[i].Id] = rolec;
            }
        }

        foreach (var item in m_JobRoleCtrl.Values)
        {
            item.ToIdle();
        }
    }

    void LogonGameServer()
    {
        RoleOperation_LogOnGameServerProto to = new RoleOperation_LogOnGameServerProto();
        to.AccountId = GlobalInit.Instance.CurAccount.Id;

        NetWorkSocket.Instance.SendMsg(to.ToArray());
    }

    private void OnLogOnGameServerReturn(RoleOperation_LogOnGameServerReturnProto buffer)
    {
        MyDebug.debug(buffer.RoleList.Count);
        if (buffer.RoleList.Count == 0)
        {
            // 新建角色

        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
