
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class UIRoleInfoDragView : UISubViewBase, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// 旋转的目标
    /// </summary>
    [SerializeField]
    private Transform m_Target;

    //开始拖拽的位置
    private Vector2 m_DragBeginPos = Vector2.zero;

    //结束拖拽的位置
    private Vector2 m_DragEndPos = Vector2.zero;

    /// <summary>
    /// 旋转的速度
    /// </summary>
    private float m_Speed = 600;

    protected override void BeforeOnDestroy()
    {
        base.BeforeOnDestroy();
        m_Target = null;
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
        m_DragEndPos = eventData.position;
        float x = m_DragBeginPos.x - m_DragEndPos.x;
        m_Target.Rotate(0, Time.deltaTime * m_Speed * (x > 0 ? 1 : -1), 0);

        m_DragBeginPos = m_DragEndPos;
    }

    /// <summary>
    /// 结束拖拽
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData)
    {

    }
}