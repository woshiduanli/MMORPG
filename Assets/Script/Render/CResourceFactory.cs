using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZRender;
using UnityEngine.SceneManagement;
using ZCore; 

public class CResourceFactory : CLoopObject
{
    #region 预加载 缓存

    public class CCacheObject : ZRender.IRenderObject { }

    interface ICacheAsset { void Cache(); }

    class CacheAsset<T> : ICacheAsset where T : ZRender.IRenderObject
    {
        public T asset;
        public string filename;
        public CacheAsset(string filename) { this.filename = filename; }

        public bool Exist
        {
            get { return asset != null; }
        }

        public void Cache()
        {
            asset = CreateInstance<T>(filename, null, PLevel.High, null);
            this.asset.owner.isCache = true;
        }
    }

    //class TimelineCache : ICacheAsset
    //{
    //    public CCacheObject asset;
    //    public string filename;
    //    public TimelineSystem timelineSys;
    //    public TimelineCache(string filename, TimelineSystem timelineSys)
    //    {
    //        this.filename = filename;
    //        this.timelineSys = timelineSys;
    //    }

    //    public void Cache()
    //    {
    //        asset = CreateInstance<CCacheObject>(filename, null, PLevel.High, null);
    //        if (timelineSys)
    //            timelineSys.AddCache(asset);
    //    }
    //}
    #endregion

    #region 空资源
    /// <summary>
    ///  空资源，实例化出来的对象约定为IEmptyObject,在游戏中会用到实例化GameObject，不从任何资源实例化。
    /// </summary>
    public class IEmptyResource : RenderResource
    {
        public IEmptyResource(CResourceFactory factory)
            : base("empty", factory) { }
    }

    /// <summary>
    /// 空资源对象，应该使用IResourceFactory.CreateEmptyInstance创建
    /// </summary>
    public class IEmptyObject : IRenderObject
    {
        protected override void OnCreate()
        {
            this.gameObject = new GameObject("empty");
        }

        protected override void OnDestroy()
        {
            CClientCommon.DestroyImmediate(this.gameObject);
        }
    }

    #endregion

    public static CResourceFactory instance;
    private IEmptyObject empty_parent = null;
    private IEmptyResource empty = null;
    private Cubemap cubemap;
    private Material SkyBox;
    public int LoadLimit = 1;

    private GameSetSystem SetSystem;
    private Queue<ICacheAsset> cache_queue = new Queue<ICacheAsset>();
    private Map<string, RenderResource> ResourceMap = new Map<string, RenderResource>();
    private Queue<RenderResource> LoadQueue = new Queue<RenderResource>();
    private Queue<RenderResource> PriorLoadQueue = new Queue<RenderResource>();
    private Queue<RenderResource> MiddleLoadQueue = new Queue<RenderResource>();
    private List<IEnumerator> load_yield = new List<IEnumerator>();
    private Queue<string> remove_queue = new Queue<string>();
    private Map<string, float> idle = new Map<string, float>();
    private Map<string, Shader> ShaderMap = new Map<string, Shader>();
    private AssetBundle Normals;

    protected override void InitData()
    {
        instance = this;
        ShaderMap["MOYU/UIStandard"] = Shader.Find("MOYU/UIStandard");
        ShaderMap["MOYU/AlphaTest"] = Shader.Find("MOYU/AlphaTest");
        ShaderMap["MOYU/VertexLit"] = Shader.Find("MOYU/VertexLit");
        ShaderMap["MOYU/Player"] = Shader.Find("MOYU/Player");
        ShaderMap["MOYU/PlayerAlpha"] = Shader.Find("MOYU/PlayerAlpha");
        ShaderMap["MOYU/PBR/Player"] = Shader.Find("MOYU/PBR/Player");
        ShaderMap["MOYU/PBR/PlayerAlpha"] = Shader.Find("MOYU/PBR/PlayerAlpha");
        ShaderMap["MOYU/WaveTree/AlphaBlend"] = Shader.Find("MOYU/WaveTree/AlphaBlend");
        ShaderMap["MOYU/WaveTree/AlphaTest"] = Shader.Find("MOYU/WaveTree/AlphaTest");

        this.SetSystem = this.GetSingleT<GameSetSystem>();
        SkyBox = Resources.Load<Material>("skybox/skybox");

        //var bundle = AssetBundle.LoadFromFile(CDirectory.MakeFullMobilePath(ConstFilePathDefine.envAssets));
        //UnityEngine.Object[] objs = bundle.LoadAllAssets();
        //if (objs.Length > 0)
        //    this.cubemap = objs[0] as Cubemap;
    }

