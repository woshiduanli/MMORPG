using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System;


public class SVNTools
{
    enum SVN_COMMAND_TYPE
    {

        UPDATE,
        COMMIT,
        LOG,
        UpdateRes,
        CommitRes,

        // 生成DLL
        GeDLL,
        CommitUI,
        CommitLua,
        CommitAll,
        PushAll,
        CommitScripts,
        RevertMust,

    }

  

    [MenuItem("SVN/      ")]
    private static void SVNToolUpdateRes3()
    {
        // If() {}
        //SVNBatCommandAndRun(SVN_COMMAND_TYPE.UpdateRes);;;;;
        // iiif (fdsf
        //fffdffdsfsfifif(){}
        // nsdfsf
    }




    [MenuItem("SVN/更新Client")]
    private static void SVNToolUpdateAll()
    {
        SVNBatCommandAndRun(SVN_COMMAND_TYPE.UPDATE);
    }

    [MenuItem("SVN/更新Res")]
    private static void SVNToolUpdateRes()
    {
        SVNBatCommandAndRun(SVN_COMMAND_TYPE.UpdateRes);
    }

    [MenuItem("SVN/      2")]
    private static void SVNToolUpdateRes32()
    {
        //SVNBatCommandAndRun(SVN_COMMAND_TYPE.UpdateRes);
    }


    [MenuItem("SVN/提交AllUI")]
    private static void SVNCommitUI()
    {
        SVNBatCommandAndRun(SVN_COMMAND_TYPE.CommitUI);
    }

    [MenuItem("SVN/提交Lua")]
    private static void SVNCommitLua()
    {
        SVNBatCommandAndRun(SVN_COMMAND_TYPE.CommitLua);
    }

    [MenuItem("SVN/提交Scripts")]
    private static void SVNCommitScripts()
    {
        SVNBatCommandAndRun(SVN_COMMAND_TYPE.CommitScripts);
    }



    [MenuItem("SVN/      3")]
    private static void SVNToolUpdateRes5()
    {
        //SVNBatCommandAndRun(SVN_COMMAND_TYPE.UpdateRes);
    }

    [MenuItem("SVN/提交Res")]
    private static void SVNToolCommitRes()
    {
        SVNBatCommandAndRun(SVN_COMMAND_TYPE.CommitRes);
    }

    [MenuItem("SVN/推送Client")]
    private static void SVNToolPushClient()
    {
        SVNBatCommandAndRun(SVN_COMMAND_TYPE.PushAll);
    }
    [MenuItem("SVN/      4")]
    private static void SVNToolUpdateRes6()
    {
        //SVNBatCommandAndRun(SVN_COMMAND_TYPE.UpdateRes);
    }

    [MenuItem("SVN/生成策划用的DLL")]
    private static void SVNToolCommitResDLL()
    {
        SVNBatCommandAndRun(SVN_COMMAND_TYPE.GeDLL);
    }

    public static void ClearTextContent(string assetPath)
    {
        FileStream stream2 = File.Open(assetPath, FileMode.OpenOrCreate, FileAccess.Write);
        stream2.Seek(0, SeekOrigin.Begin);
        stream2.SetLength(0); //清空txt文件
        stream2.Close();
        stream2.Dispose();
    }
    static string GitCommandStr;
    //string strtest
    [MenuItem("SVN/还原此对象所在的文件夹")]
    private static void SVNToolReverseCommand()
    {

        //string sssss =    ApplicationEditor.GetDictory();
        //CClientCommon.Debug("ssss:"+sssss);

        GitCommandStr = "\"C:/Program Files/TortoiseGit/bin/TortoiseGitProc.exe\" /command:revert /path:\".\\";

        string GitFilePath = Application.dataPath;
        GitFilePath = GitFilePath.Substring(0, GitFilePath.Length - "assets".Length);
        GitFilePath = GitFilePath + "mycommit";
        if (!Directory.Exists(GitFilePath))
            Directory.CreateDirectory(GitFilePath);

        GitFilePath = GitFilePath + "/mycommit.bat";
        if (!File.Exists(GitFilePath))
            File.Create(GitFilePath);

        // 清空文件内容
        ClearTextContent(GitFilePath);
        // --------------分界线
        string ClickPath = ApplicationEditor.GetDictory();
        ClickPath = ClickPath.Replace("/", "\\");
        GitCommandStr = GitCommandStr + ClickPath;

        StreamWriter write = File.CreateText(GitFilePath);
        write.Write(GitCommandStr);
        write.Close();
        write.Dispose();
        SVNBatCommandAndRun(SVN_COMMAND_TYPE.RevertMust);
    }


