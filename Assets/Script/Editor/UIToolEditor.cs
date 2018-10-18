using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Events;
using System.IO;
using System.Text;

public class UIToolEditor
{

    [MenuItem("Assets/UI相关/分离图集alpha通道/新的图集")]
    public static void CreateAlphaSpriteTex()
    {
        if (EditorUtility.DisplayDialog("必须是新图集", "必须是新图集", "新图集", "取消"))
            CreateAlpha(FlayAlpha.SpriteAlpha);
    }

    [MenuItem("Assets/UI相关/分离图集alpha通道/老的图集")]
    public static void CreateOldAlphaSpriteTex()
    {
        Object[] Assets = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
        if (Assets == null) return;
        for (int i = 0; i < Assets.Length; i++)
        {
            UnityEngine.Object temp = Assets[i];
            if (temp is Texture)
            {
                TextureImporter texImport = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(temp)) as TextureImporter;
                if (!texImport.DoesSourceTextureHaveAlpha())
                {
                    Debug.Log(temp.name + ":没有a通道");
                    continue;
                }
                CreateAlphaTex(temp as Texture, "Assets/" + temp.name + ".png", "Assets/" + temp.name + "_alpha.png", string.Empty, "Assets/" + temp.name + "_alpha.png", true);
            }
        }
    }

    [MenuItem("Assets/UI相关/分离背景图alpha通道")]
    public static void CreateAlphaBgTex()
    {
        CreateAlpha(FlayAlpha.BgAlpha);
    }

    public static void CreateDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
    enum FlayAlpha
    {
        BgAlpha = 0,
        SpriteAlpha = 1
    }
    private static void CreateAlpha(FlayAlpha enumFlagAlpha)
    {
        string alphaFolderPath = "";
        string RgbFolderPath = "";
        string MaterialsFolderPathReal = "";
        string MaterialsFolderPath = "";
        string alphaAssetPath = "";
        bool ui = true;
        if (enumFlagAlpha == FlayAlpha.BgAlpha)
        {
            ui = false;
            alphaFolderPath = CString.Concat(Application.dataPath, "/MyResources/UI/RawImage/Alpha/");
            RgbFolderPath = CString.Concat(Application.dataPath, "/MyResources/UI/RawImage/");
            MaterialsFolderPathReal = CString.Concat(Application.dataPath, "/MyResources/UI/RawImage/Material/");
            MaterialsFolderPath = CString.Concat("Assets/MyResources/UI/RawImage/Material/");
            alphaAssetPath = "Assets/MyResources/UI/RawImage/Alpha/";
        }
        else if (enumFlagAlpha == FlayAlpha.SpriteAlpha)
        {
            ui = true;
            alphaFolderPath = CString.Concat(Application.dataPath, "/MyResources/UI/Textures/Alpha/");
            RgbFolderPath = CString.Concat(Application.dataPath, "/MyResources/UI/Textures/");
            MaterialsFolderPathReal = CString.Concat(Application.dataPath, "/MyResources/UI/Textures/Material/");
            MaterialsFolderPath = CString.Concat("Assets/MyResources/UI/Textures/Material/");
            alphaAssetPath = "Assets/MyResources/UI/Textures/Alpha/";
        }

        CreateDirectory(alphaFolderPath);
        CreateDirectory(RgbFolderPath);
        CreateDirectory(MaterialsFolderPathReal);

        Object[] Assets = Selection.GetFiltered(typeof(Texture), SelectionMode.DeepAssets);
        if (Assets == null) return;
        for (int i = 0; i < Assets.Length; i++)
        {
            UnityEngine.Object temp = Assets[i];
            if (temp is Texture)
            {
                string ppp = Path.GetDirectoryName(AssetDatabase.GetAssetPath(temp));
                string[] Folders = ppp.Split('/');
                string folder = Folders[Folders.Length - 1] + "/";
                if (RgbFolderPath.EndsWith(folder))
                    folder = string.Empty;

                TextureImporter texImport = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(temp)) as TextureImporter;
                if (!texImport.DoesSourceTextureHaveAlpha())
                {
                    Debug.Log(temp.name + ":没有a通道");
                    continue;
                }

                CreateAlphaTex(temp as Texture,
                 RgbFolderPath + folder + temp.name + ".png",
                 alphaFolderPath + folder + temp.name + "_alpha" + ".png",
                 MaterialsFolderPath + folder + temp.name + "mat.mat",
                 alphaAssetPath + folder + temp.name + "_alpha.png", ui);
            }

        }
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    public static void CreateAlphaTex(Texture obj, string rgbPath, string aPath, string matPath, string alphaAssetPath, bool ui)
    {
        Texture2D tex = (Texture2D)obj;
        if (tex == null) return;
        string texturePath = AssetDatabase.GetAssetPath(obj);
        TextureImporter importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
        importer.isReadable = true;
        importer.SaveAndReimport();
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        AssetDatabase.SaveAssets();

        //保存alpha通道 left to right, bottom to top
        Color32[] colorArray = tex.GetPixels32();

        for (int i = 0; i < colorArray.Length; ++i)
        {
            colorArray[i].r = colorArray[i].a;
            colorArray[i].g = colorArray[i].a;
            colorArray[i].b = colorArray[i].a;
        }
        Texture2D alphaTex = new Texture2D(tex.width, tex.height, TextureFormat.RGB24, false, true);
        alphaTex.SetPixels32(colorArray);
        alphaTex.Apply();
        byte[] data = alphaTex.EncodeToPNG();
        File.WriteAllBytes(aPath, data);
        data = null;

        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        AssetDatabase.SaveAssets();

        importer = AssetImporter.GetAtPath(alphaAssetPath) as TextureImporter;
        CompressTex(importer, alphaAssetPath, AssetDatabase.LoadAssetAtPath<Texture2D>(alphaAssetPath), false);

        //保存rgb
        Texture2D rgbTex = new Texture2D(tex.width, tex.height, TextureFormat.RGB24, false);
        Color32[] newColorArray = tex.GetPixels32();
        rgbTex.SetPixels32(newColorArray);
        rgbTex.Apply();
        byte[] rgbData = rgbTex.EncodeToPNG();

        //AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(obj));
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        AssetDatabase.SaveAssets();


        File.WriteAllBytes(rgbPath, rgbData);

        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        AssetDatabase.SaveAssets();

        if (string.IsNullOrEmpty(matPath))
            return;

        rgbData = null;
        importer = AssetImporter.GetAtPath(texturePath) as TextureImporter;
        CompressTex(importer, texturePath, AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath), ui);
        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        AssetDatabase.SaveAssets();

        Texture2D ALPHA = AssetDatabase.LoadAssetAtPath(alphaAssetPath, typeof(Texture2D)) as Texture2D;
        if (!ALPHA)
            return;
        // 创建材质并赋值a图
        Material tempMat = AssetDatabase.LoadAssetAtPath(matPath, typeof(Material)) as Material;
        if (tempMat == null)
        {
            tempMat = new Material(Shader.Find("MOYU/UI/Default"));
            string floder = Path.GetDirectoryName(matPath);
            if (!Directory.Exists(floder))
                Directory.CreateDirectory(floder);
            AssetDatabase.CreateAsset(tempMat, matPath);
            AssetDatabase.Refresh();
            AssetDatabase.SaveAssets();
        }
        tempMat.shader = Shader.Find("MOYU/UI/Default");
        if (tempMat != null)
        {
            if (tempMat.HasProperty("_AlphaTex"))
                tempMat.SetTexture("_AlphaTex", ALPHA);
        }

        AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        AssetDatabase.SaveAssets();

    }

    public static void CompressTex(TextureImporter textureImporter, string path, Texture2D texture, bool ui)
    {
        if (!ui)
            textureImporter.textureType = TextureImporterType.Default;
        else
            textureImporter.textureType = TextureImporterType.Sprite;
        TextureImporterFormat texFormat = TextureImporterFormat.ETC_RGB4Crunched;
        if (textureImporter.DoesSourceTextureHaveAlpha())
            texFormat = TextureImporterFormat.ETC2_RGBA8Crunched;
        textureImporter.mipmapEnabled = false;
        textureImporter.isReadable = false;
        textureImporter.filterMode = FilterMode.Bilinear;
        textureImporter.npotScale = TextureImporterNPOTScale.ToNearest;
        textureImporter.anisoLevel = 1;

        textureImporter.textureCompression = TextureImporterCompression.Compressed;
        int size = Mathf.Max(texture.height, texture.width);

        SetPlatformTextureSettings("Android", size, textureImporter, texFormat);
        SetPlatformTextureSettings("iPhone", size, textureImporter, texFormat);
        AssetDatabase.ImportAsset(path);
    }

    public static void SetPlatformTextureSettings(string platform, int size, TextureImporter textureImporter, TextureImporterFormat format)
    {
        TextureImporterPlatformSettings PlatformSet = textureImporter.GetPlatformTextureSettings(platform);
        PlatformSet.overridden = true;
        PlatformSet.compressionQuality = 90;
        PlatformSet.maxTextureSize = size;
        PlatformSet.textureCompression = TextureImporterCompression.Compressed;
        PlatformSet.format = format;
        textureImporter.crunchedCompression = true;
        textureImporter.SetPlatformTextureSettings(PlatformSet);
    }

    [MenuItem("Assets/UI相关/UI打包")]
    private static void BuildUI()
    {
        Object[] Assets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object asset in Assets)
        {
            if (asset is GameObject)
                BuildUI(asset as GameObject);
        }
    }

    [MenuItem("Assets/UI相关/我的测试")]
    public static void BuildAsset2()
    {

        UnityEngine.Object[] objArr = Selection.objects;
        if (objArr == null) return;
        if (!(objArr[0] is GameObject)) return;
        BuildAsset(objArr[0], "ui", "Assets/StreamingAssets/res/ui/uiprefab");
    }

    // 如果是背景图,只压缩， 
    // 如果是图集， 和材质打包在一起

    // 如果是shader，打包出来
    // 如果是字体， 打包出来

    static void BuildUI(GameObject go)
    {
        // 重新生成hashId和脚本

        string uipath = AssetDatabase.GetAssetPath(go);
        if (Path.GetDirectoryName(uipath) != "Assets/MyResources/UIPrefab")
            return;

        //MOYU_UIToolsEditor.CreateNguiLinkValue2(go);

        //GameObject clone = Object.Instantiate(go) as GameObject;
        //PrefabUtility.ReplacePrefab(clone, go);
        //UnityEngine.Object.DestroyImmediate(clone, true);

        //AssetDatabase.Refresh();
        //AssetDatabase.SaveAssets();


        MOYU_UIToolsEditor.CreateUILuaScript2(go);
        CClientCommon.RemoveComponent<DebugUILine>(go);


        GameObject UI = go as GameObject;
        UICacheComponents uICache = CClientCommon.AddComponent<UICacheComponents>(UI);
        uICache.Buttons.Clear();
        uICache.Toggles.Clear();
        uICache.CTexts.Clear();

        uICache.Buttons.AddRange(go.GetComponentsInChildren<Button>(true));
        uICache.Toggles.AddRange(go.GetComponentsInChildren<Toggle>(true));
        uICache.CTexts.AddRange(go.GetComponentsInChildren<CText>(true));

        NGUILink[] links = UI.GetComponentsInChildren<NGUILink>(true);
        //foreach (var l in links)
        //    l.Root = go;

        string path = "Assets/MyResources/UIPrefab/" + go.name + ".txt";
        StreamWriter write = File.CreateText(path);

        string[] dep = AssetDatabase.GetDependencies(uipath, true);

        Dictionary<string, List<Object>> assetdic = new Dictionary<string, List<Object>>();
        List<Texture> Texs = new List<Texture>();
        List<Object> Shaders = new List<Object>();
        List<Object> Fonts = new List<Object>();
        for (int i = 0; i < dep.Length; i++)
        {
            MyDebug.debug("DE:" + dep[i]);
            AssetImporter importer = AssetImporter.GetAtPath(dep[i]);
            string rootpath = Path.GetDirectoryName(dep[i]);
            Object asset = AssetDatabase.LoadAssetAtPath(dep[i], typeof(object));
            if (asset is Texture && !asset.name.EndsWith("_alpha"))
            {
                if (rootpath == "Assets/MyResources/UI/Textures" || rootpath.Contains("MyResources/UI/RawImage"))
                    Texs.Add(asset as Texture);
            }
            if (asset is Shader && !Shaders.Contains(asset))
            {
                Shaders.Add(asset as Shader);
            }
            if (asset is Font)
            {
                Fonts.Add(asset);
            }
        }
        foreach (var m in Texs)
        {
            List<Object> assetlist = null;
            assetdic.TryGetValue(m.name, out assetlist);
            if (assetlist == null)
            {
                assetlist = new List<Object>();
                assetdic[m.name] = assetlist;
            }
            CompressUItex(m);
            assetlist.Add(m);

            Material mat = GetMat(m);
            if (mat)
            {
                if (!Shaders.Contains(mat.shader))
                    Shaders.Add(mat.shader);
                assetlist.Add(mat);
                CompressUItex(mat.GetTexture("_AlphaTex"));
                //assetlist.Add(mat.GetTexture("_AlphaTex"));
            }
        }

        List<AssetBundleBuild> buildMap = new List<AssetBundleBuild>();
        foreach (var f in Fonts)
        {
            buildMap.Add(CreateAssetBundle(f, f.name, "ft", "font/"));
            if (f.name == "uifont" || f.name == "uifont_title")
                continue;
            write.Write(string.Format("res/ui/font/{0}.ft\n", f.name).ToLower());
        }

        foreach (var kvp in assetdic)
        {
            buildMap.Add(CreateAssetBundle(kvp.Value, kvp.Key, "tex", "tex/"));
            write.Write(string.Format("res/ui/tex/{0}.tex\n", kvp.Key).ToLower());
        }

        write.Close();
        write.Dispose();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        buildMap.Add(CreateAssetBundle(Shaders, "myshader", "sd", ""));
        buildMap.Add(CreateAssetBundle(go, go.name.ToLower(), "ui", "uiprefab/"));

        BuildAsset("Assets/StreamingAssets/res/ui", buildMap.ToArray());

        BuildAsset(AssetDatabase.LoadAssetAtPath<TextAsset>(path), "cfg", "Assets/StreamingAssets/res/ui/manifest");
        AssetDatabase.DeleteAsset(path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public static AssetBundleBuild CreateAssetBundle(Object asset, string BundleName, string Variant, string outpath)
    {
        AssetBundleBuild publicab = new AssetBundleBuild();
        publicab.assetBundleName = outpath + BundleName;
        publicab.assetBundleVariant = Variant;
        List<string> publiclist = new List<string>();

        string path = AssetDatabase.GetAssetPath(asset);
        AssetImporter sdimporter = AssetImporter.GetAtPath(path);
        sdimporter.assetBundleName = BundleName;
        sdimporter.assetBundleVariant = Variant;
        publiclist.Add(path);

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        publicab.assetNames = publiclist.ToArray();
        return publicab;
    }

    static Material GetMat(Texture texture)
    {
        Material mat = null;
        string path = AssetDatabase.GetAssetPath(texture);
        string rootpath = Path.GetDirectoryName(path);

        if (rootpath == "Assets/MyResources/UI/Textures")
        {
            mat = AssetDatabase.LoadAssetAtPath<Material>(string.Format("Assets/MyResources/UI/Textures/material/{0}mat.mat", texture.name));
            if (!mat)
                Debug.Log(path + "--------------");
        }
        else
        {
            rootpath = rootpath.Replace("Assets/MyResources/UI/RawImage", "Assets/MyResources/UI/RawImage/Material");
            mat = AssetDatabase.LoadAssetAtPath<Material>(string.Format(rootpath + "/{0}mat.mat", texture.name));
            if (!mat)
                Debug.Log(string.Format(rootpath + "/{0}mat.mat", texture.name));
        }

        return mat;
    }

    public static void BuildAsset(Object asset, string Ext, string outputPath)
    {
        string assetfile = AssetDatabase.GetAssetPath(asset);
        AssetImporter sdimporter = AssetImporter.GetAtPath(assetfile);
        sdimporter.assetBundleName = asset.name;
        sdimporter.assetBundleVariant = Ext;
        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
        buildMap[0].assetBundleName = asset.name;
        buildMap[0].assetBundleVariant = Ext;
        buildMap[0].assetNames = new string[] { assetfile };
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);
        BuildPipeline.BuildAssetBundles(outputPath, buildMap, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }

    public static AssetBundleBuild CreateAssetBundle(List<Object> assets, string BundleName, string Variant, string outpath)
    {
        AssetBundleBuild publicab = new AssetBundleBuild();
        publicab.assetBundleName = outpath + BundleName;
        publicab.assetBundleVariant = Variant;
        List<string> publiclist = new List<string>();
        for (int i = 0; i < assets.Count; i++)
        {
            string path = AssetDatabase.GetAssetPath(assets[i]);
            AssetImporter sdimporter = AssetImporter.GetAtPath(path);
            if (!sdimporter)
            {
                Debug.Log(path);
                continue;
            }
            sdimporter.assetBundleName = BundleName;
            sdimporter.assetBundleVariant = Variant;
            publiclist.Add(path);
        }
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
        publicab.assetNames = publiclist.ToArray();
        return publicab;
    }

    public static void BuildAsset(string outputPath, AssetBundleBuild[] builds)
    {
        if (!Directory.Exists(outputPath))
            Directory.CreateDirectory(outputPath);
        BuildPipeline.BuildAssetBundles(outputPath, builds, BuildAssetBundleOptions.ChunkBasedCompression, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.Refresh();
    }


    [MenuItem("Assets/UI相关/把Image组件改成CImage")]
    static void ImageToCimage()
    {
        Object[] obj = Selection.objects;
        for (int i = 0; i < obj.Length; i++)
        {
            if (obj[i] is GameObject)
            {
                GameObject obj2 = (GameObject)obj[i];
                GetImage(obj2.transform);
            }
        }
    }

    public static void GetImage(Transform tr)
    {
        //CClientCommon.DestroyImmediate();
        Image image = tr.GetComponent<Image>();
        if (image != null && !(image is CImage))
        {
            UnityEngine.Object.DestroyImmediate(image, true);
            tr.gameObject.AddComponent<CImage>();
        }


        if (tr.childCount <= 0) return;

        for (int i = 0; i < tr.childCount; i++)
        {
            GetImage(tr.GetChild(i));
        }
    }



    private static void CompressUItex(Texture tex)
    {
        string path = AssetDatabase.GetAssetPath(tex);
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        CompressTex(importer, path, tex as Texture2D);
    }

    public static void CompressTex(TextureImporter textureImporter, string path, Texture2D texture)
    {
        TextureImporterFormat texFormat = TextureImporterFormat.ETC_RGB4Crunched;
        if (textureImporter.DoesSourceTextureHaveAlpha())
            texFormat = TextureImporterFormat.ETC2_RGBA8Crunched;
        textureImporter.mipmapEnabled = false;
        textureImporter.isReadable = false;
        textureImporter.filterMode = FilterMode.Bilinear;
        textureImporter.npotScale = TextureImporterNPOTScale.ToNearest;
        textureImporter.anisoLevel = 1;

        textureImporter.textureCompression = TextureImporterCompression.Compressed;
        int size = Mathf.Max(texture.height, texture.width);

        SetPlatformTextureSettings("Android", size, textureImporter, texFormat);
        SetPlatformTextureSettings("iPhone", size, textureImporter, texFormat);
        AssetDatabase.ImportAsset(path);
    }

    [MenuItem("Assets/UI相关/生成ID Lua脚本")]
    static void CreateUILuaScript()
    {
        if (Selection.activeGameObject)
        {
            NGUILink[] links = Selection.activeGameObject.GetComponentsInChildren<NGUILink>(true);
            foreach (var l in links)
                l.ReBuildLinkMap();

            NGUILink link = Selection.activeGameObject.GetComponent<NGUILink>();
            CreateLua("C" + link.gameObject.name + "UIID", "Assets/Lua/UI/" + link.gameObject.name);
            CreateCtrlScript("C" + link.gameObject.name + "UI", "Assets/Lua/UI/" + link.gameObject.name);
        }
    }

    private static void CreateCtrlScript(string LuaFile, string path)
    {
        string file2 = string.Format("{0}/{1}.lua.txt", path, LuaFile);

        if (File.Exists(file2))
        {
            return;
        }
        FileInfo t2 = new FileInfo(file2);
        StreamWriter write2 = File.CreateText(file2);
        string luastr2 = CString.Format("local {0} = class(GameUI)\n\n" +

            "function {1}:ctor(ui)\n    GameUI.ctor(self, ui)\n" +
            "    self.isFullScreen = true\n" +
            "    self.Layer = UIManager.Layer.FullWindow\n\n" +
            "end\n\n" +

            "function {2}.Awake(self)\n\n" +
            "end\n\n" +

            "function {3}.UIEnable(self)\n\n" +
            "end\n\n" +

            "function {4}.LoadUICallback(self)\n\n" +
            "end\n\n" +

            "function {5}.OnDestroy(self)\n\n" +
            "end\n\n" +
            "return {6}",
            LuaFile, LuaFile, LuaFile, LuaFile, LuaFile, LuaFile, LuaFile);
        write2.Write(luastr2);
        write2.Close();
        write2.Dispose();
    }

    static Dictionary<string, int> linknames = new Dictionary<string, int>();
    private static void CreateLua(string LuaFile, string path)
    {
        NGUILink[] links = Selection.activeGameObject.GetComponentsInChildren<NGUILink>(true);
        linknames.Clear();
        string file = string.Format("{0}/{1}.lua.txt", path, LuaFile);

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        FileInfo t = new FileInfo(file);
        //if (t.Exists)
        //    return;

        StreamWriter write = File.CreateText(file);
        string luastr = "local " + LuaFile + " = { }\n";

        foreach (var l in links)
        {
            if (l.gameObject == Selection.activeGameObject)
                luastr += GetSelfLinkString(LuaFile, l);
            else
                luastr += GetLinkString(LuaFile, l);
        }

        luastr += " \n return " + LuaFile;
        write.Write(luastr);
        write.Close();
        write.Dispose();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static string GetSelfLinkString(string LuaFile, NGUILink link)
    {
        string start = "\n\n" + LuaFile + ".";
        start += "HashID = " + link.HashID;
        foreach (var l in link.Links)
        {
            if (!l.LinkObj)
                continue;
            if (l.Name.Contains("(") ||
                l.Name.Contains(")") ||
                l.Name.Contains("（") ||
                l.Name.Contains("）") ||
                l.Name.Contains(" "))
            {
                EditorUtility.DisplayDialog("Error", "你的linkName:" + l.Name + " 请修改,否则无法创建ID脚本  非法字符： 如 空格 ( )", "ok");
                continue;
            }
            start += "\n" + LuaFile + "." + l.Name + " = " + l.HashID.ToString();
        }
        return start;
    }

    private static string GetLinkString(string LuaFile, NGUILink link)
    {
        string linkname = link.name;
        int count = 0;
        if (linknames.ContainsKey(link.name))
        {
            count = linknames[link.name];
            count++;
            linkname += count.ToString();
        }
        linknames[link.name] = count;
        string start = "\n\n" + LuaFile + "." + linkname + " = \n{";
        start += "\n    HashID = " + link.HashID + ",";
        foreach (var l in link.Links)
        {
            if (!l.LinkObj)
                continue;
            if (l.Name.Contains("(") ||
                l.Name.Contains(")") ||
                l.Name.Contains("（") ||
                l.Name.Contains("）") ||
                l.Name.Contains(" "))
            {
                EditorUtility.DisplayDialog("Error", "你的linkName:" + l.Name + " 请修改,否则无法创建ID脚本  非法字符： 如 空格 ( )", "ok");
                continue;
            }
            start += "\n    " + l.Name + " = " + l.HashID.ToString() + ",";
        }
        start += "\n }";
        return start;
    }

}

static public class MOYU_UIToolsEditor
{
    //public static void CreateLinkID(this NGUILink self)
    //{
    //    if (self.Links != null)
    //    {
    //        for (int i = 0; i < self.Links.Count; ++i)
    //        {
    //            NGUILink.UILink ul = self.Links[i];
    //            if (ul == null || !ul.LinkObj)
    //                continue;
    //            NGUILink child = ul.LinkObj.GetComponent<NGUILink>();
    //            if (child)
    //            {
    //                child.HashID = child.GetHashCode();
    //                ul.HashID = child.HashID;
    //            }
    //            else
    //                ul.HashID = ul.Name.GetHashCode();
    //        }
    //    }
    //}
    //public static void ReBuildLinkMap(this NGUILink self)
    //{
    //    self.HashID = self.GetHashCode();
    //    self.CreateLinkID();
    //    if (self.Links != null)
    //    {
    //        self.all_objs.Clear();
    //        for (int i = 0; i < self.Links.Count; ++i)
    //        {
    //            NGUILink.UILink ul = self.Links[i];
    //            if (ul == null || !ul.LinkObj)
    //                continue;
    //            self.all_objs[ul.Name] = ul;
    //            self.HashMap[ul.HashID] = ul;
    //        }
    //    }
    //}


    #region lua相关

    [MenuItem("Assets/UI相关/生成HashID")]
    static void CreateUILuaID()
    {
        if (Selection.activeGameObject)
        {
            NGUILink[] links = Selection.activeGameObject.GetComponentsInChildren<NGUILink>(true);
            foreach (var l in links)
                l.ReBuildLinkMap();
        }
    }


    [MenuItem("Assets/UI相关/自动生成NguiLink中的link")]
    public static void CreateNguiLinkValue()
    {
        if (Selection.activeGameObject)
        {
            if ((Selection.activeGameObject is GameObject) == false) return;

            GameObject obj = (GameObject)Selection.activeGameObject;
            if (obj.GetComponent<NGUILink>() == false) return;
            NGUILink link = obj.GetComponent<NGUILink>();
            link.Links = new List<NGUILink.UILink>();
            CImage[] CImages = obj.transform.GetComponentsInChildren<CImage>();
            for (int i = 0; i < CImages.Length; i++)
            {
                if (CImages[i].name.ToLower().StartsWith("image"))
                {
                    NGUILink.UILink link2 = new NGUILink.UILink();
                    link2.LinkObj = CImages[i].gameObject;
                    link.Links.Add(link2);
                }
            }

            Text[] Texts = obj.transform.GetComponentsInChildren<Text>();
            for (int i = 0; i < Texts.Length; i++)
            {
                if (Texts[i].name.ToLower().StartsWith("text"))
                {
                    NGUILink.UILink link2 = new NGUILink.UILink();
                    link2.LinkObj = Texts[i].gameObject;
                    link.Links.Add(link2);
                }
            }


            GetChild(obj.transform, (a) =>
            {
                NGUILink.UILink link2 = new NGUILink.UILink();
                link2.LinkObj = a.gameObject;
                link.Links.Add(link2);
            });
        }
    }

    public static void GetChild(Transform tr, System.Action<Transform> act)
    {
        if (tr.gameObject.name.ToLower().StartsWith("btn") || tr.gameObject.name.ToLower().EndsWith("_lk"))
        {
            act(tr);
        }
        if (tr.childCount <= 0) return;
        for (int i = 0; i < tr.childCount; i++)
        {
            GetChild(tr.GetChild(i), act);
        }
    }

    [MenuItem("Assets/UI相关/生成ID Lua脚本")]
    static void CreateUILuaScript()
    {
        if (Selection.activeGameObject)
        {
            CreateUILuaScript2(Selection.activeGameObject);
        }
    }

    // 创建hashid, 然后生成脚本
    public static void CreateUILuaScript2(GameObject obj)
    {
        NGUILink[] links = obj.GetComponentsInChildren<NGUILink>(true);
        foreach (var l in links)
            l.ReBuildLinkMap();

        NGUILink link = obj.GetComponent<NGUILink>();
        CreateLua("C" + link.gameObject.name + "UIID", "Assets/Lua/UI/" + link.gameObject.name);
        CreateCtrlScript("C" + link.gameObject.name + "UI", "Assets/Lua/UI/" + link.gameObject.name, obj.GetComponent<NGUILink>());
    }

    private static void CreateCtrlScript(string LuaFile, string path, NGUILink link)
    {
        string file2 = string.Format("{0}/{1}.lua.txt", path, LuaFile);


        StringBuilder OnClickSB = new StringBuilder("");
        for (int i = 0; i < link.Links.Count; i++)
        {
            if (link.Links[i].Name.ToLower().StartsWith("btn"))
            {
                // 按钮事件注册
                OnClickSB.Append(CString.Format("    self:SetEvent(self.HashIDTable.{0}, UIManager.EID.PointerClick,self:GetSelfFunc({1}.OnClick{2}))\n", link.Links[i].Name, LuaFile, link.Links[i].Name));
            }
        }
        OnClickSB.Append("\n");
        // 客户端内部事件注册
        for (int i = 0; i < link.ClientEvent.Count; i++)
        {
            OnClickSB.Append(CString.Format("    self:RegEvent(1,self:GetSelfFunc({0}.{1}Event))\n", LuaFile, link.ClientEvent[i]));
        }
        OnClickSB.Append("\n");
        // 文本的赋值
        OnClickSB.Append(CString.Format("-- 待用的text和image"));
        for (int i = 0; i < link.Links.Count; i++)
        {
            if (link.Links[i].Name.ToLower().StartsWith("text"))
            {
                OnClickSB.Append(CString.Format("    --self:SetCText(self.HashIDTable.{0},\"待赋的值\")\n", link.Links[i].Name));
            }
        }
        OnClickSB.Append("\n");
        // 图片赋值
        OnClickSB.Append(CString.Format("-- 待用的text和image"));
        for (int i = 0; i < link.Links.Count; i++)
        {
            if (link.Links[i].Name.ToLower().StartsWith("image"))
            {
                OnClickSB.Append(CString.Format("    --self:CreateSprite(self.HashIDTable.{0},\"spName\", \"spriteName\")\n", link.Links[i].Name));
            }
        }

        // 这里初始化函数结束
        OnClickSB.Append("end\n\n");

        for (int i = 0; i < link.Links.Count; i++)
        {
            if (link.Links[i].Name.ToLower().StartsWith("btn"))
            {
                OnClickSB.Append(CString.Format("function {0}:OnClick{1}()  \n"
                    + "    print(\"" + "OnClick{2}\")\n" +
            "end\n\n", LuaFile, link.Links[i].Name, link.Links[i].Name));
            }
        }

        for (int i = 0; i < link.ClientEvent.Count; i++)
        {
            OnClickSB.Append(CString.Format("function {0}:{1}Event()  \n"
                    + "    print(\"" + "Event:{2}\")\n" +
            "end\n\n", LuaFile, link.ClientEvent[i], link.ClientEvent[i]));
        }

        string OnClickSBstr = OnClickSB.ToString();





        string luastr2 = CString.Format(
            "local Base = GameUI\n" +
            "local {0} = class(Base)\n\n" +

            "function {1}:InitData(...)\n    " +
            "    self.IsFullScreen = true\n" +
            "    self.Layer = UIManager.Layer.FullWindow\n\n" +
            "end\n\n" +

            "function {2}:Initialize()\n" +
                 "{3}" +

            "function {4}:UIEnable()\n\n" +
            "end\n\n" +

            "function {5}:LoadUICallback()\n\n" +
            "end\n\n" +

            "function {6}:OnDestroy()\n\n" +
            "end\n\n" +
            "return {7}",
            LuaFile, LuaFile, LuaFile, OnClickSBstr, LuaFile, LuaFile, LuaFile, LuaFile);

        CreateTxtFile(file2, luastr2, false);
    }

    /// <summary>
    /// 创建文本文件, 
    /// </summary>
    /// <param name="fullPath"> 全路径， 路径加上 文件名和后缀</param>
    /// <param name="content">文本的内容</param>
    /// <returns></returns>
    public static bool CreateTxtFile(string fullPath, string content, bool isCover = true)
    {
        if (File.Exists(fullPath) && !isCover) return false;

        if (File.Exists(fullPath))
            File.Delete(fullPath);
        StreamWriter write2 = File.CreateText(fullPath);

        write2.Write(content);
        write2.Close();
        write2.Dispose();
        return true;
    }

    static Dictionary<string, int> linknames = new Dictionary<string, int>();
    private static void CreateLua(string LuaFile, string path)
    {
        NGUILink[] links = Selection.activeGameObject.GetComponentsInChildren<NGUILink>(true);
        linknames.Clear();
        string file = string.Format("{0}/{1}.lua.txt", path, LuaFile);

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

        FileInfo t = new FileInfo(file);
        //if (t.Exists)
        //    return;

        StreamWriter write = File.CreateText(file);
        string luastr = "local " + LuaFile + " = { }\n";

        foreach (var l in links)
        {
            if (l.gameObject == Selection.activeGameObject)
                luastr += GetSelfLinkString(LuaFile, l);
            else
                luastr += GetLinkString(LuaFile, l);
        }

        luastr += " \n return " + LuaFile;
        write.Write(luastr);
        write.Close();
        write.Dispose();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private static string GetSelfLinkString(string LuaFile, NGUILink link)
    {
        string start = "\n\n" + LuaFile + ".";
        start += "HashID = " + link.HashID;
        foreach (var l in link.Links)
        {
            if (!l.LinkObj)
                continue;
            if (l.Name.Contains("(") ||
                l.Name.Contains(")") ||
                l.Name.Contains("（") ||
                l.Name.Contains("）") ||
                l.Name.Contains(" "))
            {
                EditorUtility.DisplayDialog("Error", "你的linkName:" + l.Name + " 请修改,否则无法创建ID脚本  非法字符： 如 空格 ( )", "ok");
                continue;
            }
            start += "\n" + LuaFile + "." + l.Name + " = " + l.HashID.ToString();
        }
        return start;
    }

    private static string GetLinkString(string LuaFile, NGUILink link)
    {
        string linkname = link.name;
        int count = 0;
        if (linknames.ContainsKey(link.name))
        {
            count = linknames[link.name];
            count++;
            linkname += count.ToString();
        }
        linknames[link.name] = count;
        string start = "\n\n" + LuaFile + "." + linkname + " = \n{";
        start += "\n    HashID = " + link.HashID + ",";
        foreach (var l in link.Links)
        {
            if (!l.LinkObj)
                continue;
            if (l.Name.Contains("(") ||
                l.Name.Contains(")") ||
                l.Name.Contains("（") ||
                l.Name.Contains("）") ||
                l.Name.Contains(" "))
            {
                EditorUtility.DisplayDialog("Error", "你的linkName:" + l.Name + " 请修改,否则无法创建ID脚本  非法字符： 如 空格 ( )", "ok");
                continue;
            }
            start += "\n    " + l.Name + " = " + l.HashID.ToString() + ",";
        }
        start += "\n }";
        return start;
    }
    public static string GetDictory()
    {
        UnityEngine.Object obj = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath(obj);
        string filename = Path.GetFileName(path);
        path = path.Replace("/" + filename, "");
        return path;
    }
    [MenuItem("Assets/UI相关/把后缀名改为lua")]
    public static void ChangeFileName()
    {
        string path = GetDictory();
        foreach (string d in Directory.GetFileSystemEntries(path))
        {
            if (File.Exists(d))
            {
                if (d.Contains("lua"))
                {

                    string d2 = d.Replace("lua", "lua.txt");
                    FileInfo fi = new FileInfo(d);
                    File.Move(d, d2);
                }
                else if (d.Contains(".cs"))
                {
                    string d2 = d.Replace(".cs", ".lua");
                    FileInfo fi = new FileInfo(d);
                    File.Move(d, d2);
                }

            }
        }
        AssetDatabase.Refresh();
    }

    [MenuItem("Assets/UI相关/生成所有UI的HashID")]
    static void CreateUIsHashID()
    {
        EditorUtility.DisplayProgressBar("生成所有UI的HashID", "Searching Assets", 0);
        Object[] Assets = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        int i = 0;
        foreach (Object asset in Assets)
        {
            EditorUtility.DisplayProgressBar("生成所有UI的HashID", string.Format("{0}/{1}", i, Assets.Length), (float)i / (float)Assets.Length);
            i++;
            if (asset is GameObject)
            {
                CreateHashID(asset as GameObject);
            }

        }
        EditorUtility.ClearProgressBar();
        AssetDatabase.SaveAssets();
    }

    static void CreateHashID(GameObject asset)
    {
        GameObject UI = asset as GameObject;
        NGUILink[] links = UI.GetComponentsInChildren<NGUILink>(true);
        foreach (var l in links)
            l.ReBuildLinkMap();

        GameObject clone = Object.Instantiate(UI) as GameObject;
        PrefabUtility.ReplacePrefab(clone, UI);
        UnityEngine.Object.DestroyImmediate(clone, true);
    }

    public static void ReBuildLinkMap(this NGUILink self)
    {
        self.HashID = self.GetHashCode();
        self.CreateLinkID();
        if (self.Links != null)
        {
            self.all_objs.Clear();
            for (int i = 0; i < self.Links.Count; ++i)
            {
                NGUILink.UILink ul = self.Links[i];
                if (ul == null || !ul.LinkObj)
                    continue;
                self.all_objs[ul.Name] = ul;
                self.HashMap[ul.HashID] = ul;
            }
        }
    }

    public static void CreateLinkID(this NGUILink self)
    {
        if (self.Links != null)
        {
            for (int i = 0; i < self.Links.Count; ++i)
            {
                NGUILink.UILink ul = self.Links[i];
                if (ul == null || !ul.LinkObj)
                    continue;
                NGUILink child = ul.LinkObj.GetComponent<NGUILink>();
                if (child)
                {
                    child.HashID = child.GetHashCode();
                    ul.HashID = child.HashID;
                }
                else
                    ul.HashID = ul.Name.GetHashCode();
            }
        }
    }
    #endregion


}
