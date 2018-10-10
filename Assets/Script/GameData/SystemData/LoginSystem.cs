using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZCore; 
/// <summary>
/// 包含客户端所有的账号 密码 服务器 最近联系人
/// </summary>
public class LoginList
{
    public int lastIndex_;//上次登录的下标
    public List<LoginItem> Items = new List<LoginItem>();
}

/// <summary>
/// 上一次登录的账号 密码和 服务器
/// </summary>
public class LoginItem
{
    public string Account = string.Empty;//账号
    public string Password = string.Empty;//密码
    public string ZoneName = string.Empty; // 区名
    public string SeverName = string.Empty;// 服务
}

/// <summary>
/// // 登录记录
/// </summary>
public class LoginSystem : CObject
{
    public LoginList Loginlist_ { private set; get; }
    public LoginItem Currentloginitem { private set; get; }
    //public CServerItem CurrentSever { private set; get; }
    //private CServerList serverlist;

    public override void Initialize()
    {
        //this.serverlist = GetSingleT<CServerList>();
        //RegEventHandler<CEvent.Login.LoadRecord>(OnLoadRecord);
        //RegEventHandler<CEvent.Login.SaveLoginDate>(OnSaveLoginDate);
        //RegEventHandler<CEvent.Login.SetLoginItem>((a, b) => { this.Currentloginitem = b.loginItem; });
        //RegEventHandler<CEvent.Login.SetSever>((a, b) => { this.CurrentSever = b.serverItem; });
    }

    /// <summary>
    /// 读取本地记录 (账号 密码 上次登录服务器)
    /// </summary>
    //    private void OnLoadRecord(CObject sender, CEvent.Login.LoadRecord e)
    //    {
    //        if (serverlist == null)
    //            return;
    //        object lo = JsonIO.JsonRead<LoginList>("login.txt");
    //        if (lo != null)
    //        {
    //            Loginlist_ = lo as LoginList;
    //            if (Loginlist_.Items.Count > 0)
    //            {
    //                if (Loginlist_.lastIndex_ >= Loginlist_.Items.Count)
    //                    Loginlist_.lastIndex_ = 0;
    //                LoginItem loginitem = Loginlist_.Items[Loginlist_.lastIndex_];
    //                if (loginitem != null)
    //                {
    //                    Currentloginitem = loginitem;
    //                    CurrentSever = serverlist.Get(loginitem.ZoneName, loginitem.SeverName);
    //                }
    //            }
    //        }
    //        if (CurrentSever == null)
    //            CurrentSever = serverlist.GetNew();
    //    }



    //    private void OnSaveLoginDate(CObject sender, CEvent.Login.SaveLoginDate e)
    //    {
    //        LoginItem item = null;
    //        if (Loginlist_ == null)
    //            Loginlist_ = new LoginList();
    //        for (int i = 0; i < Loginlist_.Items.Count; i++)
    //        {
    //            if (e.Account == Loginlist_.Items[i].Account)
    //            {
    //                item = Loginlist_.Items[i];
    //                Loginlist_.lastIndex_ = i;
    //            }
    //        }
    //        LOG.Debug("***** SaveLoginData  {0} {1}", e.Account, Loginlist_.lastIndex_);
    //        if (item == null)
    //        {
    //            item = new LoginItem();
    //            Loginlist_.Items.Add(item);
    //            Loginlist_.lastIndex_ = Loginlist_.Items.Count - 1;
    //        }
    //        item.Account = e.Account;
    //        if (CurrentSever != null)
    //        {
    //            item.ZoneName = CurrentSever.Part;
    //            item.SeverName = CurrentSever.Name;
    //        }
    //        JsonIO.JsonWrite("login.txt", Loginlist_);
    //        FireEvent(new CEvent.Login.LoadRecord());//更新数据
    //    }
    //}
}
