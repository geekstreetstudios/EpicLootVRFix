using HarmonyLib;
using System.Collections;
using UnityEngine;

public static class EpicLootVRUI
{
    public static bool IsEpicLootLoaded()
    {
        try
        {
            var enchantingTableUIType = AccessTools.TypeByName("EpicLoot_UnityLib.EnchantingTableUI");
            return enchantingTableUIType != null;
        }
        catch
        {
            return false;
        }
    }

    public static void PatchEpicLootUI(Harmony harmony)
    {
        try
        {
            var enchantingTableUIType = AccessTools.TypeByName("EpicLoot_UnityLib.EnchantingTableUI");
            if (enchantingTableUIType == null) return;

            var showMethod = AccessTools.Method(enchantingTableUIType, "Show");
            if (showMethod != null)
            {
                harmony.Patch(showMethod, postfix: new HarmonyMethod(typeof(EpicLootVRUI), nameof(OnUIShown)));
                UIVRPatch.LogInfo("Successfully patched EpicLoot EnchantingTableUI for VR");
            }
        }
        catch (System.Exception e)
        {
            UIVRPatch.LogError($"Error patching EpicLoot: {e}");
        }
    }

    public static void OnUIShown()
    {
        UIVRPatch.LogInfo("=== EPICLOOT UI SHOWN ===");
        UIVRPatch.Instance.StartCoroutine(ForceVHVRProcessing());
    }

    private static IEnumerator ForceVHVRProcessing()
    {
        yield return null;

        try
        {
            // Find the EpicLoot canvas - using non-obsolete method
            Canvas epicLootCanvas = null;
            Canvas[] allCanvases = Resources.FindObjectsOfTypeAll<Canvas>();

            foreach (Canvas canvas in allCanvases)
            {
                if (canvas != null && canvas.name.Contains("EnchantingUI"))
                {
                    epicLootCanvas = canvas;
                    break;
                }
            }

            if (epicLootCanvas == null)
            {
                UIVRPatch.LogWarning("EpicLoot canvas not found");
                yield break;
            }

            UIVRPatch.LogInfo($"Found EpicLoot canvas: '{epicLootCanvas.name}'");

            // Get VHVR's VRGUI instance
            var vrguiInstance = GetVHVRVRGUI();
            if (vrguiInstance == null)
            {
                UIVRPatch.LogError("Could not get VHVR VRGUI instance");
                yield break;
            }

            var vrguiType = AccessTools.TypeByName("ValheimVRMod.VRCore.UI.VRGUI");

            // Method 1: Add to VHVR's canvas list
            AddCanvasToVHVR(epicLootCanvas, vrguiInstance, vrguiType);

            // Method 2: Manually call the processing that VHVR does
            ManuallyProcessCanvasForVHVR(epicLootCanvas, vrguiInstance, vrguiType);

            UIVRPatch.LogInfo("EpicLoot UI should now be visible in VR");

            // Initialize VR controls for this UI
            EpicLootVRControls.Initialize(epicLootCanvas);
        }
        catch (System.Exception e)
        {
            UIVRPatch.LogError($"Error: {e}");
        }
    }

    private static object GetVHVRVRGUI()
    {
        try
        {
            var vrguiType = AccessTools.TypeByName("ValheimVRMod.VRCore.UI.VRGUI");
            if (vrguiType == null) return null;

            // Using non-obsolete method
            var vrguiInstances = Resources.FindObjectsOfTypeAll(vrguiType);
            return vrguiInstances.Length > 0 ? vrguiInstances[0] : null;
        }
        catch (System.Exception e)
        {
            UIVRPatch.LogError($"Error getting VHVR VRGUI: {e}");
            return null;
        }
    }

    private static void AddCanvasToVHVR(Canvas epicLootCanvas, object vrguiInstance, System.Type vrguiType)
    {
        try
        {
            var guiCanvasesField = AccessTools.Field(vrguiType, "_guiCanvases");
            if (guiCanvasesField == null) return;

            var guiCanvases = guiCanvasesField.GetValue(vrguiInstance) as System.Collections.IList;
            if (guiCanvases == null) return;

            if (!guiCanvases.Contains(epicLootCanvas))
            {
                guiCanvases.Add(epicLootCanvas);
                UIVRPatch.LogInfo($"Added EpicLoot canvas to VHVR _guiCanvases list");
            }
        }
        catch (System.Exception e)
        {
            UIVRPatch.LogWarning($"Error adding to VHVR list: {e}");
        }
    }

    private static void ManuallyProcessCanvasForVHVR(Canvas epicLootCanvas, object vrguiInstance, System.Type vrguiType)
    {
        try
        {
            var guiCameraField = AccessTools.Field(vrguiType, "_guiCamera");
            if (guiCameraField == null)
            {
                UIVRPatch.LogError("VHVR _guiCamera field not found");
                return;
            }

            var guiCamera = guiCameraField.GetValue(vrguiInstance) as Camera;
            if (guiCamera == null)
            {
                UIVRPatch.LogError("VHVR _guiCamera is null");
                return;
            }

            UIVRPatch.LogInfo($"Using VHVR GUI camera: {guiCamera.name}");

            epicLootCanvas.worldCamera = guiCamera;
            epicLootCanvas.renderMode = RenderMode.WorldSpace;

            var guiDimensionsField = AccessTools.Field(vrguiType, "GUI_DIMENSIONS");
            if (guiDimensionsField != null)
            {
                Vector2 guiDimensions = (Vector2)guiDimensionsField.GetValue(null);
                var rectTransform = epicLootCanvas.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, guiDimensions.x);
                    rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, guiDimensions.y);
                    UIVRPatch.LogInfo($"Set canvas size to: {guiDimensions}");
                }
            }

            UIVRPatch.LogInfo("Manually processed EpicLoot canvas for VHVR");
        }
        catch (System.Exception e)
        {
            UIVRPatch.LogError($"Error manually processing canvas: {e}");
        }
    }
}