using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class TestCoCmd : CCoroutineEngine.IQueueCmd
{
    public IEnumerator Execute()
    {
        LOG.Debug("****** call 1");
        yield return null;
        LOG.Debug("****** call 2");
        yield return new WaitForSeconds(1);
        LOG.Debug("****** call 3");
    }
}

public class TestCoCmdBar : CCoroutineEngine.IQueueCmd
{
    public IEnumerator Execute()
    {
        LOG.Debug("***** call bar 1");
        yield return null;
        LOG.Debug("***** call bar 2");
    }
}

/*! CCoroutineEngine
\brief
  全局协程处理器
  提供如下功能：
  1. 开始执行一个协程, Execute接口。
  2. 下一帧并发执行协程。
  3. 串行执行协程：
    class Foo : CCoroutineEngine.IQueueCmd {
        public IEnumerator Execute() { 
            yield return null;
            LOG.Debug( "foo execute" );
        }
    }
    class Bar : CCoroutineEngine.IQueueCmd {
        public IEnumerator Execute() { 
            yield return null;
            LOG.Debug( "bar execute" );
        }
    }
    ... 
    Global.coroutine.AddQueueCmd( new Foo(), true );
    Global.coroutine.AddQueueCmd( new Bar(), true );
    
  　那么Bar将在Foo执行完毕后开始执行
*/
public class CCoroutineEngine : CLoopObject
{
    private readonly Queue<IQueueCmd> queue_ = new Queue<IQueueCmd>(10);         // Cmd队列
    private List<Object> unusedObjectList_ = new List<Object>(128);              // 等删除的Object
    private OneByOne onebyone_;

    protected override void InitData()
    {
        this.onebyone_ = this.GetGameObject().AddComponent(typeof(OneByOne)) as OneByOne;
    }

    class OneByOne : MonoBehaviour
    {
        private readonly Queue<IQueueCmd> queue_ = new Queue<IQueueCmd>(10); // 每帧执行一个的Cmd队列
        void Awake()
        {
            StartCoroutine(OneByOneCall());
        }

        public void AddQueue(IQueueCmd cmd)
        {
            lock (((ICollection)queue_).SyncRoot)
            {
                queue_.Enqueue(cmd);
            }
        }

        public bool Empty()
        {
            if (current == null && this.queue_.Count == 0)
                return true;
            else
                return false;
        }

        void Update()
        {
            if (current == null)
            {
                IQueueCmd cmd = null;
                lock (((ICollection)queue_).SyncRoot)
                {
                    if (queue_.Count > 0)
                    {
                        cmd = queue_.Dequeue();
                    }
                }

                if (cmd != null)
                {
                    current = cmd.Execute();
                }
            }
        }

        IEnumerator current = null;
        IEnumerator OneByOneCall()
        {
            while (true)
            {
                if (current != null)
                {
                    if (current.MoveNext())
                        yield return current.Current;
                    else
                    {
                        current = null;
                    }
                }
                yield return null;
            }
        }
    }

    protected override void Dispose(bool disposed)
    {
        StopAllCoroutines();
        base.Dispose(disposed);
        queue_.Clear();
        while (unusedObjectList_.Count > 0)
            SafeDestroyUnusedObject();
    }

    // 执行一个协程
    public Coroutine Execute(IEnumerator routine)
    {
        return StartCoroutine(routine);
    }

    // 停止某个协程
    public void Stop(IEnumerator routine)
    {
        StopCoroutine(routine);
    }

    public void Stop(Coroutine routine)
    {
        StopCoroutine(routine);
    }

    // 关闭所有协程执行
    public void StopAll()
    {
        StopAllCoroutines();
        queue_.Clear();
    }

    public bool HasOneByOne()
    {
        return !this.onebyone_.Empty();
    }

    public interface IQueueCmd
    {
        IEnumerator Execute();
    }

    // 在主线程里安全销毁一个Unity的Object
    public void SafeDestroy(UnityEngine.Object obj)
    {
        lock (((ICollection)unusedObjectList_).SyncRoot)
        {
            unusedObjectList_.Add(obj);
        }
    }

    // 加一个Cmd到执行队列里
    public void AddQueueCmd(IQueueCmd cmd, bool oneByOne)
    {
        if (!oneByOne)
        {
            lock (((ICollection)queue_).SyncRoot)
            {
                queue_.Enqueue(cmd);
            }
        }
        else
        {
            onebyone_.AddQueue(cmd);
        }
    }

    public void SafeDestroyUnusedObject()
    {
        Object obj = null;
        lock (((ICollection)unusedObjectList_).SyncRoot)
        {
            int i = unusedObjectList_.Count - 1;
            if (i >= 0)
            {
                obj = unusedObjectList_[i];
                unusedObjectList_.RemoveAt(i);
            }
        }
        if (obj)
        {
            Object.Destroy(obj);
        }
    }

    protected override void OnUpdate()
    {
        SafeDestroyUnusedObject();
    }

    public override void LateUpdate()
    {
        // 队列里的所有Cmd        
        while (true)
        {
            IQueueCmd cmd = null;
            lock (((ICollection)queue_).SyncRoot)
            {
                if (queue_.Count > 0)
                {
                    cmd = queue_.Dequeue();
                }
                else
                {
                    break;
                }
            }
            StartCoroutine(cmd.Execute());
        }
    }
}