    [MenuItem("SVN/还原此对象")]
    private static void SVNToolReverseCommand2()
    {
        GitCommandStr = "\"C:/Program Files/TortoiseGit/bin/TortoiseGitProc.exe\" /command:revert /path:\".\\";

        string GitFilePath = Application.dataPath;
        GitFilePath = GitFilePath.Substring(0, GitFilePath.Length - "assets".Length);
        GitFilePath = GitFilePath + "mycommit";
        if (!Directory.Exists(GitFilePath))
            Directory.CreateDirectory(GitFilePath);

        GitFilePath = GitFilePath + "/mycommit.bat";
        if (!File.Exists(GitFilePath))
            File.Create(GitFilePath);

        // 清空文件内容
        ClearTextContent(GitFilePath);


        UnityEngine.Object obj = Selection.activeObject;
        string lupaht = AssetDatabase.GetAssetPath(obj);
        lupaht = lupaht.Replace("/", "\\");

        GitCommandStr = GitCommandStr + lupaht;

        StreamWriter write = File.CreateText(GitFilePath);
        write.Write(GitCommandStr);
        write.Close();
        write.Dispose();
        SVNBatCommandAndRun(SVN_COMMAND_TYPE.RevertMust);
    }

    [MenuItem("SVN/      5")]
    private static void SVNToolShuziCommand()
    {
    }

    [MenuItem("SVN/锁定此文件")]
    private static void SVNToolLockCommand2()
    {
        SVNToolLockCommand(SuoEnum.Lock);
    }

    [MenuItem("SVN/解锁此文件")]
    private static void SVNToolLockCommand3()
    {
        SVNToolLockCommand(SuoEnum.UnLock);
    }

    [MenuItem("SVN/锁定此文件夹")]
    private static void SVNToolLockCommand4()
    {
        SVNToolLockCommandDirectory(SuoEnum.Lock);
    }

    [MenuItem("SVN/解锁此文件夹")]
    private static void SVNToolLockCommand5()
    {
        SVNToolLockCommandDirectory(SuoEnum.UnLock);
    }


