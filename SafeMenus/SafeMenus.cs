using BepInEx;
using R2API.Networking;
using R2API.Networking.Interfaces;
using R2API.Utils;
using RoR2;
using System;

namespace SafeMenus
{
    [BepInPlugin(modGuid, "SafeMenus", "1.0.1")]
    [BepInProcess("Risk of Rain 2.exe")]
    [R2APISubmoduleDependency(nameof(NetworkingAPI))]
    public class SafeMenus : BaseUnityPlugin
    {
        public const short buffMessageId = 7296;
        public const string modGuid = "org.mries92.SafeMenus";

        void Awake()
        {
            SettingsManager.Init();
            // Network messages
            NetworkingAPI.RegisterMessageType<BuffMessage>();
            NetworkingAPI.RegisterMessageType<ClientCooldownMessage>();
            // Hooks
            On.RoR2.PlayerCharacterMasterController.OnBodyStart += OnBodyStart;
            On.RoR2.UI.PickupPickerPanel.SetPickupOptions += SetPickupOptions;
        }

        void OnBodyStart(On.RoR2.PlayerCharacterMasterController.orig_OnBodyStart orig, PlayerCharacterMasterController self)
        {
            orig(self);
            ClientCooldownMessage message = new ClientCooldownMessage() { masterObject = self.master.gameObject, protectionCooldown = SettingsManager.protectionCooldown.Value };
            message.Send(NetworkDestination.Server);
        }

        public void SetPickupOptions(On.RoR2.UI.PickupPickerPanel.orig_SetPickupOptions orig, RoR2.UI.PickupPickerPanel self, RoR2.PickupPickerController.Option[] options)
        {
            orig(self, options);
            if (SettingsManager.protectionEnabled.Value)
            {
                LocalUser user = LocalUserManager.GetFirstLocalUser();
                CharacterBody body = user.cachedBody;
                BuffMessage message = new BuffMessage();
                switch (SettingsManager.protectionType.Value)
                {
                    case "invincible":
                        message = new BuffMessage() { _body = body, _buffIndex = BuffIndex.HiddenInvincibility, _buffTime = SettingsManager.protectionTime.Value, _enableShield = false, _shieldAmount = 0 };
                        break;
                    case "invisible":
                        message = new BuffMessage() { _body = body, _buffIndex = BuffIndex.Cloak, _buffTime = SettingsManager.protectionTime.Value, _enableShield = false, _shieldAmount = 0 };
                        break;
                    case "shield":
                        message = new BuffMessage() { _body = body, _buffIndex = BuffIndex.None, _buffTime = 0, _enableShield = true, _shieldAmount = (SettingsManager.protectionShieldAmount.Value / 100) * body.maxBarrier };
                        break;
                }
                message.Send(NetworkDestination.Server);
            }
        }
    }
}
