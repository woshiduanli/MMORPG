//using System;
//using System.Collections.Generic;
//using UnityEngine;
////using Eudemons.Messages;
//using UniRx;
////using Eudemons;
//using System.Collections;
//using LitJson;

//public class CRoleProperty : CObjectData
//{
//    public virtual PlayActor ActorData
//    {
//        protected set;
//        get;
//    }
//    public CRoleProperty()
//    {
//        ActorData = new PlayActor();
//    }
//    public float X
//    {
//        get
//        {
//            return ActorData.Movedata.X;
//        }
//    }
//    public float Z
//    {
//        get
//        {
//            return ActorData.Movedata.Z;
//        }
//    }
//    public float Dest_X
//    {
//        get
//        {
//            return ActorData.Movedata.Dst_X;
//        }
//    }
//    public float Dest_Z
//    {
//        get
//        {
//            return ActorData.Movedata.Dst_Z;
//        }
//    }
//    public float Dir_X
//    {
//        get
//        {
//            return ActorData.Movedata.Dir_X;
//        }
//    }
//    public float Dir_Z
//    {
//        get
//        {
//            return ActorData.Movedata.Dir_Z;
//        }
//    }
//    public int Fp
//    {
//        get
//        {
//            return GetProperty(PropertyType.FP);
//        }
//    } //怒气
//    public long Exp
//    {
//        get
//        {
//            return GetLongProperty(PropertyType.EXP);
//        }
//    }
//    public int Hp
//    {
//        get
//        {
//            return GetProperty(PropertyType.HP);
//        }
//    }
//    public int Maxhp
//    {
//        get
//        {
//            return GetProperty(PropertyType.MaxHP);
//        }
//    }
//    public int Xp
//    {
//        get
//        {
//            return GetProperty(PropertyType.XP);
//        }
//    } //XP值
//    public int moveSpeed
//    {
//        get
//        {
//            return GetProperty(PropertyType.MOVESPEED);
//        }
//    } //移动速度
//    public string Name
//    {
//        get
//        {
//            return ActorData.Name;
//        }
//    }
//    public int MaxFightvalue
//    {
//        get
//        {
//            return GetProperty(PropertyType.MAXFIGHTVALUE);
//        }
//    }
//    public int Level
//    {
//        get
//        {
//            return GetProperty(PropertyType.LEVEL);
//        }
//    }
//    public int VipLevel
//    {
//        get
//        {
//            return ActorData.VIPLevel;
//        }
//    }
//    public int Title
//    {
//        get
//        {
//            return GetProperty(PropertyType.TITLE);
//        }
//    }
//    public int Camp
//    {
//        get
//        {
//            return GetProperty(PropertyType.CAMP);
//        }
//    }
//    public int PkMode
//    {
//        get
//        {
//            return GetProperty(PropertyType.PKMODE);
//        }
//    }
//    public long SN
//    {
//        get
//        {
//            return ActorData.SN;
//        }
//    }

//    public long ZoneId
//    {
//        get
//        {
//            return ActorData.SN >> 24;
//        }
//    }

//    public int MountRefId
//    {
//        get
//        {
//            return GetProperty(PropertyType.MOUNTING);
//        }
//    }
//    public int MountAppride
//    {
//        get;
//        protected set;
//    }
//    public int XpState
//    {
//        get
//        {
//            return GetProperty(PropertyType.XPSTATE);
//        }
//    }
//    public float XpDuration
//    {
//        get
//        {
//            return GetProperty(PropertyType.XPDURATION);
//        }
//    } //xp持续时间
//    public int Appellation
//    {
//        get
//        {
//            return GetProperty(PropertyType.APPELLATION);
//        }
//    }
//    public int Prestige
//    {
//        get
//        {
//            return GetProperty(PropertyType.Prestige);
//        }
//    }
//    public int Nobility
//    {
//        get
//        {
//            return GetProperty(PropertyType.Nobility);
//        }
//    }
//    public int EudemonInBodyHp
//    {
//        get
//        {
//            return GetProperty(PropertyType.EudemonInBodyHp);
//        }
//    }
//    public int EudemonInBodyMaxHp
//    {
//        get
//        {
//            return GetProperty(PropertyType.EudemonInBodyMaxHp);
//        }
//    }
//    public int EudemonHelth
//    {
//        get
//        {
//            return GetProperty(PropertyType.EudemonHelth);
//        }
//    }
//    public int EudemonLimit
//    {
//        get
//        {
//            return GetProperty(PropertyType.EUDEMONLIMIT);
//        }
//    } //开启第三只幻兽
//    public int Rank
//    {
//        get
//        {
//            return GetProperty(PropertyType.RANK);
//        }
//    }

