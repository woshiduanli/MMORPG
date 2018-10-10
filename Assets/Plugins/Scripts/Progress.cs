using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 场景进度条
/// </summary>
public class Progress : IDisposable
{
    public void Dispose()
    {
        DownLoad = null;
        if (asset != null)
        {
            asset.Dispose();
            asset = null;
        }

        if (www != null)
        {
            www.Dispose();
            www = null;
        }

        if (async != null)
        {
            async.Dispose();
            async = null;
        }
        ResetProgressPercent();
        asset = new AssetProgress(assetPercent);
        if (WarmPromptList.Count > 0)
            WarmPrompt = WarmPromptList[UnityEngine.Random.Range(0, WarmPromptList.Count - 1)];

    }

    public float fprogress {
        get
        {
            if (DownLoad != null)
                return (DownLoad.Progress * 100f);

            float value = 0;

            if (asset != null)
                value += asset.GetValue();

            if (www != null)
                value += www.GetValue();

            if (async != null)
                value += async.GetValue();

            return Mathf.Min(value, 100f);
        }
    }

    public int progress
    {
        get
        {
            if (DownLoad != null)
                return (int)(DownLoad.Progress * 100);

            float value = 0;

            if (asset != null)
                value += asset.GetValue();

            if (www != null)
                value += www.GetValue();

            if (async != null)
                value += async.GetValue();

            return Mathf.Min((int)value, 100);
        }
    }

    public string Tips
    {
        get
        {
            if (DownLoad != null)
                return DownLoad.Tips;

            if (async != null)
               return async.Tips;

            if (www != null)
                return www.Tips;

            if (asset != null)
                return asset.Tips;

            return ConstCopyFilePath.LoadAssetsTips;
        }
    }

    public string DownLoadTips
    {
        get
        {
            if (DownLoad != null)
                return DownLoad.DownLoadTips;
            return string.Empty;
        }
    }

    public string WarmPrompt = string.Empty;
    public static Progress Instance = new Progress();
    private AssetProgress asset;
    private int assetPercent;

    private WWWSceneProgress www;
    private int wwwPercent;

    private AsyncSceneProgress async;
    private int asyncPercent;

    private DownLoadProgress DownLoad;

    private List<string> WarmPromptList = new List<string>();
    
    public Progress()
    {
        ResetProgressPercent();
        asset = new AssetProgress(assetPercent);
    }

    private void ResetProgressPercent()
    {
        assetPercent = 20;
        wwwPercent = 60;
        asyncPercent = 20;
    }

    public void SetProgressPercent(int p,int p1)
    {
        assetPercent = p;
        wwwPercent = p1;
        asyncPercent = 100 - p - p1;

        if (asset != null)
            asset.Setprogress(assetPercent);

        if (www != null)
            www.Setprogress(assetPercent);

        if (async != null)
            async.Setprogress(assetPercent);
    }

    public DownLoadProgress CreateDownLoad(LaunchState helper)
    {
        DownLoad = new DownLoadProgress(helper);
        return DownLoad;
    }

    public void DownLoadDone()
    {
        if (DownLoad != null)
            DownLoad.Done();
        DownLoad = null;
    }

    public WWWSceneProgress CreateWWW()
    {
        asset.Done();
        www = new WWWSceneProgress(wwwPercent);
        return www;
    }

    public AsyncSceneProgress CreateAsync()
    {
        asset.Done();
        async = new AsyncSceneProgress(asyncPercent);
        return async;
    }

    public void AddWarmPromp(string text)
    {
        WarmPromptList.Add(text);
    }

    public void AllDone()
    {
        DownLoadDone();
        CreateWWW().Done();
        CreateAsync().Done();
    }

    public AssetProgress.ItemProgress CreateItem(string tips = "")
    {
        if (!string.IsNullOrEmpty(tips))
            asset.Tips = tips;
        return asset.CreateItem();
    }


    public class DownLoadProgress
    {
        private LaunchState Helper;
        public bool isDone;
        public DownLoadProgress(LaunchState helper)
        {
            this.Helper = helper;
        }

        public string Tips
        {
            get
            {
               return this.Helper.Tips;
            }
        }

        public string DownLoadTips
        {
            get
            {
                return this.Helper.DownLoad;
            }
        }

        public float Progress
        {
            get
            {
                return Helper.ProgressValue;
            }
        }

        public void Done()
        {
            isDone = true;
        }
    }

    public class Base : IDisposable
    {
        public virtual void Dispose() { }
        public string Tips = string.Empty;
        public bool isDone;
        public virtual void Done()
        {
         
            isDone = true;
            this.percent = 1;
        }

        public virtual float GetValue()
        {
            return progress * this.percent;
        }

        public void SetPercent(float t)
        {
            this.percent = t;
        }

        public void Setprogress(int value)
        {
            progress = value;
        }

        public float progress { protected set; get; }
        public virtual float percent { protected set; get; }
    }

    /// <summary>
    /// 资源加载的进度条
    /// </summary>
    public class AssetProgress : Base
    {
        public override void Dispose()
        {
            itemdic.Clear();
            itemdic = null;
        }

        public AssetProgress(int value)
        {
            Tips = ConstCopyFilePath.LoadAssetsTips;
            Setprogress(value);
        }

        public class ItemProgress : Base
        {
            public ItemProgress()
            {
                progress = 1;
            }

            public override float GetValue()
            {
                if (isDone)
                    return progress;
                return 0;
            }
        }

        private List<ItemProgress> itemdic = new List<ItemProgress>();

        public ItemProgress CreateItem()
        {
            ItemProgress item = new ItemProgress();
            itemdic.Add(item);
            return item;
        }

        public override float GetValue()
        {
            if(isDone)
                return this.progress;

            float value = 0;
            float avg = this.progress / itemdic.Count;
            for (int i = 0; i < itemdic.Count; i++)
                value += itemdic[i].GetValue() * avg;

            if (value >= this.progress)
                value = (int)this.progress;
            return value;
        }

    }

    /// <summary>
    /// 异步加载场景AssetBundle的进度条
    /// </summary>
    public class WWWSceneProgress : Base
    {
        public override void Dispose()
        {
            async = null;
        }

        public WWWSceneProgress(int value)
        {
            Tips = ConstCopyFilePath.LoadAssetsTips;
            Setprogress(value);
        }

        public AsyncOperation async;

        public override float percent
        {
            get
            {
                if (isDone)
                    return 1;
                if (async != null)
                {
                    if (async.progress >= 0.99f)
                        return 1;
                    return async.progress;
                }

                return 0;
            }
        }
    }


    /// <summary>
    /// 异步加载场景的进度条
    /// </summary>
    public class AsyncSceneProgress : Base
    {
        public override void Dispose()
        {
            async = null;
        }

        public AsyncSceneProgress(int value)
        {
            Tips = ConstCopyFilePath.LoadAssetsTips;
            Setprogress(value);
        }

        public AsyncOperation async;

        public override float percent
        {
            get
            {
                if (isDone)
                    return 1;
                if (async != null)
                {
                    if (async.progress >= 0.99f)
                        return 1;
                    return async.progress;
                }

                return 0;
            }
        }
    }
}
