using UnityEngine;
using System.Collections;

//检查并更新资源服务器资源
internal class CheckVersionState : LaunchState
{
    private const string VERSION_FILE = "version.manifest";
    private string RES_SERVER_HOST = "";
    private string up_version = "0.0.0";
    private int total_size = 0;
    private int download_size = 0;

    private float record_time = 0F;
    private int record_size = 0;
    private float record_speed = 0;

    //private Stack<CUrlFile> files = new Stack<CUrlFile>();
    //private CUrlFile current = null;

    public CheckVersionState(Launcher launcher) : base(launcher) { }

    public override void Enter()
    {
        //Tips = ConstCopyFilePath.VerSionTips;
        //this.DownLoad = string.Empty;
        //// 读取资源服务器的地址
        //TextAsset asset = Resources.Load("url") as TextAsset;
        //StringReader sr = new StringReader(asset.text);
        //while ((RES_SERVER_HOST = sr.ReadLine()) != null)
        //{
        //    if (RES_SERVER_HOST.StartsWith("#"))
        //        continue;
        //    RES_SERVER_HOST = RES_SERVER_HOST.Trim(' ', '\r');
        //    if (string.IsNullOrEmpty(RES_SERVER_HOST))
        //        continue;
        //    break;
        //}
        //Resources.UnloadAsset(asset);

        //if (string.IsNullOrEmpty(RES_SERVER_HOST))
        //{
        //    this.up_version = VersionHelper.version;
        //    return;
        //}

        //LOG.Debug("client version : {0}", VersionHelper.version);

        //HTTP.Request http = new HTTP.Request("get", RES_SERVER_HOST + "redirect.txt");
        //http.synchronous = true;
        //http.Send((request) =>
        //{
        //    if (request.response == null || !(request.response.status >= 200 && request.response.status < 300))
        //    {
        //        return;
        //    }
        //    string redirect_url = request.response.Text;
        //    redirect_url = redirect_url.Trim();
        //    if (!redirect_url.EndsWith("/")) redirect_url += "/";
        //    RES_SERVER_HOST = redirect_url;
        //    LOG.Debug("===> redirect to {0}", RES_SERVER_HOST);
        //});

        //HTTP.Request.LogAllRequests = true;
        //http = new HTTP.Request("get", RES_SERVER_HOST + "version.txt");
        //http.synchronous = true;
        //http.Send((request) =>
        //{
        //    if (request.response == null || !(request.response.status >= 200 && request.response.status < 300))
        //        return;
        //    this.up_version = request.response.Text;
        //});

        //string errorStr = string.Empty;
        //// 下载version失败
        //if (this.up_version == "0.0.0")
        //{
        //    errorStr = "资源服务器版本号为：0.0.0";
        //    GSDK.SetEvent(1, false, errorStr, false, false);//检测更新失败
        //    OnVersionError(errorStr);
        //    return;
        //}
        //int up_value = VersionHelper.StrToInt(this.up_version);
        //int current_value = VersionHelper.StrToInt(VersionHelper.version);
        //if (up_value > current_value)
        //{
        //    // 获取cdn的地址, 这步是魔域多出来的
        //    bool get_cdn = false;
        //    http = new HTTP.Request("get", RES_SERVER_HOST + "cdn.txt");
        //    http.synchronous = true;
        //    http.Send((request) =>
        //    {
        //        if (request.response == null || !(request.response.status >= 200 && request.response.status < 300))
        //            return;
        //        string cdn_url = request.response.Text;
        //        cdn_url = cdn_url.Trim();
        //        if (!cdn_url.EndsWith("/")) cdn_url += "/";
        //        RES_SERVER_HOST = cdn_url;
        //        get_cdn = true;
        //        LOG.Debug("===> cdn {0}", RES_SERVER_HOST);
        //    });
        //    if (!get_cdn)
        //    {
        //        OnVersionError("获取cdn地址失败");
        //        return;
        //    }

        //    LOG.Debug("update version from {0} to {1}", VersionHelper.version, up_version);
        //    if ((up_value / 1000) > (current_value / 1000))
        //    {
        //        // 大版本变化，需强制更新
        //        OnForceUpdate();
        //        return;
        //    }

        //    bool error = false;
        //    HashSet<string> filter = new HashSet<string>();

        //    GSDK.SetEvent(1, true, "1-success", false, false);//此处为检测资源更新成功（注意是检测）
        //    // 进行更新
        //    while (up_value > current_value)
        //    {
        //        string up_path = VersionHelper.IntToStr(up_value);
        //        http = new HTTP.Request("get", RES_SERVER_HOST + up_path + "/version.manifest");
        //        http.synchronous = true;
        //        http.Send((request) =>
        //        {
        //            if (request.response == null || !(request.response.status >= 200 && request.response.status < 300))
        //            {
        //                error = true;
        //                errorStr = "资源服务器无反应或者服务器返回状态不为200-300";
        //                return;
        //            }
        //            ParseManifest(up_path, request.response.Text,
        //                delegate(string name)
        //                {
        //                    if (filter.Contains(name))
        //                        return true;
        //                    filter.Add(name);
        //                    return false;
        //                });
        //        });

        //        if (error)
        //        {
        //            OnVersionError(errorStr);
        //            GSDK.SetEvent(2, false, errorStr, false, false);//更新资源失败
        //            break;
        //        }

        //        --up_value;
        //    }

        //    HTTP.Request.LogAllRequests = false;

        //    Progress.Instance.CreateDownLoad(this);
        //}
        //else if (up_value == current_value)
        //{
        //    GSDK.SetEvent(1, true, "0", false, false);
        //}
    }

