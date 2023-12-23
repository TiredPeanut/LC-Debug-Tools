using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheLC.Plugins;
using UnityEngine;

namespace TheLC
{
    internal static class SettingsHandler
    {

        public static void RegisterSettings(ConfigFile Config)
        {
            ModSettings.InfiniteStaminaConfig = Config.Bind(ModSettingCategories.General, ModSettings.InfiniteStaminaKey, false, new ConfigDescription("Infinite stamina", new AcceptableValueRange<bool>(false, true)));

            ModSettings.JumpHeightConfig = Config.Bind(ModSettingCategories.General, ModSettings.JumpHeightKey, 10f, new ConfigDescription("Set jump height", new AcceptableValueRange<float>(1f, 200f)));

            ModSettings.MovementSpeedConfig = Config.Bind(ModSettingCategories.General, ModSettings.MovementSpeedKey, 5f, new ConfigDescription("Set movement speed", new AcceptableValueRange<float>(0f, 100f)));

            ModSettings.ClimbSpeedConfig = Config.Bind(ModSettingCategories.General, ModSettings.ClimbSpeedKey, 4f, new ConfigDescription("Set climb speed", new AcceptableValueRange<float>(0f, 100f)));

            ModSettings.ThrowPowerConfig = Config.Bind(ModSettingCategories.General, ModSettings.ThrowPowerKey, 17f, new ConfigDescription("Set throw power", new AcceptableValueRange<float>(0f, 100f)));

            ModSettings.GrabDistanceConfig = Config.Bind(ModSettingCategories.General, ModSettings.GrabDistanceKey, 5f, new ConfigDescription("Set grab distance", new AcceptableValueRange<float>(0f, 100f)));

            ModSettings.FOVConfig = Config.Bind(ModSettingCategories.Video, ModSettings.FOVKey, 66f, new ConfigDescription("Set FOV", new AcceptableValueRange<float>(66f, 130f)));

            ModSettings.HideVisorConfig = Config.Bind(ModSettingCategories.Video, ModSettings.HideVisorKey, false, "Changes visor visability");

            RegisterSettingEvents();
        }

        //This might come back to bit me 
        private static void ConfigSettingChanged(object sender, System.EventArgs e)
        {
            SettingChangedEventArgs settingChangedEventArgs = e as SettingChangedEventArgs;

            if (settingChangedEventArgs == null)
                return;

            if (settingChangedEventArgs.ChangedSetting.Definition.Key == ModSettings.FOVKey)
            {
                PlayerUpdate.targetFov = Mathf.Clamp(ModSettings.FOVConfig.Value, 66f, 130f);
                PlayerUpdate.CalculateVisorScale();
            }

            if (settingChangedEventArgs.ChangedSetting.Definition.Key == ModSettings.HideVisorKey)
            {
                PlayerUpdate.hideVisor = ModSettings.HideVisorConfig.Value;
                PlayerUpdate.CalculateVisorScale();
            }

            //if (settingChangedEventArgs.ChangedSetting.Definition.Key == KeyboardShortcutExampleKey)
            //{
            //    KeyboardShortcut newValue = (KeyboardShortcut)settingChangedEventArgs.ChangedSetting.BoxedValue;
            //}
        }

        public static void SetInitalValues()
        {
            PlayerUpdate.targetFov = Mathf.Clamp(ModSettings.FOVConfig.Value, 66f, 130f);
            PlayerUpdate.hideVisor = ModSettings.HideVisorConfig.Value;
            PlayerUpdate.CalculateVisorScale();
        }

        private static void RegisterSettingEvents()
        {
            //Register settings changed event
            ModSettings.FOVConfig.SettingChanged += ConfigSettingChanged;
            ModSettings.HideVisorConfig.SettingChanged += ConfigSettingChanged;
        }
    }

    public static class ModSettings
    {
        public static string InfiniteStaminaKey = "Infinite Stamina";
        public static string JumpHeightKey = "Jump Height";
        public static string MovementSpeedKey = "Movement Speed";
        public static string ClimbSpeedKey = "Climb Speed";
        public static string ThrowPowerKey = "Throw Item/Player Power";
        public static string GrabDistanceKey = "Item/Player Grab Distance";
        public static string FOVKey = "Player FOV";
        public static string HideVisorKey = "Hide Visor";

        //public static string KeyboardShortcutExampleKey = "Recall Keyboard Shortcut";

        public static ConfigEntry<bool> InfiniteStaminaConfig;
        public static ConfigEntry<float> JumpHeightConfig;
        public static ConfigEntry<float> MovementSpeedConfig;
        public static ConfigEntry<float> ClimbSpeedConfig;
        public static ConfigEntry<float> ThrowPowerConfig;
        public static ConfigEntry<float> GrabDistanceConfig;
        public static ConfigEntry<float> FOVConfig;
        public static ConfigEntry<bool> HideVisorConfig;
        //public static ConfigEntry<KeyboardShortcut> KeyboardShortcutExample;
    }

    public static class ModSettingCategories
    {
        public const string General = "General";
        public const string Video = "Video";
    }

    public static class ModConstraints
    {
        public const string modGuid = "Peanut.TheLC";
        public const string modName = "TheLC";
        public const string modVersion = "1.0.0";
    }
}
