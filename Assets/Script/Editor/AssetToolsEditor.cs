using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;
using System.IO;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Linq;

public static class AssetToolsEditor
{
    [MenuItem("Assets/删除manifest")]
    static void DeleteManifest()
    {
        string copypath = EditorUtility.OpenFolderPanel("DeleteManifest", Path.GetDirectoryName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");

        //string copypath = EditorUtility.SaveFolderPanel("CopyRes", Application.dataPath, "");
        if (string.IsNullOrEmpty(copypath))
            return;
        string[] files = Directory.GetFiles(copypath, "*.*", SearchOption.AllDirectories);
        for (int j = 0; j < files.Length; j++)
        {
            string child = files[j];
            child = Path.GetExtension(child);
            if (child == ".manifest" || string.IsNullOrEmpty(child))
            {
                string path = Path.GetFullPath(files[j]);
                File.Delete(path);
            }
        }
        AssetDatabase.Refresh();
    }

    //[MenuItem("Assets/工具/压缩资源/贴图尺寸重置")]
    //public static void ResetTextureres()
    //{
    //    EditorUtility.DisplayProgressBar("CompressAsset", "Searching Assets", 0);
    //    Object[] Assets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
    //    int i = 0;
    //    foreach (Object asset in Assets)
    //    {
    //        EditorUtility.DisplayProgressBar("CompressAsset", string.Format("{0}/{1}", i, Assets.Length), (float)i / (float)Assets.Length);
    //        i++;
    //        string path = AssetDatabase.GetAssetPath(asset);
    //        AssetImporter importer = AssetImporter.GetAtPath(path);
    //        if (importer is TextureImporter && asset is Texture2D)
    //            ResetTex(importer as TextureImporter);
    //        AssetDatabase.Refresh();
    //    }
    //    EditorUtility.ClearProgressBar();
    //}

    public static void ResetTex(TextureImporter textureImporter)
    {
        if (!textureImporter || textureImporter.textureType == TextureImporterType.Lightmap)
            return;

        ResetTextureSettings("Android", textureImporter);
        ResetTextureSettings("iPhone", textureImporter);
    }

    public static void ResetTextureSettings(string platform, TextureImporter textureImporter)
    {
        TextureImporterPlatformSettings PlatformSet = textureImporter.GetPlatformTextureSettings(platform);
        if (PlatformSet.overridden)
        {
            PlatformSet.overridden = false;
            textureImporter.SaveAndReimport();
            textureImporter.SetPlatformTextureSettings(PlatformSet);
        }
    }
    //--------------------------------------------------------------

    # region 场景整理

    [MenuItem("魔域/整理场景回滚")]
    private static void Reback()
    {
        if (!(Selection.activeObject is GameObject))
            return;
        GameObject go = Selection.activeObject as GameObject;
        Renderer[] renders = go.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer r in renders)
        {
            r.shadowCastingMode = ShadowCastingMode.On;
            r.receiveShadows = true;
            foreach (Material m in r.sharedMaterials)
            {
                if (!m || !m.shader)
                    continue;
                string shadername = m.shader.name;

                if (shadername == "MOYU/VertexLit")
                {
                    m.shader = Shader.Find("Legacy Shaders/Diffuse");
                    continue;
                }
                if (shadername == ("MOYU/AlphaTest"))
                {
                    m.shader = Shader.Find("Legacy Shaders/Transparent/Cutout/Diffuse");
                    continue;
                }
                if (shadername == ("MOYU/AlphaBlend"))
                {
                    m.shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
                    continue;
                }

                if (shadername == "MOYU/T4M4Textures")
                {
                    m.shader = Shader.Find("T4MShaders/T4M 4 Textures");
                    continue;
                }

                if (shadername.Contains("MOYU/"))
                    continue;

                if (shadername == ("Unlit/Texture"))
                    continue;

                if (shadername == "Particles/Additive (Soft)")
                    continue;
            }
        }
    }

    [MenuItem("魔域/整理场景配置")]
    public static void CheckRoot()
    {
        Transform root = (Selection.activeObject as GameObject).transform;
        if (root.tag != "root")
        {
            EditorUtility.DisplayDialog("Error", "请选择root检查", "OK");
            return;
        }
        root.name = "root";
        DoCheckShader(root);
        CheckMainCamera();
        string groups = "effect/mainbuilding/prop/terrain/Reflection/collider/level/animtion/audio/LightProbe/quality/fast/light/jump";
        List<string> rootGroups = new List<string>(groups.Split('/'));

        for (int i = 0; i < root.childCount; i++)
        {
            Transform child = root.GetChild(i);
            if (Camera.main.transform == child)
                continue;
            if (!rootGroups.Contains(child.name))
                Debug.LogError(string.Format("错误的打组{0} ", child.name), child);
            else
                DoCheckNode(child);
        }
        MissingScriptsEditor.CleanUpAsset(root.gameObject);
        EditorUtility.DisplayDialog("", "整理完毕!", "OK");
    }

