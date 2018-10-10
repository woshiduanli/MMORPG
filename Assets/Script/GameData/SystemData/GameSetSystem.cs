using System.Collections.Generic;
using LitJson;
using UnityEngine;


public enum SetType
{
    Other,
    Quality,
    Role,
    Music,
    Audio,
    Volume,
    World,
    System,
    Group,
    Team,
    Friends,
    TeXiao,
    TongZhi,
    Eduo,
    PlayerName,
    GroupName,
    Title,
}

public class GameSet
{
    public int Num = GameSetSystem.MaxRole;//同屏人数
    public int Quality = (int)GameSetSystem.Quality.HD;//画质
    public bool Music = true;//音乐
    public bool Audio = true;//音效
    public bool ClosePlayerName = false;
    public bool CloseTerritoryName = false;
    public bool CloseEduName = false;
    public bool CloseTitle = false;
    public float Volume = 1;//音量
    public bool TeXiao = true;//爵位特效展示
    public bool TongZhi = true;//爵位跑马灯通知
    public List<SetItem> Item = new List<SetItem>();
    //聊天频道
    public bool World = true;
    public bool System = true;
    public bool Group = true;
    public bool Team = true;
    public bool Friends = true;
    public bool WorldVoice = false;
    public bool GroupVoice = false;
    public bool TeamVoice = false;
    public bool FriendsVoice = false;
    public bool VoiceText = false;
}

public class SetItem
{
    public int SN;//角色SN
    public List<PushItem> Activity = new List<PushItem>();//活动推送
    public bool Joystick = true;//移动遥感 - 跟随
    public int AutoAtk = (int)GameSetSystem.AutoType.Screen;
    public int Target = (int)GameSetSystem.Attack.Unlimited;//目标 - 不限
    public int EudCombine = (int)GameSetSystem.EudCombineType.Normal;//合体类型-普通类型
    public bool AutoPick = true;//自动拾取
    public bool WhiteArm = true;//白色品质
    public bool GreenArm = true;//绿色以上品质
    public bool Diamond = true;//宝石
    public bool PetEgg = true;//幻兽蛋
    public bool Coin = true;//金币
    public bool OtherItem = true;//其他
    public bool AutoSell = false;//自动出售
    public List<SkillItem> Skill = new List<SkillItem>();//技能
}

public class SkillItem
{
    public int SkillIndex = Def.INVALID_ID;
    public bool IsOn;
}

public class PushItem
{
    public int ActId = Def.INVALID_ID;
    public bool Push;
}

public class GameSetSystem : CObjectData
{
    public static DeviceLevel mDeviceLevel;
    public const int MaxRole = 30;
    private SetItem Current;
    public GameSet SetData { private set; get; }
    public int ActiveRoleCount { get { return SetData.Num; } }
    public int quality { get { return SetData.Quality; } }//画质
    public bool Music { get { return SetData.Music; } }//音乐
    public bool Audio { get { return SetData.Audio; } }//音效
    public float Volume { get { return SetData.Volume; } }//音量 
    public bool Texiao { get { return SetData.TeXiao; } }//爵位特效展示
    public bool TongZhi { get { return SetData.TongZhi; } }//爵位跑马灯通知
    private string _serverName = string.Empty;//服务器名字
    public enum Attack
    {
        Player = 0,
        Unlimited,
        Other,
    }

    public enum AutoType
    {
        Screen,//当前屏
        Point,//定点
    }

    public enum Quality
    {
        Rapidly,
        HD,
        Perfect
    }

    public enum EudCombineType
    {
        Beastdefend,
        Normal
    }

    //public override void Initialize()
    //{
    //    base.Initialize();    
    //}

    protected override void InitData()
    {
        //JsonRead();
        //Router = GetSingleT<StoreComponent>().Router;
        //Router.Info(EventTag.Settings, Reducer.Create<PlayActor, JsonData>(OnSettings).Cast<PlayActor, Actor>());
    }

    protected override void RegEvents()
    {
        //RegEventHandler<CEvent.Set.SaveSetEvent>((a, b) =>
        //{
        //    MusicControll.Volume = this.Music ? this.Volume : 0;
        //    SoundControll.Volume = this.Audio ? this.Volume : 0;
        //    JsonIO.JsonWrite("Setting.txt", SetData);
        //    Dispatcher.Request(EventTag.SaveSetting, JsonMapper.ToJson(Current));
        //});

        //RegEventHandler<CEvent.Set.SetFastEvent>((a, b) =>
        //{
        //    SetData.Quality = (int)Quality.Rapidly;
        //    FireEvent(new CEvent.Set.SaveSetEvent(Current, SetType.Quality));
        //});

    }

