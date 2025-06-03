using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100;
    private int currentHP;

    private void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            DieAndRespawn();
        }
    }

    private void DieAndRespawn()
    {
        GameObject respawnPoint = GameObject.FindGameObjectWithTag("Respawn");
        if (respawnPoint != null)
        {
            transform.position = respawnPoint.transform.position;
        }
        else
        {
            Debug.LogWarning("Respawn point not found! Make sure you have a GameObject with tag 'Respawn'.");
        }

        currentHP = maxHP;
        Debug.Log("Player respawned with full HP.");
    }
}