    private static void DoCheckShader(Transform root)
    {
        Renderer[] renders = root.GetComponentsInChildren<Renderer>(true);
        foreach (Renderer r in renders)
        {
            r.shadowCastingMode = ShadowCastingMode.Off;
            r.receiveShadows = false;
            r.lightProbeUsage = LightProbeUsage.Off;
            r.reflectionProbeUsage = ReflectionProbeUsage.Off;
            r.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
            foreach (Material m in r.sharedMaterials)
            {
                if (!m || !m.shader || !m.HasProperty("_MainTex"))
                    continue;

                Texture tex = m.mainTexture;
                if (tex)
                {
                    string path = AssetDatabase.GetAssetPath(tex);
                    TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                    if (importer == null)
                    {
                        Debug.LogError(path + "    " + r.name);
                        continue;
                    }
                }

                string shadername = m.shader.name;

                if (shadername.EndsWith("/Additive") || shadername == "Particles/Additive")
                {
                    m.shader = Shader.Find("MOYU/Particles/Additive");
                    continue;
                }
                if (shadername.EndsWith("/AlphaBlended") || shadername == "Particles/Alpha Blended")
                {
                    m.shader = Shader.Find("MOYU/Particles/AlphaBlended");
                    continue;
                }
                if (shadername.Contains("T4M 4 Textures"))
                {
                    m.shader = Shader.Find("MOYU/T4M4Textures");
                    continue;
                }
                if (shadername.Contains("T4M 6 Textures"))
                {
                    m.shader = Shader.Find("MOYU/T4M6Textures");
                    continue;
                }

                if (shadername == ("Unlit/Texture"))
                    continue;

                if (shadername == "Particles/Additive (Soft)")
                    continue;

                if (shadername == "Legacy Shaders/Diffuse" || shadername == "Mobile/Diffuse")
                {
                    m.shader = Shader.Find("MOYU/VertexLit");
                    continue;
                }
                if ((shadername.StartsWith("Legacy Shaders")))
                {
                    if (shadername.Contains("Legacy Shaders/Transparent/Cutout"))
                        m.shader = Shader.Find("MOYU/AlphaTest");
                    else
                        m.shader = Shader.Find("MOYU/AlphaBlend");
                    continue;
                }

                if (shadername.Contains("MOYU/"))
                    continue;

                if (tex)
                {
                    string path = AssetDatabase.GetAssetPath(tex);
                    TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
                    if (importer != null && !importer.DoesSourceTextureHaveAlpha())
                        m.shader = Shader.Find("MOYU/VertexLit");
                }

                Debug.LogError(string.Format("无法识别的shader {0} ", shadername), r.gameObject);
                EditorUtility.DisplayDialog("", string.Format("无法识别的shader {0} {1}", shadername, r.gameObject.name), "OK");

                m.shader = Shader.Find("MOYU/VertexLit");
            }

            if (!r.enabled)
            {
                MeshFilter mf = r.gameObject.GetComponent<MeshFilter>();
                if (mf)
                    UnityEngine.Object.DestroyImmediate(mf);
                UnityEngine.Object.DestroyImmediate(r);
            }
        }

        RemoveAni(root.gameObject, false);
    }

    private static void RemoveAni(GameObject root, bool clear)
    {
        Animation[] anis = root.GetComponentsInChildren<Animation>(true);
        for (int i = 0; i < anis.Length; i++)
        {
            if (anis[i].GetClipCount() == 0 || clear)
                UnityEngine.Object.DestroyImmediate(anis[i]);
        }

        Animator[] Animators = root.GetComponentsInChildren<Animator>(true);
        for (int i = 0; i < Animators.Length; i++)
        {
            if (!Animators[i].runtimeAnimatorController || clear)
                UnityEngine.Object.DestroyImmediate(Animators[i]);
        }
    }

    private static void CheckMainCamera()
    {
        if (!Camera.main)
        {
            Debug.Log("没有设置主摄像机");
            return;
        }
        Camera.main.gameObject.layer = 19;
        RemoveScripts(Camera.main.gameObject);
    }

    private static void RemoveScripts(GameObject go)
    {
        MonoBehaviour[] components = go.GetComponents<MonoBehaviour>();
        for (int i = 0; i < components.Length; i++)
        {
            if (components[i] == null)
            {
                Debug.LogError(string.Format("the 'Main Camera' have missing component,please remove it."));
                continue;
            }

            if (components[i].ToString() == "CameraControll")
                UnityEngine.Object.DestroyImmediate(components[i], true);
            else if (components[i].ToString() == "T4MObjSC")
                UnityEngine.Object.DestroyImmediate(components[i], true);
        }
    }

    private static void DoCheckNode(Transform node)
    {
        switch (node.name)
        {
            case "effect":
                {
                    CheckEffectNode(node);
                }
                break;
            case "mainbuilding":
                {
                    RemoveAni(node.gameObject, true);
                    //RemoveCollider(node.gameObject);
                    RemoveAudio(node.gameObject);
                    CheckTagLayer(node, "Untagged", "MainBuilding");
                    //CheckWave(node);
                }
                break;
            case "prop":
                {
                    RemoveAni(node.gameObject, true);
                    //RemoveCollider(node.gameObject);
                    RemoveAudio(node.gameObject);
                    CheckTagLayer(node, "Untagged", "Prop");
                    //CheckWave(node);
                }
                break;
            case "terrain":
                {
                    RemoveAni(node.gameObject, true);
                    //RemoveCollider(node.gameObject);
                    RemoveAudio(node.gameObject);
                    CheckTagLayer(node, "Untagged", "Terrain");
                    //CheckWave(node);
                }
                break;
            case "Reflection":
                {
                    RemoveAni(node.gameObject, true);
                    //RemoveCollider(node.gameObject);
                    RemoveAudio(node.gameObject);
                    CheckTagLayer(node, "Untagged", "Reflection");

                }
                break;
            case "collider":
                {
                    Renderer[] renders = node.gameObject.GetComponentsInChildren<Renderer>(true);
                    for (int i = 0; i < renders.Length; i++)
                        UnityEngine.Object.DestroyImmediate(renders[i], true);

                    MeshFilter[] filters = node.gameObject.GetComponentsInChildren<MeshFilter>(true);
                    for (int i = 0; i < filters.Length; i++)
                        UnityEngine.Object.DestroyImmediate(filters[i], true);

                    CheckTagLayer(node, "Untagged", "Terrain");
                }
                break;
            case "level":
                {
                    node.tag = "Untagged";
                    for (int i = 0; i < node.childCount; i++)
                    {
                        Transform child = node.GetChild(i);
                        int value;
                        if (!int.TryParse(child.name, out value))
                            Debug.LogError(string.Format("关卡命名错误{0} ", child.name), child);

                        CheckTagLayer(child, "Untagged", "Prop");

                        child.gameObject.layer = LayerMask.NameToLayer("Prop");
                        child.tag = "AI";
                    }
                }
                break;
            case "LightProbe":
                {
                    RemoveAni(node.gameObject, true);

                    Renderer[] renders = node.gameObject.GetComponentsInChildren<Renderer>(true);
                    for (int i = 0; i < renders.Length; i++)
                        UnityEngine.Object.DestroyImmediate(renders[i], true);

                    MeshFilter[] filters = node.gameObject.GetComponentsInChildren<MeshFilter>(true);
                    for (int i = 0; i < filters.Length; i++)
                        UnityEngine.Object.DestroyImmediate(filters[i], true);

                    CheckTagLayer(node, "Untagged", "Probe");
                }
                break;
            case "fast":
                {
                    node.gameObject.tag = "Fast";
                }
                break;
            case "light":
                {
                    Light[] Lights = node.GetComponentsInChildren<Light>();
                    for (int i = 0; i < Lights.Length; i++)
                        Lights[i].gameObject.tag = "Light";
                }
                break;
            case "jump":
                {

                }
                break;
        }
    }

