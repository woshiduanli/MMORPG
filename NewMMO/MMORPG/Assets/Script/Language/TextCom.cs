
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class TextCom : MonoBehaviour
{
    /// <summary>
    /// 模块
    /// </summary>
    [HideInInspector]
    public string Module;

    /// <summary>
    /// Key
    /// </summary>
    [HideInInspector]
    public string Key;

    public void Refresh()
    {
        if (string.IsNullOrEmpty(Module) || string.IsNullOrEmpty(Key)) return;
        Text text = GetComponent<Text>();
        text.text = LanguageDBModel.Instance.GetText(Module, Key);
    }
}