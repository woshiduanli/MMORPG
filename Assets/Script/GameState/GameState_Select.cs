using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using LitJson;
using UnityEngine.SceneManagement;

// 选择角色状态
public class CGameState_Select : CGameState
{
    //public JsonData MPData = null;
    //private JsonData player_list = null;
    //private static List<CLoginRole> roles_ = new List<CLoginRole>();
    //private StoreRouter Router;
    //public static List<CLoginRole> roles
    //{
    //    get
    //    {
    //        return roles_;
    //    }
    //}

    protected override void InitData()
    {
        ////2018-8-31 机型性能适配 CJ
        //ConfigHelper.SetFrameRate((int)GameSetSystem.mDeviceLevel);
        //Time.fixedDeltaTime = 0.02f;
        //this.Router = GetSingleT<StoreComponent>().Router;
        //if (this.Args.Length > 0)
        //    this.player_list = (JsonData)this.Args[0];

        //Progress.Instance.AllDone();
        //this.Router.Response(EventTag.Login, Reducer.Create<PlayActor, long, long>(OnLogin).Cast<PlayActor, Actor>());
        //this.Router.Info("", Reducer.Create<PlayActor, JsonData>(OnLoginInfo).Cast<PlayActor, Actor>());
        //Disposabledic.Add(Dispatcher.Receive<Response>().Where(x => x.Tag.Equals(EventTag.loginduplicat)).Subscribe(x => OnLoginduplicat(x.Args)));
        ////封号提示
        //Disposabledic.Add(Dispatcher.Receive<Response>().Where(x => x.Tag.Equals(EventTag.ServerBanMsg)).Subscribe(x => OnBanMsg(x.Args[0] as JsonData)));
        ////处理login返回消息和登录角色信息
        ////创建成功进入游戏
        //Disposabledic.Add(Dispatcher.Receive<Response>().Where(x => x.Tag.Equals(EventTag.Create))
        //    .Subscribe(x =>
        //    {
        //        int result = Convert.ToInt32(x.Args[1]);
        //        if (result == 0)
        //            RequestEnterGame(x.Args[0].ToString());
        //        else if (result == 1)
        //            FireEvent(new CEvent.UI.OpenUI("CMessageboxUI", "", Localization.Get("NAME_IS_BAD"), Buttons.OK));
        //        else if (result == 2)
        //            FireEvent(new CEvent.UI.OpenUI("CMessageboxUI", "", Localization.Get("NAME_IS_REPETITION"), Buttons.OK));
        //    }));
        
        UpdateDBPlayerList();
        //QualitySettings.shadows = ShadowQuality.All;
    }

    private void OnLoginduplicat(object[] Args)
    {
        //FireEvent(new CEvent.NetWork.DisConnectToServer());
        //System.Action ok = () => { ChangeState(GameState.LOGIN); };
        //FireEvent(new CEvent.UI.OpenUI("CMessageboxUI", Localization.Get("COMMON_TITLE"), Localization.Get("SAME_ACCOUNT_LOGIN_TIPS"), Buttons.OK, ok));
    }

    protected override void RegEvents()
    {
        //this.RegEventHandler<CEvent.NetWork.NetError>(OnNetError);
        //this.RegEventHandler<CEvent.NetWork.NetDisconnect>(OnNetDisconnect);
        //this.RegEventHandler<CEvent.Scene.LevelWasLoaded>(OnLevelWasLoaded);
        ////收到backLogin事件返回登录
        //this.RegEventHandler<CEvent.GameState.ReturnLogin>((s, e) => ChangeState(GameState.LOGIN));
    }

    /// <summary>
    /// 服务器连接异常错误
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //private void OnNetError(object sender, CEvent.NetWork.NetError e)
    //{
    //    //System.Action onOK = delegate
    //    //{
    //    //    ChangeState(GameState.LOGIN);
    //    //};
    //    //FireEvent(new CEvent.UI.OpenUI("CMessageboxUI", Localization.Get("COMMON_TITLE"), Localization.Get("NETWORK_FAIL"), Buttons.OK, onOK));
    //}

