#define DEBUG24
using System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using LitJson;
using UnityEditor;
using UnityEditor.Macros;
using UnityEngine;
using Debug = UnityEngine.Debug;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;





public class BaseClass
{
    [ThreadStatic]
    public int total;

    [CLSCompliant(false)]
    public virtual uint MethodA()
    {
        return (uint)100;
    }
}






public class stu
{
    public string s1;
    public string s3;

    public stu(string s1)
    {
        this.s1 = s1;

    }

}

public class ApplicationEditor : Editor
{

   
    /// <summary>
    /// 
    /// </summary>
    static void TestNetFrameWorkVersion()
    {
        //TestVersion  
        Type type = Type.GetType("Mono.Runtime");
        if (type != null)
        {
            MethodInfo info = type.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);
            if (info != null)
                UnityEngine.Debug.Log(info.Invoke(null, null)); ; ;
            //if () 
            //if () { }
        }
    }

    [MenuItem("开发场景悠悠/BuildAllLuaFile")]
    public static void BuildAllLua()
    {
        string dir = Application.dataPath + "/Lua";
        ClearFolder(dir);
        UnityEngine.Debug.Log("Build All Lua Success-----------");
    }

    /// <summary>
    ///  buildAllLua 
    /// </summary>
    /// <param name="dir"></param>
    static void ClearFolder(string dir)
    {
        foreach (string d in Directory.GetFileSystemEntries(dir))
        {
            if (File.Exists(d))
            {

                if (d.EndsWith(".lua")

                    &&

                    !d.Contains("MindQuiz") &&
                    !d.Contains("CoreSystem") &&
                    !d.Contains("GameSystem") &&
                    !d.Contains("FunTemplate") &&
                    !d.Contains("ChapterDuplicateLeft") &&
                    !d.Contains("Login")

                )
                {
                    UnityEngine.Debug.Log("没有加入到build里面的是 ："

                        +
                        "CoreSystem" + " " +
                        "GameSystem" +
                        "FunTemplate" +
                        "ChapterDuplicateLeft" +
                        "Login"

                    );
                    string dddd = Application.dataPath;
                    dddd = dddd.Replace("Assets", "");
                    dddd = d.Substring(dddd.Length, d.Length - dddd.Length);
                    UnityEngine.Object ddd = AssetDatabase.LoadAssetAtPath(dddd, typeof(UnityEngine.Object));
                    BuidlLuaSource(ddd);
                }
            }
            else
            {
                ClearFolder(d);
            }
        }
    }

    static void BuidlLuaSource(UnityEngine.Object asset)
    {

        string path = AssetDatabase.GetAssetPath(asset);
        if (Path.GetExtension(path) != ".lua")
            return;
        string text = File.ReadAllText(path);
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

    static void BuildLua(UnityEngine.Object lua)
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

        BuildPipeline.BuildAssetBundles(outpath, buildMap.ToArray(), BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }




    [MenuItem("开发场景悠悠/Create Controller")]
    static void CreateController()
    {
        //
        //
        //if (
        // Creates the controller
        var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath("Assets/StateMachineTransitions.controller");

        // Add parameters
        controller.AddParameter("TransitionNow", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("Reset", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("GotoB1", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("GotoC", AnimatorControllerParameterType.Trigger);

        // Add StateMachines
        var rootStateMachine = controller.layers[0].stateMachine;
        var stateMachineA = rootStateMachine.AddStateMachine("smA");
        var stateMachineB = rootStateMachine.AddStateMachine("smB");
        var stateMachineC = stateMachineB.AddStateMachine("smC");





        // Add States
        var stateA1 = stateMachineA.AddState("stateA1");
        var stateB1 = stateMachineB.AddState("stateB1");
        var stateB2 = stateMachineB.AddState("stateB2");
        stateMachineC.AddState("stateC1");
        var stateC2 = stateMachineC.AddState("stateC2"); // don’t add an entry transition, should entry to state by default

        // Add Transitions
        var exitTransition = stateA1.AddExitTransition();
        exitTransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "TransitionNow");
        exitTransition.duration = 0;

        var resetTransition = rootStateMachine.AddAnyStateTransition(stateA1);
        resetTransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "Reset");
        resetTransition.duration = 0;

        var transitionB1 = stateMachineB.AddEntryTransition(stateB1);
        transitionB1.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "GotoB1");
        stateMachineB.AddEntryTransition(stateB2);
        stateMachineC.defaultState = stateC2;
        var exitTransitionC2 = stateC2.AddExitTransition();
        exitTransitionC2.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "TransitionNow");
        exitTransitionC2.duration = 0;

        var stateMachineTransition = rootStateMachine.AddStateMachineTransition(stateMachineA, stateMachineC);
        stateMachineTransition.AddCondition(UnityEditor.Animations.AnimatorConditionMode.If, 0, "GotoC");
        rootStateMachine.AddStateMachineTransition(stateMachineA, stateMachineB);
    }


    [MenuItem("开发场景悠悠/打开窗口执行复制代码")]
    public static void AddWindow()
    {
        //创建窗口
        Rect wr = new Rect(0, 0, 800, 150);
        MyEditor window = (MyEditor)EditorWindow.GetWindowWithRect(typeof(MyEditor), wr, true, "文件夹复制窗口");
        window.Show();
        window = null;
        AssetDatabase.Refresh();
    }

    [MenuItem("开发场景悠悠/测试读取")]
    public static void AddWindow2()
    {
        //CClientCommon.Debug
        //创建窗口
        string str = Application.dataPath + "/ATest/TestColor/TestColor.json";
        //CClientCommon.Debug(str);
        JsonData dd = JsonRead<JsonData>(str);
        for (int i = 0; i < dd.Count; i++)
        {
            JsonData data = dd[i];
        }
        //CClientCommon.Debug(dd.ToJson());

    }

    static StreamReader GetStreamReader(string path)
    {

        FileInfo t = new FileInfo(path);

        if (!t.Exists)
        {
            return null;
        }
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path);
        }
        catch (Exception /*e*/ )
        {
            //GameMessgeUI.ShowPrompt( e.StackTrace );
            return null;
        }
        return sr;
    }


    public static JsonData JsonRead<T>(string filename)
    {
        //filename =/* CDirectory.MakeCachePath(filename);*/
        JsonData result = null;
        StreamReader sr = GetStreamReader(filename);

        if (sr == null)
            return result;
        try
        {
            result = JsonMapper.ToObject(sr);
        }
        catch (System.Exception ex)
        {
            if (ex != null)
            {
                sr.Close();
                sr.Dispose();
                //DeleteFile(filename);
                return result;
            }
        }
        sr.Close();
        sr.Dispose();
        return result;
    }

    //GeneratorConfig.common_path
    static void clear(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
            AssetDatabase.DeleteAsset(path.Substring(path.IndexOf("Assets") + "Assets".Length));

            AssetDatabase.Refresh();
        }
    }

    /// <summary>
    ///  获取当前点击的资源它所在的文件夹路径
    /// </summary>
    /// <returns></returns>
    public static string GetDictory()
    {
        UnityEngine.Object obj = Selection.activeObject;
        string path = AssetDatabase.GetAssetPath(obj);
        string filename = Path.GetFileName(path);
        path = path.Replace("/" + filename, "");
        return path;
    }

    // 源文件路径
    private static string SourceFolder = @"F:\luaDemo\luaDemo2\Lua";
    // 目标文件路径
    private static string AimsFolder = @"D:\workproject\moyu0321\EudemonsClient\Assets\Resources\Lua";
    [MenuItem("开发场景悠悠/直接复制代码到目标文件夹")]
    public static void AddWindow22()
    {
        MyEditor window = (MyEditor)EditorWindow.GetWindowWithRect(typeof(MyEditor), new Rect(), true, "文件夹复制窗口");
        window.CopyTotal2(PlayerPrefs.GetString("source"), PlayerPrefs.GetString("goal"));
        window.Close();
        window = null;
        AssetDatabase.Refresh();
    }

    [MenuItem("开发场景悠悠/把后缀名改为lua")]
    public static void ChangeFileName()
    {
        string path = GetDictory();
        foreach (string d in Directory.GetFileSystemEntries(path))
        {
            if (File.Exists(d))
            {
                if (d.Contains("txt"))
                {

                    string d2 = d.Replace("txt", "lua");
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

    // 选中的对象
    public static void SelectObject(System.Action<UnityEngine.Object> f)
    {
        UnityEngine.Object[] objs = Selection.objects;
        if (objs == null) return;
        for (int i = 0; i < objs.Length; i++)
        {
            f(objs[i]);
        }

    }

    // 获取文件的全路径
    private static string GetFileFullPath(UnityEngine.Object obj)
    {
        string GitFilePath = Application.dataPath;
        GitFilePath = GitFilePath.Substring(0, GitFilePath.Length - "assets".Length);
        UnityEngine.Object obj2 = obj;
        string lupaht3 = AssetDatabase.GetAssetPath(obj2);
        GitFilePath = GitFilePath + lupaht3;
        return GitFilePath;
    }

    [SerializeField]
    private static Font targetFont;
    [SerializeField]
    private static TextAsset fntData;
    [SerializeField]
    private static Material fontMaterial;
    [SerializeField]
    private static Texture2D fontTexture;
//    static BMFont bmFont = new BMFont();

//    static void ddd()
//    {
//        BMFontReader.Load(bmFont, fntData.name, fntData.bytes); // 借用NGUI封装的读取类
//        CharacterInfo[] characterInfo = new CharacterInfo[bmFont.glyphs.Count];
//        for (int i = 0; i < bmFont.glyphs.Count; i++)
//        {
//            BMGlyph bmInfo = bmFont.glyphs[i];
//            CharacterInfo info = new CharacterInfo();
//            info.index = bmInfo.index;
//#pragma warning disable CS0618 // 类型或成员已过时
//            info.uv.x = (float)bmInfo.x / (float)bmFont.texWidth;
//            info.uv.y = 1 - (float)bmInfo.y / (float)bmFont.texHeight;
//            info.uv.width = (float)bmInfo.width / (float)bmFont.texWidth;
//            info.uv.height = -1f * (float)bmInfo.height / (float)bmFont.texHeight;
//            info.vert.x = 0;
//            info.vert.y = -(float)bmInfo.height;
//            info.vert.width = (float)bmInfo.width;
//            info.vert.height = (float)bmInfo.height;
//            info.width = (float)bmInfo.advance;
//#pragma warning restore CS0618 // 类型或成员已过时
//            characterInfo[i] = info;
//        }
//        targetFont.characterInfo = characterInfo;
//        if (fontMaterial)
//        {
//            fontMaterial.mainTexture = fontTexture;
//        }
//        targetFont.material = fontMaterial;
//        fontMaterial.shader = Shader.Find("UI/Default");

//        Debug.Log("create font <" + targetFont.name + "> success");
//        //Close();
//    }

    // 距离活动的开始时间
    public static int GetOpenTimeSpan()
    {
        string str = "22:10";
        str.Substring(0, 2);

        string hour = str.Substring(0, 2);
        string min = str.Substring(3, 2);

        if (int.Parse(min.Substring(0, 1)) == 0 && int.Parse(min.Substring(1, 1)) == 0)
            min = "0";

        DateTime now = DateTime.Now;

        if (hour == now.Hour.ToString())
        {
            int Spanemin2 = 60 * ((int.Parse(min) - now.Minute) - 1) + (60 - now.Second);
            return Spanemin2;
        }
        else
        {
            if (int.Parse(hour) - now.Hour == 1)
            {
                if (min == 0.ToString())
                {
                    return (60 - now.Minute - 1) * 60 + (60 - now.Second);
                }
                else
                {
                    return (60 - now.Minute - 1) * 60 + (60 - now.Second) + (int.Parse(min) * 60);
                }
            }
        }

        return 10000;
    }


    static string texPath = "";
    static string combindPath = "";
    private static void CombineTex_Mesh()
    {
        List<Texture2D> textures = new List<Texture2D>();
        string[] rGuids = AssetDatabase.FindAssets("t:Texture2D", new string[] { texPath });
        for (int guid = 0; guid < rGuids.Length; guid++)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(rGuids[guid]);
            Texture2D rTex = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath) as Texture2D;
            textures.Add(rTex);
        }
        if (textures.Count > 0)
        {
            Texture2D tempTex = new Texture2D(2048, 2048);
            Rect[] uvs = tempTex.PackTextures(textures.ToArray(), 0);
            tempTex.Apply();
            Texture2D rCombineTex = new Texture2D(tempTex.width, tempTex.height, TextureFormat.ARGB32, false);
            rCombineTex.SetPixels32(tempTex.GetPixels32());
            rCombineTex.Apply();
            byte[] bytes = rCombineTex.EncodeToPNG();

            File.WriteAllBytes(combindPath + "/combineTex.png", bytes);
            AssetDatabase.Refresh(ImportAssetOptions.ImportRecursive);
            rCombineTex = AssetDatabase.LoadAssetAtPath<Texture2D>(combindPath + "/combineTex.png");
            CombineMesh(SceneManager.GetSceneByName("Combine"), textures.ToArray(), rCombineTex, uvs);
        }
    }


    private static void CombineMesh(Scene scene, Texture2D[] texs, Texture2D mainTexture, Rect[] rect)
    {
        GameObject combineGo = new GameObject("combineGo");
        GameObject[] allGameObjects = scene.GetRootGameObjects();
        List<Vector2[]> usList = new List<Vector2[]>();
        List<Color> colorList = new List<Color>();
        //Color color = new Color();
        for (int object_index = 0; object_index < allGameObjects.Length; object_index++)
        {
            if (allGameObjects[object_index].name.Equals("CombineMesh"))
            {
                MeshFilter[] allFilter = allGameObjects[object_index].GetComponentsInChildren<MeshFilter>();
                MeshRenderer[] allRender = allGameObjects[object_index].GetComponentsInChildren<MeshRenderer>();
                CombineInstance[] combineMesh = new CombineInstance[allFilter.Length];
                Material[] materials = new Material[allFilter.Length];

                for (int i = 0; i < allFilter.Length; i++)
                {
                    materials[i] = allRender[i].sharedMaterial;
                    combineMesh[i].mesh = allFilter[i].sharedMesh;
                    combineMesh[i].transform = allFilter[i].transform.localToWorldMatrix;
                    usList.Add(allFilter[i].sharedMesh.uv);
                }
                MeshFilter combineFilter = combineGo.AddComponent<MeshFilter>();
                MeshRenderer combineRender = combineGo.AddComponent<MeshRenderer>();
                combineFilter.sharedMesh = new Mesh();
                combineFilter.sharedMesh.CombineMeshes(combineMesh);
                Vector2[] uv = new Vector2[combineFilter.sharedMesh.vertices.Length];

                int count = 0;
                for (int i = 0; i < usList.Count; i++)
                {
                    for (int filter_index = 0; filter_index < allFilter.Length; filter_index++)
                    {
                        float scaleX = ((float)(texs[filter_index].width) / mainTexture.width);
                        float scaleY = ((float)(texs[filter_index].height) / mainTexture.height);
                        for (int j = 0; j < allFilter[filter_index].sharedMesh.vertices.Length; j++)
                        {
                            uv[count] = new Vector2((float)(rect[filter_index].xMin + allFilter[filter_index].sharedMesh.uv[j].x * scaleX),
                                (float)(rect[filter_index].yMin + allFilter[filter_index].sharedMesh.uv[j].y * scaleY));
                            count++;
                        }
                    }
                    combineFilter.sharedMesh.uv = uv;
                    combineRender.sharedMaterials = materials;
                    combineRender.sharedMaterial.mainTexture = mainTexture;
                    AssetDatabase.CreateAsset(combineRender.sharedMaterial, combindPath + "/material.mat");
                    AssetDatabase.CreateAsset(combineFilter.sharedMesh, combindPath + "/mesh.asset");
                    combineGo.transform.SetParent(allGameObjects[object_index].transform);
                }
            }
        }
    }

    public static DateTime GetTime(string timeStamp)
    {
        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = long.Parse(timeStamp + "0000000");
        TimeSpan toNow = new TimeSpan(lTime);
        return dtStart.Add(toNow);
    }

    [MenuItem("开发场景悠悠/代码测试 %u", false, 100)]
    public static void TestCode代码测试()
    {

        MyDebug.debug(System.Net.Dns.GetHostName()); 

        uint m_timestamp = 1537340159 + 3600;
        DateTime dt = GetTime(m_timestamp.ToString());
        Debug.Log(string.Format("将时间戳转换成日期_2 = {0} -> {1}", m_timestamp, dt.ToString("yyyy-MM-dd HH:mm:ss")));
        //    //s.getb?
        //    byte[] byteArray = System.Text.Encoding.Unicode .GetBytes ( s );
        //string ss =     Encoding.Unicode.GetString (byteArray);

        //char[] car =      s.ToCharArray ();
        //     new string (car, new  Encoding.UTF8.G);

        // JsonMapper.ToObject<Role>("@{"name":"\u540D\u5B57","id":"999","player":{"attack":"9999999","hp":"100"},"list":[]}");

        //Debug.LogError(ss + "     999ddd9d");
    }

    [MenuItem("开发场景悠悠/移动图片 %p", false, 101)]
    static void MoveTexture()
    {
        foreach (var item in Selection.objects)
        {
            if (item is Texture2D)
            {
                string filePath_1 = Application.dataPath + "/codeandweb.com/Editor/" + item.name + ".png";
                string filePath_2 = Application.dataPath + "/Resources/UI/Textures/" + item.name + ".png";

                string filePath_3 = Application.dataPath + "/codeandweb.com/Editor/" + item.name + ".png.meta";
                string filePath_4 = Application.dataPath + "/Resources/UI/Textures/" + item.name + ".png.meta";

                if (File.Exists(filePath_2))
                {
                    File.Delete(filePath_2);
                }

                if (File.Exists(filePath_4))
                {
                    File.Delete(filePath_4);
                }

                File.Move(filePath_1, filePath_2);
                File.Move(filePath_3, filePath_4);
            }
        }
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

    }


    static int _currentScence;
    [MenuItem("开发场景悠悠/运行游戏", false, 99)]
    public static void OpenAndRunLoginScene()
    {

        int index = GetSceneIndex("launcher");

        if (EditorApplication.currentScene == "launcher")
        {
            return;
        }

        if (index == -1)
        {
            return;
        }

        OpenScene(index);

        if (GameObject.Find("launcher") != null)
        {
            GameObject.Find("launcher").gameObject.SetActive(true); ;
            GameObject.Find("launcher").gameObject.GetComponent<Launcher>().enabled = true;
        }
        EditorApplication.isPlaying = true;
    }

    [MenuItem("开发场景悠悠/用于测试的场景")]
    public static void OpenCharChooserrScene()
    {
        int index = GetSceneIndex("mytest");

        if (index == -1)
        {
            return;
        }

        if (_currentScence == index)
            return;
        OpenScene(index);
    }

    [MenuItem("开发场景悠悠/运行游戏的场景")]
    public static void OpenCharChooserrScene2()
    {
        int index = GetSceneIndex("launcher");

        if (index == -1)
        {
            return;
        }

        if (_currentScence == index)
            return;
        OpenScene(index);
    }

    [MenuItem("开发场景悠悠/生成图集")]
    public static void CreateBuildAtlas()
    {
        GetActiveObj((obj) =>
        {
            if (obj is Texture2D)
            {




            }
            else
            {
                Debug.LogError(obj.name + "   is not texture2d");
            }
        });
    }

    public static void GetActiveObj(System.Action<UnityEngine.Object> action)
    {
        // 生成图集
        UnityEngine.Object[] objs = Selection.objects;
        for (int i = 0; i < objs.Length; i++)
        {
            action(objs[i]);
        }
    }

    [MenuItem("开发场景悠悠/有后端的通讯消息头log")]
    public static void OpenCharChooserrScene21()
    {
        // *************** 清空原文件的内容 **************/
        string infoPath = CString.Format("{0}{1}", Application.dataPath, "/Script/Messages/PacketHandler.cs");
        FileStream stream2 = File.Open(infoPath, FileMode.OpenOrCreate, FileAccess.Write);
        stream2.Seek(0, SeekOrigin.Begin);
        stream2.SetLength(0); //清空txt文件
        stream2.Close();

        // *************** 把目标文件的内容赋值到文件 **************/
        FileStream rfs = new FileStream("F:/replaceFile/haveLog/PacketHandler.cs", FileMode.Open, FileAccess.Read, FileShare.Read); //打开源文件读
        FileStream wfs = new FileStream(infoPath, FileMode.Open, FileAccess.Write); //打开源文件读
        for (int i = 0; i < rfs.Length; i++)
            wfs.WriteByte((byte)rfs.ReadByte()); //从一个文件写到另一个文件
        rfs.Dispose();
        wfs.Dispose();
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

    }

    [MenuItem("开发场景悠悠/没有后端的通讯消息头log")]
    public static void OpenCharChooserrScene23()
    {
        // *************** 清空原文件的内容 **************/
        string infoPath = CString.Format("{0}{1}", Application.dataPath, "/Script/Messages/PacketHandler.cs");
        FileStream stream2 = File.Open(infoPath, FileMode.OpenOrCreate, FileAccess.Write);
        stream2.Seek(0, SeekOrigin.Begin);
        stream2.SetLength(0); //清空txt文件
        stream2.Close();

        // *************** 把目标文件的内容赋值到文件 **************/
        FileStream rfs = new FileStream("F:/replaceFile/noLog/PacketHandler.cs", FileMode.Open, FileAccess.Read, FileShare.Read); //打开源文件读
        FileStream wfs = new FileStream(infoPath, FileMode.Open, FileAccess.Write); //打开源文件读
        for (int i = 0; i < rfs.Length; i++)
            wfs.WriteByte((byte)rfs.ReadByte()); //从一个文件写到另一个文件
        rfs.Dispose();
        wfs.Dispose();
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

    }

    [MenuItem("开发场景悠悠/我的NGUILink")]
    public static void OpenCharChooserrScene24()
    {
        // *************** 清空原文件的内容 **************/
        string infoPath = CString.Format("{0}{1}", Application.dataPath, "/Script/Editor/NGUILinkEditor.cs");
        FileStream stream2 = File.Open(infoPath, FileMode.OpenOrCreate, FileAccess.Write);
        stream2.Seek(0, SeekOrigin.Begin);
        stream2.SetLength(0); //清空txt文件
        stream2.Close();

        // *************** 把目标文件的内容赋值到文件 **************/
        FileStream rfs = new FileStream("F:/replaceFile/MyNguiLink/MyNguiLink.cs", FileMode.Open, FileAccess.Read, FileShare.Read); //打开源文件读
        FileStream wfs = new FileStream(infoPath, FileMode.Open, FileAccess.Write); //打开源文件读
        for (int i = 0; i < rfs.Length; i++)
            wfs.WriteByte((byte)rfs.ReadByte()); //从一个文件写到另一个文件
        rfs.Dispose();
        wfs.Dispose();
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

    }

    [MenuItem("开发场景悠悠/还原NGUILink")]
    public static void OpenCharChooserrScene25()
    {
        // *************** 清空原文件的内容 **************/
        string infoPath = CString.Format("{0}{1}", Application.dataPath, "/Script/Editor/NGUILinkEditor.cs");
        FileStream stream2 = File.Open(infoPath, FileMode.OpenOrCreate, FileAccess.Write);
        stream2.Seek(0, SeekOrigin.Begin);
        stream2.SetLength(0); //清空txt文件
        stream2.Close();

        // *************** 把目标文件的内容赋值到文件 **************/
        FileStream rfs = new FileStream("F:/replaceFile/HuanYuanNguilink/NGUILinkEditor.cs", FileMode.Open, FileAccess.Read, FileShare.Read); //打开源文件读
        FileStream wfs = new FileStream(infoPath, FileMode.Open, FileAccess.Write); //打开源文件读
        for (int i = 0; i < rfs.Length; i++)
            wfs.WriteByte((byte)rfs.ReadByte()); //从一个文件写到另一个文件
        rfs.Dispose();
        wfs.Dispose();
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

    }

    [MenuItem("开发场景悠悠/删除meta文件")]
    public static void DebugLua1()
    {

        string str = Application.streamingAssetsPath + "/res/lua";
        ClearFolder2(str);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

    }


    /// <summary>
    /// 查找某个文件夹下，所有文件
    /// </summary>
    /// <param name="dir"></param>
    /// <param name="fileAct"></param>
    public static void ClearData(string dir, System.Action<string, FileInfo> fileAct)
    {
        foreach (string d in Directory.GetFileSystemEntries(dir))
        {
            if (File.Exists(d))
            {
                FileInfo fi = new FileInfo(d);
                fileAct(d, fi);
            }
            else
            {

                DirectoryInfo dl = new DirectoryInfo(d);
                if (dl.GetFiles().Length != 0)
                    ClearData(dl.FullName, fileAct);
            }
        }
    }

    [MenuItem("开发场景悠悠/实现自己的委托功能")]
    public static void DebugLua15()
    {
        UnityEngine.Object obj = Selection.activeObject;
        if (obj is GameObject)
        {
            UnityEngine.GameObject obj2 = (GameObject)(obj);
            Transform tr = obj2.transform;
            DIgui2(tr, GetDigui);
        }
    }

    public static void DIgui2(Transform tr, System.Action<Transform> trdd)
    {
        trdd(tr);
        if (tr.childCount > 0)
        {
            for (int i = 0; i < tr.childCount; i++)
            {
                if (!tr.gameObject.name.Contains("tem"))
                    DIgui2(tr.GetChild(i), trdd);
            }
        }
    }

    public static void GetDigui(Transform tr)
    {

    }


    [MenuItem("开发场景悠悠/取消所有按钮响应")]
    public static void DebugLua12()
    {

        UnityEngine.Object obj = Selection.activeObject;
        if (obj is GameObject)
        {
            UnityEngine.GameObject obj2 = (GameObject)(obj);
            Transform tr = obj2.transform;
            DIgui(tr);
        }
    }

    [MenuItem("开发场景悠悠/清除并生成Code")]
    public static void DebugLua13()
    {
        //CSObjectWrapEditor.Generator.ClearAll();
        //AssetDatabase.Refresh();
        //AssetDatabase.SaveAssets();
        //CSObjectWrapEditor.Generator.GenAll();
        //AssetDatabase.Refresh();
        //AssetDatabase.SaveAssets();
    }



    [MenuItem("开发场景悠悠/UI相关/改变颜色")]
    public static void UI_()
    {
        UnityEngine.Object[] d = Selection.objects;
        for (int i = 0; i < d.Length; i++)
            UI_changeColor(d[i]);
    }
    static bool b;
    static string 关闭打开影藏ObjStr = "FenBaoScene";
    [MenuItem("开发场景悠悠/UI相关/关闭打开影藏模板")]
    public static void UI_close_show()
    {
        foreach (var g in GameObject.FindObjectsOfType<Transform>())
        {

            //CClientCommon.Debug(g.gameObject.name);

            if (g.gameObject.name == (关闭打开影藏ObjStr))
            {
                for (int i = 0; i < g.transform.childCount; i++)
                {
                    if (g.GetChild(i).gameObject.name.Contains("MyDeMoBan"))
                    {

                        if (g.GetChild(i).GetComponent<CRawImage>().color == new Color(1, 1, 1, 1))
                        {

                            g.GetChild(i).GetComponent<CRawImage>().color = new Color(1, 1, 1, 100 / 255f);
                            g.GetChild(i).gameObject.SetActive(!b);
                        }
                        else
                        {
                            g.GetChild(i).gameObject.SetActive(!b);
                            g.GetChild(i).GetComponent<CRawImage>().color = new Color(1, 1, 1, 100 / 255f);
                        }
                        b = !b;

                    }

                }
                break;
            }
        }
    }
    [MenuItem("开发场景悠悠/UI相关/显示UI模板并高亮")]
    public static void UI_close_show2()
    {
        foreach (var g in GameObject.FindObjectsOfType<Transform>())
        {

            //CClientCommon.Debug(g.gameObject.name);

            if (g.gameObject.name == (关闭打开影藏ObjStr))
            {
                for (int i = 0; i < g.transform.childCount; i++)
                {

                    if (g.GetChild(i).gameObject.name.Contains("MyDeMoBan"))
                    {



                        g.GetChild(i).gameObject.SetActive(true);
                        g.GetChild(i).GetComponent<CRawImage>().color = new Color(1, 1, 1, 1);
                    }

                }
                break;
            }
        }
    }

    private static void UI_changeColor(UnityEngine.Object obj)
    {
        //UnityEngine.Object obj = Selection.activeObject;
        //CClientCommon.Debug("选中：" + obj.name);
        if (obj is GameObject)
        {
            UnityEngine.GameObject obj2 = (GameObject)(obj);
            Transform tr = obj2.transform;
            if (tr.GetComponent<CText>() != null)
            {
                CText dd = tr.GetComponent<CText>();
                dd.color = new Color(192 / 255f, 163 / 255f, 126 / 255f);
                dd.fontSize = 20;
            }
            //DIgui(tr);
        }
    }


    public static void DIgui(Transform tr)
    {
        OneTr(tr);
        if (tr.childCount > 0)
        {
            for (int i = 0; i < tr.childCount; i++)
            {
                if (!tr.gameObject.name.Contains("tem"))
                    DIgui(tr.GetChild(i));
            }
        }
    }

    public static void OneTr(Transform tr)
    {
        if (tr.gameObject.name.Contains("guanbi") || tr.gameObject.name.Contains("tem") || tr.gameObject.name.Contains("btn") || tr.gameObject.name.Contains("Btn"))
            return;

        CRawImage RawImag = tr.GetComponent<CRawImage>();
        if (RawImag != null)
        {
            Debug.Log("name:" + RawImag.gameObject.name);
            RawImag.raycastTarget = false;
        }

        CText text = tr.GetComponent<CText>();
        if (text != null)
        {
            Debug.Log("name:" + text.gameObject.name);
            text.raycastTarget = false;
        }

        CImage image = tr.GetComponent<CImage>();
        if (image != null)
        {
            Debug.Log("name:" + image.gameObject.name);
            image.raycastTarget = false;
        }


    }

    internal static void ClearFolder2(string dir)
    {
        foreach (string d in Directory.GetFileSystemEntries(dir))
        {
            if (File.Exists(d))
            {
                FileInfo fi = new FileInfo(d);
                if (fi.Attributes.ToString().IndexOf("ReadOnly") != -1)
                    fi.Attributes = FileAttributes.Normal;
                try
                {
                    if (d.EndsWith(".meta") || d.Contains(".manifest") || !d.Contains(".l"))
                    {
                        File.Delete(d);
                    }
                }
                catch { }
            }
            else
            {
                DirectoryInfo dl = new DirectoryInfo(d);
                if (dl.GetFiles().Length != 0)
                    ClearFolder2(dl.FullName);
            }
        }
    }

    public static int GetSceneIndex(string name)
    {
        int index = -1;

        for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
        {
            //var item = EditorBuildSettings.scenes[i];
            string fname = System.IO.Path.GetFileName(EditorBuildSettings.scenes[i].path);
            if (fname == name + ".unity")
            {
                return i;
            }
        }

        return index;
    }
    public static void OpenScene(int i)
    {

        if (EditorApplication.SaveCurrentSceneIfUserWantsTo())
        {
            _currentScence = i;
            EditorApplication.OpenScene(EditorBuildSettings.scenes[i].path);
        }
    }
}

public class MyEditor : EditorWindow
{

    [MenuItem("GameObject/wind3ow")]
    static void AddWindow()
    {
        //创建窗口
        Rect wr = new Rect(0, 0, 800, 150);
        MyEditor window = (MyEditor)EditorWindow.GetWindowWithRect(typeof(MyEditor), wr, true, "widow name");
        window.Show();
    }

    // 源文件路径
    private string SourceFolder = @"F:\luaDemo\luaDemo2\Lua";
    // 目标文件路径
    private string AimsFolder = @"D:\workproject\moyu0321\EudemonsClient\Assets\Resources\Lua";
    //选择贴图的对象
    //private Texture texture;

    public void Awake()
    {
        //在资源中读取一张贴图
        //texture = Resources.Load("1") as Texture;
    }
    public static void ChangeFileContent(string sourceFilePath, string aimFilePath)
    {
        // *************** 清空原文件的内容 **************/
        FileStream stream2 = File.Open(aimFilePath, FileMode.OpenOrCreate, FileAccess.Write);
        stream2.Seek(0, SeekOrigin.Begin);
        stream2.SetLength(0); //清空txt文件
        stream2.Close();

        // *************** 把目标文件的内容赋值到文件 **************/
        FileStream rfs = new FileStream(sourceFilePath, FileMode.Open, FileAccess.Read, FileShare.Read); //打开源文件读
        FileStream wfs = new FileStream(aimFilePath, FileMode.Open, FileAccess.Write); //打开源文件读
        for (int i = 0; i < rfs.Length; i++)
            wfs.WriteByte((byte)rfs.ReadByte()); //从一个文件写到另一个文件
        rfs.Dispose();
        wfs.Dispose();
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourcePath"> 源文件夹路径</param>
    /// <param name="sourceFileName">当前的文件名字</param>
    /// <param name="aimPath">路径文件夹路径</param>
    public void CopyFile(string sourcePath, string sourceFileName, string aimPath)
    {
        /************ 复制文件 *********/
        if (!File.Exists(Path.Combine(aimPath, sourceFileName)))
            File.Copy(Path.Combine(sourcePath, sourceFileName), Path.Combine(aimPath, sourceFileName));
        else
            /********** 改变文件内容 ***********/
            ChangeFileContent(Path.Combine(sourcePath, sourceFileName), Path.Combine(aimPath, sourceFileName));
    }
    private void CopyFolderFile(string SourceFolder, string AimsFolder)
    {
        if (!System.IO.Directory.Exists(SourceFolder))
        {
            EditorUtility.DisplayDialog("Error", "源路径有错" + SourceFolder, "OK");
            return;
        }
        if (!System.IO.Directory.Exists(AimsFolder))
            Directory.CreateDirectory(AimsFolder);

        /************ 删除不存在的文件 **********/
        string[] str = Directory.GetFiles(SourceFolder);
        string[] strAim = Directory.GetFiles(AimsFolder);
        foreach (var item in strAim)
        {
            bool notHaveThisFile = false;
            foreach (var item1 in str)
            {
                if (item1 == item)
                {
                    notHaveThisFile = true;
                    break;
                }

            }
            if (!notHaveThisFile)
                File.Delete(item);
        }

        /************ 复制文件 **********/
        foreach (var item in str)
        {
            if (item.EndsWith("meta"))
                continue;
            FileInfo info = new FileInfo(item);
            CopyFile(SourceFolder, info.Name, AimsFolder);
        }
    }

    public void CopyDirectory(string sourcePath, string dirName, string AimPath)
    {
        string AimFullPath = Path.Combine(AimPath, dirName);
        if (!System.IO.Directory.Exists(AimFullPath))
            Directory.CreateDirectory(AimFullPath);

        CopyFolderFile(sourcePath, AimFullPath);
    }
    //绘制窗口时调用
    void OnGUI()
    {
        string source = PlayerPrefs.GetString("source");
        string goal = PlayerPrefs.GetString("goal");

        //输入框控件
        SourceFolder = EditorGUILayout.TextField("源文件的路径:", source);
        SourceFolder = SourceFolder.Trim();

        if (SourceFolder != source)
            PlayerPrefs.SetString("source", SourceFolder);

        AimsFolder = EditorGUILayout.TextField("目标文件路径:", goal);
        AimsFolder = AimsFolder.Trim();

        if (goal != AimsFolder)
            PlayerPrefs.SetString("goal", AimsFolder);

        if (GUILayout.Button("执行复制", GUILayout.Width(200)))
        {
            try
            {
                CopyTotal2(SourceFolder, AimsFolder);
                AssetDatabase.Refresh();
            }
            catch (Exception)
            {
                EditorUtility.DisplayDialog("Error", "权限问题再执行一次就可以了", "OK");
                throw;
            }
        }

        if (GUILayout.Button("关闭通知", GUILayout.Width(200)))
        {
            //关闭通知栏
            this.RemoveNotification();
        }

        //文本框显示鼠标在窗口的位置
        EditorGUILayout.LabelField("鼠标在窗口的位置", Event.current.mousePosition.ToString());

        //选择贴图
        //texture = EditorGUILayout.ObjectField("添加贴图", texture, typeof(Texture), true) as Texture;

        if (GUILayout.Button("关闭窗口", GUILayout.Width(200)))
        {
            //关闭窗口
            this.Close();
        }

    }

    public void CopyTotal2(string SourceFolder, string AimsFolder)
    {
        if (System.IO.Directory.Exists(SourceFolder))
        {
            DirectoryInfo SourceFolderInfo = new DirectoryInfo(SourceFolder);
            // 没有这个文件夹就创建一下.没有我输入的文件夹
            if (!System.IO.Directory.Exists(AimsFolder))
            {
                // 第一步创建
                Directory.CreateDirectory(AimsFolder);
                // 如果我输入的和他一样， 
                DirectoryInfo anis2 = new DirectoryInfo(AimsFolder);
                if (anis2.Name == SourceFolderInfo.Name)
                    CopyTotal(SourceFolder, AimsFolder); // 直接执行这个文件
                else
                {
                    // 如果不一样， 继续创建一个文件件， 然后再执行
                    Directory.CreateDirectory(Path.Combine(AimsFolder, SourceFolderInfo.Name));
                    CopyTotal(SourceFolder, Path.Combine(AimsFolder, SourceFolderInfo.Name));
                }
            }
            else
            {
                if (new DirectoryInfo(AimsFolder).Name == SourceFolderInfo.Name)
                    CopyTotal(SourceFolder, AimsFolder);
                else
                {

                    // 已经有这个文件夹，查询下面的文件夹有没有我需要的文件夹
                    bool haveFile = false;
                    DirectoryInfo[] strAim24 = new DirectoryInfo(AimsFolder).GetDirectories();
                    foreach (var item in strAim24)
                    {
                        if (item.Name.Equals(SourceFolderInfo.Name))
                        {
                            // 如果有， 直接执行，
                            CopyTotal(SourceFolder, item.FullName);
                            return;
                        }
                    }
                    if (!haveFile) // 如果没有， 我自己创建一个
                        Directory.CreateDirectory(Path.Combine(AimsFolder, SourceFolderInfo.Name));
                    CopyTotal(SourceFolder, Path.Combine(AimsFolder, SourceFolderInfo.Name));
                }
            }
        }
        else
        {
            EditorUtility.DisplayDialog("Error", "源路径有错" + SourceFolder, "OK");
        }
        AssetDatabase.Refresh();
    }

    private void CopyTotal(string SourceFolder, string AimsFolder)
    {
        CopyFolderFile(SourceFolder, AimsFolder);

        if (!System.IO.Directory.Exists(AimsFolder))
            Directory.CreateDirectory(AimsFolder);

        string[] listFolderFullName = Directory.GetDirectories(SourceFolder);
        if (listFolderFullName == null || listFolderFullName.Length == 0)
            return;

        /************ 删除不存在的文件夹 **********/
        string[] str = Directory.GetDirectories(SourceFolder);
        string[] strAim = Directory.GetDirectories(AimsFolder);
        foreach (var item in strAim)
        {
            bool notHaveThisFile = false;
            foreach (var item1 in str)
            {
                if (item1 == item)
                {
                    notHaveThisFile = true;
                    break;
                }

            }
            if (!notHaveThisFile)
                Directory.Delete(item, true);
        }

        /************ 复制文件夹内容 **********/
        foreach (var folderFullPath in listFolderFullName)
        {
            DirectoryInfo info = new DirectoryInfo(folderFullPath);
            CopyTotal(folderFullPath, Path.Combine(AimsFolder, info.Name));
        }
    }

    //更新
    void Update()
    {

    }

    void OnFocus()
    {
        Debug.Log("当窗口获得焦点时调用一次");
    }

    void OnLostFocus()
    {
        Debug.Log("当窗口丢失焦点时调用一次");
    }

    void OnHierarchyChange()
    {
        Debug.Log("当Hierarchy视图中的任何对象发生改变时调用一次");
    }

    void OnProjectChange()
    {
        Debug.Log("当Project视图中的资源发生改变时调用一次");
    }

    void OnInspectorUpdate()
    {
        //Debug.Log("窗口面板的更新");
        //这里开启窗口的重绘，不然窗口信息不会刷新
        this.Repaint();
    }

    void OnSelectionChange()
    {
        //当窗口出去开启状态，并且在Hierarchy视图中选择某游戏对象时调用
        foreach (Transform t in Selection.transforms)
        {
            //有可能是多选，这里开启一个循环打印选中游戏对象的名称
            Debug.Log("OnSelectionChange" + t.name);
        }
    }

    void OnDestroy()
    {

        Debug.Log("当窗口关闭时调用");
    }
}

#if 事件系统代码

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class PListNode {
    public PListNode next;
    public PListNode prev;
}

public class PList : PListNode {
    public int Count = 0;

    public PList () {
        Init ();
    }

    public bool Empty () {
        return this.prev == this;
    }

    public void Init () {
        this.next = this;
        this.prev = this;
        this.Count = 0;
    }

    public void Add (PListNode node) {
        // p1 的前面指向了整个p2    p2的后面指向了整个p1   p2的前面指向了整个p1     p1的后面指向了整个p2 

        // p2的前面指向了整个p3    p3的后面指向了整个p2   p3的前面指向了整个p1   p1的后面 指向了p3 

        this.next.prev = node;
        node.next = this.next;
        node.prev = this;
        this.next = node;
        this.Count++;
    }

    public void AddTail (PListNode node) {

        //p1的前面指向了整个p2     p2的后面指向整个p1   p2的前面指向了整个p1     p1的后面指向了整个p2 

        // p1的前面指向了整个p3     p3的后面， 指向了整个p1   p3的前面指向了整个p2   p2的后面指向了整个p3 

        // p这个是我自己this p1
        PListNode p = prev;
        // node p2 在p1 的前面
        this.prev = node;
        // p1在p2的后面
        node.next = this;
        // p1 变成了p2的前面
        node.prev = p;
        // p2 在p1的后面
        p.next = node;
        Count++;
    }

    public void Remove (PListNode node) {
        node.prev.next = node.next;
        node.next.prev = node.prev;
        node.prev = null;
        node.next = null;
        Count--;
    }

    public PListNode Pop () {
        if (prev == this)
            return null;

        PListNode node = this.next;
        node.prev.next = node.next;
        node.next.prev = node.prev;
        node.prev = null;
        node.next = null;
        Count--;

        return node;
    }

    public void AddListTail (PList list) {
        if (list.next == list)
            return;

        PListNode first = list.next;
        PListNode last = list.prev;
        PListNode at = this.prev;

        at.next = first;
        first.prev = at;

        this.prev = last;
        last.next = this;

        this.Count += list.Count;
        list.Init ();
    }
}

public class CObject { }
public interface IEvent { }

public class EventManager2 {
    public delegate void OnEventRecv<T> (CObject sender, T e) where T : IEvent;
    public interface Handler {
        int Key { get; }
        Type EventType { get; }
        void Call (CObject s, ref IEvent e);
    }

    internal class HandlerGroup {

        private Handler handlerHead;

        public Handler Create<T> (OnEventRecv<T> recv) where T : IEvent {
            Handler temp;
            if (handlerHead == null)
                handlerHead = new Handler2 ();

            temp = new Handler2 (recv);
            Handler2 handler2 = handlerHead as Handler2;
            handler2.MyAdd ((Handler2) temp);

            /// 1 指向2    2 指向1             头和他是一样的， 那么删除头，  
            // 因为我指向了他， 所以

            return temp;
        }

        public void RemoveAll () {
            Handler2 h = (Handler2) handlerHead;
            h.MyRemoveAll ();
        }

        public void Remove (Handler node) {

            //if (node == handlerHead)
            //{
            //    UnityEngine.Debug.LogError("111");
            //}

            //if (((Handler2)node).next == node)
            //{
            //    UnityEngine.Debug.LogError("2222");
            //}
            // 传入的是头.  头当前和头相等， 

            // 头的前面，指向了第二
            // 第二的后面， 执行了头
            // 第二的前面， 指向了头
            // 后的后面， 执行了第二  
            Handler2 hand = (Handler2) node;
            Handler2 h2 = ((Handler2) handlerHead);
            h2.MyRemove (hand);
            return; //---------------------------------

            if (node == h2 && h2.Count == 2) {
                handlerHead = (Handler2) h2.next;
                h2.Remove (h2.next);
                return;
            }
            PListNode tempnode;;

            for (int i = 0; i < h2.Count; i++) {
                if (h2.next == node) {
                    h2.Remove (h2.next);
                    break;
                }
                h2 = (Handler2) h2.next;
            }
        }

        public void CallHandler<T> (CObject s, T e) where T : IEvent {
            Handler2 handlerTemp = handlerHead as Handler2;

            while (handlerTemp.next != null) {
                handlerTemp = (Handler2) handlerTemp.next;
                handlerTemp.Call444444<T> (s, e);
            }
            return;

            int count_ = handlerTemp.Count;
            int i = 0;
            while (true) {
                handlerTemp.Call444444<T> (s, e);
                handlerTemp = (Handler2) handlerTemp.next;
                ++i;
                if (i >= count_)
                    break;
            }
        }
    }

    public Handler Reg<T> (EventManager2.OnEventRecv<T> callBack) where T : IEvent {
        Type type = typeof (T);
        HandlerGroup h;
        if (!event_handlers.TryGetValue (type, out h)) {
            h = new HandlerGroup ();
            event_handlers.Add (type, h);
        }
        return h.Create<T> (callBack);
    }
    public void FireEvent<T> (CObject sender, T t) where T : IEvent {
        Type d = t.GetType ();
        HandlerGroup h;
        if (event_handlers.TryGetValue (d, out h)) {
            h.CallHandler<T> (sender, t);
        }
    }

    public void UnReg<T> (Handler callBack) where T : IEvent {
        Type d = typeof (T);
        HandlerGroup h;
        if (event_handlers.TryGetValue (d, out h)) {
            h.Remove (callBack);
        }
    }
    public void UnRegAll<T> () where T : IEvent {
        Type d = typeof (T);
        HandlerGroup h;
        if (event_handlers.TryGetValue (d, out h)) {
            h.RemoveAll ();
        }
    }

    public void FireEvent<T> (T e) where T : IEvent {

        FireEvent<T> (new CObject (), e);
    }

    private Map<Type, HandlerGroup> event_handlers = new Map<Type, HandlerGroup> ();
    public class Handler2 : PList, Handler {
        private int key;
        private Type eventType;
        private object recv;

        public Type EventType { get { return this.eventType; } }
        public int Key { get { return this.key; } }

        public Handler2 (System.Object obj) : base () {
            recv = obj;
        }

        public Handler2 () {
            MyInit ();
        }
        public void Call444444<T> (CObject s, T e) where T : IEvent {
            if (recv == null) {
                UnityEngine.Debug.LogError ("you kong ");
                return;
            }
            if (recv is OnEventRecv<T>) {
                OnEventRecv<T> dddddd5 = (OnEventRecv<T>) recv;
                dddddd5 (s, e);
            }
        }
        void Call (CObject s, ref IEvent e) {

        }

        void Handler.Call (CObject s, ref IEvent e) {

        }
    }
}
public class dddddd : IEvent {

}

public class Evnet {
    EventManager2 d = new EventManager2 ();
    //public void Reg<T>(EventManager2.OnEventRecv redv, T t) where T : IEvent
    //{
    //    //d.Reg<T>(redv, t);
    //}
}

// ***************  测试代码 ******************
//EventManager2.Handler a1;
//EventManager2.Handler a2;
//EventManager2.Handler a3;
//EventManager2 e2 = new EventManager2();
//a1 = e2.Reg<testIeven>((a, b) =>
//       {
//           Debug.LogError("1");
//       });
//        a2 = e2.Reg<testIeven>((a, b) =>
//    {
//        Debug.LogError("bbbb:"+ b.str); 
//        Debug.LogError("2");
//    });

//        a3 = e2.Reg<testIeven>((a, b) =>
//        {
//            Debug.LogError("3");
//        });
//         e2.Reg<testIeven>((a, b) =>
//        {
//            Debug.LogError("4");
//        });
//        e2.Reg<testIeven>((a, b) =>
//        {
//            Debug.LogError("5");
//        });
//        e2.Reg<testIeven>((a, b) =>
//        {
//            Debug.LogError("bbbb:" + b.str);
//            Debug.LogError("6");
//        });
//        e2.Reg<testIeven>((a, b) =>
//        {
//            Debug.LogError("7");
//        });

//        e2.UnReg<testIeven>(a3);
//        e2.UnReg<testIeven>(a1);
//        //e2.UnReg<testIeven>(a2);
//        //e2.UnRegAll<testIeven>(); 
//        //e2.UnRegAll<testIeven>(); 
//        e2.FireEvent<testIeven>(new testIeven("999999999999999999"));   
#endif

#if 编辑器扩展
GUI.lable
GUI.Box
GUI.TextField
Gui.HorizontalSlider
Rect
EditorGUILayout.TextField ("输入技能ID号", ChooseModelID);

//GUILayout.Button(); 
//https://unity3d.com/cn/learn/tutorials/topics/interface-essentials/adding-buttons-custom-inspector

第一节Editor常用的标签
(1)  [InitializeOnLoad]
[InitializeOnLoad]  // 启动unity的时候，把这个脚本放在编辑器下 会执行下面的静态构造方法
/** Checking for updates on startup */  
public static class UpdateChecker {  
    static UpdateChecker()  
    {  
        Debug.LogError("执行了开启加载"); 
    }  
}  

(2)  [ContextMenu("OutputInfo")] // 运行状态下，在一个MonoBehaviour 
                                 // 脚本中的OutputInfo方法上去加入这个标签，在Inspector面板上右键那个脚本，
                                 // 多出 OutputInfo 这个选项，点击就会执行这个OutputInfo方法

    [ContextMenuItem("OutputInfo")]
    void OutputInfo()               
    {                               
     print("我是那个脚本带的方法");
    }


     [ContextMenuItem("OutputInfo")]  // 运行状态下，在一个MonoBehaviour 
        public int i;                // 脚本中的OutputInfo方法上去加入这个标签，在Inspector面板上右键那个字段，
                                  // 多出 OutputInfo 这个选项，点击就会执行这个OutputInfo方法
     
    

(3)  [Tooltip("用于设置性别!")]       // 在sex字段上面加上这个标签，在inspector面板上鼠标停在这个字段
                                     // 就会显示你设置的描述
     public string sex;

(4)  [Space(100)]                    // 字段上面加上这个标签，在inspector面板上，这个字段和下个字段就会留出100px的空隙
     public string sss;

(5)  [Header("我是标题")]            // 给字段加这个，在inspector面板上，这个字段的头顶上多一个标题
(6)  [Multiline(5)]                 // 只能用于string， 让string的行数扩大到5
(7)  [Range(-2, 2)]                 // 只能用于int float, 给字段加这个，作用在inspector面板上，增加一个滑块


(8)  [HideInInspector] [SerializeField] ,有些东西要影藏
     [NonSerialized] 

(9)  [ExecuteInEditMode]  编辑器模式下执行Mono脚本

(10)  [System.Serializable] 在inspector面板上序列号一个普通的类，需要给这个类加上此标签


第二节 Editor常用的标签

#endif