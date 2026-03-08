using HarmonyLib;
using UnityEngine;

namespace EpicLootVRFix
{
    [HarmonyPatch]
    public static class EpicLootVRTooltipFix
    {
        static System.Reflection.MethodBase TargetMethod()
        {
            return AccessTools.Method("EpicLoot.PatchOnHoverFix:AddScrollbar");
        }

        static void Postfix(GameObject tooltipObject)
        {
            try
            {
                if (!UIVRPatch.EnableVRFix.Value)
                    return;

                if (tooltipObject == null)
                    return;

                if (UIVRPatch.DebugMode.Value)
                    UIVRPatch.LogInfo("AddScrollbar patch triggered");

                Transform canvasTransform = tooltipObject.transform.Find("Canvas");

                if (canvasTransform == null)
                {
                    if (UIVRPatch.DebugMode.Value)
                        UIVRPatch.LogInfo("Canvas container not found");
                    return;
                }

                if (UIVRPatch.DebugMode.Value)
                    UIVRPatch.LogInfo("Canvas container found");

                Transform scrollView = canvasTransform.Find("Scroll View");

                if (scrollView == null)
                {
                    if (UIVRPatch.DebugMode.Value)
                        UIVRPatch.LogInfo("Scroll View not found under Canvas");
                    return;
                }

                if (UIVRPatch.DebugMode.Value)
                    UIVRPatch.LogInfo("Scroll View found");

                // Move ScrollView out of Jotunn canvas
                scrollView.SetParent(tooltipObject.transform, true);

                if (UIVRPatch.DebugMode.Value)
                    UIVRPatch.LogInfo("Scroll View moved to tooltip root");

                // Fix layer so VRGUI is rendering it correctly
                int uiLayer = LayerMask.NameToLayer("UI");
                SetLayerRecursive(scrollView.gameObject, uiLayer);

                if (UIVRPatch.DebugMode.Value)
                    UIVRPatch.LogInfo("Scroll View layer set to UI");

                // Remove contianer canvas
                Object.Destroy(canvasTransform.gameObject);

                if (UIVRPatch.DebugMode.Value)
                    UIVRPatch.LogInfo("Canvas container destroyed");
            }
            catch (System.Exception ex)
            {
                UIVRPatch.LogError("EpicLootVRTooltipFix error: " + ex);
            }
        }

        private static void SetLayerRecursive(GameObject obj, int layer)
        {
            obj.layer = layer;

            foreach (Transform child in obj.transform)
            {
                SetLayerRecursive(child.gameObject, layer);
            }
        }
    }
}