    /// <summary>
    /// 服务器连接失败
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //private void OnNetDisconnect(object sender, CEvent.NetWork.NetDisconnect e)
    //{
    //    System.Action onOK = delegate
    //    {
    //        ChangeState(GameState.LOGIN);
    //    };
    //    FireEvent(new CEvent.UI.OpenUI("CMessageboxUI", Localization.Get("COMMON_TITLE"), e.Erro, Buttons.OK, onOK));
    //}

    /// <summary>
    /// 更新玩家角色列表
    /// </summary>
    private void UpdateDBPlayerList()
    {
        //roles_.Clear();
        //if (player_list != null)
        //{
        //    for (int i = 0; i < player_list.Count; i++)
        //        roles_.Add(new CLoginRole(player_list[i] as JsonData));
        //}

        // 改场景__
        if (SceneManager.GetActiveScene().name != SceneName.ROLE_SELECT_SCENE)
        {
            List<string> assets = null;
            //if (roles_.Count < 4)
            //{
                assets = new List<string>();
                assets.Add("res/role/10212.role");
                assets.Add("res/role/10201.role");
                assets.Add("res/role/10221.role");
                assets.Add("res/effect/go_fs_xj_eff01.go");
                assets.Add("res/effect/go_xz_xj_eff01.go");
                assets.Add("res/effect/go_zs_xj_eff01.go");
                assets.Add("res/effect/go_zs_xj_eff02.go");
                assets.Add("res/effect/go_zs_xj_eff03.go");
            //}
            FireEvent(new CEvent.Scene.LoadLevel(SceneName.ROLE_SELECT_SCENE, assets, true));
        }
        //else
        //    FireEvent(new CEvent.NetWork.PlayerDBList());
    }

    /// <summary>
    /// 收到服务器创角ID
    /// </summary>
    /// <param name="actor"></param>
    /// <param name="sn"></param>
    /// <param name="worldsn"></param>
    /// <returns></returns>
    //public PlayActor OnLogin(PlayActor actor, long sn, long worldsn)
    //{
    //    actor = new PlayActor();
    //    this.MPData = new JsonData();
    //    this.MPData["SN"] = actor.SN = sn;
    //    this.MPData["worldSN"] = actor.worldSN = worldsn;
    //    return actor;
    //}

    ///// <summary>
    ///// 禁止进入游戏消息
    ///// </summary>
    ///// <param name="actor"></param>
    ///// <param name="data"></param>
    ///// <returns></returns>
    //private void OnBanMsg(JsonData data)
    //{
    //    FireEvent(new CEvent.Login.BanMsg());
    //    string msg = "";
    //    long endTime = CTime.time_tick;
    //    if (data.ContainsKey("msg"))
    //    {
    //        msg = data["msg"].ToString();
    //    }
    //    if (data.ContainsKey("ban_end_time"))
    //    {
    //        endTime = Convert.ToInt64(data["ban_end_time"]);
    //    }
    //    string formatTime = CString.Format("yyyy{0}MM{1}dd{2} HH:mm:ss", Localization.Get("NIANFORBIT1"), Localization.Get("NIANFORBIT2"), Localization.Get("NIANFORBIT3"));
    //    msg = CString.Format(Localization.Get("SERVERFORBIT3"), msg, TimeHelper.GetTime(endTime.ToString()).ToString(formatTime));
    //    FireEvent(new CEvent.UI.OpenUI("CMessageboxUI", Localization.Get("COMMON_TITLE"), msg, Buttons.OK, null));
    //}

