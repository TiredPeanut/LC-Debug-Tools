using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace TheLC.Plugins
{
    [HarmonyPatch(typeof(PlayerControllerB))]
    public sealed class PlayerUpdate
    {
        [HarmonyPatch("Update")]
        [HarmonyPrefix]


        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void PostPatchUpdate(PlayerControllerB ___instance ) 
        {
            ___instance.sprintMeter = ModSettings.InfiniteStaminaConfig.Value == true ? 1f : ___sprintMeter;
            ___instance.jumpForce = ModSettings.JumpHeightConfig.Value;
            ___instance.movementSpeed = ModSettings.MovementSpeedConfig.Value;
            ___instance.climbSpeed = ModSettings.ClimbSpeedConfig.Value;
            ___instance.grabDistance = ModSettings.GrabDistanceConfig.Value;
        }
    }
}
