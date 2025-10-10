using System.Collections;
using HarmonyLib;
using UnityEngine;

public static class EpicLootVRControls
{
    private static Coroutine _inputCoroutine;

    // Cached reflection results to avoid expensive lookups every frame
    private static System.Type _vrControlsType;
    private static System.Reflection.PropertyInfo _instanceProperty;
    private static System.Reflection.MethodInfo _getButtonDownMethod;
    private static bool _reflectionCached = false;

    public static void Initialize(Canvas canvas)
    {
        // Stop any existing input coroutine
        if (_inputCoroutine != null)
        {
            UIVRPatch.Instance.StopCoroutine(_inputCoroutine);
            _inputCoroutine = null;
        }

        // Cache reflection results once
        CacheReflection();

        _inputCoroutine = UIVRPatch.Instance.StartCoroutine(WaitForVRInput());
        UIVRPatch.LogInfo("EpicLoot VR controls initialized");
    }

    private static void CacheReflection()
    {
        if (_reflectionCached) return;

        try
        {
            _vrControlsType = AccessTools.TypeByName("ValheimVRMod.VRCore.UI.VRControls");
            if (_vrControlsType != null)
            {
                _instanceProperty = AccessTools.Property(_vrControlsType, "instance");
                if (_instanceProperty != null)
                {
                    _getButtonDownMethod = AccessTools.Method(_vrControlsType, "GetButtonDown");
                }
            }
            _reflectionCached = true;
            UIVRPatch.LogInfo("VR controls reflection cached");
        }
        catch (System.Exception e)
        {
            UIVRPatch.LogError($"Error caching reflection: {e}");
        }
    }

    private static IEnumerator WaitForVRInput()
    {
        // Just wait for the VR button press - no need to check canvas state
        while (true)
        {
            yield return null;

            try
            {
                // Check for VR menu buttons
                if (IsVRMenuButtonPressed())
                {
                    UIVRPatch.LogInfo("VR button pressed - closing EpicLoot UI");
                    CloseEpicLootUI();
                    yield break;
                }
            }
            catch (System.Exception e)
            {
                UIVRPatch.LogError($"Error in VR input check: {e}");
            }
        }
    }

    private static bool IsVRMenuButtonPressed()
    {
        if (!_reflectionCached || _vrControlsType == null || _instanceProperty == null || _getButtonDownMethod == null)
            return false;

        try
        {
            var vrControlsInstance = _instanceProperty.GetValue(null);
            if (vrControlsInstance != null)
            {
                // Check both menu and inventory buttons using cached method
                bool menuPressed = (bool)_getButtonDownMethod.Invoke(vrControlsInstance, new object[] { "JoyMenu" });
                bool inventoryPressed = (bool)_getButtonDownMethod.Invoke(vrControlsInstance, new object[] { "Inventory" });

                return menuPressed || inventoryPressed;
            }
        }
        catch (System.Exception e)
        {
            UIVRPatch.LogError($"Error checking VR menu button: {e}");
        }

        return false;
    }

    private static void CloseEpicLootUI()
    {
        try
        {
            // Directly call EpicLoot's Hide method - same as pressing Escape
            var enchantingTableUIType = AccessTools.TypeByName("EpicLoot_UnityLib.EnchantingTableUI");
            var hideMethod = AccessTools.Method(enchantingTableUIType, "Hide");

            if (hideMethod != null)
            {
                hideMethod.Invoke(null, null);
                UIVRPatch.LogInfo("Successfully called EpicLoot Hide() method");
            }
            else
            {
                UIVRPatch.LogError("Could not find EpicLoot Hide method");
            }
        }
        catch (System.Exception e)
        {
            UIVRPatch.LogError($"Error closing EpicLoot UI: {e}");
        }

        Cleanup();
    }

    private static void Cleanup()
    {
        if (_inputCoroutine != null)
        {
            UIVRPatch.Instance.StopCoroutine(_inputCoroutine);
            _inputCoroutine = null;
        }
        // Removed the "EpicLoot VR controls cleaned up" log message
    }
}