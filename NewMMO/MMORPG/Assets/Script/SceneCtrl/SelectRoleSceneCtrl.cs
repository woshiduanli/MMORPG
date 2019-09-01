using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class SelectRoleSceneCtrl : MonoBehaviour
{
    List<JobEntity> m_JobList;
    /// <summary>
    ///  鑱屼笟闀滃儚瀛楀吀
    /// </summary>

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

        EventDispatcher.Instance.RegProto<RoleOperation_SelectRoleInfoReturnProto>(ProtoCodeDef.RoleOperation_SelectRoleInfoReturn, OnSelectRoleInfoReture);

        EventDispatcher.Instance.RegProto<RoleOperation_DeleteRoleReturnProto>(ProtoCodeDef.RoleOperation_DeleteRoleReturn, OnRoleOperation_DelectRoleInfoReturnProto);

        // 服务器返回角色学会的技能信息
        EventDispatcher.Instance.RegProto<RoleData_SkillReturnProto>(ProtoCodeDef.RoleData_SkillReturn, OnSkillReture);




        m_UISceneSelectRoleView.OnBtnBeginGameClick = OnBtnBeginGameClick;
        m_UISceneSelectRoleView.OnBtnDeleteRoleClick = OnBtnDeleteRoleClick;
        m_UISceneSelectRoleView.OnBtnReturnClick = OnBtnReturnClick;
        m_UISceneSelectRoleView.OnBtnCreateRoleClick = OnBtnCreateRoleClick;



        LoadToObject();

        LogonGameServer();
    }

    void OnSkillReture(RoleData_SkillReturnProto proto)
    {
        GlobalInit.Instance.MainPlayerInfo.LoadSkill(proto); 
    }

        void ToCreateRoleUI()
    {
        // 删除角色模型
        if (m_currenRoleModel != null) DestroyImmediate(m_currenRoleModel);

        // 切换到新建角色
        m_IsCreateRole = true;
        SetCreateRoleModelShow(true);

        // 玩家角色数量为0

        m_UISceneSelectRoleView.SetUICreateRoleActive(true);
        m_UISceneSelectRoleView.SetUISelectRoleActive(false);

        CloneCreateRole();

        m_CurSelectJobId = 1;

        // 设置ui
        SetSelectJob();
        m_UISceneSelectRoleView.RandomName();
    }

    private void OnBtnCreateRoleClick()
    {
        ToCreateRoleUI();
    }

    List<GameObject> m_CloneCreateRoleList = new List<GameObject>();
    void ClearRole()
    {
        if (m_CloneCreateRoleList != null)
        {
            for (int i = 0; i < m_CloneCreateRoleList.Count; i++)
            {
                if (m_CloneCreateRoleList[i] != null)
                    Destroy(m_CloneCreateRoleList[i]);
            }
            m_CloneCreateRoleList.Clear();
        }
    }

    // 返回按钮点击
    private void OnBtnReturnClick()
    {
        // 新建角色
        if (m_IsCreateRole)
        {
            if (m_RoleList == null || m_RoleList.Count == 0)
            {
                NetWorkSocket.Instance.OnDisconnecte();
                SceneMgr.Instance.LoadToLogOn();
            }
            else
            {
                // 在新建角色里面， 点击返回， 如果玩家已经存在了角色， 那么就要显示玩家已经有的角色
                // 并且存在已角色

                // 摄像机
                dragTarget.eulerAngles = Vector3.up * 0;

                ClearRole();
                m_curSelectRoleId = 0;

                m_IsCreateRole = false;
                m_UISceneSelectRoleView.SetUICreateRoleActive(false);
                m_UISceneSelectRoleView.SetUISelectRoleActive(true);

                m_UISceneSelectRoleView.SetRoleList(m_RoleList, SelectRoleCallBack);
                m_UISceneSelectRoleView.SetSelected(m_RoleList[0].RoleId);

                if (CreateRoleSceneModel != null)
                    CreateRoleSceneModel[0].gameObject.SetActive(true);

                // 创建3d模型
                SetSelectRole(m_RoleList[0].RoleId);

            }
        }
        else
        {
            //ToCreateRoleUI(); 
            ////// 选择角色界面
            ////if (m_RoleList == null || m_RoleList.Count == 0)
            //{
            NetWorkSocket.Instance.OnDisconnecte();
            SceneMgr.Instance.LoadToLogOn();
            //}
            //else
            //{

            //}
        }
    }

    private void OnRoleOperation_DelectRoleInfoReturnProto(RoleOperation_DeleteRoleReturnProto protoValue)
    {
        MyDebug.debug("删除角色成功?:" + protoValue.IsSuccess);
        if (protoValue.IsSuccess)
        {
            DeleteRole(m_curSelectRoleId);
            m_UISceneSelectRoleView.CloseDeleteRoleView();
        }
        else
        {
            MessageCtrl.Instance.Show("提示", "删除失败    ");
        }
    }

    void DeleteRole(int roleId)
    {
        for (int i = m_RoleList.Count - 1; i >= 0; i--)
        {
            if (m_RoleList[i].RoleId == roleId)
            {
                m_RoleList.RemoveAt(i);
            }
        }

        if (m_RoleList.Count == 0)
        {
            ToCreateRoleUI();
        }
        else
        {
            m_UISceneSelectRoleView.SetUICreateRoleActive(false);
            m_UISceneSelectRoleView.SetUISelectRoleActive(true);


            m_UISceneSelectRoleView.SetRoleList(m_RoleList, SelectRoleCallBack);

            m_UISceneSelectRoleView.SetSelected(m_RoleList[0].RoleId);

            if (CreateRoleSceneModel != null)
                CreateRoleSceneModel[0].gameObject.SetActive(true);

            // 创建3d模型
            SetSelectRole(m_RoleList[0].RoleId);
        }

    }

    private void SetCreateRoleModelShow(bool show)
    {
        if (CreateRoleSceneModel != null)
        {
            for (int i = 0; i < CreateRoleSceneModel.Length; i++)
                CreateRoleSceneModel[i].gameObject.SetActive(show);
        }
    }

    private void OnBtnDeleteRoleClick()
    {
        RoleOperation_LogOnGameServerReturnProto.RoleItem item = GetRoleItem(m_curSelectRoleId);
        m_UISceneSelectRoleView.DeleteSelectRole(item.RoleNickName, () => { OnDeleteSelectRoleCallBack(); });
    }

    void OnDeleteSelectRoleCallBack()
    {
        RoleOperation_DeleteRoleProto pro = new RoleOperation_DeleteRoleProto();
        pro.RoleId = m_curSelectRoleId;
        NetWorkSocket.Instance.SendMsg(pro.ToArray());
    }

    private void OnSelectRoleInfoReture(RoleOperation_SelectRoleInfoReturnProto protoValue)
    {
        MyDebug.debug(protoValue.RoleNickName);
        if (protoValue.IsSuccess)
        {
            GlobalInit.Instance.MainPlayerInfo = new RoleInfoMainPlayer(protoValue);
            MyDebug.debug("玩家当前的地图"+ GlobalInit.Instance.MainPlayerInfo.LastInWorldMapId);
            // 进入主城
            SceneMgr.Instance.LoadToWorldMap(GlobalInit.Instance.MainPlayerInfo.LastInWorldMapId);
        }
        else
        {

        }
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

    GameObject m_currenRoleModel;
    int m_curSelectRoleId;

    void SetSelectRole(int RoleId)
    {
        if (m_curSelectRoleId == RoleId) return;
        this.m_curSelectRoleId = RoleId;
        if (m_currenRoleModel != null) DestroyImmediate(m_currenRoleModel);
        RoleOperation_LogOnGameServerReturnProto.RoleItem item = GetRoleItem(RoleId);


        m_currenRoleModel = Instantiate(GlobalInit.Instance.JobObjectDic[item.RoleJob]);
        m_currenRoleModel.gameObject.SetActive(true);
        m_currenRoleModel.transform.parent = roleContainer[0].transform;
        m_currenRoleModel.transform.localPosition = Vector3.zero;
        m_currenRoleModel.transform.localScale = Vector3.one;
        m_currenRoleModel.transform.localRotation = Quaternion.Euler(Vector3.zero);

        RoleCtrl rolec = m_currenRoleModel.GetComponent<RoleCtrl>();
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
                    items[i].gameObject.SetActive(true);

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
            GameObject obj = AssetBundleMgr.Instance.Load("Role/" + m_JobList[i].PrefabName.ToLower() + ".assetbundle", m_JobList[i].PrefabName);
            if (obj != null)
            {
                GlobalInit.Instance.JobObjectDic[m_JobList[i].Id] = obj;

            }
        }
    }

    private void CloneCreateRole()
    {
        if (roleContainer == null || roleContainer.Length < 4) return;
        ClearRole();
        for (int i = 0; i < m_JobList.Count; i++)
        {
            GameObject obj = Instantiate(GlobalInit.Instance.JobObjectDic[m_JobList[i].Id]);
            obj.gameObject.SetActive(true);
            //obj.transform.localScale = Vector3.one*10;
            m_CloneCreateRoleList.Add(obj);
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

        MyDebug.debug("fsdff ffffffffffffffffffffffffffff"+buffer.RoleList.Count);

        m_UISceneSelectRoleView.SetUICreateRoleActive(buffer.RoleCount == 0);
        m_UISceneSelectRoleView.SetUISelectRoleActive((buffer.RoleCount != 0));

        if (buffer.RoleCount == 0)
        {
            m_IsCreateRole = true;
            // 玩家角色数量为0
            CloneCreateRole();

            m_CurSelectJobId = 1;

            // 设置ui
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
                // 设置ui信息
                m_RoleList = buffer.RoleList;
                m_UISceneSelectRoleView.SetRoleList(buffer.RoleList, SelectRoleCallBack);
            }
            m_UISceneSelectRoleView.SetSelected(buffer.RoleList[0].RoleId);

            if (CreateRoleSceneModel != null)
                CreateRoleSceneModel[0].gameObject.SetActive(true);

            // 创建3d模型
            SetSelectRole(buffer.RoleList[0].RoleId);
        }
    }

    private void SelectRoleCallBack(int roleId)
    {
        SetSelectRole(roleId);
        m_UISceneSelectRoleView.SetSelected(roleId);
        MyDebug.debug("哈哈:" + roleId);
    }



    // Update is called once per frame
    void Update()
    {

    }

    private void OnDestroy()
    {
        EventDispatcher.Instance.RemoveProto(ProtoCodeDef.RoleOperation_LogOnGameServerReturn);

        EventDispatcher.Instance.RemoveProto(ProtoCodeDef.RoleOperation_CreateRoleReturn);

        EventDispatcher.Instance.RemoveProto(ProtoCodeDef.RoleOperation_EnterGameReturn);

        EventDispatcher.Instance.RemoveProto(ProtoCodeDef.RoleOperation_SelectRoleInfoReturn);
    }
}