    public override void Update()
    {
        //Tips = ConstCopyFilePath.DownLoadAssetsTips;
        //if (current == null)
        //{
        //    if (this.files.Count > 0)
        //    {
        //        current = this.files.Pop();
        //        current.Download();
        //    }
        //    else
        //    {
        //        Tips = ConstCopyFilePath.LoadAssetsTips;
        //        this.ProgressValue = 0;
        //        this.DownLoad = string.Empty;
        //        // complete
        //        string manifest_file = CDirectory.MakeCachePath(VERSION_FILE);
        //        System.IO.File.WriteAllText(manifest_file, this.up_version, System.Text.Encoding.UTF8);
        //        VersionHelper.version = this.up_version;
        //    }
        //    return;
        //}
        //else
        //{
        //    if (this.record_time == 0)
        //    {
        //        this.record_time = Time.realtimeSinceStartup;
        //        this.record_size = this.download_size;
        //    }

        //    int volatile_size = this.download_size + current.Length;
        //    if (Time.realtimeSinceStartup >= this.record_time + 1.5f)
        //    {
        //        this.record_speed = (float)(volatile_size - this.record_size);
        //        this.record_time = Time.realtimeSinceStartup;
        //        this.record_size = volatile_size;
        //    }
        //    this.DownLoad = CString.Format(ConstCopyFilePath.DownLoadSizeTips, GetSizeToken(this.total_size));
        //    if (this.total_size > 0)
        //        this.ProgressValue = (float)volatile_size / (float)this.total_size;
        //    if (current.IsDone)
        //    {
        //        if (current.HasError)
        //        {
        //            this.record_time = 0;
        //            OnFileError();
        //            return;
        //        }
        //        if (!current.Save())
        //        {
        //            this.record_time = 0;
        //            OnFileError();
        //            return;
        //        }

        //        this.download_size += current.content_length;
        //        current = null;
        //    }
        //}
    }

    public override void Exit() { }

    public override LaunchState CheckTransition()
    {
        //if (VersionHelper.StrToInt(this.up_version) <= VersionHelper.StrToInt(VersionHelper.version))
        //{
        //    GSDK.SetEvent(2, true, "2-success", false, false);//更新成功
        //    return new EnterGameState(this.launcher);
        //}
        //else
        //    return null;
        return null; 
    }

    private static string[] sizetoken = { "B", "KB", "MB", "GB" };

    private static string GetSizeToken(double size)
    {
        int order = 0;
        while (size >= 1024.0f && order + 1 < sizetoken.Length)
        {
            ++order;
            size /= 1024.0f;
        }
        return string.Format("{0:0.##}{1}", size, sizetoken[order]);
    }

    // 强制更新
    private void OnForceUpdate()
    {
        //this.launcher.cLauncherUI.SetAction(Application.Quit);
        //this.launcher.cLauncherUI.ShowMessagebox(ConstCopyFilePath.CheckVersionForceUpdate);
        //SetPause(true);
        //GSDK.SetEvent(1, true, "2-success", false, false);//此处为检测完整包更新成功（注意是检测）
    }

    // 下载版本出错鸟~
    private void OnVersionError(string errorStr)
    {
        //this.launcher.cLauncherUI.SetAction(Application.Quit);
        //this.launcher.cLauncherUI.ShowMessagebox(ConstCopyFilePath.CheckVersionFailure + ":" + errorStr);
        //SetPause(true);
    }

    // 文件下载出错
    private void OnFileError()
    {
        //System.Action OK = delegate() { current.Download(); SetPause(false); };
        //this.launcher.cLauncherUI.SetAction(OK, Application.Quit);
        //this.launcher.cLauncherUI.ShowMessagebox(ConstCopyFilePath.DownErroMsgStr);
        //SetPause(true);
        //GSDK.SetEvent(2, false, ConstCopyFilePath.DownErroMsgStr, false, false);
    }

    //private void ParseManifest(string field, string value, Predicate<string> filter)
    //{
    //    //StringReader reader = new StringReader(value);
    //    //string line = reader.ReadLine();
    //    //int total_lenth = 0;
    //    //int.TryParse(line, out total_lenth);
    //    //while (!string.IsNullOrEmpty((line = reader.ReadLine())))
    //    //{
    //    //    string[] p = line.Split(',');
    //    //    string name = p[0];
    //    //    string path = p[1];
    //    //    int length = 0;
    //    //    int.TryParse(p[2], out length);
    //    //    string md5 = p.Length > 3 ? p[3] : "";

    //    //    if (filter != null && filter(path + name))
    //    //        continue;

    //    //    CUrlFile file = new CUrlFile(name, RES_SERVER_HOST + field + path, CDirectory.MakeCachePath(path), length, md5);
    //    //    this.files.Push(file);

    //    //    this.total_size += length;
    //    //}
    //    //DownLoad = CString.Format(ConstCopyFilePath.DownLoadSizeTips, GetSizeToken(this.total_size));
    //}
}
