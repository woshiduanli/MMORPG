using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using ZCore;


//初始化
public class CGameState_Init : CGameState
{
    public override void Initialize()
    {
        base.Initialize();

#if UNITY_EDITOR
#endif

    }

    protected override void InitData()
    {


        CCoroutineEngine coroutine = this.GetSingleT<CCoroutineEngine>();
        LoadPreLoadShader(Application.isEditor ? ConstFilePathDefine.PCshaders : ConstFilePathDefine.shaders);
        //LoadPreLoadAssets(ConstFilePathDefine.publicAssets);
        //LoadPreLoadAssets(ConstFilePathDefine.texturesAssets);
        //LoadPreLoadAssets(ConstFilePathDefine.CtrlAssets);
        ////coroutine.AddQueueCmd(CResourceFactory.CreateInstance<CCaches>(ConstFilePathDefine.CachePath, null, this), true);
        ////coroutine.AddQueueCmd(CResourceFactory.CreateInstance<CWelcomeWords>(ConstFilePathDefine.WelComeWordsPath, null), true);

        //coroutine.AddQueueCmd(CreateSingleT<CReferenceManager>(), false);
        //coroutine.AddQueueCmd(CResourceFactory.CreateInstance<CServerLists>(ConstFilePathDefine.severList, null, PLevel.High, this), true);
        //coroutine.AddQueueCmd(CResourceFactory.CreateInstance<CWarmprompts>(ConstFilePathDefine.Warmpromptpath, null, PLevel.High), true);
        //coroutine.AddQueueCmd(CResourceFactory.CreateInstance<CBadWords>(ConstFilePathDefine.BadWordFileName, null, PLevel.High, this), true);
        coroutine.AddQueueCmd(new LoadDone(this), true);
        ChangeState(GameState.LOGIN);
    }

    protected override void ChangeState(GameState state, ObjArgs args = null)
    {
        if (state == GameState.LOGIN)
            CreateSingleT<CGameState_Login>(args);
    }


    class LoadDone : CCoroutineEngine.IQueueCmd
    {
        private CGameState_Init State;
        //private CReferenceManager CReference;
        public LoadDone(CGameState_Init state)
        {
            this.State = state;
            //this.CReference = this.State.GetSingleT<CReferenceManager>();
        }

        public IEnumerator Execute()
        {
            //while (!CReference.IsReferenceDone("ReferenceManager`1[MPRoleReference]"))
            //    yield return null;
            //while (!CReference.IsReferenceDone("ReferenceManager`1[EquipReference]"))
            //    yield return null;
            //while (!CReference.IsReferenceDone("ReferenceManager`1[NewDressReference]"))
            //    yield return null;
            Progress.Instance.AllDone();
            this.State.ChangeState(GameState.LOGIN);
            yield return null;
        }
    }

    //----------------------------------
    #region [一系列资源初始化]

    public void LoadPreLoadShader(string path)
    {
        AssetBundle dd = AssetBundle.LoadFromFile(CDirectory.MakeFullMobilePath(path));
    }

#if false
    class CWelcomeWords : ZRender.IRenderObject, CCoroutineEngine.IQueueCmd
    {
        Progress.AssetProgress.ItemProgress progress;
        public CWelcomeWords()
        {
            progress = Progress.Instance.CreateItem();
        }
        protected override void OnCreate() {
            System.Text.StringBuilder sb = this.GetOwner().GetText();
            sb.Replace("﻿", string.Empty); // 这里有一个特殊字符
            ByteReader reader = new ByteReader(System.Text.Encoding.UTF8.GetBytes(sb.ToString()));
            string line;
            while ((line = reader.ReadLine()) != null) 
                Global.welcome_words_.Add(line);
        }

        protected override void OnDestroy() { }

        public IEnumerator Execute() {
            while (!this.complete)
                yield return null;
            Destroy();

            progress.Done();
        }
    }

        class CCaches : ZRender.IRenderObject, CCoroutineEngine.IQueueCmd
    {
        private Progress.AssetProgress.ItemProgress progress;
        private List<string> Assets = new List<string>();
        private CGameState_Init initState;
        public CCaches(CGameState_Init init)
        {
            this.initState = init;
            this.progress = Progress.Instance.CreateItem();
        }