    private static void SVNToolLockCommand(SuoEnum suoNum)
    {
        string fullPpath2 = GetClickFileFullPath(Selection.activeObject);
        // 拿到扩展名字
        FileInfo info = new FileInfo(fullPpath2);

        if (suoNum == SuoEnum.Lock)
            fullPpath2 = fullPpath2.Replace(info.Extension, "Lock.bat");
        else if (suoNum == SuoEnum.UnLock)
            fullPpath2 = fullPpath2.Replace(info.Extension, "LockUN.bat");

        // 创建bat
        FileStream file = null;
        if (!File.Exists(fullPpath2))
            file = File.Create(fullPpath2);
        file.Close();
        file.Dispose();

        //写出命令
        GitCommandStr = GetSuoStr(suoNum);
        GitCommandStr= GitCommandStr + info.Name;
        WriteStrToFile(fullPpath2, GitCommandStr);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    private static void SVNToolLockCommandDirectory(SuoEnum suoNum)
    {
        string fullPpath2 = GetClickFileFullPath(Selection.activeObject);
        // 拿到扩展名字
        FileInfo info = new FileInfo(fullPpath2);

        if (suoNum == SuoEnum.Lock)
            fullPpath2 = fullPpath2.Replace(info.Extension, "Lock.bat");
        else if (suoNum == SuoEnum.UnLock)
            fullPpath2 = fullPpath2.Replace(info.Extension, "LockUN.bat");

        // 创建bat
        FileStream file = null;
        if (!File.Exists(fullPpath2))
            file = File.Create(fullPpath2);
        file.Close();
        file.Dispose();

        //写出命令
        GitCommandStr = GetSuoStr(suoNum);
        GitCommandStr = GitCommandStr + "*.cs";
        WriteStrToFile(fullPpath2, GitCommandStr);
        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    public enum SuoEnum
    {
        Lock,
        UnLock
    }

    public static string GetSuoStr(SuoEnum suoEnum)
    {
        if (SuoEnum.Lock == suoEnum)
            return "git update-index --assume-unchanged ";
        else if (SuoEnum.UnLock == suoEnum)
            return "git update-index --no-assume-unchanged ";
        return "";
    }

    public static void WriteStrToFile(string filePath, string content)
    {
        if (!File.Exists(filePath))
            File.Create(filePath);
        ClearTextContent(filePath);
        StreamWriter write = File.CreateText(filePath);
        write.Write(content);
        write.Close();
        write.Dispose();
    }

  

    /// <summary>
    ///  获取文件的全路径
    /// </summary>
    /// <returns></returns>
    private static string GetClickFileFullPath(UnityEngine.Object obj)
    {
        string GitFilePath = Application.dataPath;
        GitFilePath = GitFilePath.Substring(0, GitFilePath.Length - "assets".Length);
        UnityEngine.Object obj2 = obj;
        string lupaht3 = AssetDatabase.GetAssetPath(obj2);
        GitFilePath = GitFilePath + lupaht3;
        return GitFilePath;
    }

    /// <summary>
    ///  获取文件所在文件夹的全路径
    /// </summary>
    /// <returns></returns>
    private static string GetClickDirectoyFullPath()
    {
        string path = GetClickFileFullPath(Selection.activeObject);
        FileInfo info = new FileInfo(path);
        return info.Directory.FullName;
    }

  

    private static void RevertMustFunctionTestSuo(object obj)
    {

        //C:\WorkProject\MyStudy2\EudemonsClient\Assets\Script\GameUI\Battleground

        //"C:/WorkProject/MyStudy2/EudemonsClient/Assets/Script/GameUI/Battleground.bat";
        //CClientCommon.Debug("huai le e -------");
        string updatePath = obj.ToString();
        //UpdateClient
        string batFilePath = "C:/WorkProject/MyStudy2/EudemonsClient/Assets/Script/GameUI/Battleground/unsuo.bat";
        //CClientCommon.Debug("地址：" + batFilePath);
        //CClientCommon.Debug("更新批准被：" + batFilePath);
        ////return;
        Process p = new Process();
        p.StartInfo.FileName = batFilePath;
        p.StartInfo.Arguments = updatePath;
        p.Start();

        p.WaitForExit();
        p.Close();
    }

    public static void CallBat()
    {
        Process proc = null;
        try
        {

            //C:/WorkProject/MyStudy2/EudemonsClient/mycommit/mycommit.bat
            string targetDir = string.Format(@"C:/WorkProject/MyStudy2/EudemonsClient/mycommit/");//this is where testChange.bat lies
            proc = new Process();
            proc.StartInfo.WorkingDirectory = targetDir;
            proc.StartInfo.FileName = "mycommit.bat";
            proc.StartInfo.Arguments = string.Format("10");//this is argument
                                                           //proc.StartInfo.CreateNoWindow = true;
                                                           //proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;//这里设置DOS窗口不显示，经实践可行
            proc.Start();
            proc.WaitForExit();

            //CClientCommon.Debug ();

        }
        catch (Exception ex)
        {
            //CClientCommon.Debug("chu guo le error --------");
        }
    }

    private static string GetPath()
    {

        UnityEngine.Object obj = Selection.activeObject;

        return "";
    }

    //    @echo off
    //echo 魔域更新 客户端资源-----------------------
    //"C:/Program Files/TortoiseGit/bin/TortoiseGitProc.exe" /command:commit /path:".\Assets\Lua\Activity\Battleground.lua" /logmsg:"Lua修改"
    //exit



    enum CommandEnum
    {
        Commit,
        Revert
    }

    private string GitGetPath(CommandEnum commane, string path)
    {
        string path_ = "";
        switch (commane)
        {
            case CommandEnum.Commit:

                string str = "\"C:/Program Files/TortoiseGit/bin/TortoiseGitProc.exe\" /command:";



                break;
            case CommandEnum.Revert:
                break;
            default:
                break;
        }
        return path_;

    }




    [MenuItem("SVN/提交AllClient")]
    private static void SVNToolCommitResAll()
    {
        //SVNBatCommandAndRun(SVN_COMMAND_TYPE.RevertMust);
        SVNBatCommandAndRun(SVN_COMMAND_TYPE.CommitAll);
    }

    /// <summary>  
    /// 获取选中文件路径  
    /// </summary>  
    /// <returns></returns>  
    private static string GetSelectPath()
    {
        List<string> assetPaths = new List<string>();
        string path = null;
        foreach (var guid in UnityEditor.Selection.assetGUIDs)
        {
            if (string.IsNullOrEmpty(guid)) continue;

            path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
            if (!string.IsNullOrEmpty(path))
                assetPaths.Add(path);
        }

        if (assetPaths == null) return null;

        path = "";
        string assetPath = null;
        for (int i = 0; i < assetPaths.Count; ++i)
        {
            assetPath = assetPaths[i];
            if (i != 0)
                path += "*";
            path += assetPath;
        }

        return path;
    }

    /// <summary>  
    /// 调用小乌龟SVN  
    /// </summary>  
    /// <param name="commandType"></param>  
    /// <param name="path"></param>  
    private static void SVNCommandAndRun2(SVN_COMMAND_TYPE commandType, string path = null)
    {
        #region //拼接小乌龟svn命令  
        string command = "TortoiseProc.exe /command:";
        switch (commandType)
        {
            case SVN_COMMAND_TYPE.UPDATE:
                command += "update /path:\"";
                break;
            case SVN_COMMAND_TYPE.COMMIT:
                command += "commit /path:\"";
                break;
            case SVN_COMMAND_TYPE.LOG:
            default:
                command += "log /path:\"";
                break;
        }
        if (path == null || path == "")
        {
            command += Application.dataPath;
            command = command.Substring(0, command.Length - 6);
        }
        else
            command += path;
        command += "\"";
        command += " /closeonend:0";
        //UnityEngine.Debug.LogError("command: " + command);  
        #endregion

        // 新开线程防止锁死  
        Thread newThread = new Thread(new ParameterizedThreadStart(SVNCmdThread));
        newThread.Start(command);
    }


    /// <summary>  
    /// 调用小乌龟SVN  
    /// </summary>  
    /// <param name="commandType"></param>  
    /// <param name="path"></param>  
    private static void SVNCommandAndRun(SVN_COMMAND_TYPE commandType, string path = null)
    {
        #region //拼接小乌龟svn命令  
        string command = "TortoiseProc.exe /command:";
        switch (commandType)
        {
            case SVN_COMMAND_TYPE.UPDATE:
                command += "update /path:\"";
                break;
            case SVN_COMMAND_TYPE.COMMIT:
                command += "commit /path:\"";
                break;
            case SVN_COMMAND_TYPE.LOG:
            default:
                command += "log /path:\"";
                break;
        }
        if (path == null || path == "")
        {
            command += Application.dataPath;
            command = command.Substring(0, command.Length - 6);
        }
        else
            command += path;
        command += "\"";
        command += " /closeonend:0";
        //UnityEngine.Debug.LogError("command: " + command);  
        #endregion

        // 新开线程防止锁死  
        Thread newThread = new Thread(new ParameterizedThreadStart(SVNCmdThread));
        newThread.Start(command);
    }

    private static void SVNCmdThread(object obj)
    {
        Process p = new Process();
        p.StartInfo.FileName = "cmd.exe";
        //UnityEngine.Debug.LogError("command: " + obj.ToString());  
        p.StartInfo.Arguments = "/c " + obj.ToString();
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.CreateNoWindow = true;
        p.Start();

        p.WaitForExit();
        p.Close();
    }


    private static void SVNBatCommandAndRun(SVN_COMMAND_TYPE commandType)
    {
        string updatePath = Application.dataPath;
        updatePath = updatePath.Substring(0, updatePath.Length - 6);

        Thread newThread = null;
        switch (commandType)
        {
            case SVN_COMMAND_TYPE.CommitScripts:
                newThread = new Thread(new ParameterizedThreadStart(CommitScriptsFunction));
                break;

            case SVN_COMMAND_TYPE.RevertMust:
                newThread = new Thread(new ParameterizedThreadStart(RevertMustFunction));
                break;
            case SVN_COMMAND_TYPE.PushAll:
                newThread = new Thread(new ParameterizedThreadStart(PushClientFunction));
                break;
            case SVN_COMMAND_TYPE.UPDATE:
                newThread = new Thread(new ParameterizedThreadStart(UpdateClientFunction));
                break;
            case SVN_COMMAND_TYPE.CommitLua:
                newThread = new Thread(new ParameterizedThreadStart(CommitLuaFunction));
                break;
            case SVN_COMMAND_TYPE.CommitUI:
                newThread = new Thread(new ParameterizedThreadStart(CommitUIFunction));
                break;
            case SVN_COMMAND_TYPE.CommitAll:
                newThread = new Thread(new ParameterizedThreadStart(CommitAllFunction));
                break;
            case SVN_COMMAND_TYPE.COMMIT:
                newThread = new Thread(new ParameterizedThreadStart(CommitBatThread));
                break;
            case SVN_COMMAND_TYPE.UpdateRes:
                newThread = new Thread(new ParameterizedThreadStart(UpdateBatRes));
                break;
            case SVN_COMMAND_TYPE.CommitRes:
                newThread = new Thread(new ParameterizedThreadStart(CommitResFunction));
                break;
            case SVN_COMMAND_TYPE.GeDLL:
                ApplicationEditor.DebugLua13();
                newThread = new Thread(new ParameterizedThreadStart(GeDLLFunction));
                break;
        }
        // 新开线程防止锁死  
        if (newThread != null)
            newThread.Start(updatePath);
    }

    private static void GeDLLFunction(object obj)
    {
        string updatePath = obj.ToString();
        string batFilePath = updatePath + "build.bat";
        //CClientCommon.Debug("更新批准被：" + batFilePath);
        Process p = new Process();
        p.StartInfo.FileName = batFilePath;
        p.StartInfo.Arguments = updatePath;
        p.Start();

        p.WaitForExit();
        p.Close();
    }
    private static void CommitAllFunction(object obj)
    {
        string updatePath = obj.ToString();
        string batFilePath = updatePath + "MyGitCommand/CommitAll.bat";
        //CClientCommon.Debug("更新批准被：" + batFilePath);
        Process p = new Process();
        p.StartInfo.FileName = batFilePath;
        p.StartInfo.Arguments = updatePath;
        p.Start();

        p.WaitForExit();
        p.Close();
    }

    private static void CommitUIFunction(object obj)
    {
        string updatePath = obj.ToString();
        string batFilePath = updatePath + "MyGitCommand/CommitUI.bat";
        //CClientCommon.Debug("更新批准被：" + batFilePath);
        Process p = new Process();
        p.StartInfo.FileName = batFilePath;
        p.StartInfo.Arguments = updatePath;
        p.Start();

        p.WaitForExit();
        p.Close();
    }
    private static void CommitLuaFunction(object obj)
    {
        string updatePath = obj.ToString();
        string batFilePath = updatePath + "MyGitCommand/CommitLua.bat";
        //CClientCommon.Debug("更新批准被：" + batFilePath);
        Process p = new Process();
        p.StartInfo.FileName = batFilePath;
        p.StartInfo.Arguments = updatePath;
        p.Start();

        p.WaitForExit();
        p.Close();
    }


    private static void CommitResFunction(object obj)
    {
        string updatePath = obj.ToString();
        //UpdateClient
        string batFilePath = updatePath + "MyGitCommand/CommitRes.bat";
        //CClientCommon.Debug("更新批准被：" + batFilePath);
        Process p = new Process();
        p.StartInfo.FileName = batFilePath;
        p.StartInfo.Arguments = updatePath;
        p.Start();

        p.WaitForExit();
        p.Close();
    }

    private static void UpdateBatRes(object obj)
    {
        string updatePath = obj.ToString();
        //UpdateClient
        string batFilePath = updatePath + "MyGitCommand/UpdateRes.bat";
        //CClientCommon.Debug("更新批准被：" + batFilePath);
        Process p = new Process();
        p.StartInfo.FileName = batFilePath;
        p.StartInfo.Arguments = updatePath;
        p.Start();

        p.WaitForExit();
        p.Close();
    }
    private static void RevertMustFunction(object obj)
    {
        //CClientCommon.Debug("huai le e -------");
        string updatePath = obj.ToString();
        //UpdateClient
        string batFilePath = updatePath + "mycommit/mycommit.bat";
        //CClientCommon.Debug("地址：" + batFilePath);
        //CClientCommon.Debug("更新批准被：" + batFilePath);
        //return;
        Process p = new Process();
        p.StartInfo.FileName = batFilePath;
        p.StartInfo.Arguments = updatePath;
        p.Start();

        p.WaitForExit();
        p.Close();
    }

    private static void CommitScriptsFunction(object obj)
    {
        string updatePath = obj.ToString();
        //UpdateClient
        string batFilePath = updatePath + "MyGitCommand/CommitScripts.bat";
        //CClientCommon.Debug("更新批准被：" + batFilePath);
        //return;
        Process p = new Process();
        p.StartInfo.FileName = batFilePath;
        p.StartInfo.Arguments = updatePath; ; ;
        p.Start();

        p.WaitForExit();
        p.Close();
    }


    private static void PushClientFunction(object obj)
    {
        string updatePath = obj.ToString();
        //UpdateClient
        string batFilePath = updatePath + "MyGitCommand/PushClient.bat";
        //CClientCommon.Debug("更新批准被：" + batFilePath);
        //return;
        Process p = new Process();
        p.StartInfo.FileName = batFilePath;
        p.StartInfo.Arguments = updatePath;
        p.Start();

        p.WaitForExit();
        p.Close();
    }

    private static void UpdateClientFunction(object obj)
    {
        string updatePath = obj.ToString();
        //UpdateClient
        string batFilePath = updatePath + "MyGitCommand/UpdateClient.bat";
        //CClientCommon.Debug("更新批准被：" + batFilePath);
        //return;
        Process p = new Process();
        p.StartInfo.FileName = batFilePath;
        p.StartInfo.Arguments = updatePath;
        p.Start();

        p.WaitForExit();
        p.Close();
    }

    private static void CommitBatThread(object obj)
    {
        string batFilePath = obj.ToString() + "/Tools/SVNTools/SVNCommit.bat";

        Process p = new Process();
        p.StartInfo.FileName = batFilePath;
        p.Start();

        p.WaitForExit();
        p.Close();
    }
}