    private static void CheckWave(Transform node)
    {
        GameObject testwave = null;
        GameObject blendwave = null;

        if (node.GetChild(0).name == "WaveTree_Test" || node.GetChild(0).name == "WaveTree_Blend")
        {
            Transform testtf = node.GetChild(0);
            if (testtf)
            {
                Renderer[] Renderers = testtf.GetComponentsInChildren<Renderer>(true);
                foreach (Renderer r in Renderers)
                    r.sharedMaterial.shader = Shader.Find("MOYU/WaveTree/AlphaTest");
            }

            Transform blendtf = node.GetChild(1);
            if (blendtf)
            {
                Renderer[] Renderers = blendtf.GetComponentsInChildren<Renderer>(true);
                foreach (Renderer r in Renderers)
                    r.sharedMaterial.shader = Shader.Find("MOYU/WaveTree/AlphaBlend");
            }
        }
        else
        {
            Renderer[] Renderers = node.GetComponentsInChildren<Renderer>(true);
            foreach (Renderer r in Renderers)
            {
                if (r.sharedMaterial.shader.name.Contains("WaveTree/AlphaTest"))
                {
                    if (!testwave)
                    {
                        testwave = new GameObject("WaveTree_Test");
                        testwave.transform.parent = node;
                        testwave.transform.localPosition = Vector3.zero;
                        testwave.transform.localEulerAngles = Vector3.zero;
                        testwave.transform.localScale = Vector3.one;
                        testwave.transform.SetAsFirstSibling();
                    }
                    if (r.transform.parent.GetComponent<BuildingTranslucence>())
                        r.transform.parent.parent = testwave.transform;
                    else
                        r.transform.parent = testwave.transform;
                }

                if (r.sharedMaterial.shader.name.Contains("WaveTree/AlphaBlend"))
                {
                    if (!blendwave)
                    {
                        blendwave = new GameObject("WaveTree_Blend");
                        blendwave.transform.parent = node;
                        blendwave.transform.localPosition = Vector3.zero;
                        blendwave.transform.localEulerAngles = Vector3.zero;
                        blendwave.transform.localScale = Vector3.one;
                        blendwave.transform.SetSiblingIndex(1);
                    }
                    if (r.transform.parent.GetComponent<BuildingTranslucence>())
                        r.transform.parent.parent = blendwave.transform;
                    else
                        r.transform.parent = blendwave.transform;
                }
            }
        }
    }

    private static void CheckEffectNode(Transform node)
    {
        string groups = "prop/mainbuilding/terrain";
        List<string> rootGroups = new List<string>(groups.Split('/'));

        for (int i = 0; i < node.childCount; i++)
        {
            Transform child = node.GetChild(i);
            if (!rootGroups.Contains(child.name))
                Debug.LogError(string.Format("特效包含错误的打组{0} ", child.name), child);
            else
            {
                switch (node.name)
                {
                    case "prop":
                        {
                            CheckTagLayer(node, "Untagged", "Effect");
                        }
                        break;
                    case "mainbuilding":
                        {
                            CheckTagLayer(node, "Untagged", "MainBuilding");
                        }
                        break;
                    case "terrain":
                        {
                            CheckTagLayer(node, "Untagged", "Terrain");
                        }
                        break;
                }
            }
        }
    }

    private static void RemoveCollider(GameObject go)
    {
        Collider[] meshcolliders = go.GetComponentsInChildren<Collider>(true);
        for (int i = 0; i < meshcolliders.Length; i++)
            UnityEngine.Object.DestroyImmediate(meshcolliders[i], true);
    }

    private static void RemoveAudio(GameObject go)
    {
        AudioSource[] Audios = go.GetComponentsInChildren<AudioSource>(true);
        for (int i = 0; i < Audios.Length; i++)
            UnityEngine.Object.DestroyImmediate(Audios[i], true);
    }

