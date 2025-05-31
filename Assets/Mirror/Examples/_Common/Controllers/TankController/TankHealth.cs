using System.Collections;
using UnityEngine;
using Mirror;

public class TankHealth : NetworkBehaviour
{
    [SyncVar]
    public int currentHP = 100;

    public int maxHP = 100;
    public byte respawnTime = 3;

    private bool isRespawning = false;

    private void Update()
    {
        if (!isServer) return; // logiku HP a respawnu dìlej jen na serveru

        if (currentHP <= 0 && !isRespawning)
        {
            StartCoroutine(RespawnCoroutine());
        }
    }

    [Server]
    public void TakeDamage(int damage)
    {
        if (currentHP <= 0) return;

        currentHP -= damage;
        if (currentHP < 0)
            currentHP = 0;
    }

    [Server]
    IEnumerator RespawnCoroutine()
    {
        isRespawning = true;

        // Poèkej respawnTime sekund
        yield return new WaitForSeconds(respawnTime);

        // Najdi spawn pozici (pokud nemáš, respawn na Y=5)
        Vector3 spawnPos = Vector3.up * 5f;
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        if (spawnPoint != null)
            spawnPos = spawnPoint.position;

        // Pøesuò tank (hráèe) na spawn
        transform.position = spawnPos;

        // Resetuj HP
        currentHP = maxHP;

        isRespawning = false;
    }
}