//    protected virtual void LevelChange()
//    { }
//    protected virtual void HpChange()
//    { }
//    protected virtual void XpStateChange()
//    { }
//    protected virtual void MountStateChange()
//    { }
//    protected virtual void RankChanged()
//    { }
//    public virtual void NameChange(string newName)
//    { }

//    private CReferenceManager Ref_mgr_;
//    public CReferenceManager Ref_mgr
//    {
//        get
//        {
//            if (!Ref_mgr_)
//                Ref_mgr_ = GetSingleT<CReferenceManager>();
//            return Ref_mgr_;
//        }
//    }

//    public T GetReference<T>(long refid) where T : PropDefine, new()
//    {
//        return Ref_mgr.Get<T>(refid);
//    }

//    public List<T> GetReferences<T>() where T : PropDefine, new()
//    {
//        return Ref_mgr.GetReferences<T>();
//    }

//    public T GetReference<T>(String key) where T : PropDefine, new()
//    {
//        return Ref_mgr.Get<T>(key);
//    }

//    private List<HeadInfoData> _headinfo = new List<HeadInfoData>();
//    public GameSetSystem _GameSet;
//    public virtual List<HeadInfoData> GetHeadInfo(RoleType objectType)
//    {
//        switch (objectType)
//        {
//            case RoleType.Eudemons:
//                return GetEudemonHeadInfo();
//            case RoleType.NPC:
//                return GetNpcHeadInfo();
//            case RoleType.Monster:
//                return GetMonsterHeadInfo();
//            case RoleType.Player:
//                return GetPlayerHeadInfo();
//            case RoleType.VirTual:
//                return GetPlayerHeadInfo();
//            default:
//                _headinfo.Clear();
//                return _headinfo;
//        }
//    }

//    public virtual List<HeadInfoData> GetNpcHeadInfo()
//    {
//        _headinfo.Clear();
//        return _headinfo;
//    }

//    public virtual List<HeadInfoData> GetMonsterHeadInfo()
//    {
//        _headinfo.Clear();
//        return _headinfo;
//    }
//    public virtual List<HeadInfoData> GetEudemonHeadInfo()
//    {
//        _headinfo.Clear();
//        return _headinfo;
//    }
//    public virtual List<HeadInfoData> GetPlayerHeadInfo()
//    {
//        _headinfo.Clear();
//        return _headinfo;
//    }