    private static void CheckTagLayer(Transform node, string tag, string layer)
    {
        RemoveScripts(node.gameObject);

        node.gameObject.layer = LayerMask.NameToLayer(layer);
        node.tag = tag;

        Transform[] childs = node.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i].tag = tag;
            childs[i].gameObject.layer = node.gameObject.layer;
        }
    }

    #endregion

    //--------------------------------------------------------------

    #region 资源压缩
    [MenuItem("Assets/工具/压缩资源")]
    public static void CompressUnReadAssets()
    {
        EditorUtility.DisplayProgressBar("CompressAsset", "Searching Assets", 0);
        Object[] Assets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        int i = 0;
        foreach (Object asset in Assets)
        {
            EditorUtility.DisplayProgressBar("CompressAsset", string.Format("{0}/{1}", i, Assets.Length), (float)i / (float)Assets.Length);
            i++;
            CompressAsset(asset);
        }
        EditorUtility.ClearProgressBar();
    }

    public static void CompressAsset(Object asset)
    {
        string path = AssetDatabase.GetAssetPath(asset);
        AssetImporter importer = AssetImporter.GetAtPath(path);
        if (importer is TextureImporter && asset is Texture2D)
            CompressTex(importer as TextureImporter, path, asset as Texture2D, true);
        else if (importer is ModelImporter)
            CompressMesh(path, false);
        else if (importer is AudioImporter)
            CompressAudio(asset,path);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    public static void CompressTex(TextureImporter textureImporter, string path, Texture2D texture, bool mipmap)
    {
        TextureImporterPlatformSettings PlatformSet = textureImporter.GetPlatformTextureSettings("Android");
        if (PlatformSet.format == TextureImporterFormat.RGB16 || PlatformSet.format == TextureImporterFormat.RGBA16)
            return;
        if (textureImporter.textureType == TextureImporterType.Sprite)
            return;
#if UNITY_2017_4_OR_NEWER
        TextureImporterFormat texFormat = TextureImporterFormat.ETC_RGB4Crunched;
        if (textureImporter.DoesSourceTextureHaveAlpha())
            texFormat = TextureImporterFormat.ETC2_RGBA8Crunched;
#else

        TextureImporterFormat texFormat = TextureImporterFormat.ETC2_RGB4;
        if (textureImporter.DoesSourceTextureHaveAlpha())
            texFormat = TextureImporterFormat.ETC2_RGBA8;
#endif
        if (textureImporter.textureType == TextureImporterType.Default|| 
            textureImporter.textureType == TextureImporterType.NormalMap)
        {
            textureImporter.mipmapEnabled = mipmap;
            if (mipmap)
                textureImporter.mipmapFilter = TextureImporterMipFilter.KaiserFilter;
        }
        else
        {
            textureImporter.mipmapEnabled = false;
        }
        textureImporter.npotScale = TextureImporterNPOTScale.ToNearest;
        textureImporter.isReadable = false;
        textureImporter.filterMode = FilterMode.Bilinear;
        textureImporter.anisoLevel = 1;
        textureImporter.textureCompression = TextureImporterCompression.Compressed;

        SetPlatformTextureSettings("Android", texture, textureImporter, texFormat);
        SetPlatformTextureSettings("iPhone", texture, textureImporter, texFormat);
    }
    

    public static void SetPlatformTextureSettings(string platform, Texture2D texture,TextureImporter textureImporter, TextureImporterFormat format)
    {
        TextureImporterPlatformSettings PlatformSet = textureImporter.GetPlatformTextureSettings(platform);
        if (PlatformSet.format == TextureImporterFormat.RGB16 || PlatformSet.format == TextureImporterFormat.RGBA16)
            return;
        //if(PlatformSet.format== format)
        //    return;
        int size = Mathf.Max(texture.width, texture.height);
        PlatformSet.overridden = true;
        PlatformSet.compressionQuality = 70;
        if (texture.name.EndsWith("_n") || texture.name.EndsWith("_s") || texture.name.EndsWith("_m") || texture.name.EndsWith("_sd") || texture.name.EndsWith("_se"))
            PlatformSet.compressionQuality = 50;

        PlatformSet.maxTextureSize = size;
        PlatformSet.textureCompression = TextureImporterCompression.Compressed;
        PlatformSet.format = format;
        textureImporter.crunchedCompression = true;
        textureImporter.SaveAndReimport();
        textureImporter.SetPlatformTextureSettings(PlatformSet);
    }

    public static void CompressMesh(string path, bool isRead)
    {
        AssetImporter importer = AssetImporter.GetAtPath(path);
        if (importer is ModelImporter)
        {
            ModelImporter modelImporter = importer as ModelImporter;
            if (modelImporter.meshCompression == ModelImporterMeshCompression.Medium)
                return;
            modelImporter.optimizeMesh = true;
            modelImporter.isReadable = isRead;
            if (modelImporter.meshCompression == ModelImporterMeshCompression.Off)
                modelImporter.meshCompression = ModelImporterMeshCompression.Medium;
            AssetDatabase.ImportAsset(path);
            AssetDatabase.Refresh();
        }
    }

    public static void CompressAudio(Object asset,string path)
    {
        AssetImporter importer = AssetImporter.GetAtPath(path);
        if (importer is AudioImporter)
        {
            AudioClip audio = asset as AudioClip;
            AudioImporter audioImporter = importer as AudioImporter;
            audioImporter.forceToMono = true;
            audioImporter.loadInBackground = true;
            audioImporter.preloadAudioData = false;
            audioImporter.ambisonic = false;
            SetPlatformAudioSettings("Android", audioImporter, audio.length > 5);
            AssetDatabase.ImportAsset(path);
            AssetDatabase.Refresh();
        }
    }

    public static void SetPlatformAudioSettings(string platform , AudioImporter importer, bool big)
    {
        AudioImporterSampleSettings settings = importer.GetOverrideSampleSettings(platform);
        if (big)
        {
            settings.loadType = AudioClipLoadType.Streaming;
            settings.compressionFormat = AudioCompressionFormat.MP3;
            settings.sampleRateSetting = AudioSampleRateSetting.PreserveSampleRate;
            settings.quality = 0.3f;
        }
        else
        {
            settings.loadType = AudioClipLoadType.DecompressOnLoad;
            settings.compressionFormat = AudioCompressionFormat.ADPCM;
            settings.sampleRateSetting = AudioSampleRateSetting.OptimizeSampleRate;
        }
        importer.SetOverrideSampleSettings(platform, settings);
    }
    #endregion

    //------------------------------------------------------------------------------------------------------
    #region 场景打包
    [MenuItem("Assets/魔域/BuildSences")]
    static void BuildSences()
    {
        List<UnityEngine.Object> objects = new List<UnityEngine.Object>();
        objects.AddRange(Selection.objects);

        foreach (Object asset in objects)
            BuildSingleSence(asset, "Assets/StreamingAssets/res/scenes");

        Selection.objects = objects.ToArray();
    }

    public static void BuildSingleSence(Object asset, string outputPath)
    {
        List<AssetBundleBuild> buildMap = new List<AssetBundleBuild>();
        AssetImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(asset));
        importer.assetBundleName = asset.name.ToLower();
        importer.assetBundleVariant = "scene";
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        ExportGameObjects(importer, buildMap);
        BuildShaderInScene(buildMap);

        AssetBundleBuild ab1 = new AssetBundleBuild();
        ab1.assetBundleName = importer.assetBundleName;
        ab1.assetBundleVariant = importer.assetBundleVariant;
        ab1.assetNames = new string[] { importer.assetPath };
        buildMap.Add(ab1);

        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

#if UNITY_IPHONE || UNITY_IOS
        BuildPipeline.BuildAssetBundles(outputPath, buildMap.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
#else
        BuildPipeline.BuildAssetBundles(outputPath, buildMap.ToArray(), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
#endif
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

        AssetDatabase.DeleteAsset(SceneAssetsPath + "model");
    }

    static void BuildShaderInScene(List<AssetBundleBuild> buildMap)
    {
        Selection.activeObject = AssetDatabase.LoadAssetAtPath("Assets/Resources/Shader/MOYU", typeof(Object));
        //List<string> check = new List<string>();
        //check.AddRange(nomalsd);
        AssetBundleBuild ab = new AssetBundleBuild();
        ab.assetBundleName = "myshader";
        ab.assetBundleVariant = "sd";

        List<string> sdlist = new List<string>();
        Object[] shaders = Selection.GetFiltered(typeof(Shader), SelectionMode.DeepAssets);
        for (int i = 0; i < shaders.Length; i++)
        {
            string path = AssetDatabase.GetAssetPath(shaders[i]);
            AssetImporter sdimporter = AssetImporter.GetAtPath(path);
            sdimporter.assetBundleName = "myshader";
            sdimporter.assetBundleVariant = "sd";

            //string file = Path.GetFileNameWithoutExtension(path);
            //if (check.Contains(file))
            sdlist.Add(path);
        }
        AssetDatabase.Refresh();
        ab.assetNames = sdlist.ToArray();
        buildMap.Add(ab);
    }

    static void BuildTextureInScene(List<AssetBundleBuild> buildMap, AssetImporter importer, string outpath)
    {
        AssetBundleBuild ab = new AssetBundleBuild();
        ab.assetBundleName = outpath + importer.assetBundleName;
        ab.assetBundleVariant = importer.assetBundleVariant;
        ab.assetNames = new string[] { importer.assetPath };
        buildMap.Add(ab);
    }

    static string SceneAssetsPath = "Assets/StreamingAssets/res/scenes/";
    static List<Material> Materials = new List<Material>();
    static StreamWriter write;
    static List<string> texlist = new List<string>();

    static void ExportGameObjects(AssetImporter simporter, List<AssetBundleBuild> buildMap)
    {
        Materials.Clear();
        string sceneName = simporter.assetBundleName.ToLower();
        //获取当前场景完整路径
        Scene savescene = EditorSceneManager.OpenScene(simporter.assetPath);
        string scenePath = savescene.path;

        string[] dep = AssetDatabase.GetDependencies(scenePath, true);
        for (int i = 0; i < dep.Length; i++)
            CompressAsset(AssetDatabase.LoadAssetAtPath(dep[i], typeof(object)));

        for (int i = 0; i < dep.Length; i++)
        {
            AssetImporter importer = AssetImporter.GetAtPath(dep[i]);
            Object asset = AssetDatabase.LoadAssetAtPath(dep[i], typeof(object));
            if (importer is TextureImporter)
            {
                TextureImporter textureImporter = importer as TextureImporter;
                if (textureImporter.textureType == TextureImporterType.Lightmap)
                    continue;
                importer.assetBundleName = asset.name.ToLower();
                importer.assetBundleVariant = "tex";

                BuildTextureInScene(buildMap, importer, "texture/");
            }
        }

        AssetDatabase.Refresh();

        GameObject root = null;
        GameObject[] Roots = savescene.GetRootGameObjects();
        for (int i = 0; i < Roots.Length; i++)
        {
            if (Roots[i].name == "root")
            {
                root = Roots[i];
                break;
            }
        }

        if (!root)
        {
            EditorUtility.DisplayDialog("", sceneName + "没有root,请检查", "确定");
            return;
        }

        GameObject mainbuilding = null, terrain = null;
        List<Transform> others = new List<Transform>();
        foreach (Transform child in root.transform)
        {
            if (child.name == "terrain")
            {
                terrain = child.gameObject;
                continue;
            }
            if (child.name == "mainbuilding")
            {
                mainbuilding = child.gameObject;
                continue;
            }
            others.Add(child);
        }

        SceneConfig sceneConfig = root.GetComponent<SceneConfig>();
        if (!sceneConfig)
            sceneConfig = root.AddComponent<SceneConfig>();
        sceneConfig.Other.Clear();
        sceneConfig.Mainbuilding.Clear();

        string path = string.Format("{0}/txt", SceneAssetsPath);
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        string file = string.Format("{0}/{1}.txt", path, sceneName);
        write = File.CreateText(file);
        texlist.Clear();

        //地表资源terrain
        AddTerrain(sceneConfig, terrain, sceneName);
        //物件mainbuilding
        AddMainbuilding(sceneConfig, mainbuilding, sceneName);
        //其他物件
        foreach (Transform child in others)
            AddProp(sceneConfig, child.gameObject, sceneName);

        EditorSceneManager.SaveScene(savescene);
        EditorSceneManager.MarkAllScenesDirty();

        foreach (string t in texlist)
            write.WriteLine(t);
        write.Close();
        write.Dispose();
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }


    static void AddTerrain(SceneConfig sceneConfig, GameObject go, string sceneName)
    {
        if (!go)
        {
            Debug.LogError(sceneName + "没有terrain节点");
            return;
        }

        AddTerrainChildren(go, sceneName);

    }

    static void AddTerrainChildren(GameObject trans, string sceneName)
    {
       
        Transform[] childs = trans.GetComponentsInChildren<Transform>(true);
        //遍历场景中的所有物体
        foreach (Transform tf in childs)
        {
            Renderer renderer = tf.GetComponent<Renderer>();
            if (!renderer || !renderer.sharedMaterial)
                continue;

            string[] dep = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(renderer.sharedMaterial), true);
            for (int i = 0; i < dep.Length; i++)
            {
                AssetImporter importer = AssetImporter.GetAtPath(dep[i]);
                //Object asset = AssetDatabase.LoadAssetAtPath(dep[i], typeof(object));

                if (importer is TextureImporter)
                {
                    TextureImporter textureImporter = importer as TextureImporter;
                    if (textureImporter.textureType == TextureImporterType.Lightmap || textureImporter.textureShape == TextureImporterShape.TextureCube)
                        continue;
                    string texture = importer.assetBundleName + "." + importer.assetBundleVariant;
                    if (texlist.Contains(texture))
                        continue;
                    texlist.Add(texture);
                }
            }
        }
        texlist.Add("SceneLoad");
    }

    static void AddMainbuilding(SceneConfig sceneConfig, GameObject go, string sceneName)
    {
        if (!go)
        {
            Debug.LogError(sceneName + "没有mainbuilding节点");
            return;
        }

        AddChildren(sceneConfig.Mainbuilding, go);
    }


    static void AddProp(SceneConfig sceneConfig, GameObject go, string sceneNamet)
    {
        AddChildren(sceneConfig.Other, go);
    }

    //循环获取物件的子物体
    static void AddChildren(List<SceneConfig.AssetConfig> list, GameObject trans)
    {
        Transform[] childs = trans.GetComponentsInChildren<Transform>(true);
        //遍历场景中的所有物体
        foreach (Transform tf in childs)
        {
            Renderer renderer = tf.GetComponent<Renderer>();
            if (!renderer || !renderer.sharedMaterial || Materials.Contains(renderer.sharedMaterial))
                continue;

            List<TextureImporter> importers = new List<TextureImporter>();
            string[] dep = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(renderer.sharedMaterial), true);
            for (int i = 0; i < dep.Length; i++)
            {
                AssetImporter importer = AssetImporter.GetAtPath(dep[i]);
                //Object asset = AssetDatabase.LoadAssetAtPath(dep[i], typeof(object));

                if (importer is TextureImporter)
                {
                    TextureImporter textureImporter = importer as TextureImporter;
                    if (textureImporter.textureType == TextureImporterType.Lightmap || textureImporter.textureShape == TextureImporterShape.TextureCube)
                        continue;
                    importers.Add(importer as TextureImporter);
                }
            }
            if (importers.Count == 0)
            {
                Debug.LogError(tf.name, tf);
                continue;
            }
            //创建每个物体
            SceneConfig.AssetConfig assetConfig = new SceneConfig.AssetConfig();
            assetConfig.Configs = GetTextureConfigs(importers, renderer.sharedMaterial);
            assetConfig.Material = renderer.sharedMaterial;
            assetConfig.Shader = renderer.sharedMaterial.shader.name;
            list.Add(assetConfig);
            Materials.Add(renderer.sharedMaterial);
        }
    }

    public static List<SceneConfig.TextureConfig> GetTextureConfigs(List<TextureImporter> importers, Material mat)
    {
        List<SceneConfig.TextureConfig> Configs = new List<SceneConfig.TextureConfig>();
        foreach (var importer in importers)
        {
            SceneConfig.TextureConfig textureConfig = new SceneConfig.TextureConfig();
            textureConfig.Texture = importer.assetBundleName + "." + importer.assetBundleVariant;
            if (!texlist.Contains(textureConfig.Texture))
                texlist.Add(textureConfig.Texture);
            textureConfig.PropertyName = GetPropertyName(importer.assetPath, mat);
            Configs.Add(textureConfig);
        }
        return Configs;
    }

    private static string GetPropertyName(string path,Material mat)
    {
        Shader s = mat.shader;
        int count = UnityEditor.ShaderUtil.GetPropertyCount(s);
        for (int i = 0; i < count; i++)
        {
            string name = UnityEditor.ShaderUtil.GetPropertyName(s, i);
            UnityEditor.ShaderUtil.ShaderPropertyType propertyType = UnityEditor.ShaderUtil.GetPropertyType(s, i);
            if (propertyType != UnityEditor.ShaderUtil.ShaderPropertyType.TexEnv)
                continue;
            Texture tex = mat.GetTexture(name);
            if (tex && AssetDatabase.GetAssetPath(tex) == path)
                return name;
        }
        return string.Empty;
    }
    #endregion

    //public static string[] nomalsd = new string[] { "MYParticlesAdd", "MYParticlesBlend", "MYAlphaBlend","MYAlphaTestBumpDir","MYAlphaTestBumpPoint",
    //                                                "MYVertexLitBumpDir","MYVertexLitBumpPoint","MYAlphaTest", "MYPlayer","MYPlayerAlpha", "MYPlayerCombine",
    //                                                "MYVertexLit","MYSrcAdd", "MYSrcAddOff","MYParticlesAddOcclusion", "MYParticlesBlendOcclusion",
    //                                                "MYAlphaBlendWaveTree" ,"MYAlphaTestWaveTree"};
    [MenuItem("Assets/魔域/AddShaders")]
    static void AddShaders()
    {
        ShaderVariantCollection sv = AssetDatabase.LoadAssetAtPath("Assets/Resources/Shader/MOYU/MOYUShaders.shadervariants", typeof(ShaderVariantCollection)) as ShaderVariantCollection;

        List<string> included = new List<string>();
        included.Add("Hidden/CubeBlur");
        included.Add("Hidden/CubeCopy");
        included.Add("Hidden/CubeBlend");
        included.Add("Sprites/Default");
        included.Add("UI/Default");
        included.Add("Hidden/VideoDecode");
        included.Add("Hidden/VideoDecodeAndroid");
        included.Add("Mobile/Diffuse");
        included.Add("Mobile/VertexLit");
        included.Add("Skybox/6 Sided");
        included.Add("Legacy Shaders/Diffuse");
        included.Add("Legacy Shaders/Transparent/Cutout/Diffuse");
        included.Add("Legacy Shaders/Transparent/Diffuse");

        Selection.activeObject = AssetDatabase.LoadAssetAtPath("Assets/Resources/Shader", typeof(Object));
        Object[] shaders = Selection.GetFiltered(typeof(Shader), SelectionMode.DeepAssets);
        for (int i = 0; i < shaders.Length; i++)
        {
            Shader shader = shaders[i] as Shader;
            included.Add(shader.name);
            //string file = Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(shader));

            if (!sv)
                continue;
            ShaderVariantCollection.ShaderVariant sd = new ShaderVariantCollection.ShaderVariant();
            sd.shader = shader;
            sv.Add(sd);
        }

        SerializedObject graphicsSettings = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/GraphicsSettings.asset")[0]);
        SerializedProperty it = graphicsSettings.GetIterator();
        SerializedProperty dataPoint;
        while (it.NextVisible(true))
        {
            if (it.name == "m_AlwaysIncludedShaders")
            {
                it.ClearArray();

                for (int i = 0; i < included.Count; i++)
                {
                    it.InsertArrayElementAtIndex(i);
                    dataPoint = it.GetArrayElementAtIndex(i);
                    dataPoint.objectReferenceValue = Shader.Find(included[i]);
                }

                graphicsSettings.ApplyModifiedProperties();
            }
        }
    }

    [MenuItem("Assets/魔域/BuildShaders")]
    static void BuildShaders()
    {
        //List<string> check = new List<string>();
        //check.AddRange(nomalsd);

        Selection.activeObject = AssetDatabase.LoadAssetAtPath("Assets/MyResources/Shader/MOYU", typeof(Object));
        Object[] shaders = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        AssetBundleBuild ab = new AssetBundleBuild();
        ab.assetBundleName = "myshader";
        ab.assetBundleVariant = "sd";
        List<string> sdlist = new List<string>();
        for (int i = 0; i < shaders.Length; i++)
        {
            string path = AssetDatabase.GetAssetPath(shaders[i]);
            AssetImporter sdimporter = AssetImporter.GetAtPath(path);
            sdimporter.assetBundleName = "myshader";
            sdimporter.assetBundleVariant = "sd";
            //string file = Path.GetFileNameWithoutExtension(path);
            sdlist.Add(path);
        }
        AssetDatabase.Refresh();
        ab.assetNames = sdlist.ToArray();
        List<AssetBundleBuild> buildMap = new List<AssetBundleBuild>();
        buildMap.Add(ab);

        string outputPath = "Assets/StreamingAssets/res";

        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);

        BuildPipeline.BuildAssetBundles(outputPath, buildMap.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/魔域/BuildLuas")]
    static void BuildLuas()
    {
        Object[] Assets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object asset in Assets)
        {
            string path = AssetDatabase.GetAssetPath(asset);
            string ext = Path.GetExtension(path);
            if (ext != ".lua" && ext != ".txt")
                continue;
            string text = File.ReadAllText(path);
            if (ext != ".txt")
                path = Path.ChangeExtension(path, ".txt");
            StreamWriter write = File.CreateText(path);
            write.Write(text);
            write.Close();
            write.Dispose();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            BuildLua(AssetDatabase.LoadAssetAtPath<TextAsset>(path));
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("Assets/魔域/Buildtxt")]
    static void Buildtxts()
    {
        Object[] Assets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object asset in Assets)
        {
            string path = AssetDatabase.GetAssetPath(asset);
            string ext = Path.GetExtension(path);
            if (ext != ".lua" && ext != ".txt")
                continue;
            string text = File.ReadAllText(path);
            if (ext != ".txt")
                path = Path.ChangeExtension(path, ".txt");
            StreamWriter write = File.CreateText(path);
            write.Write(text);
            write.Close();
            write.Dispose();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            BuildLua(AssetDatabase.LoadAssetAtPath<TextAsset>(path));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }

    static void BuildLua(Object lua)
    {
        string path = AssetDatabase.GetAssetPath(lua);
        AssetImporter luaimporter = AssetImporter.GetAtPath(path);
        luaimporter.assetBundleName = lua.name;
        luaimporter.assetBundleVariant = "l";
        AssetDatabase.Refresh();

        List<AssetBundleBuild> buildMap = new List<AssetBundleBuild>();
        AssetBundleBuild ab = new AssetBundleBuild();
        ab.assetBundleName = luaimporter.assetBundleName;
        ab.assetBundleVariant = luaimporter.assetBundleVariant;
        ab.assetNames = new string[] { luaimporter.assetPath };
        buildMap.Add(ab);

        string outpath = Path.GetDirectoryName(path).Remove(0, "Assets/".Length);



        outpath = "Assets/StreamingAssets/res/" + outpath.ToLower();
        if (!Directory.Exists(outpath))
            Directory.CreateDirectory(outpath);

        BuildPipeline.BuildAssetBundles(outpath, buildMap.ToArray(), BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    static string ParentFolder = "Assets/NewSceneAssets";
    static string[] folders = new string[] { "Texture", "Material", "Audio", "Animation", "Model", "Prefab", "Other", "LightMap" };
    static Dictionary<string, string> PathMaps = new Dictionary<string, string>();

    [MenuItem("Assets/魔域/Root一键整理")]
    static void ClearupRootAssets()
    {
        if (!(Selection.activeObject is GameObject))
        {
            EditorUtility.DisplayDialog("", "请选择场景root", "OK");
            return;
        }

        if (!EditorUtility.DisplayDialog("", "确认整理" + Selection.activeObject.name, "OK"))
            return;

        if (!AssetDatabase.LoadAssetAtPath<Object>(ParentFolder))
            AssetDatabase.CreateFolder("Assets", "NewSceneAssets");

        PathMaps.Clear();
        for (int i = 0; i < folders.Length; i++)
            PathMaps[folders[i]] = CreateSingleFolder(folders[i], i < (folders.Length - 1));
        AssetDatabase.Refresh();

        string[] paths = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(Selection.activeObject));
        for (int i = 0; i < paths.Length; i++)
        {
            string path = paths[i];
            Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
            if (asset is Shader || asset is SceneAsset|| asset is Material)
                continue;
            string filename = Path.GetFileName(path);
            if (Path.GetExtension(filename) == ".cs" || Path.GetExtension(filename) == ".dll")
                continue;
            if (Path.GetExtension(filename) == ".exr" || filename.Contains("Lightmap") || filename.Contains("NavMesh") || filename.Contains("LightingData"))
                MoveAsset(path, PathMaps[folders[7]], filename);
            else if (asset is Texture || asset is Texture2D)
                MoveAsset(path, PathMaps[folders[0]], filename);
            else if (asset is Material)
                MoveAsset(path, PathMaps[folders[1]], filename);
            else if (asset is AudioClip)
                MoveAsset(path, PathMaps[folders[2]], filename);
            else if (asset is AnimationClip || asset is UnityEditor.Animations.AnimatorController)
                MoveAsset(path, PathMaps[folders[3]], filename);
            else
            {
                AssetImporter importer = AssetImporter.GetAtPath(path);
                if (importer is ModelImporter)
                    MoveAsset(path, PathMaps[folders[4]], filename);
                else if (asset is GameObject)
                    MoveAsset(path, PathMaps[folders[5]], filename);
                else
                    MoveAsset(path, PathMaps[folders[6]], filename);
            }
        }

        AssetDatabase.Refresh();
    }


    [MenuItem("Assets/魔域/场景一键整理")]
    static void ClearupSceneAssets()
    {
        if (!(Selection.activeObject is SceneAsset))
        {
            EditorUtility.DisplayDialog("", "请选择场景", "OK");
            return;
        }

        if (!AssetDatabase.LoadAssetAtPath<Object>(ParentFolder))
            AssetDatabase.CreateFolder("Assets", "NewSceneAssets");

        PathMaps.Clear();
        for (int i = 0; i < folders.Length; i++)
            PathMaps[folders[i]] = CreateSingleFolder(folders[i], i < (folders.Length - 1));
        AssetDatabase.Refresh();

        string[] paths = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath(Selection.activeObject));
        for (int i = 0; i < paths.Length; i++)
        {
            string path = paths[i];
            Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
            if (asset is Shader || asset is SceneAsset)
                continue;
            string filename = Path.GetFileName(path);
            if (Path.GetExtension(filename) == ".cs")
                continue;
            if (Path.GetExtension(filename) == ".dll")
                continue;
            if (Path.GetExtension(filename) == ".exr" || filename.Contains("Lightmap") || filename.Contains("NavMesh") || filename.Contains("LightingData"))
                MoveAsset(path, PathMaps[folders[7]], filename);
            else if (asset is Texture || asset is Texture2D)
                MoveAsset(path, PathMaps[folders[0]], filename);
            else if (asset is Material)
                MoveAsset(path, PathMaps[folders[1]], filename);
            else if (asset is AudioClip)
                MoveAsset(path, PathMaps[folders[2]], filename);
            else if (asset is AnimationClip || asset is UnityEditor.Animations.AnimatorController)
                MoveAsset(path, PathMaps[folders[3]], filename);
            else
            {
                AssetImporter importer = AssetImporter.GetAtPath(path);
                if (importer is ModelImporter)
                    MoveAsset(path, PathMaps[folders[4]], filename);
                else if (asset is GameObject)
                    MoveAsset(path, PathMaps[folders[5]], filename);
                else
                    MoveAsset(path, PathMaps[folders[6]], filename);
            }
        }

        AssetDatabase.Refresh();
    }

    private static void MoveAsset(string path, string newpath, string filename)
    {
        if (filename.StartsWith("go_"))
            AssetDatabase.MoveAsset(path, string.Format("{0}/{1}/{2}", newpath, "Effect", filename));
        else
            AssetDatabase.MoveAsset(path, Path.Combine(newpath, filename));
    }

    private static string CreateSingleFolder(string name, bool effect = true)
    {
        if (!AssetDatabase.LoadAssetAtPath<Object>(Path.Combine(ParentFolder, name)))
            AssetDatabase.CreateFolder(ParentFolder, name);

        string path = string.Format("{0}/{1}/{2}", ParentFolder, name, Selection.activeObject.name);

        if (!AssetDatabase.LoadAssetAtPath<Object>(path))
            AssetDatabase.CreateFolder(Path.Combine(ParentFolder, name), Selection.activeObject.name);

        if (effect && !AssetDatabase.LoadAssetAtPath<Object>(Path.Combine(path, "Effect")))
            AssetDatabase.CreateFolder(path, "Effect");

        return path;
    }

    public class ResAsset
    {
        public string Path;
        public string AssetType;
        public string Help;
        public bool folder;

    }

    [MenuItem("Assets/魔域/Res资源清单检查")]
    static void CheckResAssets()
    {
        List<ResAsset> includes = new List<ResAsset>();

        ResAsset asset= CreateResAsset("Assets/StreamingAssets/res/action", "action资源", "",true); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/audio", "音效资源", "", true); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/db", "策划配置表资源", "", true); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/effect", "特效资源", "", true); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/language", "语言包资源", "", true); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/loading_pic", "过图图片资源", "", true); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/lua", "lua代码资源", "", true); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/model", "模型资源", "", true); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/role", "Role资源", "", true); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/scenes", "场景资源", "", true); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/timeline", "TimeLine资源", "", true); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/txt", "txt资源", "", true); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/ui/font", "UI字体资源", "", true); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/ui/tex", "UI图片资源", "", true); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/ui/uiprefab", "UI资源", "", true); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/ui/uiroot.go", "UIRoot资源", ""); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/controllers.ctrl", "角色控制器资源", ""); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/devices.txt", "手机适配txt资源", ""); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/launcher.l", "启动配置资源", ""); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/myshader.sd", "shader资源", ""); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/normals.go", "角色法线资源包资源", ""); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/pc_myshader.sd", "PC shader资源", ""); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/public.go", "通用图片包资源", ""); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/serverlist.txt", "服务器列表资源", ""); includes.Add(asset);
        asset = CreateResAsset("Assets/StreamingAssets/res/textures.go", "角色贴图资源包资源", ""); includes.Add(asset);

        foreach (var ass in includes)
        {
            Object obj = AssetDatabase.LoadAssetAtPath(ass.Path, typeof(Object));
            if (!obj)
            {
                EditorUtility.DisplayDialog("", "缺少" + ass.AssetType, "OK");
                return;
            }
            if (ass.folder)
            {
                string[] objs = System.IO.Directory.GetFiles(ass.Path);
                if (objs.Length == 0)
                {
                    EditorUtility.DisplayDialog("", "缺少" + ass.AssetType, "OK");
                    return;
                }
            }
        }
         EditorUtility.DisplayDialog("", "资源检查完成，一个不少", "OK");

    }

    static ResAsset CreateResAsset(string p,string t,string h,bool f=false)
    {
        ResAsset asset = new ResAsset();
        asset.AssetType = t;
        asset.Path = p;
        asset.Help = h;
        asset.folder = f;
        return asset;
    }
}

