using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectRoleSceneCtrl : MonoBehaviour
{
    List<JobEntity> m_JobList;
    /// <summary>
    ///  鑱屼笟闀滃儚瀛楀吀
    /// </summary>
    private Dictionary<int, GameObject> m_JobObjectDic = new Dictionary<int, GameObject>();
    private Dictionary<int, RoleCtrl> m_JobRoleCtrl = new Dictionary<int, RoleCtrl>();

    UISceneSelectRoleView m_UISceneSelectRoleView;

    public Transform[] CreateRoleSceneModel;

    public Transform[] roleContainer;
    private void Awake()
    {
        m_UISceneSelectRoleView = UISceneCtrl.Instance.LoadSceneUI(UISceneCtrl.SceneUIType.SelectRole).GetComponent<UISceneSelectRoleView>();



    }
    // Use this for initialization
    void Start()
    {
        if (DelegateDefine.Instance.OnSceneLoadOk != null)
        {
            DelegateDefine.Instance.OnSceneLoadOk();
        }

        if (m_UISceneSelectRoleView != null)
        {
            m_UISceneSelectRoleView.SelectRoleDragView.OnSelectRoleDrag = OnSelectRoleDrag;
            if (m_UISceneSelectRoleView.JobItems != null && m_UISceneSelectRoleView.JobItems.Length > 0)
            {

                for (int i = 0; i < m_UISceneSelectRoleView.JobItems.Length; i++)
                {
                    m_UISceneSelectRoleView.JobItems[i].OnSelectJob = OnSelectJobCallBack;
                }
            }

        }

        // 登录服务器返回
        EventDispatcher.Instance.RegProto<RoleOperation_LogOnGameServerReturnProto>(ProtoCodeDef.RoleOperation_LogOnGameServerReturn, OnLogOnGameServerReturn);
        // 创建角色返回
        EventDispatcher.Instance.RegProto<RoleOperation_CreateRoleReturnProto>(ProtoCodeDef.RoleOperation_CreateRoleReturn, OnCrateRoleReturn);
        // 进入游戏返回
        EventDispatcher.Instance.RegProto<RoleOperation_EnterGameReturnProto>(ProtoCodeDef.RoleOperation_EnterGameReturn, OnEnterGameReturn);

        EventDispatcher.Instance.RegProto<RoleOperation_SelectRoleInfoReturnProto>(ProtoCodeDef.RoleOperation_SelectRoleInfoReturn, OnRoleOperation_SelectRoleInfoReturnProto);


        m_UISceneSelectRoleView.OnBtnBeginGameClick = OnBtnBeginGameClick;

        LoadToObject();

        LogonGameServer();
    }

    private void OnRoleOperation_SelectRoleInfoReturnProto(RoleOperation_SelectRoleInfoReturnProto protoValue)
    {
        MyDebug.debug(protoValue.RoleNickName);
    }

    private void OnEnterGameReturn(RoleOperation_EnterGameReturnProto buffer)
    {
        MyDebug.debug(buffer.MsgCode);
    }

    public int m_CurSelectJobId;

    /// <summary>
    ///  ui涓婄偣鍑?
    /// </summary>
    /// <param name="jobId"></param>
    /// <param name="rotateAngle"></param>
    private void OnSelectJobCallBack(int jobId, int rotateAngle)
    {
        MyDebug.debug("" + jobId + "   " + rotateAngle);
        if (m_isRotate) return;
        m_CurSelectJobId = jobId;
        m_isRotate = true;
        m_target = rotateAngle;
        SetSelectJob();
    }

    public Transform dragTarget;
    private float m_rorate = 90;
    private float m_target = 90;
    bool m_isRotate = false;
    float m_rotaeSpeed = 200;

    private void OnSelectRoleDrag(int obj)
    {
        if (m_isRotate) return;
        m_rorate = Math.Abs(m_rorate) * (obj);
        m_isRotate = true;
        m_target = dragTarget.eulerAngles.y + m_rorate;
        MyDebug.debug(obj);

        if (obj == 1)
        {
            --m_CurSelectJobId;
            if (m_CurSelectJobId == 0)
            {
                m_CurSelectJobId = 4;

            }
        }
        else
        {

            ++m_CurSelectJobId;
            if (m_CurSelectJobId == 5)
            {
                m_CurSelectJobId = 1;
            }
        }
        int d = m_CurSelectJobId - 1;
        m_target = m_UISceneSelectRoleView.JobItems[d].m_RotateAngle;


        SetSelectJob();
    }

    List<RoleOperation_LogOnGameServerReturnProto.RoleItem> m_RoleList;

    RoleOperation_LogOnGameServerReturnProto.RoleItem GetRoleItem(int RoleId)
    {
        if (m_RoleList == null) return default(RoleOperation_LogOnGameServerReturnProto.RoleItem);

        for (int i = 0; i < m_RoleList.Count; i++)
        {
            if (RoleId == m_RoleList[i].RoleId)
            {
                return m_RoleList[i];
            }
        }
        return default(RoleOperation_LogOnGameServerReturnProto.RoleItem);
    }

    GameObject obj;
    int m_curSelectRoleId;

    void SetSelectRole(int RoleId)
    {
        if (m_curSelectRoleId == RoleId) return;
        this.m_curSelectRoleId = RoleId;
        if (obj != null) DestroyImmediate(obj);
        RoleOperation_LogOnGameServerReturnProto.RoleItem item = GetRoleItem(RoleId);


        obj = Instantiate(m_JobObjectDic[item.RoleJob]);
        obj.gameObject.SetActive(true);
        obj.transform.parent = roleContainer[0].transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.transform.localRotation = Quaternion.Euler(Vector3.zero);

        RoleCtrl rolec = obj.GetComponent<RoleCtrl>();
        //if (rolec != null)
        //{
        //    m_JobRoleCtrl[m_JobList[i].Id] = rolec;
        //}

    }

    void SetSelectJob()
    {
        if (m_JobList != null)
        {
            for (int i = 0; i < m_JobList.Count; i++)
            {
                if (m_JobList[i].Id == m_CurSelectJobId)
                {
                    m_UISceneSelectRoleView.SelectRoleJobDescView.SetUI(m_JobList[i].Name, m_JobList[i].Desc);

                    break;
                }
            }

            UISelectRoleJobItemView[] items = m_UISceneSelectRoleView.JobItems;
            int curValue = m_CurSelectJobId - 1;
            if (items != null)
            {
                for (int i = 0; i < items.Length; i++)
                {
                    if (curValue == i)
                    {
                        items[i].gameObject.transform.localScale = Vector3.one * 1.1f;
                    }
                    else
                    {
                        items[i].gameObject.transform.localScale = Vector3.one;
                    }
                }
            }
        }
    }

    bool m_IsCreateRole = false;
    private void OnBtnBeginGameClick()
    {
        MyDebug.debug("点击进入主城");
        if (m_IsCreateRole)
        {
            RoleOperation_CreateRoleProto roleOperation_CreateRoleProto = new RoleOperation_CreateRoleProto();
            roleOperation_CreateRoleProto.JobId = (byte)m_CurSelectJobId;
            if (string.IsNullOrEmpty(m_UISceneSelectRoleView.txtNickName.text))
            {
                MessageCtrl.Instance.Show("提示", "请输入昵称");
                return;
            }
            roleOperation_CreateRoleProto.RoleNickName = m_UISceneSelectRoleView.txtNickName.text;
            NetWorkSocket.Instance.SendMsg(roleOperation_CreateRoleProto.ToArray());
        }
        else
        {
            RoleOperation_EnterGameProto roleOperation_EnterGameProto = new RoleOperation_EnterGameProto();
            roleOperation_EnterGameProto.RoleId = m_curSelectRoleId;
            NetWorkSocket.Instance.SendMsg(roleOperation_EnterGameProto.ToArray());
            // 这是有一个渠道号
            //roleOperation_EnterGameProto.RoleId = m_curSelectRoleId;
        }
    }

    private void OnCrateRoleReturn(RoleOperation_CreateRoleReturnProto buffer)
    {
        MyDebug.debug("是否有错误码" + buffer.MsgCode);
        if (buffer.MsgCode == 1000)
        {
            MessageCtrl.Instance.Show("提示", "创建失败");
            return;
        }
        else
        {

        }
    }

    private void FixedUpdate()
    {
        if (m_isRotate)
        {
            float toAngle = Mathf.MoveTowardsAngle(dragTarget.eulerAngles.y, m_target, Time.deltaTime * m_rotaeSpeed);
            dragTarget.eulerAngles = Vector3.up * toAngle;
            if (m_target == Mathf.RoundToInt(toAngle))
            {
                MyDebug.debug("ddd:" + m_CurSelectJobId);
                m_isRotate = false;
                dragTarget.eulerAngles = Vector3.up * toAngle;

            }
        }
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

        m_UISceneSelectRoleView.SetUICreateRoleActive(buffer.RoleCount == 0);
        m_UISceneSelectRoleView.SetUISelectRoleActive((buffer.RoleCount != 0));

        if (buffer.RoleCount == 0)
        {
            m_IsCreateRole = true;
            // 玩家角色数量为0
            CloneCreateRole();

            m_CurSelectJobId = 1;

            SetSelectJob();

            m_UISceneSelectRoleView.RandomName();

            if (CreateRoleSceneModel != null)
            {
                for (int i = 0; i < CreateRoleSceneModel.Length; i++)
                    CreateRoleSceneModel[i].gameObject.SetActive(true);
            }
        }
        else
        {
            m_IsCreateRole = false;
            if (buffer.RoleList != null)
            {
                m_RoleList = buffer.RoleList;
                m_UISceneSelectRoleView.SetRoleList(buffer.RoleList, SelectRoleCallBack);
            }

            if (CreateRoleSceneModel != null)
                CreateRoleSceneModel[0].gameObject.SetActive(true);

            SetSelectRole(buffer.RoleList[0].RoleId);
        }
    }

    private void SelectRoleCallBack(int roleId)
    {
        SetSelectRole(roleId);
        MyDebug.debug("sff:" + roleId);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
