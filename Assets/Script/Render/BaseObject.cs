using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Collections;
using UnityEngine.Rendering;
//using Pool;

public class CBaseObject : ZRender.IRenderObject, IDisposable 
{
    #region IDisposable 接口
    public bool disposed { protected set; get; }
    public virtual void Dispose()
    {
        if (disposed)
            return;
        disposed = true;
        //CClientCommon.DestroyImmediate(this);
    }

    protected override void OnDestroy()
    {
        DestroyComponent();

//         for (equips.Begin(); equips.Next();)
//             CClientCommon.DestroyImmediate(equips.Value);
//         equips.Clear();
// 
//         for (int i = 0; i < loadEquips.Count; i++)
//             CClientCommon.DestroyImmediate(loadEquips[i]);
//         loadEquips.Clear();

        if (DeadMat)
            CClientCommon.DestroyImmediate(DeadMat);

        if (WoundMat)
            CClientCommon.DestroyImmediate(WoundMat);

        if (this.BoneMap != null)
        {
            this.BoneMap.Clear();
            this.BoneMap = null;
        }

//         if (this.PoolList != null)
//         {
//             Transform temp = CategorySettings.GetObject("_effect/");
//             foreach (var pool in PoolList)
//             {
//                 if (pool != null)
//                     pool.SetParentTransform(temp);
//             }
//             PoolList.Clear();
//             PoolList = null;
//         }
// 
//         if (cSkeleton)
//         {
//             if (!CanPool || IsUI_obj)
//                 CClientCommon.DestroyImmediate(cSkeleton);
//             else
//                 cSkeleton.Cache(this.Apprid.ToString());
// 
//             cSkeleton = null;
//         }

        if (this.gameObject)
            CClientCommon.DestroyImmediate(gameObject);

        if(HeadRoot)
            CClientCommon.DestroyImmediate(HeadRoot);
    }

    public static implicit operator bool(CBaseObject exists)
    {
        return exists != null && !exists.disposed && exists.gameObject;
    }

    #endregion

    //>--------------------------------------------------------------------
    #region 一系列虚接口，供派生类重载
    protected virtual void OnInitData() { }
    public virtual void OnSelectBound() { }
    protected virtual void InitializeComponent() { }
    protected virtual void DestroyComponent() { }
    //骨骼加载完成回调
    protected virtual void OnBoneLoadDone() { }

    //protected virtual void OnModelLoadDone(CModelObject eo, CEquipObject e)
    //{
    //    e.SetEffectLayer(IsUI_obj ? CDefines.Layer.UIPlayer : CDefines.Layer.Probe);
    //}

    protected virtual void OnAllModelsLoadDone(){}
    #endregion
    //>--------------------------------------------------------------------

    #region 动画相关

    public bool PlayJumpEnd
    {
        get
        {
            if (Animator)
                return Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == StateToHash(Model.MotionState.jump3);
            return false;
        }
    }

    public Animator Animator
    {
        get
        {
            //if (cSkeleton == null)
            //    return null;
            //return cSkeleton.Animator;
            return null; 
        }
    }
    public Model.MotionState StandState = MotionState.stand;
    public void CreateAniClip(string clip)
    {
        //if (cSkeleton == null)
        //{
        //    Clips.Add(clip);
        //    return;
        //}

        //cSkeleton.CreateAniClip(clip);
        
    }

    public bool HasAniClip(Model.MotionState state)
    {
        return false; 
        //if (cSkeleton == null)
        //    return false;
        //return cSkeleton.HasAniClip(state);
    }

    public AnimationClip GetAniClip(MotionState state)
    {
        //if (cSkeleton == null)
        //    return null;
        //return cSkeleton.GetAniClip(state);
        return null; 
    }

    public int StateToHash(Model.MotionState state)
    {
        return 0; 
        //return AnimatorHelp.State2Hash(state);
    }

