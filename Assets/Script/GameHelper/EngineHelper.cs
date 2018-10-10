using UnityEngine;
using System.Collections;



public class TimerEvent
{
    private int socket_index_;
    private int socket_size_;
    private PList[] list_;

    public TimerEvent(int socket_size)
    {
        socket_size_ = socket_size;
        socket_index_ = 0;

        list_ = new PList[socket_size_];
        for (int i = 0; i < socket_size_; i++)
            list_[i] = new PList();
    }

    public TimerEventObject Add(int start, int interval, TimerEventObject.TimerProc proc, object o, int p1, int p2)
    {
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

    public void Remove(TimerEventObject obj)
    {
        if (null != obj)
            obj.enable = 0;
    }

    public void Clear()
    {
        for (int i = 0; i < socket_size_; i++)
        {
            PList head = list_[i];
            for (PListNode pos = head.next; pos != head; pos = pos.next)
            {
                TimerEventObject obj = (TimerEventObject)pos;
                obj.enable = 0;
                obj.proc = null;
                obj.obj = null;
            }
            head.Init();
        }
    }

    public void Process()
    {
        PList head = list_[socket_index_];
        int cur_index = socket_index_;
        socket_index_++;
        if (socket_index_ >= socket_size_)
            socket_index_ = 0;

        PListNode pos = null, n = null;
        for (pos = head.next, n = pos.next; pos != head; pos = n, n = pos.next)
        {
            TimerEventObject obj = (TimerEventObject)pos;
            if (obj.circle <= 0)
            {
                head.Remove(pos);
                if ((0 != obj.enable) && obj.proc(obj.obj, obj.p1, obj.p2))
                {
                    obj.circle = (short)(obj.interval / socket_size_);
                    int index = obj.interval % socket_size_ + cur_index;
                    if (index == cur_index)
                        obj.circle--;
                    if (index >= socket_size_)
                        index = index - socket_size_;
                    list_[index].Add(pos);
                }
                else
                {
                    obj.proc = null;
                    obj.obj = null;
                }
            }
            else
                obj.circle--;
        }
    }
}



public class TimerEventObject : PListNode
{

    public delegate bool TimerProc(object obj, int p1, int p2);

    public TimerProc proc;
    public object obj;
    public int p1, p2;
    public int interval;
    public short circle;
    public short enable;
}


public class PList : PListNode
{
    public int Count = 0;

    public PList()
    {
        Init();
    }

    public bool Empty()
    {
        return this.prev == this;
    }

    public void Init()
    {
        this.next = this;
        this.prev = this;
        this.Count = 0;
    }

    public void Add(PListNode node)
    {
        this.next.prev = node;
        node.next = this.next;
        node.prev = this;
        this.next = node;
        this.Count++;
    }

    public void AddTail(PListNode node)
    {
        PListNode p = prev;
        this.prev = node;
        node.next = this;
        node.prev = p;
        p.next = node;
        Count++;
    }

    public void Remove(PListNode node)
    {
        node.prev.next = node.next;
        node.next.prev = node.prev;
        node.prev = null;
        node.next = null;
        Count--;
    }

    public PListNode Pop()
    {
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

    public void AddListTail(PList list)
    {
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
public class PListNode
{
    public PListNode next;
    public PListNode prev;
}