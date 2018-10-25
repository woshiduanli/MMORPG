
using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public partial class LanguageDBModel
{
    /// <summary>
    /// 当前语言
    /// </summary>
    public Language CurrLanguage { get; set; }

    /// <summary>
    /// 根据模块和key获取值
    /// </summary>
    /// <param name="module"></param>
    /// <param name="key"></param>
    /// <returns></returns>
    public string GetText(string module, string key)
    {
        if (m_List != null && m_List.Count > 0)
        {
            for (int i = 0; i < m_List.Count; i++)
            {
                //如果模块和key都对应上
                if (m_List[i].Module.Equals(module, StringComparison.CurrentCultureIgnoreCase) &&
                    m_List[i].Key.Equals(key, StringComparison.CurrentCultureIgnoreCase))
                {
                    switch (CurrLanguage)
                    {
                        case Language.CN:
                            return m_List[i].CN;
                        case Language.EN:
                            return m_List[i].EN;
                    }
                }
            }
        }
        return null;
    }

    /// <summary>
    /// 返回所有模块
    /// </summary>
    /// <returns></returns>
    public List<string> GetModules()
    {
        if (m_List != null && m_List.Count > 0)
        {
            List<string> retList = new List<string>();

            for (int i = 0; i < m_List.Count; i++)
            {
                if (retList.Contains(m_List[i].Module)) continue;
                retList.Add(m_List[i].Module);
            }
            return retList;
        }
        return null;
    }

    /// <summary>
    /// 根据模块名称返回他的所有Key
    /// </summary>
    /// <param name="module"></param>
    /// <returns></returns>
    public List<string> GetKeysByModule(string module)
    {
        if (m_List != null && m_List.Count > 0)
        {
            List<string> retList = new List<string>();

            for (int i = 0; i < m_List.Count; i++)
            {
                if (!m_List[i].Module.Equals(module, StringComparison.CurrentCultureIgnoreCase)) continue;
                retList.Add(m_List[i].Key);
            }
            return retList;
        }
        return null;
    }
}