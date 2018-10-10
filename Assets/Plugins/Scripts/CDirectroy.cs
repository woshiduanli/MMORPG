using System;
using System.Text;
using System.IO;
using UnityEngine;

public static class CString
{
    private static StringBuilder cachedStringBuilder;

    private static StringBuilder AcquireBuilder()
    {
        StringBuilder result = cachedStringBuilder;
        if (result == null)
            return new StringBuilder();
        result.Remove(0, result.Length);
        cachedStringBuilder = null;
        return result;
    }

    private static string GetStringAndReleaseBuilder(StringBuilder sb)
    {
        string result = sb.ToString();
        cachedStringBuilder = sb;
        return result;
    }

    public static string Concat(params object[] args)
    {
        if (args == null || args.Length == 0)
            return string.Empty;
        StringBuilder sb = AcquireBuilder();
        for (int i = 0; i < args.Length; i++)
            sb.Append(args[i]);
        return GetStringAndReleaseBuilder(sb);
    }

    public static string Format(string format, params object[] args)
    {
        if (string.IsNullOrEmpty(format) || args == null || args.Length == 0)
            return string.Empty;
        StringBuilder sb = AcquireBuilder();
        sb.AppendFormat(format, args);
        return sb.ToString();
    }
}


public static class CDirectory {
    private static string streaming_patch;
    private static string cache_path;

    static CDirectory() {
        Init();
    }

    public static void Init() {
        streaming_patch = string.Concat(Application.streamingAssetsPath, "/");
        switch ( Application.platform ) {
        case RuntimePlatform.Android:
            cache_path = string.Concat(Application.persistentDataPath , "/");
            break;
        case RuntimePlatform.IPhonePlayer:
            cache_path = string.Concat(Application.persistentDataPath , "/");
            break;
        case RuntimePlatform.OSXEditor:
        case RuntimePlatform.WindowsEditor:
            cache_path = CString.Format( "{0}/../main.dir", Application.dataPath );
            if ( !Directory.Exists( cache_path ) )
                Directory.CreateDirectory( cache_path );
            cache_path = string.Concat(cache_path , "/");
            break;
        case RuntimePlatform.WindowsPlayer:
            cache_path = CString.Format( "{0}/main.dir", Application.dataPath );
            if ( !Directory.Exists( cache_path ) )
                Directory.CreateDirectory( cache_path );
            cache_path = string.Concat(cache_path , "/");
            break;
        }
    }

    /// <summary>
    /// 返回文件路劲（不支持www）
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static string MakeFilePath(string filename)
    {
        if (File.Exists(MakeCachePath(filename)))
            return MakeCachePath(filename);
        else
            return MakeStreamingPath(filename);
    }

    /// <summary>
    /// 返回文件路劲(只用于www)
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public static string MakeFullWWWPath(string filename)
    {
        if (File.Exists(MakeCachePath(filename)))
        {
            return CString.Format("file://{0}{1}", cache_path, filename);
        }
        else
        {
            return MakeWWWStreamPath(filename);
        }
    }

    public static string MakeWWWStreamPath(string filename)
    {
        switch (Application.platform)
        {
            case RuntimePlatform.OSXEditor:
            case RuntimePlatform.IPhonePlayer:
                return CString.Format("{0}{1}", "file://", MakeStreamingPath(filename));
            case RuntimePlatform.Android:
                return MakeStreamingPath(filename);
            default:
                return CString.Format("{0}{1}", "file:///", MakeStreamingPath(filename));
        }
    }
 
    public static string MakeCachePath(string filename)
    {
        return CString.Format("{0}{1}",cache_path , filename);
    } 

    public static string MakeStreamingPath(string filename)
    {
        return CString.Format("{0}{1}", streaming_patch, filename);
    }

    public static string MakeFullMobilePath(string filename)
    {
        string path = string.Empty;
        path = CDirectory.MakeCachePath(filename);
        if (File.Exists(path))
            return path;
        if (Application.platform == RuntimePlatform.Android)
            return CString.Format("{0}{1}{2}", Application.dataPath, "!assets/", filename);
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
            return CString.Format("{0}{1}{2}", Application.dataPath, "/Raw/", filename);
        else
            return CString.Format("{0}{1}", streaming_patch, filename);
    }