//    [Obsolete("此货币已移除")]
//    public int Gold
//    {
//        get
//        {
//            return GetProperty(PropertyType.Gold);
//        }
//    }
//    public long Coin
//    {
//        get
//        {
//            return GetLongProperty(PropertyType.COIN);
//        }
//    }
//    public int Ep
//    {
//        get
//        {
//            return GetProperty(PropertyType.Ep);
//        }
//    }
//    public int BEp
//    {
//        get
//        {
//            return GetProperty(PropertyType.BEp);
//        }
//    }
//    public int Et
//    {
//        get
//        {
//            return GetProperty(PropertyType.Et);
//        }
//    }
//    public int Honor
//    {
//        get
//        {
//            return GetProperty(PropertyType.Honor);
//        }
//    }
//    public int CurTeamId
//    {
//        get
//        {
//            return GetProperty(PropertyType.TEAMID);
//        }
//    }
//    public int GroupId
//    {
//        get
//        {
//            return GetProperty(PropertyType.GROUPID);
//        }
//    }
//    public string GroupName
//    {
//        get
//        {
//            return ActorData.GroupName;
//        }
//    }
//    public int GroupMemberType
//    {
//        get
//        {
//            return ActorData.GroupMemberType;
//        }
//    }
//    public int GroupSecondMemberType
//    {
//        get
//        {
//            return ActorData.GroupSecondMemberType;
//        }
//    }
//    public int DevilLayer
//    {
//        get
//        {
//            return GetProperty(PropertyType.DEVILLAYER);
//        }
//    }

//    public int GetProperty(string key)
//    {
//        int value = 0;
//        this.ActorData.PropertyMap.TryGetValue(key, out value);
//        return value;
//    }
//    public long GetLongProperty(string key)
//    {
//        long value = 0;
//        this.ActorData.LongPropertyMap.TryGetValue(key, out value);
//        return value;
//    }
//    public virtual void OnPropertyChange(JsonData data)
//    {
//        IDictionaryEnumerator itemDatas = ((IDictionary)data).GetEnumerator();
//        while (itemDatas.MoveNext())
//        {
//            DictionaryEntry itemData = itemDatas.Entry;
//            string key = itemData.Key.ToString();
//            if (string.IsNullOrEmpty(key) || key.Equals("id"))
//                continue;
//            JsonData value = (JsonData)itemData.Value;
//            if (value == null) continue;
//            if (value.IsObject)
//                OnSetProperty(value);
//            else if (key == PropertyType.PKMODE)
//            {
//                if (this.ActorData.PropertyMap[key] != Convert.ToInt32(itemData.Value))
//                {
//                    this.ActorData.PropertyMap[key] = Convert.ToInt32(itemData.Value);
//                    FireEvent(new CEvent.PkModeEvent(ActorData.SN));
//                }
//            }
//            else if (key == PropertyType.GROUPNAME)  // 2018-8-31 修改BUG:别人创建、退出军团报错
//                this.ActorData.GroupName = Convert.ToString(data[PropertyType.GROUPNAME]);
//            else
//            {
//                if (value.IsInt || value.IsLong)
//                {
//                    //暂时只写coin
//                    if (PropertyType.COIN == key || PropertyType.EXP == key)
//                    {
//                        this.ActorData.LongPropertyMap[key] = Convert.ToInt64(value);
//                    }
//                    else
//                    {
//                        this.ActorData.PropertyMap[key] = Convert.ToInt32(value);
//                    }
//                }
//            }

