using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZCore.InternalUtil {

    public class PListNode {
        public PListNode next;
        public PListNode prev;
    }

    public class PList : PListNode {
        public int Count = 0;

        public PList() {
            Init();
        }

        public bool Empty() {
            return this.prev == this;
        }

        public void Init() {
            this.next = this;
            this.prev = this;
            this.Count = 0;
        }

        public void Add(PListNode node) {
            this.next.prev = node;
            node.next = this.next;
            node.prev = this;
            this.next = node;
            this.Count++;
        }

        public void AddTail(PListNode node) {
            PListNode p = prev;
            this.prev = node;
            node.next = this;
            node.prev = p;
            p.next = node;
            Count++;
        }

        public void Remove(PListNode node) {
            node.prev.next = node.next;
            node.next.prev = node.prev;
            node.prev = null;
            node.next = null;
            Count--;
        }

        public PListNode Pop() {
            if (prev == this)
                return null;

            PListNode node = this.next;
            node.prev.next = node.next;
            node.next.prev = node.prev;
            node.prev = null;
            node.next = null;
            Count--;

            return node;
        }

        public void AddListTail(PList list) {
            if (list.next == list)
                return;

            PListNode first = list.next;
            PListNode last = list.prev;
            PListNode at = this.prev;

            at.next = first;
            first.prev = at;

            this.prev = last;
            last.next = this;

            this.Count += list.Count;
            list.Init();
        }
    }


#if false
    public class PListNode<T> where T : PListNode<T> {
        protected PListNode<T> next;
        protected PListNode<T> prev;
        public static implicit operator T(PListNode<T> n) { return n as T; }

        public class List : PListNode<T> {
            public int Count = 0;

            public List() {
                Init();
            }

            public bool Empty() {
                return this.prev == this;
            }

            public void Init() {
                this.next = this;
                this.prev = this;
                this.Count = 0;
            }

            public void Add(T node) {
                this.next.prev = node;
                node.next = this.next;
                node.prev = this;
                this.next = node;
                this.Count++;
            }

            public void AddTail(T node) {
                PListNode<T> p = prev;
                this.prev = node;
                node.next = this;
                node.prev = p;
                p.next = node;
                Count++;
            }

            public void Remove(T node) {
                node.prev.next = node.next;
                node.next.prev = node.prev;
                node.prev = null;
                node.next = null;
                Count--;
            }

            public T Pop() {
                if (prev == this)
                    return null;

                PListNode<T> node = this.next;
                node.prev.next = node.next;
                node.next.prev = node.prev;
                node.prev = null;
                node.next = null;
                Count--;

                return node;
            }

            public void AddListTail(PList<T> list) {
                if (list.next == list)
                    return;

                PListNode<T> first = list.next;
                PListNode<T> last = list.prev;
                PListNode<T> at = this.prev;

                at.next = first;
                first.prev = at;

                this.prev = last;
                last.next = this;

                this.Count += list.Count;
                list.Init();
            }

            public T Next(T node) {
                if (node == null)
                    return this.next;
                if (node.next == this)
                    return null;
                return node.next;
            }
        }
    }

    public class PList<T> : PListNode<T>.List
        where T : PListNode<T> { }

#endif

    public class PListNode<T> where T : PListNode<T> {
        internal PListNode<T> next;
        internal PListNode<T> prev;
        public static implicit operator T(PListNode<T> n) { return n as T; }
    }

    public class PList<T> : PListNode<T> where T : PListNode<T> {
        public int Count = 0;

        public PList() {
            Init();
        }

        public bool Empty() {
            return this.prev == this;
        }

        public void Init() {
            this.next = this;
            this.prev = this;
            this.Count = 0;
        }

        public void Add(T node) {
            this.next.prev = node;
            node.next = this.next;
            node.prev = this;
            this.next = node;
            this.Count++;
        }

        public void AddTail(T node) {
            PListNode<T> p = prev;
            this.prev = node;
            node.next = this;
            node.prev = p;
            p.next = node;
            Count++;
        }

        public void Remove(T node) {
            node.prev.next = node.next;
            node.next.prev = node.prev;
            node.prev = null;
            node.next = null;
            Count--;
        }

        public T Pop() {
            if (prev == this)
                return null;

            PListNode<T> node = this.next;
            node.prev.next = node.next;
            node.next.prev = node.prev;
            node.prev = null;
            node.next = null;
            Count--;

            return (T)node;
        }

        public void AddListTail(PList<T> list) {
            if (list.next == list)
                return;

            PListNode<T> first = list.next;
            PListNode<T> last = list.prev;
            PListNode<T> at = this.prev;

            at.next = first;
            first.prev = at;

            this.prev = last;
            last.next = this;

            this.Count += list.Count;
            list.Init();
        }

        public T Next(T node) {
            if (node == null)
                return (T)this.next;
            if (node.next == this)
                return null;
            return (T)node.next;
        }
    }


    /// <summary>
    /// 一个简单的LRU容器
    ///  - by figo 2013-06-29
    /// </summary>
    /// <typeparam name="T"></typeparam>
    //public class LRU<T> where T : IDisposable {
    //    public class LRUItem : PListNode {
    //        public string key;
    //        public T value;
    //        public float time;
    //    }

    //    private Dictionary<string, LRUItem> item_table = new Dictionary<string, LRUItem>();
    //    private PList item_list = new PList();
    //    private readonly float life_time;
    //    //private readonly int count_limit;

    //    public LRU(float life_time = 60.0f, int count_limit = 0) {
    //        this.life_time = life_time;
    //        //this.count_limit = count_limit;
    //    }

    //    public int Count {
    //        get { return this.item_list.Count; }
    //    }

    //    /// <summary>
    //    /// 返回一个节点
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <returns></returns>
    //    public T GetItem(string key) {
    //        LRUItem item;
    //        if (this.item_table.TryGetValue(key, out item)) {
    //            item.time = GameTimer.time;
    //            //LOG.Debug("LRU GetItem " + key);
    //            return item.value;
    //        } else {
    //            //LOG.Debug("LRU GetItem " + key + " null");
    //            return default(T);
    //        }
    //    }

    //    /// <summary>
    //    /// 添加一个节点
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="value"></param>
    //    public void AddItem(string key, T value) {
    //        //LOG.Debug("LRU AddItem " + key);

    //        LRUItem item;
    //        if (this.item_table.TryGetValue(key, out item)) {
    //            RemoveItem(item);
    //        }
    //        item = new LRUItem();
    //        item.key = key;
    //        item.value = value;
    //        item.time = GameTimer.time;
    //        this.item_table.Add(key, item);
    //        this.item_list.Add(item as PListNode);
    //    }

    //    /// <summary>
    //    /// 删除一个节点
    //    /// </summary>
    //    /// <param name="key"></param>
    //    public void RemoveItem(string key) {
    //        LRUItem item;
    //        if (this.item_table.TryGetValue(key, out item)) {
    //            RemoveItem(item);
    //        }
    //    }

    //    /// <summary>
    //    /// 删除所有节点
    //    /// </summary>
    //    public void RemoveAllItem() {
    //        PListNode node = this.item_list.prev;
    //        while (node != this.item_list) {
    //            PListNode prev = node.prev;
    //            RemoveItem(node as LRUItem);
    //            node = prev;
    //        }
    //    }

    //    /// <summary>
    //    /// 更新节点
    //    /// </summary>
    //    /// <param name="needkeeper">可选，外部决定是否需要删除过期节点</param>
    //    public void Update(Predicate<T> needkeeper) {
    //        //从尾部开始遍历
    //        PListNode node = this.item_list.prev;
    //        while (node != this.item_list) {
    //            LRUItem item = node as LRUItem;
    //            PListNode prev = node.prev;
    //            if (item.time + this.life_time < GameTimer.time) {
    //                if (needkeeper == null || !needkeeper(item.value)) {
    //                    RemoveItem(item);
    //                }
    //            } else {
    //                break;
    //            }
    //            node = prev;
    //        }
    //    }

    //    /// <summary>
    //    /// 遍历节点，提供visitor如果返回true时，停止遍历
    //    /// </summary>
    //    /// <param name="visitor"></param>
    //    public void ForEach(Predicate<T> visitor) {
    //        if (visitor == null)
    //            return;
    //        if (Count == 0)
    //            return;
    //        PListNode node = this.item_list.next;
    //        while (node != this.item_list) {
    //            LRUItem item = node as LRUItem;
    //            if (visitor(item.value)) {
    //                break;
    //            }
    //            node = node.next;
    //        }
    //    }

    //    private void RemoveItem(LRUItem item) {
    //        //LOG.Debug("LRU RemoveItem " + item.key);
    //        this.item_list.Remove(item);
    //        this.item_table.Remove(item.key);
    //        if (item.value != null) {
    //            item.value.Dispose();
    //        }
    //    }
    //}


    /// <summary>
    /// BinaryHeap 
    ///   - by figo     Mon,17,2011
    ///   - Modified    Oct,25,2013
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HeapNode {
        public int node_index = -1;
    }

    public class BinaryHeap<T> where T : HeapNode {
        protected List<T> objlist;
        private IComparer comparer;
        public BinaryHeap(IComparer comparer = null) {
            this.objlist = new List<T>();
            if (comparer == null)
                this.comparer = Comparer.Default;
            else
                this.comparer = comparer;
        }

        public void Push(T obj) {
            this.objlist.Add(obj);
            this.objlist[this.objlist.Count - 1].node_index = this.objlist.Count - 1;
            int i = this.objlist.Count;
            while (i > 1) {
                if (this.comparer.Compare(this.objlist[i / 2 - 1], this.objlist[i - 1]) > 0) {
                    Swap(i / 2 - 1, i - 1);
                    i = i / 2;
                } else {
                    break;
                }
            }
            FixSiblingNodeUp(this.objlist.Count, i);
        }

        public void Update(T obj) {
            int index = obj.node_index;
            if (index < 0 || index >= this.objlist.Count)
                return;
            this.objlist[index] = obj;
            int m = index + 1;
            bool raise = false;
            while (m > 1) {
                if (this.comparer.Compare(this.objlist[m / 2 - 1], this.objlist[m - 1]) > 0) {
                    Swap(m / 2 - 1, m - 1);
                    m = m / 2;
                    raise = true;
                } else {
                    break;
                }
            }
            FixSiblingNodeUp(index + 1, m);
            if (!raise) {
                int v = m, u;
                do {
                    u = v;
                    if (2 * u + 1 <= this.objlist.Count) {
                        if (this.comparer.Compare(this.objlist[u - 1], this.objlist[2 * u - 1]) > 0) {
                            v = 2 * u;
                        }
                        if (this.comparer.Compare(this.objlist[v - 1], this.objlist[2 * u]) > 0) {
                            v = 2 * u + 1;
                        }
                    } else {
                        if (2 * u <= this.objlist.Count) {
                            if (this.comparer.Compare(this.objlist[u - 1], this.objlist[2 * u - 1]) > 0) {
                                v = 2 * u;
                            }
                        }
                    }
                    if (v != u) {
                        Swap(v - 1, u - 1);
                    } else {
                        break;
                    }
                } while (true);
                FixSiblingNodeDown(u, index + 1);
            }
        }

        public T Top() {
            if (this.objlist.Count <= 0) {
                return default(T);
            }
            return this.objlist[0];
        }

        // 该接口慎用
        public T Pop() {
            if (this.objlist.Count < 0) {
                return default(T);
            }
            T obj = this.objlist[0];
            Delete(obj);
            return obj;
        }

        // 该接口慎用
        public void Delete(T obj) {
            int index = obj.node_index;
            if (index < 0 || index >= this.objlist.Count)
                return;
            for (int i = index; i < this.objlist.Count - 1; i++) {
                Swap(i, i + 1);
            }
            this.objlist.RemoveAt(this.objlist.Count - 1);
        }

        public T Left(T obj) {
            int index = obj.node_index;
            int u = index + 1;
            if (2 * u <= this.objlist.Count) {
                return this.objlist[2 * u - 1];
            }
            return default(T);
        }

        public T Right(T obj) {
            int index = obj.node_index;
            int u = index + 1;
            if (2 * u + 1 <= this.objlist.Count) {
                return this.objlist[2 * u];
            }
            return default(T);
        }

        public T Parent(T obj) {
            int index = obj.node_index;
            int m = index + 1;
            if (m > 1)
                return this.objlist[m / 2 - 1];
            else
                return default(T);
        }

        public void ForEach(Action<T> action) {
            for (int i = 0; i < this.objlist.Count; ++i) {
                if (this.objlist[i] != null)
                    action(this.objlist[i]);
            }
        }

        //安全版ForEach
        public void ForEachSafe(Action<T> action) {
            int count = this.objlist.Count;
            for (int i = 0; i < count; ) {
                if (this.objlist[i] != null)
                    action(this.objlist[i]);
                if (this.objlist.Count != count)
                    count = this.objlist.Count;
                else
                    ++i;
            }
        }

        public void ForEach(Action<T> action, int start, int count) {
            for (int i = start; i < this.objlist.Count && (i - start) < count; ++i) {
                if (this.objlist[i] != null)
                    action(this.objlist[i]);
            }
        }

        public void Visit(Predicate<T> visitor) {
            for (int i = 0; i < this.objlist.Count; ++i) {
                if (this.objlist[i] != null) {
                    if (!visitor(this.objlist[i]))
                        break;
                }
            }
        }

        public void Swap(int u, int v) {
            T temp = this.objlist[u];
            this.objlist[u] = this.objlist[v];
            this.objlist[u].node_index = u;
            this.objlist[v] = temp;
            this.objlist[v].node_index = v;
        }

        public void Clear() {
            for (int i = 0; i < this.objlist.Count; ++i) {
                if (this.objlist[i] != null)
                    this.objlist[i].node_index = -1;
            }
            this.objlist.Clear();
        }

        public T PopTail() {
            if (this.objlist.Count <= 0)
                return null;
            T obj = this.objlist[this.objlist.Count - 1];
            this.objlist.RemoveAt(this.objlist.Count - 1);
            return obj;
        }

        private void FixSiblingNodeUp(int from, int to) {
            for (int j = from; j >= to; j = j / 2) {
                int jp = j / 2;
                for (int k = j - 1; k > jp; k--) {
                    if (this.comparer.Compare(this.objlist[k - 1], this.objlist[k]) > 0)
                        Swap(k - 1, k);
                    else
                        break;
                }
            }
        }

        private void FixSiblingNodeDown(int from, int to) {
            for (int j = from; j >= to; j = j / 2) {
                int jlc = j * 2 + 1;
                jlc = jlc < this.objlist.Count + 1 ? jlc : this.objlist.Count + 1;
                for (int k = j + 1; k < jlc; k++) {
                    if (this.comparer.Compare(this.objlist[k - 2], this.objlist[k - 1]) > 0)
                        Swap(k - 2, k - 1);
                    else
                        break;
                }
            }
        }
    }

    //public class UniqueIndex {
    //    private int[] indexes_ = null;
    //    private int alloc_index_ = 0;
    //    private int count_ = 0;

    //    public UniqueIndex(int count) {
    //        this.indexes_ = new int[count];
    //        this.count_ = count;
    //        this.alloc_index_ = count_;

    //        for (int i = 0; i < count_; i++)
    //            this.indexes_[i] = count_ - 1 - i; //小的放后面,从0开始分配
    //    }

    //    public void Grow(int n) {
    //        Array.Resize(ref indexes_, count_ + n);
    //        for (int i = 0; i < n; i++)
    //            indexes_[alloc_index_ + i] = count_ + i;
    //        count_ += n;
    //        alloc_index_ += n;
    //    }

    //    public void RemoveAll() {
    //        alloc_index_ = 0;
    //    }

    //    public int Alloc() {
    //        if (alloc_index_ > 0)
    //            return indexes_[--alloc_index_];
    //        else
    //            return Def.INVALID_ID;
    //    }

    //    public void Free(int index) {
    //        if (alloc_index_ < indexes_.Length)
    //            indexes_[alloc_index_++] = index;
    //    }

    //    public bool CanAlloc() {
    //        return alloc_index_ > 0;
    //    }

    //    public int CanAllocCount() {
    //        return alloc_index_;
    //    }

    //    public void Print() {
    //        Console.WriteLine("alloc_index:{0} ", alloc_index_);
    //        for (int i = 0; i < alloc_index_; i++)
    //            Console.WriteLine("  {0}", indexes_[i]);
    //    }

    //    public int Count {
    //        get { return count_; }
    //    }
    //}


    
    internal class FixQueue<T> {
        private T[] queue = null;
        private readonly int capacity;
        private volatile uint write;
        private volatile uint read;
        public FixQueue(int capacity) {
            this.capacity = capacity;
            this.queue = new T[capacity];
            this.write = 0;
            this.read = 0;
        }

        public int Count { get { return (int)(this.write - this.read); } }

        public IEnumerable<T> GetEnumerator() {
            uint write_count = this.write;
            uint read_count = this.read;
            for (uint i = read_count; i < write_count; ++i) {
                int offset = (int)(i % (uint)this.capacity);
                yield return this.queue[offset];
            }
            yield break;
        }

        public bool Enqueue(T obj) {
            uint write_count = this.write;
            uint read_count = this.read;

            int count = (int)(write_count - read_count);
            if (count >= this.capacity)
                return false;
            int offset = (int)(write_count % (uint)this.capacity);
            this.queue[offset] = obj;
            System.Threading.Thread.MemoryBarrier();
            ++this.write;
            return true;
        }

        public T Dequeue() {
            uint write_count = this.write;
            uint read_count = this.read;
            if (write_count <= read_count)
                return default(T);

            int offset = (int)(read_count % (uint)this.capacity);
            T obj = this.queue[offset];
            this.queue[offset] = default(T);
            System.Threading.Thread.MemoryBarrier();
            ++this.read;
            return obj;
        }

        public void Clear() {
            for (int i = 0; i < this.queue.Length; ++i) {
                this.queue[i] = default(T);
            }
            this.write = this.read = 0;
        }
    }


    public class TimerEventObject : PListNode {
        public delegate bool TimerProc(object obj, int p1, int p2);

        public TimerProc proc;
        public object obj;
        public int p1, p2;
        public int interval;
        public short circle;
        public short enable;
    }

    public class TimerEvent {
        private int socket_index_;
        private int socket_size_;
        private PList[] list_;

        public TimerEvent(int socket_size) {
            socket_size_ = socket_size;
            socket_index_ = 0;

            list_ = new PList[socket_size_];
            for (int i = 0; i < socket_size_; i++)
                list_[i] = new PList();
        }

        public TimerEventObject Add(int start, int interval, TimerEventObject.TimerProc proc, object o, int p1, int p2) {
            TimerEventObject obj = new TimerEventObject();
            obj.proc = proc;
            if (interval <= 0)
                interval = 1;
            obj.interval = interval;
            obj.obj = o;
            obj.p1 = p1;
            obj.p2 = p2;
            obj.enable = 1;
            obj.circle = (short)(start / socket_size_);
            int index = start % socket_size_ + socket_index_;
            if (index >= socket_size_)
                index = index - socket_size_;

            list_[index].AddTail(obj);
            return obj;
        }

        public void AddTimer(float time, TimerEventObject.TimerProc proc, object obj, int p1, int p2) {
            int frame = Convert.ToInt32(Application.targetFrameRate * time);
            Add(frame, frame, proc, obj, p1, p2);
        }

        public void Remove(TimerEventObject obj) {
            if (null != obj)
                obj.enable = 0;
        }

        public void Clear() {
            for (int i = 0; i < socket_size_; i++) {
                PList head = list_[i];
                for (PListNode pos = head.next; pos != head; pos = pos.next) {
                    TimerEventObject obj = (TimerEventObject)pos;
                    obj.enable = 0;
                    obj.proc = null;
                    obj.obj = null;
                }
                head.Init();
            }
        }

        public void Process() {
            PList head = list_[socket_index_];
            int cur_index = socket_index_;
            socket_index_++;
            if (socket_index_ >= socket_size_)
                socket_index_ = 0;

            PListNode pos = null, n = null;
            for (pos = head.next, n = pos.next;
                  pos != head;
                  pos = n, n = pos.next) {
                TimerEventObject obj = (TimerEventObject)pos;
                if (obj.circle <= 0) {
                    head.Remove(pos);
                    if ((0 != obj.enable) && obj.proc(obj.obj, obj.p1, obj.p2)) {
                        obj.circle = (short)(obj.interval / socket_size_);
                        int index = obj.interval % socket_size_ + cur_index;
                        if (index == cur_index)
                            obj.circle--;
                        if (index >= socket_size_)
                            index = index - socket_size_;
                        list_[index].Add(pos);
                    } else {
                        obj.proc = null;
                        obj.obj = null;
                    }
                } else
                    obj.circle--;
            }
        }
    }
}