    public static string AppendDirectoryChar( string dir ) {
        if ( dir == null || dir.Length == 0 ) {
            return string.Empty;
        } else if ( dir[dir.Length - 1] != '\\' ) {
            return string.Concat(dir , '\\');
        } else {
            return dir;
        }
    }

    public static string Standardize( string path ) {
        if ( string.IsNullOrEmpty( path ) ) return null;
        return path.Replace( '\\', '/' );
    }

    public static string res_path {
        get {
            return string.Concat(cache_path , "res/");
        }
    }

    // by Conglin
    public static string CachePath { get { return cache_path; } }

    public static void ClearCachePath( string dir ) {
        dir = string.Concat(cache_path , dir);
        if ( !Directory.Exists( dir ) )
            return;
        ClearFolder( dir );
    }

    internal static void ClearFolder( string dir ) {
        foreach ( string d in Directory.GetFileSystemEntries( dir ) ) {
            if ( File.Exists( d ) ) {
                FileInfo fi = new FileInfo( d );
                if ( fi.Attributes.ToString().IndexOf( "ReadOnly" ) != -1 )
                    fi.Attributes = FileAttributes.Normal;
                try {
                    File.Delete( d );
                } catch { }
            } else {
                DirectoryInfo dl = new DirectoryInfo( d );
                if ( dl.GetFiles().Length != 0 )
                    ClearFolder( dl.FullName );
                //Directory.Delete( d );
            }
        }
    }
}

#if  false
public static class CDirectory {

    private static bool initialized_;

    private static string projectRoot_;

    private static string res_;

    private static string log_;

    static CDirectory() {
        res_ = projectRoot_ = @".\";
        log_ = @".\log\";
    }

    public static string ProjectRoot {
        get { return projectRoot_; }
    }

    public static string Res {
        get { return res_; }
    }

    public static string Log {
        get { return log_; }
    }

    /// <summary>
    ///     如果是Unity环境，用这个来初始化
    /// </summary>
    public static void Init() {
        if ( !initialized_ ) {
            initialized_ = true;
            StringBuilder path = new StringBuilder();
            if ( Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer )
                path.Append( Application.streamingAssetsPath );
            else
                path.AppendFormat( "{0}/StreamingAssets", Application.dataPath );
            path.Append( '/' );
            res_ = path.ToString();
#if  false
            initialized_ = true;
            string path = string.Empty;
#if UNITY_IPHONE || UNITY_ANDROID
            if (Application.platform == RuntimePlatform.Android||Application.platform == RuntimePlatform.IPhonePlayer)
            {
                path = Application.streamingAssetsPath +"/";
                res_ = path;
            }
            else
#endif
            {
                path = Application.dataPath + "/StreamingAssets";
                var sb = new StringBuilder(path.Replace('/', '\\'), 512);

                if (sb.Length > 0 && sb[sb.Length - 1] != '\\')
               	{
                	sb.Append('\\');
           		}
                
                projectRoot_ = sb.ToString();
                res_ = projectRoot_;
                //log_ = res_ + "log\\";
                //LOG.Debug(log_);
            }
#endif
        }
    }

    /// <summary>
    ///     如果不是Unity环境（比如ModuleBuilder等），用这个来初始化
    /// </summary>
    /// <param name="rootDir">根</param>
    public static void Init( string rootDir, string resDir, string logDir ) {
        if ( !initialized_ ) {
            initialized_ = true;
            projectRoot_ = AppendDirectoryChar( rootDir );
            res_ = AppendDirectoryChar( resDir );
            log_ = AppendDirectoryChar( logDir );
        }
    }

    public static string AppendDirectoryChar( string dir ) {
        if ( dir == null || dir.Length == 0 ) {
            return string.Empty;
        } else if ( dir[dir.Length - 1] != '\\' ) {
            return dir + '\\';
        } else {
            return dir;
        }
    }

    public static string Standardize( string path ) {
        if ( string.IsNullOrEmpty( path ) ) return null;
        return path.Replace( '\\', '/' );
    }
}
#endif