//            switch (key)
//            {
//                case PropertyType.LEVEL:
//                    LevelChange();
//                    break;
//                case PropertyType.HP:
//                    HpChange();
//                    FireEvent(new CEvent.Eudemon.EudemonUiHpChangeEvent(SN));
//                    break;
//                case PropertyType.MaxHP:
//                    FireEvent(new CEvent.MaxHpChange(SN));
//                    break;
//                case PropertyType.BODYSTATE:
//                    XpStateChange();
//                    break;
//                case PropertyType.MOUNTING:
//                    LOG.Debug(this.Name + "/mountting:" + this.ActorData.PropertyMap[PropertyType.MOUNTING]);
//                    MountStateChange();
//                    break;
//                case PropertyType.MOVESPEED:
//                    LOG.Debug(this.Name + "/moveSpeed:" + this.ActorData.PropertyMap[PropertyType.MOVESPEED]);
//                    break;
//                case PropertyType.RANK:
//                    RankChanged();
//                    break;
//                //case PropertyType.TITLE:
//                //    CWorld._FireEvent(new CEvent.TitleChange(key, this.SN));
//                //    break;
//                case PropertyType.GROUPID:
//                    this.ActorData.PropertyMap[PropertyType.GROUPID] = Convert.ToInt32(data[PropertyType.GROUPID]);
//                    FireEvent(new CEvent.Group.OnPropertyGroupChange(SN));
//                    FireEvent(new CEvent.Group.OnPropertyGroupIdChange(SN));
//                    break;
//                case PropertyType.GROUPNAME:
//                    FireEvent(new CEvent.Group.OnPropertyGroupChange(SN));
//                    break;
//                case PropertyType.TITLE:
//                    FireEvent(new CEvent.Title.TitleChange(PropertyType.TITLE, SN));
//                    break;
//                case PropertyType.APPELLATION:
//                    FireEvent(new CEvent.Appellation.AppellationChange(SN));
//                    break;
//                case PropertyType.NAME:
//                    NameChange(data["name"].ToString());
//                    FireEvent(new CEvent.PlayerInfo.PlayerNameChanged(SN));
//                    break;
//                case PropertyType.Prestige:
//                case PropertyType.Nobility:
//                    FireEvent(new CEvent.Nobility.NobilityOrPrestigeChanged(SN));
//                    break;
//                case PropertyType.GROUPMEMBERTYPE:
//                    this.ActorData.GroupMemberType = Convert.ToInt32(data[PropertyType.GROUPMEMBERTYPE]);
//                    FireEvent(new CEvent.PlayerInfo.PlayerGroupInfoChanged(SN));
//                    break;
//                case PropertyType.GROUPSECONDMEMBERTYPE:
//                    this.ActorData.GroupSecondMemberType = Convert.ToInt32(data[PropertyType.GROUPSECONDMEMBERTYPE]);
//                    FireEvent(new CEvent.PlayerInfo.PlayerGroupInfoChanged(SN));
//                    break;
//                case PropertyType.TEAMID:
//                    this.ActorData.PropertyMap[PropertyType.TEAMID] = Convert.ToInt32(data[PropertyType.TEAMID]);
//                    FireEvent(new CEvent.OnPropertyTeamIdChange(SN));
//                    break;
//                case PropertyType.EudemonInBodyHp:
//                case PropertyType.EudemonInBodyMaxHp:
//                    FireEvent(new CEvent.EudemonInBodyHpChanged(SN));
//                    break;
//                    //default:
//                    //    FireEvent(new CEvent.PropertyChange(key, this.SN));
//                    //    break;
//            }
//        }
//    }

//    public void OnSetProperty(JsonData data)
//    {
//        try
//        {
//            IDictionaryEnumerator itemDatas = ((IDictionary)data).GetEnumerator();
//            while (itemDatas.MoveNext())
//            {
//                DictionaryEntry itemData = itemDatas.Entry;
//                string key = itemData.Key.ToString();
//                JsonData value = itemData.Value as JsonData;
//                if (string.IsNullOrEmpty(key) || key.Equals("id"))
//                    continue;
//                //if (data.ContainsKey(PropertyType.MOUNTING))
//                //{
//                //    LOG.Debug("mainplyaer:" + Convert.ToInt32(data[PropertyType.MOUNTING]));
//                //}
//                //int value = itemData.Value.ToString().ToInt();
//                //暂时只写coin
//                if (value.IsObject)
//                    OnSetProperty(value);
//                else
//                {
//                    if (PropertyType.COIN == key || PropertyType.EXP == key)
//                    {
//                        ActorData.LongPropertyMap[key] = Convert.ToInt64(itemData.Value);
//                    }
//                    else
//                    {
//                        this.ActorData.PropertyMap[key] = Convert.ToInt32(itemData.Value);
//                    }
//                }
//                //LOG.Debug(CString.Format("key:{0} | value:{1}", key, value));
//            }
//        }
//        catch (Exception)
//        {
//            LOG.LogError("报错了  :" + data.ToJson());
//            throw;
//        }
//    }
//}