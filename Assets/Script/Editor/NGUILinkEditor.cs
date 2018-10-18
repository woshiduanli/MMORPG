using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

[CustomEditor(typeof(NGUILink))]
public class NGUILinkEditor : Editor
{
    [MenuItem("GameObject/AddGameObjectToLink &n", false, 10000)]
    private static void AddGameObjectToLink()
    {
        UnityEngine.GameObject select = Selection.activeGameObject;
        if (select == null)
            return;
        if (!select.activeInHierarchy)
        {
            Debug.LogError(string.Format("【NGUILink】自动添加对象失败：请先激活需要添加的对象和NGUILink对象", select.name), select);
            return;
        }

        NGUILink link = FindInParents<NGUILink>(select.transform);
        if (link == null)
        {
            Debug.LogError(string.Format("【NGUILink】自动添加对象失败：{0}父节点无NGUILink组件", select.name), select);
            return;
        }

        if (link.gameObject == select && select.transform.parent != null)
            link = FindInParents<NGUILink>(select.transform.parent);
        if (link == null)
        {
            Debug.LogError(string.Format("【NGUILink】自动添加对象失败：{0}父节点无NGUILink组件", select.name), select);
            return;
        }
        link.ReBuildLinkMap();
        //if (link.Get(select.name) == null)
        //{
        //    NGUILink.UILink item = new NGUILink.UILink();
        //    item.Name = select.name;
        //    item.LinkObj = select;
        //    link.Links.Add(item);
        //    Debug.Log(string.Format("【NGUILink】自动添加对象成功：{0}  NGUILink:{1}", select.name, link.name), link);
        //}
        //else
        //    Debug.LogError(string.Format("【NGUILink】自动添加对象失败：已经存在重复名字{0}的对象  NGUILink:{1}", select.name, link.name), link);
    }

    bool doOnce;
    int countTemp;
    string errostr = "Erro！！！Link丢失物件，检查";
    public override void OnInspectorGUI()
    {


        NGUILink link = target as NGUILink;
        SetUIDebug(link);
        GUI.changed = false;
        if (link.Links != null)
        {
            ModifyLink(link);
            RegisterUndo("NGUILink Change", link);
            for (int i = 0; i < link.Links.Count; i++)
            {
                NGUILink.UILink uilink = link.Links[i];
                if (uilink == null) continue;
                GameObject linkobj = uilink.LinkObj;
                if (!linkobj)
                {
                    uilink.Name = errostr;
                    continue;
                }
                if (linkobj)
                {
                    if (string.IsNullOrEmpty(uilink.Name) || uilink.Name == errostr)
                        uilink.Name = linkobj.name;
                    if (uilink.Name != linkobj.name)
                    {
                        uilink.Name = linkobj.name;
                    }
                    if (!uilink.component || uilink.component.gameObject != linkobj.gameObject)
                        uilink.component = linkobj.gameObject.GetComponent<MonoBehaviour>();
                }
            }
            EditorUtility.SetDirty(link);
        }

        base.OnInspectorGUI();
    }

    private static void SetUIDebug(NGUILink link)
    {
        CClientCommon.AddComponent<DebugUILine>(link.gameObject);
    }


    public static void GetImage(Transform tr, System.Action<Transform> action)
    {
        action(tr);
        if (tr.childCount <= 0) return;

        for (int i = 0; i < tr.childCount; i++)
        {
            GetImage(tr.GetChild(i), action);
        }
    }


    bool isFrist;
    private void ModifyLink(NGUILink link)
    {
        if (isFrist == false)
        {
            isFrist = true;

            UnityEngine.UI.Text[] texts = link.transform.GetComponentsInChildren<UnityEngine.UI.Text>();
            for (int i = 0; i < texts.Length; i++)
            {
                if (texts[i].gameObject.transform.parent.GetComponent<UnityEngine.UI.InputField>() == false)
                {
                    texts[i].raycastTarget = false;
                }
                else
                {
                    texts[i].raycastTarget = true;
                }
            }

            UnityEngine.UI.Image[] imges = link.transform.GetComponentsInChildren<UnityEngine.UI.Image>();
            for (int i = 0; i < imges.Length; i++)
            {

                if (!imges[i].gameObject.name.ToLower().StartsWith("btn"))
                {
                    imges[i].raycastTarget = false;
                }
                else
                {
                    imges[i].raycastTarget = true;
                }

                if (imges[i].gameObject.GetComponent<UnityEngine.UI.InputField>() == true)
                {
                    imges[i].raycastTarget = true;
                }
            }
        }

        for (int i = 0; i < link.Links.Count; i++)
        {
            // 修改对象的名字
            if (link.Links[i] != null && link.Links[i].LinkObj != null)
            {
                if (!link.Links[i].LinkObj.name.ToLower().StartsWith("image") &&
                    !link.Links[i].LinkObj.name.ToLower().StartsWith("text") &&
                    !link.Links[i].LinkObj.name.ToLower().StartsWith("btn") &&
                   !link.Links[i].LinkObj.name.ToLower().EndsWith("_lk"))
                {
                    link.Links[i].LinkObj.name = link.Links[i].LinkObj.name + "_lk";
                }
            }
        }

        if (!doOnce)
        {
            countTemp = link.Links.Count;
            doOnce = true;
        }
        //return;
        if (countTemp != link.Links.Count)
        {
            // 置空新link
            if (link.Links.Count > countTemp)
            {
                for (int i = countTemp; i < link.Links.Count; i++)
                    link.Links[i] = null;
            }

            // 去空格
            for (int i = 0; i < link.Links.Count; i++)
            {


                if (link.Links[i] != null && link.Links[i].LinkObj != null && link.Links[i].Name != null)
                {
                    link.Links[i].Name = link.Links[i].Name.Trim();
                    link.Links[i].LinkObj.name = link.Links[i].LinkObj.name.Trim();
                }
            }
            // 更新数量
            countTemp = link.Links.Count;
        }
    }



    static public void RegisterUndo(string name, params Object[] objects)
    {
        if (objects != null && objects.Length > 0)
        {
            UnityEditor.Undo.RecordObjects(objects, name);

            foreach (Object obj in objects)
            {
                if (obj == null) continue;
                EditorUtility.SetDirty(obj);
            }
        }
    }

    static public T FindInParents<T>(Transform trans) where T : Component
    {
        if (trans == null)
            return null;
        return trans.GetComponentInParent<T>();
    }
}