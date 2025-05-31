using UnityEngine;
using Mirror;

public class PlayerCombat : NetworkBehaviour
{
    public int maxHP = 100;
    [SyncVar] public int currentHP;

    void Start()
    {
        currentHP = maxHP;
    }

    [Server]
    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            // respawn / death
        }
    }

    [Command]
    public void CmdAttack(GameObject target)
    {
        if (target.TryGetComponent<MonsterAI>(out var monster))
        {
            monster.TakeDamage(20);
        }
    }
}
