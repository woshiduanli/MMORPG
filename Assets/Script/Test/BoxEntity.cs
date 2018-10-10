//===================================================
//作    者：边涯  http://www.u3dol.com  QQ群：87481002
//创建时间：2015-11-16 21:38:24
//备    注：
//===================================================
using UnityEngine;
using System.Collections;

/// <summary>
/// 自定义类 类名 BoxEntity
/// </summary>
public class BoxEntity
{
    /// <summary>
    /// 成员变量
    /// </summary>
    private int m_BoxSize;

    /// <summary>
    /// 属性
    /// </summary>
    public int BoxSize
    {
        get { return m_BoxSize; }
        set { m_BoxSize = value; }
    }

    /// <summary>
    /// 自动属性
    /// </summary>
    public string BoxName
    {
        get;
        set;
    }

    /// <summary>
    /// 委托原型
    /// </summary>
    public delegate void OnClickHandler();

    /// <summary>
    /// 事件
    /// </summary>
    public event OnClickHandler OnClick;

    /// <summary>
    /// 构造函数
    /// </summary>
    public BoxEntity()
    {

    }

    /// <summary>
    /// 无返回值 无参数的方法
    /// </summary>
    private void getSize()
    { 
    
    }

    /// <summary>
    /// 有返回值 无参数方法
    /// </summary>
    /// <returns></returns>
    private int getMaxSize()
    {
        return 0;
    }

    /// <summary>
    /// 有返回值 有参数方法
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns></returns>
    private string getDisplayName(string name)
    {
        return string.Format("{0}_AA", name);
    }
}