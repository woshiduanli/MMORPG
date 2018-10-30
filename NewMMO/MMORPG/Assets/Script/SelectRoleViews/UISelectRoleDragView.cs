
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class UISelectRoleDragView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //开始拖拽的位置
    private Vector2 m_DragBeginPos = Vector2.zero;

    //结束拖拽的位置
    private Vector2 m_DragEndPos = Vector2.zero;

    /// <summary>
    /// 拖拽委托 0=左 1=右
    /// </summary>
    public Action<int> OnSelectRoleDrag;

    void Start () 
	{
	
	}

    void OnDestroy()
    {
        OnSelectRoleDrag = null;
    }

    /// <summary>
    /// 开始拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData)
    {
        m_DragBeginPos = eventData.position;
    }

    /// <summary>
    /// 拖拽中
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData)
    {

    }

    /// <summary>
    /// 结束拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {
        m_DragEndPos = eventData.position;

        float x = m_DragBeginPos.x - m_DragEndPos.x;

        //这个20是容错范围
        if (x > 20)
        {
            OnSelectRoleDrag(-1);
        }
        else if (x < -20)
        {
            OnSelectRoleDrag(1);
        }
    }
}