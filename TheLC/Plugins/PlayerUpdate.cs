using GameNetcodeStuff;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace TheLC.Plugins
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    public sealed class PlayerUpdate
    {
        public static float targetFov = 66f;
        public static bool hideVisor;
        public static float preFov = 0f;
        private static Vector3 visorScale;
        public static Vector3 visorScaleBottom = new Vector3(0.68f, 0.8f, 0.95f);
        public static Vector3 visorScaleTop = new Vector3(0.68f, 0.35f, 0.99f);
        public static float linToSinLerp = 0.6f;
        public static float visorScaleTopRefFOV = 130f;


        [HarmonyPatch("Update")]
        [HarmonyPrefix]
        public static void PrePatchUpdate(PlayerControllerB __instance)
        {
            preFov = __instance.gameplayCamera.fieldOfView;
            if (__instance.localVisor.localScale != visorScale)
            {
                __instance.localVisor.localScale = visorScale;
            }
        }

        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void PostPatchUpdate(PlayerControllerB __instance) 
        {
            __instance.sprintMeter = ModSettings.InfiniteStaminaConfig.Value == true ? 1f : __instance.sprintMeter;
            __instance.jumpForce = ModSettings.JumpHeightConfig.Value;
            __instance.movementSpeed = ModSettings.MovementSpeedConfig.Value;
            __instance.climbSpeed = ModSettings.ClimbSpeedConfig.Value;
            __instance.grabDistance = ModSettings.GrabDistanceConfig.Value;
            __instance.UpdatePlayerFOV();
        }

        [HarmonyPatch("LateUpdate")]
        [HarmonyPostfix]
        public static void PostPatchLateUpdate(PlayerControllerB __instance)
        {
            if (targetFov > 66f)
            {
                __instance.localVisor.position = __instance.localVisor.position + __instance.localVisor.rotation * new Vector3(0f, 0f, -0.06f);
            }
        }

        [HarmonyPatch("Crouch_performed")]
        [HarmonyPrefix]
        public static void PatchPreCrouch(Inp)
        {
            
        }

        public static void CalculateVisorScale()
        {
            if (hideVisor)
            {
                visorScale = new Vector3(0f, 0f, 0f);
            }
            else if (targetFov > 66f)
            {
                float num = (targetFov - 66f) / (visorScaleTopRefFOV - 66f);
                num = Mathf.Lerp(num, EaseOutSine(num), linToSinLerp);
                visorScale = Vector3.LerpUnclamped(visorScaleBottom, visorScaleTop, num);
            }
            else
            {
                visorScale = new Vector3(0.36f, 0.49f, 0.49f);
            }
        }
        public static float EaseOutSine(float x) => Mathf.Sin(x * 3.1415927f / 2f);
    }

    public static class PlayerUpdateExtentions
    {
        public static PlayerControllerB UpdatePlayerFOV(this PlayerControllerB instance)
        {
            float num = PlayerUpdate.targetFov;
            if (instance.inTerminalMenu)
            {
                num = 60f;
            }
            else if (instance.IsInspectingItem)
            {
                num = 46f;
            }
            else if (instance.isSprinting)
            {
                num *= 1.03f;
            }
            instance.gameplayCamera.fieldOfView = Mathf.Lerp(PlayerUpdate.preFov, num, 6f * Time.deltaTime);

            return instance;
        }
    }

}