        protected override void OnCreate()
        {
            System.Text.StringBuilder sb = this.GetOwner().GetText();
           // sb.Replace("﻿", string.Empty); // 这里有一个特殊字符
            ByteReader reader = new ByteReader(System.Text.Encoding.UTF8.GetBytes(sb.ToString()));
            string line;
            while ((line = reader.ReadLine()) != null)
                Assets.Add(line);
        }

        protected override void OnDestroy()
        {
            Assets.Clear();
            Assets = null;
        }

        public IEnumerator Execute()
        {
            while (!this.complete)
                yield return null;
            this.initState.FireEvent(new CEvent.ResourceFactory.CacheAssets(Assets));
            progress.Done();

            Destroy();
        }
    }
#endif

    //class CWarmprompts : ZRender.IRenderObject, CCoroutineEngine.IQueueCmd
    //{
    //    Progress.AssetProgress.ItemProgress progress;
    //    public CWarmprompts()
    //    {
    //        progress = Progress.Instance.CreateItem();
    //    }
    //    protected override void OnCreate()
    //    {
    //        System.Text.StringBuilder sb = this.GetOwner().GetText();
    //        //sb.Replace("﻿", string.Empty); // 这里有一个特殊字符
    //        ByteReader reader = new ByteReader(System.Text.Encoding.UTF8.GetBytes(sb.ToString()));
    //        string line;
    //        while ((line = reader.ReadLine()) != null)
    //            Progress.Instance.AddWarmPromp(line);
    //    }

    //    protected override void OnDestroy() { }

    //    public IEnumerator Execute()
    //    {
    //        while (!this.complete)
    //            yield return null;
    //        Destroy();

    //        progress.Done();
    //    }
    //}

    //class CBadWords : ZRender.IRenderObject, CCoroutineEngine.IQueueCmd
    //{
    //    Progress.AssetProgress.ItemProgress progress;
    //    private CGameState_Init initState;
    //    public CBadWords(CGameState_Init init)
    //    {
    //        this.initState = init;
    //        progress = Progress.Instance.CreateItem();
    //    }

    //    protected override void OnCreate()
    //    {
    //        List<string> BadWordList = new List<string>();
    //        System.Text.StringBuilder sb = this.GetOwner().GetText();
    //        //sb.Replace("﻿", string.Empty); // 这里有一个特殊字符
    //        ByteReader reader = new ByteReader(System.Text.Encoding.UTF8.GetBytes(sb.ToString()));
    //        string line;
    //        while ((line = reader.ReadLine()) != null)
    //        {
    //            if (string.IsNullOrEmpty(line))
    //                continue;
    //            BadWordList.Add(line);
    //        }
    //        this.initState.FireEvent(new CEvent.GameState.BadWords(BadWordList));
    //    }

    //    public IEnumerator Execute()
    //    {
    //        while (!this.complete)
    //            yield return null;
    //        Destroy();

    //        progress.Done();
    //    }
    //}

    //class CServerLists : ZRender.IRenderObject, CCoroutineEngine.IQueueCmd
    //{
    //    Progress.AssetProgress.ItemProgress progress;
    //    private CGameState_Init State;
    //    public CServerLists(CGameState_Init state)
    //    {
    //        this.State = state;
    //        progress = Progress.Instance.CreateItem();
    //    }
    //    protected override void OnCreate()
    //    {
    //        System.Text.StringBuilder sb = this.GetOwner().GetText();
    //        //sb.Replace("﻿", string.Empty); // 这里有一个特殊字符
    //        this.State.GetSingleT<CServerList>().ReadFromString(sb.ToString());
    //    }

    //    public IEnumerator Execute()
    //    {
    //        while (!this.complete)
    //            yield return null;
    //        Destroy();

    //        progress.Done();
    //    }
    //}

    //class LoadDone : CCoroutineEngine.IQueueCmd
    //{
    //    private CGameState_Init State;
    //    private CReferenceManager CReference;
    //    public LoadDone(CGameState_Init state)
    //    {
    //        this.State = state;
    //        this.CReference = this.State.GetSingleT<CReferenceManager>();
    //    }

    //    public IEnumerator Execute()
    //    {
    //        while (!CReference.IsReferenceDone("ReferenceManager`1[MPRoleReference]"))
    //            yield return null;
    //        while (!CReference.IsReferenceDone("ReferenceManager`1[EquipReference]"))
    //            yield return null;
    //        Progress.Instance.AllDone();
    //        this.State.ChangeState(GameState.LOGIN);
    //    }
    //}
    #endregion
}