    protected bool CheckState(Model.MotionState mstate)
    {
        if (!Animator)
            return false;
        AnimatorStateInfo state = Animator.GetCurrentAnimatorStateInfo(0);
        if (state.shortNameHash == StateToHash(mstate))
            return true;

        AnimatorStateInfo next = Animator.GetNextAnimatorStateInfo(0);
        if (next.shortNameHash == StateToHash(mstate))
            return true;

        return false;
    }

    public virtual Model.MotionState GetStand()
    {
        if (IsUI_obj)
        {
            if (HasAniClip(Model.MotionState.uiidle))
                return Model.MotionState.uiidle;
            return Model.MotionState.stand;
        }
        return Model.MotionState.idle;
    }

    public virtual MotionState GetMove()
    {
        return Model.MotionState.run;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="state"></param>
    /// <param name="check"></param 检查当前动作和需要播放的动作是否一样>
    /// <param name="speed"></param>
    public void SetTrigger(Model.MotionState state, float speed = 1)
    {
        if (CheckState(state))
            return;
        CrossFade(state, speed);
    }

    protected virtual void CrossFade(Model.MotionState state, float speed = 1)
    {
        if (!HasAniClip(state))
            return;
        if (!Animator.enabled)
            return;
        Animator.speed = speed;
        if (StandState != state)
            Animator.ResetTrigger(StateToHash(StandState));
        Animator.SetTrigger(StateToHash(state));
        AnimateEquips(state);
    }

    protected void AnimateEquips(Model.MotionState state)
    {
        //for (equips.Begin(); equips.Next();)
        //    equips.Value.CrossFade(AnimatorHelp.State2String(state));
    }
    #endregion
    //>--------------------------------------------------------------------
    public bool isAnimal { get { return (Size.z / Size.y) > 1.3f; } }
    public bool IsUI_obj { get; protected set; }
    public bool IsUsingByStory;
    //public bool IsInDialogue;//是否在剧情对话中

    protected bool CanPool = true;
    public bool IsLoadOver;
    public string Name { get; protected set; }
    public string Tag { get; protected set; }
    public int Apprid { get; protected set; }

    //public CSelectableBound BoundObject { get; protected set; }
    public SkinnedMeshRenderer Skin { get; private set; }
    //public CSkeletonObject cSkeleton;
    public Role RoleConfig
    {
        get
        {
            return null; 
        }
        //         get
        //         {
        //             if (cSkeleton == null)
        //                 return null;
        //             return cSkeleton.RoleConfig;
        //         }
    }

    private Map<string, Transform> BoneMap = new Map<string, Transform>();
    protected Material CacheMat = null;
    private Material DeadMat = null;
    protected Material WoundMat = null;
    private List<string> Clips = new List<string>();
    private Transform transform;
    //private List<CPoolObject> PoolList = new List<CPoolObject>();
    public Transform Transform 
    {
        get 
        {
            if (transform != null)
                return transform;
            if (this.gameObject)
                transform = this.gameObject.transform;
            return transform;
        }
    }

    public GameObject RoleGameObject { get; private set; }
    /// <summary>
    /// 挂载点
    /// </summary>
    private Transform mounttranform;
    public Transform MountTransform
    {
        get {
            if (mounttranform)
                return mounttranform;
            if (RoleGameObject)
                mounttranform = RoleGameObject.transform;
            return mounttranform;
        }
    }
    public GameObject HeadRoot { private set; get; }
    /// <summary>
    /// 根骨骼
    /// </summary>
    public Transform RootBone { get; protected set; }

    //>--------------------------------------------------------------------
    //碰撞
    public float Radius { get { return Mathf.Max(Size.x, Size.z) * 0.5f; } }
    //碰撞盒
    public virtual Vector3 Size
    {
        get
        {
            //if (cSkeleton != null)
            //    return cSkeleton.Size;
            return Vector3.zero;
        }
    }

    public virtual Vector3 CenterPos
    {
        get
        {
            Transform ts = this.Transform;
            return new Vector3(ts.position.x, ts.position.y + Size.y / 2, ts.position.z);
        }
    }

    //public virtual bool isCameraVisible
    //{
    //    get
    //    {
    //        if (IsUsingByStory)
    //            return true;
    //        if (BoundObject)
    //            return BoundObject.isCameraVisible;
    //        return false;
    //    }
    //}

    protected bool isWorldVisible_ = true;
    public virtual bool isWorldVisible
    {
        get
        {
            return isWorldVisible_;
        }
        set
        {
            if (isWorldVisible_ != value && value)
                OnWorldVisible();
            isWorldVisible_ = value;
            if (isWorldVisible_ && DoUpdateEquip)
            {
                DoUpdateEquip = false;
                UpdateEquip();
            }
        }
    }
    public bool isSkinVisible = true;
    public PLevel pLevel { protected set; get; }
    protected virtual void OnWorldVisible() { }

    protected override void OnVisible()
    {
        //bool visible = this.isWorldVisible && this.isSkinVisible && this.isCameraVisible;
        //if (IsInDialogue)
        bool visible = IsVisible();

        //for (equips.Begin(); equips.Next();)
        //    equips.Value.SetVisible(visible);

        if (Animator && Animator.enabled != this.isWorldVisible)
            Animator.enabled = this.isWorldVisible;

        if (RootBone )
            RootBone.gameObject.SetActive(visible);

        if (Skin && Skin.enabled != visible)
            Skin.enabled = visible;
    }

    public override void SetScale(Vector3 scale)
    {
        if (scale.x < 0.001f || scale.x > 10f)
            return;
        base.SetScale(scale);
    }

    public CBaseObject(int apprid)
    {
        pLevel = PLevel.Low;
        Name = apprid.ToString();
        this.gameObject = new GameObject(Name);
        this.Apprid = apprid;
        if (!IsUI_obj)
        {
            HeadRoot = new GameObject(Name + "_headroot");
            CategorySettings.Attach(HeadRoot.transform, "_roles/_headroot", false);
        }
    }

    protected override void OnCreate()
    {
        if (!string.IsNullOrEmpty(Tag))
            this.gameObject.tag = Tag;
        if (!string.IsNullOrEmpty(Name))
            this.gameObject.name = Name;

        LoadSkeleton();
    }


    public virtual void LoadSkeleton()
    {
        //cSkeleton = CPoolManager.Spawn_static(this.Apprid.ToString()) as CSkeletonObject;
        //if (cSkeleton != null)
        //{
        //    cSkeleton.SetParentTransform(this.Transform);
        //    this.Initialize();
        //    return;
        //}
        //cSkeleton = CResourceFactory.CreateInstance<CSkeletonObject>(CString.Format("res/role/{0}.role", Apprid), this, pLevel, this.Transform);
        //AddTimer(OnSkeletonCompleteEvent, null, 0, 0);
    }

    protected override void OnUpdate()
    {
        bool visible = isWorldVisible && isSkinVisible; 
        if (Skin && Skin.enabled != visible || RootBone && RootBone.gameObject.activeInHierarchy != visible)
            SetVisible(visible);
    }

    private bool OnSkeletonCompleteEvent(object obj, int p1, int p2)
    {
        //if (!cSkeleton.IsAbsoluteComplete())
        //    return true;
        this.Initialize();
        return false;
    }

    private void Initialize()
    {
        if (disposed || destroy)
            return;

        //if (!cSkeleton)
        //    return;

        if (Clips.Count > 0)
        {
            foreach (var clip in Clips)
                CreateAniClip(clip);

            Clips.Clear();
            Clips = null;
        }

        if (this.Animator)
            this.Animator.enabled = this.isWorldVisible;

        //this.RoleGameObject = cSkeleton.gameObject;
        //this.RootBone = RootBone = this.RoleGameObject.transform.GetChild(0);
        //if (this.RoleGameObject.transform.childCount > 1 && RootBone == RoleConfig.BoxCollider.transform)
        //    RootBone = this.RoleGameObject.transform.GetChild(1);
        //StandState = GetStand();
        //OnBoneLoadDone();
        //InitializeComponent();

        //if (cSkeleton.EquipCount == 0)
        //{
        //    SetSkinData();
        //    this.IsLoadOver = true;
        //    OnVisible();
        //    OnAllModelsLoadDone();
        //}
        //else
        //    UpdateEquip();

        this.OnInitData();
    }

    public Transform FindBoneTrans(string name)
    {
        if (!RoleConfig)
            return null;
        Transform result;
        BoneMap.TryGetValue(name, out result);
        if (result)
            return result;
        for (int i = 0; i < RoleConfig.Bones.Length; i++)
        {
            GameObject bone = RoleConfig.Bones[i];
            if (!bone)
                continue;
            BoneMap[bone.name] = bone.transform;
            if (bone && name == bone.name)
                return bone.transform;
        }
        return null;
    }

    //public void LeftClick()
    //{
    //    if (this.BoundObject)
    //        this.BoundObject.LeftClick();
    //}

    //public void CachePoolObject(CPoolObject Pool)
    //{
    //    if (PoolList == null || !Pool || disposed)
    //        return;
    //    PoolList.Add(Pool);
    //}


    #region 换装

    protected bool DoUpdateEquip;
    //角色身上当前部位列表
    //protected Map<string,CEquipObject> equips = new Map<string, CEquipObject>();
    //private List<CEquipObject> loadEquips = new List<CEquipObject>();

    //private void SetSkinData()
    //{
    //    if (cSkeleton == null)
    //        return;

    //    FindSingleSkin();
    //    this.Skin = cSkeleton.Skin;
    //    if (this.Skin)
    //    {
    //        this.Skin.quality = IsUI_obj ? SkinQuality.Auto : SkinQuality.Bone1;
    //        CacheMat = this.Skin.sharedMaterial;
    //        this.Skin.reflectionProbeUsage = IsUI_obj ? ReflectionProbeUsage.Simple : ReflectionProbeUsage.Off;
    //    }
    //}

    protected virtual void FindSingleSkin()
    {
        //cSkeleton.FindSingleSkin(false, false);
    }

    /// <summary>
    /// 更新\换装外部接口
    /// </summary>
    public void UpdateEquip()
    {
        if (!isWorldVisible)
        {
            DoUpdateEquip = true;
            return;
        }

//         if (!cSkeleton)
//             return;
// 
//         if (loadEquips.Count > 0)
//         {
//             DoUpdateEquip = true;
//             return;
//         }

        Map<int,string> equipAssets = new Map<int, string>();
        CollectEquips(ref equipAssets);
        if (equipAssets.Count >= 0)
        {
            //for (equipAssets.Begin(); equipAssets.Next();)
            //{
            //    string equip = equipAssets.Value;
            //    if (equips.ContainsKey(equip))
            //        continue;
            //    this.loadEquips.Add(CResourceFactory.CreateInstance<CEquipObject>(equip, this, pLevel, equip));
            //}
            AddTimer(OnAllEquipsCompleteEvent, null, 0, 0);
        }
    }

    protected virtual void CollectEquips(ref Map<int, string> assets)
    {
        if (RoleConfig.Equips == null)
            return;
        for (int i = 0; i < RoleConfig.Equips.Length; i++)
        {
            BaseLook baseLook = RoleConfig.Equips[i];
            assets[baseLook.Mask] = baseLook.Equip;
        }
    }

    private bool OnAllEquipsCompleteEvent(object obj, int p1, int p2)
    {
        return true; 
//         for (int i = 0; i < loadEquips.Count; ++i)
//         {
//             CEquipObject eo = loadEquips[i];
//             if (!eo.IsAllLoadDone())
//                 return true;
//         }
// 
//         for (int i = 0; i < loadEquips.Count; ++i)
//         {
//             CEquipObject eo = loadEquips[i];
//             if (eo.model != null)
//                 OnModelLoadDone(eo.model, eo);
//             else
//                 LOG.LogError("BaseObject OnSingleEquipCompleteEvent 'model' is null");
//         }
// 
//         //通过加载列表，删除身上需要被更换的部位
//         for (int i = 0; i < loadEquips.Count; ++i)
//         {
//             CEquipObject neweo = loadEquips[i];
//             CEquipObject old = GetOldEquip(neweo.mask);
//             if (old != null)
//             {
//                 equips.Remove(old.Assetname);
//                 CClientCommon.DestroyImmediate(old);
//             }
//             equips.Add(neweo.Assetname, neweo);
//         }
//         this.loadEquips.Clear();
// 
//          if (DoUpdateEquip)
//         {
//             UpdateEquip();
//             DoUpdateEquip = false;
//             return false;
//         }
// 
//         for (equips.Begin(); equips.Next();)
//         {
//             equips.Value.SetVisible(true);
//             if (equips.Value.mask == EquipMask.Body)
//             {
//                 this.Skin = equips.Value.GetSkin();
//                 CacheMat = this.Skin.sharedMaterial;
//             }
//         }
// 
//         this.IsLoadOver = true;
//         this.CacheRenders(true);
//         this.OnAllModelsLoadDone();
// 
//         return false;
    }

//     private CEquipObject GetOldEquip(int mask)
//     {
//         for (equips.Begin(); equips.Next();)
//         {
//             if (equips.Value.mask == mask)
//                 return equips.Value;
//         }
//         return null;
//     }

    #endregion

    #region 特殊效果
    /// <summary>
    /// 死亡溶剂效果
    /// </summary>
    /// <param name="dissolvetex"></param>
    public DissolveBurn DoDeadEffect(Texture dissolvetex, float speed = 0.5f, float start = 0, float end = 1.2f)
    {
        if (disposed || !Skin || !CacheMat)
            return null;

        if (!DeadMat)
            DeadMat = new Material(CacheMat);

        Skin.sharedMaterial = DeadMat;
        DissolveBurn db = DissolveBurn.Begin(this.gameObject, 0.5f, 0, 1.2f);
        db.SetMats(DeadMat, dissolvetex, Color.red);

        return db;
    }

    public void DoDissolveAppear(Texture dissolvetex, float speed = 0.5f, float start = 1.2f, float end = 0)
    {
        DissolveBurn db = DoDeadEffect(dissolvetex, speed, start, end);
        if (db == null)
            return;
        db.SetFinish(() =>
        {
            if (Skin)
                Skin.sharedMaterial = CacheMat;
        });
    }

    public void ChangeColor(string pname, Color color)
    {
        if (Skin)
        {
            if (Skin.sharedMaterial.HasProperty(pname))
                Skin.sharedMaterial.SetColor(pname, color);
        }
        else
        {
            //for (equips.Begin(); equips.Next();)
            //{
            //    if (equips.Value.model != null)
            //        equips.Value.model.ChangeColor(pname,color);
            //}
        }
    }

    public void ChangeProperty(string pname, float value)
    {
        if (Skin)
        {
            if (Skin.sharedMaterial.HasProperty(pname))
                Skin.sharedMaterial.SetFloat(pname, value);
        }
        else
        {
            //for (equips.Begin(); equips.Next();)
            //{
            //    if (equips.Value.model != null)
            //        equips.Value.model.ChangeProperty(pname, value);
            //}
        }
    }

    public virtual void OnSelect(Color color)
    {
        EnterWoundColor(color);
    }

    /// <summary>
    /// 受伤效果
    /// </summary>
    public void EnterWoundColor(Color color)
    {
        if (!CacheMat)
            return;

        if (Skin)
            Skin.sharedMaterial = CacheMat;

        if (!WoundMat)
        {
            WoundMat = new Material(CacheMat);
            WoundMat.SetFloat("_MatCapPower", 1);
            WoundMat.SetColor("_RimColor", color);
        }
        if(Skin)
        {
            Skin.sharedMaterial = WoundMat;
            AddTimer(0.08f, OnTimerLevelWoundColor, null, 0, 0);
        }
    }

    private bool OnTimerLevelWoundColor(object obj, int p1, int p2)
    {
        if (Skin)
            Skin.sharedMaterial = CacheMat;
        return false;
    }
    #endregion
}