    protected override void RegEvents()
    {
        //RegEventHandler<CEvent.Scene.LoadLevelBegin>(OnLoadLevelBegin);
        //RegEventHandler<CEvent.ResourceFactory.ClearResource>(OnClearResource);
        //RegEventHandler<CEvent.ResourceFactory.CacheTimeline>((a, e) =>
        //{
        //    for (int i = 0; i < e.Assets.Count; i++)
        //        cache_queue.Enqueue(new TimelineCache(e.Assets[i], e.timelineSys));
        //});
    }

    public static T CreateInstance<T>(string filename, ZRender.IRenderObject parent, params object[] args)
        where T : ZRender.IRenderObject
    {
        return instance.CreateInstanceT<T>(filename, parent, PLevel.Low, 0f, args) as T;
    }

    public static T CreateInstance<T>(string filename, ZRender.IRenderObject parent, PLevel level = PLevel.Low, params object[] args)
        where T : ZRender.IRenderObject
    {
        return instance.CreateInstanceT<T>(filename, parent, level, 0f, args) as T;
    }

    public static T CreateTimeInstance<T>(string filename, ZRender.IRenderObject parent, PLevel level = PLevel.Low, float linger_time = 0f, params object[] args)
        where T : ZRender.IRenderObject
    {
        return instance.CreateInstanceT<T>(filename, parent, level, linger_time, args) as T;
    }

    public T CreateInstanceT<T>(string filename, IRenderObject parent, PLevel level, float linger_time, params object[] args)
        where T : IRenderObject
    {
        if (string.IsNullOrEmpty(filename))
            return null;
        if (Application.isEditor)
        {
            // 只在编辑器环境做检查，减少运行时开销
            //if (CMisc.isLegalNumber(filename))
            //    LOG.Warning(Localization.Format("INVALID_LOAD_PATH", filename));
        }

        // 修改高优先级加载的策略，figo 2018-08-11
        RenderResource resource = null;
        ResourceMap.TryGetValue(filename, out resource);

        if (resource == null)
        {
            resource = new RenderResource(filename, this, level, linger_time);
            ResourceMap[filename] = resource;
            switch (level)
            {
                case PLevel.High: PriorLoadQueue.Enqueue(resource); break;
                case PLevel.Middle: MiddleLoadQueue.Enqueue(resource); break;
                default: LoadQueue.Enqueue(resource); break;
            }
        }
        else
        {
            // Remove from idle
            idle.Remove(filename);
        }
        resource.isdestroy = false;
        T result = resource.CreateInstance<T>(parent, args) as T;
        return result;
    }

    public static T CreateEmptyInstance<T>(IRenderObject parent, params object[] args)
        where T : IRenderObject
    {
        if (instance.empty == null)
        {
            instance.empty = new IEmptyResource(instance);
            instance.empty_parent = instance.empty.CreateInstance<IEmptyObject>(null) as IEmptyObject;
            instance.ResourceMap.Add("empty", instance.empty);
            instance.empty.Load();
        }
        instance.empty.complete = true;
        return instance.empty.CreateInstance<T>(parent, args) as T;
    }

    public static T CreateSceneInstance<T>(string filename, params object[] args)
        where T : IRenderObject
    {
        RenderResource resource = null;
        instance.ResourceMap.TryGetValue(filename, out resource);

        if (resource == null)
        {
            //var cache_resource = new SceneResource(filename, instance);
            //resource = cache_resource;
            //instance.ResourceMap[filename] = resource;
            //// 场景资源立即加载？
            //instance.PriorLoadQueue.Enqueue(resource);
        }
        else
        {
            // remove resource from idle
            instance.idle.Remove(filename);
        }

        T result = resource.CreateInstance<T>(null, args) as T;
        return result;
    }

