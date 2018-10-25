
using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(TextCom), true)]
public class TextComEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TextCom com = base.target as TextCom;

        int valueIndex = 0, index = 0;

        //模块
        valueIndex = LanguageDBModel.Instance.GetModules().IndexOf(com.Module);

        index = EditorGUILayout.Popup("模块", valueIndex, LanguageDBModel.Instance.GetModules().ToArray(), new GUILayoutOption[0]);
        if (valueIndex != index)
        {
            com.Module = LanguageDBModel.Instance.GetModules()[index];

            //通知面板 值改变了, 通知预制的值发生了改变
            EditorUtility.SetDirty(base.target);

            //通知Unity 改变了， 通知场景中的值发生了改变
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

            com.Refresh();
        }


        //Key
        valueIndex = LanguageDBModel.Instance.GetKeysByModule(com.Module).IndexOf(com.Key);

        index = EditorGUILayout.Popup("Key", valueIndex, LanguageDBModel.Instance.GetKeysByModule(com.Module).ToArray(), new GUILayoutOption[0]);
        if (valueIndex != index)
        {
            com.Key = LanguageDBModel.Instance.GetKeysByModule(com.Module)[index];

            //通知面板 值改变了
            EditorUtility.SetDirty(base.target);

            //通知Unity 改变了
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());

            com.Refresh();
        }
    }
}