    ///// <summary>
    ///// 收到服务器完整主角属性
    ///// </summary>
    ///// <param name="actor"></param>
    ///// <param name="props"></param>
    ///// <returns></returns>
    //public PlayActor OnLoginInfo(PlayActor actor, JsonData props)
    //{
    //    IDictionaryEnumerator itemDatas = ((IDictionary)props).GetEnumerator();
    //    while (itemDatas.MoveNext())
    //    {
    //        DictionaryEntry Entry = itemDatas.Entry;
    //        string key = (Entry.Key).ToString();
    //        this.MPData[key] = props[key];
    //    }
    //    OnStartGame();
    //    return actor;
    //}
    ///// <summary>
    ///// 服务器异常信息
    ///// </summary>
    //private void OnSeverLoginMsg()
    //{

    //}

    ///// <summary>
    ///// 向服务器请求请求进入游戏
    ///// </summary>
    ///// <param name="args"></param>
    //public void RequestEnterGame(string args)
    //{
    //    Dispatcher.Request(EventTag.Login, long.Parse(args));
    //}

    ///// <summary>
    ///// 收到角色数据，点击按钮切换到GAME状态
    ///// </summary>
    //private void OnStartGame()
    //{
    //    List<string> preloads = new List<string>();
    //    preloads.Add("res/ui/font/vip1.ft");
    //    preloads.Add("res/ui/tex/backgroud.tex");
    //    preloads.Add("res/ui/tex/famehall.tex");
    //    preloads.Add("res/ui/tex/icons.tex");
    //    preloads.Add("res/ui/tex/newpublic.tex");
    //    preloads.Add("res/ui/tex/dadiban.tex");
    //    preloads.Add("res/ui/tex/newshop.tex");
    //    preloads.Add("res/ui/tex/escort.tex");
    //    preloads.Add("res/ui/tex/qq_wx_vip.tex");
    //    preloads.Add("res/ui/tex/newchat.tex");
    //    preloads.Add("res/ui/tex/royalmine.tex");
    //    preloads.Add("res/ui/tex/headicons.tex");
    //    preloads.Add("res/ui/tex/systemopen.tex");
    //    preloads.Add("res/ui/tex/goldduplicate.tex");
    //    preloads.Add("res/ui/tex/newxiaojiemian.tex");
    //    preloads.Add("res/ui/tex/newteam.tex");
    //    preloads.Add("res/ui/tex/character.tex");
    //    preloads.Add("res/ui/tex/newmainface.tex");
    //    preloads.Add("res/ui/tex/hud.tex");
    //    //preloads.Add("res/ui/uiprefab/mainface.ui");


    //    int gene = Convert.ToInt32(this.MPData["gene"]);
    //    MPRoleReference PlayerRoleRef = CReferenceManager.GGet<MPRoleReference>(Convert.ToInt32(gene));
    //    preloads.Add(string.Format("res/role/{0}.role", PlayerRoleRef.AppearanceId));
    //    ChangeState(GameState.GAME, this.MPData, preloads);
    //}

    //protected override void ChangeState(GameState state, params object[] args)
    //{
    //    if (state == GameState.LOGIN)
    //    {
    //        base.ChangeState(state, args);
    //        CreateSingleT<CGameState_Login>(args);
    //    }
    //    else if (state == GameState.GAME)
    //    {
    //        base.ChangeState(state, args);
    //        CreateSingleT<CGameState_Game>(args);
    //        GetSingleT<MSDKTool>().InitMainPlayerData(args[0] as JsonData);
    //    }
    //}

    //private void OnLevelWasLoaded(CObject sender, CEvent.Scene.LevelWasLoaded e)
    //{
    //    if (e.sceneName == SceneName.LOGIN_SCENE || e.sceneName == SceneName.ROLE_SELECT_SCENE)
    //    {
    //        if (this.World)
    //            this.World.Dispose();
    //        DestroySingleT<CMainPlayer>();

    //        if(e.sceneName == SceneName.ROLE_SELECT_SCENE)
    //        {
    //            if (roles_ != null)
    //            {
    //                for (int i = 0; i < roles_.Count; i++)
    //                    roles_[i].ReadLoginData();
    //            }
    //        }
    //    }
    //}
}