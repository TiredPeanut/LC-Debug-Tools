using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System.Linq;
using System.Reflection;
using TheLC.Plugins;
using UnityEngine;

namespace TheLC
{
    [BepInPlugin(ModConstraints.modGuid, ModConstraints.modName, ModConstraints.modVersion)]
    public class PluginMain : BaseUnityPlugin
    {
        private readonly Harmony harmony = new Harmony(ModConstraints.modGuid);
        public static ManualLogSource Log = new ManualLogSource(ModConstraints.modName);

        void Awake()
        {
            SettingsHandler.RegisterSettings(Config);
            SettingsHandler.SetInitalValues();

            //Grab all classes under the "TheLC.Plugins" and inject them into harmony
            var allPatches = Assembly.GetExecutingAssembly().GetTypes().Where(x => x.IsClass && x.Namespace.Contains("TheLC.Plugins"));
            foreach (var type in allPatches)
            {
                harmony.PatchAll(type);
            }

        }
    }
}