    private int frame_limit = 0;
    protected override void OnUpdate()
    {
        // 修改高优先级加载的策略，figo 2018-08-11
        //
        if (LoadQueue.Count == 0 && PriorLoadQueue.Count == 0 && MiddleLoadQueue.Count == 0)
        {
            if (cache_queue.Count > 0)
            {
                ICacheAsset cache = cache_queue.Dequeue();
                cache.Cache();
            }
        }

        if (frame_limit == 0)
        {
            switch (MobileHelper.GetDeviceLevel())
            {
                case DeviceLevel.UNKNOWN:
                case DeviceLevel.LEVEL_1:
                    frame_limit = 2;
                    break;
                case DeviceLevel.LEVEL_2:
                    frame_limit = 2;
                    break;
                default:
                    frame_limit = 2;
                    break;
            }
        }
        // 限祯加载
        bool process_load = true;
        if (LoadLimit == 1 && (Time.frameCount % frame_limit != 0))
            process_load = false;
        if (process_load)
        {
            int n = 0;
            while (n < LoadLimit && PriorLoadQueue.Count > 0)
            {
                var resource = PriorLoadQueue.Dequeue();
                if (resource.isdestroy)
                {
                    // 要判断是否destroy吗?
                    continue;
                }
                var itr = resource.Load();
                this.load_yield.Add(itr);
                ++n;
            }
            while (n < LoadLimit && MiddleLoadQueue.Count > 0)
            {
                var resource = MiddleLoadQueue.Dequeue();
                if (resource.isdestroy)
                    // 要判断是否destroy吗?
                    continue;
                var itr = resource.Load();
                this.load_yield.Add(itr);
                ++n;
            }
            while (n < LoadLimit && LoadQueue.Count > 0)
            {
                var resource = LoadQueue.Dequeue();
                if (resource.isdestroy)
                {
                    // 要判断是否destroy吗?
                    continue;
                }
                var itr = resource.Load();
                this.load_yield.Add(itr);
                ++n;
            }
        }

        // Drive the load IEnumerator
        if (this.load_yield.Count > 0)
        {
            int i = 0;
            while (i < this.load_yield.Count)
            {
                var yield = this.load_yield[i];
                bool goon = false;
                try
                {
                    goon = yield.MoveNext();
                }
                catch
                {
                    goon = false;
                    // TODO: log
                }
                if (!goon)
                {
                    this.load_yield.RemoveAt(i);
                    continue;
                }
                ++i;
            }
        }

        // Remove idle resource.
        const float linger_time = 0f;
        float time = Time.realtimeSinceStartup;
        for (idle.Begin(); idle.Next();)
        {
            if (time < idle.Value + linger_time)
                continue;
            remove_queue.Enqueue(idle.Key);
        }
        while (remove_queue.Count > 0)
        {
            var filename = remove_queue.Dequeue();
            RenderResource resource;
            if (ResourceMap.TryGetValue(filename, out resource))
            {
                // Can not destroy loading resource.
                if (!resource.complete)
                    continue;
                resource.Destroy();
                ResourceMap.Remove(filename);
            }
            this.idle.Remove(filename);
        }
    }

    public void RemoveResource(RenderResource resource)
    {
        string name = resource.Assetname;
        // Check for safe
        if (!ResourceMap.ContainsKey(name))
        {
            if (resource.loading)
                throw new Exception(string.Format("destroy loading resource {0}", name));
            resource.Destroy();
            return;
        }

        float linger_time = resource.linger_time;
        float time = Time.realtimeSinceStartup + linger_time;
        if (this.idle.ContainsKey(name))
            this.idle[name] = time;
        else
            this.idle.Add(name, time);
    }

    private void OnSceneConfig()
    {
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        RenderSettings.ambientSkyColor = new Color(0.192f, 0.36f, 0.85f, 1);
        RenderSettings.ambientEquatorColor = new Color(0.27f, 0.23f, 0.29f, 1);
        RenderSettings.ambientGroundColor = new Color(0.67f, 0.55f, 0.9f, 1);

        CheckGameState();
        if (this.cubemap)
        {
            RenderSettings.defaultReflectionMode = UnityEngine.Rendering.DefaultReflectionMode.Custom;
            RenderSettings.customReflection = this.cubemap;
            RenderSettings.reflectionIntensity = 0.28f;
        }
        //临时代码
        //2018-8-10 成娟修改天空合错误
        string ActiveName = SceneManager.GetActiveScene().name;
        Color skycolor = Color.gray;
        //if (this.World && this.World.Ref.SkyBox)
        //    skycolor = Color.black;
        RenderSettings.skybox = this.SkyBox;
        RenderSettings.skybox.shader = CResourceFactory.FindShader(this.SkyBox.shader.name);
        RenderSettings.skybox.SetColor("_Tint", skycolor);
    }

    //private void OnClearResource(CObject sender, CEvent.ResourceFactory.ClearResource e)
    //{
    //    this.StopAllCoroutines();
    //    this.OnSceneConfig();
    //    this.CheckGameState();
    //    this.LoadLimit = this.isInGame ? 1 : 6;
    //}

    private void OnLoadLevelBegin(CObject sender, CEvent.Scene.LoadLevelBegin e)
    {
        if (e.sceneName == SceneName.lAUNCHER || e.sceneName == SceneName.ASYNC_LOADER_SCENE)
            return;

        //if (e.sceneName == SceneName.ROLE_SELECT_SCENE || GetSingleT<GameSetSystem>().quality != (int)GameSetSystem.Quality.Rapidly)
        //{
        //    if (!Normals)
        //        Normals = AssetBundle.LoadFromFile(CDirectory.MakeFullMobilePath(ConstFilePathDefine.normalsAssets));
        //}
        else if (Normals)
        {
            Normals.Unload(true);
            Normals = null;
        }
    }

    public static Shader FindShader(string name)
    {
        Shader s = null;
        instance.ShaderMap.TryGetValue(name, out s);
        if (!s)
        {
            s = Shader.Find(name);
            instance.ShaderMap[name] = s;
        }
        return s;
    }
}

