using R2API.Networking.Interfaces;
using UnityEngine;
using UnityEngine.Networking;

namespace SafeMenus
{
    /**
     * A message sent to the server by the clients
     * This caches their cooldown value on the server side.
     */
    class ClientCooldownMessage : INetMessage
    {
        public void Serialize(NetworkWriter writer)
        {
            writer.Write(protectionCooldown);
            writer.Write(masterObject);
        }

        public void Deserialize(NetworkReader reader)
        {
            protectionCooldown = reader.ReadInt32();
            masterObject = reader.ReadGameObject();
        }

        public void OnReceived()
        {
            if (SettingsManager.forceClientSettings.Value == false)
            {
                SettingsManager.playerCooldowns.Add(new PlayerCooldownInfo()
                {
                    cooldownTime = protectionCooldown,
                    lastBuffTime = 0,
                    masterObject = this.masterObject
                });
            }
            else
            {
                SettingsManager.playerCooldowns.Add(new PlayerCooldownInfo()
                {
                    cooldownTime = SettingsManager.protectionCooldown.Value,
                    lastBuffTime = 0,
                    masterObject = this.masterObject
                });
            }
        }

        public int protectionCooldown;
        public GameObject masterObject;
    }
}