    public bool CanMoveJoystick()
    {
        return GetCurrent().Joystick;
    }

    //public bool IsTarget(CClientRole role)
    //{
    //    if (!this.Mp)
    //        return false;
    //    if (role.IsNpc)
    //        return false;
    //    SetItem setItem = GetCurrent();
    //    if (setItem.Target == (int)Attack.Unlimited)
    //        return true;
    //    else if (role.IsPlayer && setItem.Target == (int)Attack.Player)
    //    {
    //        if (role.SN == this.Mp.SN)
    //            return false;
    //        else
    //            return this.Mp.IsEnemyCamp(role);
    //    }
    //    else if (role.IsMonster && this.Mp.IsEnemyCamp(role) && setItem.Target == (int)Attack.Other)
    //        return true;
    //    else
    //        return false;
    //}

    public SetItem GetCurrent()
    {
        //if (!this.Mp)
        //    return null;

        //if (Current != null)
        //    return Current;

        //for (int i = 0; i < SetData.Item.Count; i++)
        //{
        //    if (SetData.Item[i].SN == this.Mp.SN)
        //    {
        //        Current = SetData.Item[i];
        //        break;
        //    }
        //}

        //if (Current == null)
        //{
        //    Current = CreateItem();
        //    SetData.Item.Add(Current);
        //}

        return Current;
    }

    private SetItem CreateItem()
    {
        return null; 
        //if (!this.Mp)
        //    return null;

        //SetItem item = new SetItem();
        //item.SN = (int)this.Mp.SN;
        //item.Joystick = true;
        //item.Target = (int)GameSetSystem.Attack.Unlimited;

        //var configs = GetSingleT<CReferenceManager>().GetReferences<ActivityExhibitionReference>();
        //foreach (var v in configs)
        //{
        //    if (v.TimeProp == null)
        //        continue;
        //    PushItem pi = new PushItem();
        //    pi.ActId = v.Id;
        //    pi.Push = true;
        //    item.Activity.Add(pi);
        //}

        //item.Skill.Add(CreateSkillItem(Def.SKILL_XP_ATTACK));
        //for (int i = 1; i <= 4; ++i)
        //    item.Skill.Add(CreateSkillItem(i));

        //return item;
    }

    private SkillItem CreateSkillItem(int i)
    {
        SkillItem si = new SkillItem();
        si.SkillIndex = i;
        si.IsOn = true;
        return si;
    }

    private void JsonRead()
    {
        mDeviceLevel = DeviceLevel.LEVEL_3;
        if (Application.isMobilePlatform)
        {
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                mDeviceLevel = DeviceLevel.LEVEL_2;
            }
            else
            {
                mDeviceLevel = MobileHelper.GetDeviceLevel();
            }
        }
        else
        {
            mDeviceLevel = DeviceLevel.LEVEL_1;
        }

        object lo = JsonIO.JsonRead<GameSet>("Setting.txt");
        if (lo != null)
            SetData = lo as GameSet;
        if (SetData == null)
        {
            SetData = new GameSet();
            if (mDeviceLevel == DeviceLevel.LEVEL_1 || mDeviceLevel == DeviceLevel.LEVEL_2)
            {
                SetData.Num = 20;
                SetData.Quality = (int)GameSetSystem.Quality.HD;
            }
            else
            {
                SetData.Num = 10;
                SetData.Quality = (int)GameSetSystem.Quality.Rapidly;
            }
        }

        MusicControll.Volume = this.Music ? this.Volume : 0;
        SoundControll.Volume = this.Audio ? this.Volume : 0;
    }

    //private PlayActor OnSettings(PlayActor actor, JsonData data)
    //{
    //    if (data != null && data.ContainsKey("settings"))
    //    {
    //        object lo = JsonMapper.ToObject<SetItem>(data["settings"].ToString());
    //        if (lo != null)
    //            Current = lo as SetItem;
    //    }
    //    return actor;
    //}

    /// <summary>
    /// 设置服务器名字
    /// </summary>
    /// <param name="name"></param>
    public void SetServerName(string name)
    {
        _serverName = name;
    }

    /// <summary>
    /// 获取服务器名字
    /// </summary>
    /// <returns></returns>
    public string GetServerName()
    {
        //_serverName = GameLoginInfo.ServerName;
        //if (string.IsNullOrEmpty(_serverName))
        //    _serverName = Localization.Get("SETTING_DEFAULT");
        //return _serverName;
        return ""; 
    }
}