public class MissingScriptsEditor : EditorWindow
{
    private static EditorWindow window;

    [MenuItem("Custom/MissingScripteEditor")]
    private static void Execute()
    {
        if (window == null)
            window = (MissingScriptsEditor)GetWindow(typeof(MissingScriptsEditor));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical("box");
        if (GUILayout.Button("查找丢失脚本", GUILayout.Height(30f)))
        {
            CheckMissScripts();
        }
        if (GUILayout.Button("删除丢失脚本", GUILayout.Height(30f)))
        {
            CleanUpSelection();
        }

        GUILayout.EndVertical();
    }

    private void CleanUpSelection()
    {
        EditorUtility.DisplayProgressBar("Checking", "逐个分析中，请勿退出！", 0);
        var lstSelection = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        for (int i = 0; i < lstSelection.Length; ++i)
        {
            EditorUtility.DisplayProgressBar("Checking", "逐个分析中，请勿退出！", (float)i / (float)lstSelection.Length);
            if (!(lstSelection[i] is GameObject))
                continue;
            var gameObject = lstSelection[i] as GameObject;
            var Transforms = gameObject.GetComponentsInChildren<Transform>(true);

            for (int j = 0; j < Transforms.Length; j++)
            {
                CleanUpAsset(Transforms[j].gameObject);
            }
        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static void CleanUpAsset(GameObject go)
    {
        // 创建一个序列化对象
        SerializedObject serializedObject = new SerializedObject(go);
        // 获取组件列表属性
        SerializedProperty prop = serializedObject.FindProperty("m_Component");

        var components = go.GetComponents<Component>();
        int r = 0;
        for (int j = 0; j < components.Length; j++)
        {
            // 如果组建为null
            if (components[j] == null)
            {
                // 按索引删除
                prop.DeleteArrayElementAtIndex(j - r);
                r++;
            }
        }

        // 应用修改到对象
        serializedObject.ApplyModifiedProperties();
    }


    public static void CheckMissScripts()
    {
        EditorUtility.DisplayProgressBar("Checking", "逐个分析中，请勿退出！", 0);
        Object[] Assets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        int i = 0;
        foreach (Object go in Assets)
        {
            EditorUtility.DisplayProgressBar("Checking", "逐个分析中，请勿退出！", (float)i / (float)Assets.Length);
            if (go is GameObject)
                CheckMissScript(go as GameObject);
            i++;
        }
        EditorUtility.ClearProgressBar();
    }

    public static void CheckMissScript(GameObject parent)
    {
        Transform[] gos = parent.GetComponentsInChildren<Transform>(true);
        for (int j = 0; j < gos.Length; j++)
        {
            Transform go = gos[j];
            MonoBehaviour[] monos = go.GetComponents<MonoBehaviour>();

            for (int i = 0; i < monos.Length; i++)
            {
                if (!monos[i])
                {
                    Debug.LogError(string.Format("{0} 有脚本丢失", parent.name + "/" + go.name), parent);
                }
            }
        }
    }
}
