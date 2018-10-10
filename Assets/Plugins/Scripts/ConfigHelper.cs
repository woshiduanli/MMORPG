using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using LitJson;

public class ConfigHelper
{
    public static ConfigHelper instance;
    public string ReferenceUrl { get; private set; }
    public string checkUrl { get; private set; }
    public string testUrl { private set; get; }
    public bool debug { get; private set; }      // 是否调试模式
    public string GVoiceAppID { private set; get; }
    public string GVoiceAppKey { private set; get; }
    public bool GVoice { private set; get; }
    public string userProtocol;
    public string userProtocolEn;
    public string userPrivaty;
    public string userPrivatyEn;
    public static bool EnableLog
    {
        get
        {
            if (instance !=null)
                return instance.debug;
            return false;
        }
    }
    public bool sdk { get; private set; }//是否使用sdk登陆
    public bool uiEditor;//读取ui 相关的资源路劲是否使用Resources
    public static bool uiEditor_
    {
        get
        {
            return true; 
            if (Application.isEditor && instance != null)
                return instance.uiEditor;
            return false;
        }
    }

    public bool version;//是否检查更新
    public static bool CheckVersion
    {
        get
        {
            if (Application.isEditor && instance != null)
                return instance.version;
            return true;
        }
    }

    public bool localDB;//是否读取本地配置
    public static bool LocalDB
    {
        get
        {
            if (Application.isEditor && instance != null)
                return instance.localDB;
            return true;
        }
    }

    public uint gameID;//安全SDK game id
    public static uint GameID
    {
        get
        {
            if (instance != null)
                return instance.gameID;
            return 0;
        }
    }

    public static void LoadConfig(TextAsset asset) {
        try
        {
            string config = asset.text;
            //if (!string.IsNullOrEmpty(CApplication.GetConfig()))
            //    config = CApplication.GetConfig();
            instance = JsonMapper.ToObject<ConfigHelper>(config);
        }
        catch (Exception ex)
        {
            LOG.LogError("[ConfigHelper] load fail." + ex.Message);
        }

    }

    public static void SetFrameRate(int deviceLevel) {
        Application.targetFrameRate = 30;
        //降低FPS：省电，减少手机发热
        QualitySettings.softParticles = false;
        QualitySettings.realtimeReflectionProbes = false;
        QualitySettings.billboardsFaceCameraPosition = false;
        QualitySettings.shadows = ShadowQuality.Disable;
        QualitySettings.lodBias = 0.5f;
        QualitySettings.shadowCascades = 0;

        //2018-8-31 机型性能适配 CJ
        if (deviceLevel == 1)
            QualitySettings.antiAliasing = 4;
        else if (deviceLevel == 2)
            QualitySettings.antiAliasing = 2;
        else
            QualitySettings.antiAliasing = 0;

        QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        QualitySettings.pixelLightCount = 2;
        QualitySettings.shadowDistance = 40;
        QualitySettings.shadowProjection = ShadowProjection.StableFit;
        QualitySettings.particleRaycastBudget = 0;
        QualitySettings.masterTextureLimit = 0;
        QualitySettings.blendWeights = BlendWeights.FourBones;
    }


}
