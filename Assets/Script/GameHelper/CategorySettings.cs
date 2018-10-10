using System;
using UnityEngine;
using System.Collections.Generic;

public class CategoryObjectDestroyListener : MonoBehaviour {
    public Action<Transform,Transform> onDestroy = null;
    void OnDestroy() {
        if (onDestroy != null)
            onDestroy(transform.parent,transform);
        onDestroy = null;
    }
}

public static class CategorySettings
{
    #region Category Method
    //---------------------------------------------------------------------
    public static void Initialize(List<string> categoryList)
    {
        m_CategoryMap.Clear();
        for (int i = 0; i < categoryList.Count; ++i)
        {
            string category = categoryList[i];
            CreateObject(category);
        }

        GameObject UnknownObject = GameObject.Find("Unknowns");
        if (UnknownObject == null)
        {
            UnknownObject = new GameObject();
            UnityEngine.Object.DontDestroyOnLoad(UnknownObject);
            UnknownObject.name = "Unknowns";
        }

        m_UnknownParent = UnknownObject.transform;
        m_CategoryMap[m_UnknownParent.name] = m_UnknownParent;

        for (m_CategoryMap.Begin(); m_CategoryMap.Next();)
        {
            Transform categoryTrans = m_CategoryMap.Value;
            CClientCommon.NormalizeTransform(categoryTrans);
        }

        m_IsInitialize = true;
    }

    //---------------------------------------------------------------------
    public static void Shutdown()
    {
        m_IsInitialize = false;
        m_UnknownParent = null;
        m_CategoryMap.Clear();
    }

    //---------------------------------------------------------------------
    public static bool IsInitialize()
    {
        return m_IsInitialize;
    }

    //---------------------------------------------------------------------
    public static Transform GetObject(string categoryName)
    {
        if (!IsInitialize())
        {
            return null;
        }

        // Find path or name
        if (m_CategoryMap.ContainsKey(categoryName))
        {
            return m_CategoryMap[categoryName];
        }

        // Not Path
        if (categoryName.Contains("/"))
        {
            CreateObject(categoryName);
            return m_CategoryMap[categoryName];
        }

        // Find any contains the name
        for (m_CategoryMap.Begin(); m_CategoryMap.Next();)
        {
            if (m_CategoryMap.Key.Contains(categoryName))
                return m_CategoryMap.Value;
        }

        // Lookup unknown node
        Transform categoryObject = m_UnknownParent.Find(categoryName);
        if (categoryObject != null)
        {
            return categoryObject;
        }

        // Create to unknown child
        GameObject newObject = new GameObject();
        newObject.name = categoryName;
        CClientCommon.AttachChild(m_UnknownParent, newObject.transform, true);
        newObject.transform.position = Vector3.zero;
        newObject.transform.rotation = Quaternion.identity;
        CClientCommon.NormalizeTransform(newObject);

        return newObject.transform;
    }

    //---------------------------------------------------------------------
    public static bool Attach(Transform transform, string category)
    {
        return Attach(transform, category, true);
    }
   

   

    //---------------------------------------------------------------------
    public static bool Attach(Transform transform, string category, bool inheritLayer)
    {
        Transform categoryObject = GetObject(category);
        if (categoryObject == null)
        {
            return false;
        }

        CClientCommon.SaveTransform(transform);
        CClientCommon.AttachChild(categoryObject, transform, inheritLayer);
        CClientCommon.RevertTransform(transform);
        if (Application.isEditor)
            UpdateCategoryCount(transform,categoryObject, category);    
        return true;
    }


    //---------------------------------------------------------------------
    public static bool AttachToUnknown(Transform transform)
    {
        return Attach(transform, m_UnknownParent.name);
    }
    #endregion

    #region Internal Method
    //---------------------------------------------------------------------
    private static void CreateObject(string category)
    {
        if (category.Length == 0)
        {
            return;
        }

        string[] names = category.Split(':');
        if (names.Length == 1)
        {
            CreateObjectImpl(category);
        }
        else
        {
            GameObject categoryObject =
                CreateObjectImpl(names[0]);
            int layerId = LayerMask.NameToLayer(names[1]);
            if (layerId != -1 && categoryObject != null)
            {
                categoryObject.layer = layerId;
            }
        }
    }

    //---------------------------------------------------------------------
    private static GameObject CreateObjectImpl(string category)
    {
        string[] pathItems = category.Split('/');// 
        if (pathItems.Length == 0)
        {
            return null;
        }

        GameObject categoryObject = null;
        GameObject parentObject = null;
        string currentPath = "";
        for (int index = 0; index < pathItems.Length; ++index)
        {
            string itemName = pathItems[index];
            if (currentPath.Length == 0)
            {
                currentPath += itemName;
                Transform[] transforms = GameObject.FindObjectsOfType(
                    typeof(Transform)) as Transform[];
                for (int i = 0; i < transforms.Length; ++i)
                {
                    Transform child = transforms[i];
                    if (child.parent != null)
                    {
                        continue;
                    }

                    if (child.name == itemName)
                    {
                        categoryObject = child.gameObject;
                        break;
                    }
                }
            }
            else
            {
                currentPath += "/" + itemName;
                categoryObject = GameObject.Find(currentPath);
            }

            if (categoryObject == null)
            {
                categoryObject = new GameObject();
                categoryObject.name = itemName;

                if (parentObject != null)
                {
                    //CClientCommon.AttachChild(
                    //    parentObject.transform,
                    //    categoryObject.transform, true);
                }
                else
                {
                    //categoryObject.AddComponent<ImmortalComponent>();
                    UnityEngine.Object.DontDestroyOnLoad(categoryObject);
                }
            }

            parentObject = categoryObject;
        }

        if (categoryObject != null)
        {
            m_CategoryMap.Add(category, categoryObject.transform);
        }

        return categoryObject;
    }

    //---------------------------------------------------------------------
    private static void UpdateCategoryCount(Transform transform,Transform categoryObject, string category) {
        CategoryObjectDestroyListener listener = transform.gameObject.GetComponent<CategoryObjectDestroyListener>();
        if (listener)
            return;
        string tempName = string.Empty;
        string[] names = category.Split('/');
        if (!category.EndsWith("/"))
            tempName = names[names.Length - 1];
        else
            tempName = names[names.Length - 2];
        listener = transform.gameObject.AddComponent<CategoryObjectDestroyListener>();
        listener.onDestroy += delegate(Transform parent, Transform child) {
            if (!parent)
                return;
            string[] transNames = parent.name.Split('[');
            parent.name = CString.Format("{0} [{1}]", transNames[0].Trim()
                , parent.childCount-1);
        };
        categoryObject.name = CString.Format("{0} [{1}]", tempName, categoryObject.childCount);
    }
    #endregion

    #region Internal Member
    //---------------------------------------------------------------------
    private static bool m_IsInitialize = false;
    private static Transform m_UnknownParent = null;
    private static Map<string, Transform> m_CategoryMap = new Map<string, Transform>();
    #endregion
}

