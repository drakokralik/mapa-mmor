using UnityEngine;
using Mirror;

namespace Mirror.Examples.Common
{
    public class PlayerCombat : NetworkBehaviour
    {
        public int maxHP = 100;

        [SyncVar]
        public int currentHP;

        void Start()
        {
            currentHP = maxHP;
        }

        [Server]
        public void TakeDamage(int amount)
        {
            if (currentHP <= 0) return;

            currentHP -= amount;

            if (currentHP <= 0)
            {
                Die();
            }
        }

        [Server]
        void Die()
        {
            byte respawnDelay = 5;
            bool respawn = true;

            Respawn.RespawnPlayer(respawn, respawnDelay, connectionToClient);
        }

        public void ResetStats()
        {
            currentHP = maxHP;
        }
    }
}
