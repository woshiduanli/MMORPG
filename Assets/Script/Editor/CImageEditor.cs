using UnityEngine;
using UnityEngine.UI;
using UnityEditor.UI;
using UnityEditor;

[CustomEditor(typeof(CImage))]
public class CImageEditor : ImageEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CImage image = target as CImage;
        GUI.changed = false;
        if (image.sprite != null)
        {
            NGUILinkEditor.RegisterUndo("CImage Change", image);
            if (image.sprite)
            {
                image.SpriteName = image.sprite.name.ToLower();
                image.AtlasName = image.sprite.texture.name.ToLower();
            }
        }

        // 赋值材质球
        SetMat(image);
        EditorGUILayout.TextField("Atlas", image.AtlasName);
        EditorGUILayout.TextField("Sprite", image.SpriteName);
        if (GUILayout.Button("材质球赋值"))
        {
            if (!string.IsNullOrEmpty(image.AtlasName) && image.material != null && image.material.name != image.AtlasName)
            {
                Material mat = AssetDatabase.LoadAssetAtPath("Assets/MyResources/UI/Textures/material/" + image.AtlasName + "mat.mat", typeof(Material)) as Material;
                if (mat != null)
                    image.material = mat;
            }
        }
        EditorUtility.SetDirty(image);
    }

    private static void SetMat(CImage image)
    {
        if (!string.IsNullOrEmpty(image.AtlasName) && image.material != null && image.material.name == "Default UI Material")
        {
            Material mat = AssetDatabase.LoadAssetAtPath("Assets/Resources/UI/Textures/material/" + image.AtlasName + "mat.mat", typeof(Material)) as Material;
            if (mat != null)
                image.material = mat;
        }
    }

    [MenuItem("GameObject/UI/CImage")]
    private static void AddCImage()
    {
        UnityEngine.GameObject select = new GameObject("CImage");
        select.AddComponent<CImage>();
        UnityEngine.GameObject parent = Selection.activeGameObject;
        if (!parent)
            parent = new GameObject("Canvas");
        select.transform.SetParent(parent.transform);

        Canvas canvas = parent.transform.root.GetComponentInParent<Canvas>();
        if (!canvas)
        {
            canvas = parent.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            CanvasScaler canvasScaler = parent.AddComponent<CanvasScaler>();
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1280, 720);
            canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
            parent.AddComponent<GraphicRaycaster>();

        }
    }
}
