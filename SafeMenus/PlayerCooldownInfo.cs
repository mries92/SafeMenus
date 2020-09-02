using System;
using UnityEngine;

namespace SafeMenus
{
    class PlayerCooldownInfo
    {
        public GameObject masterObject;
        public int cooldownTime;
        public float lastBuffTime;

        public override string ToString()
        {
            return String.Format("Master Object: {0}, Cooldown: {1}, LastBuffTime: {2}", masterObject.name, cooldownTime, lastBuffTime);
        }    
    }
}
