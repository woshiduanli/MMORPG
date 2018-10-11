local Resources = CS.UnityEngine.Resources
local TextAsset = CS.UnityEngine.TextAsset
local Application = CS.UnityEngine.Application
local QualitySettings = CS.UnityEngine.QualitySettings

local ShadowQuality = CS.UnityEngine.ShadowQuality
local AnisotropicFiltering = CS.UnityEngine.AnisotropicFiltering
local ShadowProjection = CS.UnityEngine.ShadowProjection
local BlendWeights = CS.UnityEngine.BlendWeights

local Shader = CS.UnityEngine.Shader
local Json = require "LuaHelper.JsonHelper"

local ConfigHelper = {}

function ConfigHelper.LoadConfig()
    local ta = Resources.Load("config", typeof(TextAsset))
    local table, a, error = Json.decode(ta.text)
    if (error ~= nil) then
        return
    end
    ConfigHelper.EnableLog = table.debug
    ConfigHelper.LocalDB = table.localDB
    ConfigHelper.isMobilePlatform = Application.isMobilePlatform
    ConfigHelper.isEditor = Application.isEditor
    ConfigHelper.SetFrameRate()
end

function ConfigHelper.SetFrameRate()
    Application.targetFrameRate = 30
    QualitySettings.softParticles = false
    QualitySettings.realtimeReflectionProbes = false
    QualitySettings.billboardsFaceCameraPosition = false
    QualitySettings.shadows = ShadowQuality.Disable
    QualitySettings.lodBias = 0.5
    QualitySettings.shadowCascades = 0
    QualitySettings.antiAliasing = 4
    QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable
    QualitySettings.pixelLightCount = 2
    QualitySettings.shadowDistance = 40
    QualitySettings.shadowProjection = ShadowProjection.StableFit
    QualitySettings.particleRaycastBudget = 0
    QualitySettings.masterTextureLimit = 0
    QualitySettings.blendWeights = BlendWeights.FourBones
end

return ConfigHelper