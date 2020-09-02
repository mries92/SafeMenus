using BepInEx;
using BepInEx.Configuration;
using RoR2;
using System.Collections.Generic;

namespace SafeMenus
{
    class SettingsManager
    {
        public static string modGuid = "org.mries92.SafeMenus";
        public static ConfigFile configFile;
        // Protection
        public static ConfigEntry<bool> protectionEnabled, forceClientSettings;
        public static ConfigEntry<string> protectionType;
        public static ConfigEntry<int> protectionTime, protectionCooldown, protectionShieldAmount;
        public static List<PlayerCooldownInfo> playerCooldowns = new List<PlayerCooldownInfo>();

        public static void Init()
        {
            // Base config file
            configFile = new ConfigFile(System.IO.Path.Combine(Paths.ConfigPath, modGuid + ".cfg"), true);

            // Protection
            bool defaultProtectionEnabled = true;
            bool defaultForceClientSettings = false;
            string defaultProtectionType = "invisible";
            int defaultProtectionTime = 6;
            int defaultProtectionCooldown = 30;
            int defaultProtectionShieldAmount = 100;
            protectionEnabled = configFile.Bind<bool>("protection", "protectionEnabled", defaultProtectionEnabled, new ConfigDescription("Toggle protection on/off"));
            forceClientSettings = configFile.Bind<bool>("protection", "forceClientSettings", defaultForceClientSettings, new ConfigDescription("Should clients be forced to use the servers/hosts protection settings?"));
            string[] acceptableProtectionValues = { "invisible", "shield", "invincible" };
            protectionType = configFile.Bind<string>("protection", "protectionType", defaultProtectionType, new ConfigDescription("The type of protection used when a command menu is opened.", new AcceptableValueList<string>(acceptableProtectionValues)));
            protectionTime = configFile.Bind<int>("protection", "protectionTime", defaultProtectionTime, new ConfigDescription("The lengh of time in seconds protection should be enabled for. (Does not apply to `shield` protection type)."));
            protectionCooldown = configFile.Bind<int>("protection", "protectionCooldown", defaultProtectionCooldown, new ConfigDescription("Limit how often protection can be granted"));
            protectionShieldAmount = configFile.Bind<int>("protection", "protectionShieldAmount", defaultProtectionShieldAmount, new ConfigDescription("If protection type is set to `shield` this is how much should be applied. This is a percentage of your max shield, from 1-100."));
        }

        public static BuffIndex GetProtectionBuffIndex()
        {
            switch(protectionType.Value)
            {
                case "invisible":
                    return BuffIndex.Cloak;
                case "invincible":
                    return BuffIndex.HiddenInvincibility;
                default:
                    return BuffIndex.None;
            }
        }
    }
}
