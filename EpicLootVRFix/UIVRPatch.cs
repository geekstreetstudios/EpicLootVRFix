using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections;
using UnityEngine;

[BepInPlugin("EpicLootVRFix", "Epic Loot VR Fix", "1.0.1")]
[BepInDependency("randyknapp.mods.epicloot", BepInDependency.DependencyFlags.SoftDependency)]
public class UIVRPatch : BaseUnityPlugin
{
    public static ConfigEntry<bool> EnableVRFix;
    public static ConfigEntry<bool> DebugMode;
    public static UIVRPatch Instance;

    private Harmony _harmony;

    public new ManualLogSource Logger => base.Logger;

    void Awake()
    {
        Instance = this;

        EnableVRFix = Config.Bind("General", "Enable VR Fix", true, "Enable VR UI fixes");
        DebugMode = Config.Bind("General", "Debug Mode", false, "Enable debug logging (for testing only)");

        if (EnableVRFix.Value)
        {
            _harmony = new Harmony("EpicLootVRFix");

            // Patch alle Harmony patches i assembly
            _harmony.PatchAll();

            StartCoroutine(WaitForEpicLootAndPatch());
        }
    }

    void OnDestroy()
    {
        _harmony?.UnpatchSelf();
    }

    private IEnumerator WaitForEpicLootAndPatch()
    {
        while (!EpicLootVRUI.IsEpicLootLoaded())
        {
            yield return new WaitForSeconds(2f);
        }

        LogInfo("EpicLoot detected! Applying VR fixes...");

        EpicLootVRUI.PatchEpicLootUI(_harmony);
    }

    public static void LogInfo(string message)
    {
        if (DebugMode.Value)
        {
            Instance.Logger.LogInfo(message);
        }
    }

    public static void LogWarning(string message)
    {
        if (DebugMode.Value)
        {
            Instance.Logger.LogWarning(message);
        }
    }

    public static void LogError(string message)
    {
        Instance.Logger.LogError(message);
    }
}