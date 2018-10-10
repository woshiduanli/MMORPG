using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class UIEventTrigger : UnityEngine.EventSystems.EventTrigger
{
	static UIEventTrigger current;

    public readonly List<EventDelegate> onClick = new List<EventDelegate>();
    public readonly List<EventDelegate> onPress = new List<EventDelegate>();
	public readonly List<EventDelegate> onRelease = new List<EventDelegate>();

	public readonly List<EventDelegate> onDragStart = new List<EventDelegate>();
	public readonly List<EventDelegate> onDrag = new List<EventDelegate>();
	public readonly List<EventDelegate> onDrop = new List<EventDelegate>();
    public readonly List<EventDelegate> onDragEnd = new List<EventDelegate>();

    public List<EventDelegate> GetDelegateList(EventTriggerType ev)
    {
        switch (ev)
        {
            case EventTriggerType.PointerClick:
                return this.onClick;
            case EventTriggerType.PointerDown:
                return this.onPress;
            case EventTriggerType.PointerUp:
                return this.onRelease;
            case EventTriggerType.BeginDrag:
                return this.onDragStart;
            case EventTriggerType.Drag:
                return this.onDrag;
            case EventTriggerType.Drop:
                return this.onDrop;
            case EventTriggerType.EndDrag:
                return this.onDragEnd;
            default:
                return null;
        }
    }

    /// <summary>
    /// 在同一物体上按下并释放
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (current != null)
            return;
        current = this;
        EventDelegate.Execute(onClick, eventData);
        current = null;
    }

    /// <summary>
    /// 按下
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (current != null)
            return;
        current = this;
        EventDelegate.Execute(onPress, eventData);
        current = null;
    }

    /// <summary>
    /// 抬起
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (current != null)
            return;
        current = this;
        EventDelegate.Execute(onRelease, eventData);
        current = null;
    }

    /// <summary>
    /// 开始拖拽.
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (current != null) 
            return;
        current = this;
        EventDelegate.Execute(onDragStart, eventData);
        current = null;
    }

    /// <summary>
    /// 拖拽中
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnDrag(PointerEventData eventData)
    {
        if (current != null) 
            return;
        current = this;
        EventDelegate.Execute(onDrag, eventData);
        current = null;
    }

    /// <summary>
    /// 拖拽结束（拖拽结束后的位置（即鼠标位置）如果有物体，则那个物体调用）
    /// </summary>
    /// <param name="eventData"></param>
    public override void OnDrop(PointerEventData eventData)
    {
        if (current != null)
            return;
        current = this;
        EventDelegate.Execute(onDrop, eventData);
        current = null;
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (current != null)
            return;
        current = this;
        EventDelegate.Execute(onDragEnd, eventData);
        current = null;
    }

    void OnDestroy()
    {
        DestroyEvents(onClick);
        DestroyEvents(onPress);
        DestroyEvents(onRelease);

        DestroyEvents(onDragStart);
        DestroyEvents(onDrag);
        DestroyEvents(onDrop);
        DestroyEvents(onDragEnd);
    }

    private void DestroyEvents(List<EventDelegate> Events)
    {
        for (int i = 0; i < Events.Count; i++)
        {
            EventDelegate ed = Events[i];
            if (ed == null)
                continue;
            ed.Clear();
        }
        Events.Clear();
        Events = null;
    }
}
