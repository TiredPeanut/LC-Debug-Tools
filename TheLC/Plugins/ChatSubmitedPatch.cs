using GameNetcodeStuff;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

namespace TheLC.Plugins
{
    [HarmonyPatch(typeof(HUDManager))]
    public sealed class ChatSubmitedPatch
    {
        [HarmonyPatch("SubmitChat_performed")]
        [HarmonyPrefix]
        public static void PostPatchLoadNewLevelWait(HUDManager __instance)
        {
            string text = __instance.chatTextField.text;
            if (text.ToLower().Equals("/transform"))
            {
                var localPlayer = GameNetworkManager.Instance.localPlayerController;
                TeleportPlayerToRandomDungeonLocation(localPlayer);
                BaseMonsterChanges();

                ChangePlayerToSpringMan();

            }
                
        }

        private static void TeleportPlayerToRandomDungeonLocation(PlayerControllerB player)
        {
            Vector3 randomDungeonLocation = RoundManager.Instance.insideAINodes[UnityEngine.Random.Range(0, RoundManager.Instance.insideAINodes.Length)].transform.position;
            randomDungeonLocation = RoundManager.Instance.GetRandomNavMeshPositionInRadiusSpherical(randomDungeonLocation);

            player.DropAllHeldItems();
            player.isInElevator = false;
            player.isInHangarShipRoom = false;
            player.isInsideFactory = true;
            player.averageVelocity = 0f;
            player.velocityLastFrame = Vector3.zero;
            player.TeleportPlayer(randomDungeonLocation);
        }

        private static void ChangePlayerToSpringMan()
        {
            ModSettings.MovementSpeedConfig.Value = 14.5f; //Movement speed

        }

        private static void BaseMonsterChanges()
        {
            ModSettings.JumpHeightConfig.Value = 0f; //Turn off jump
            ModSettings.GrabDistanceConfig.Value = 0f; //Turn off interactions
        }

       
    }
    //Players finished generating the new floor
}
