using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHP = 100;
    private int currentHP;

    [SerializeField] private HorizontalProgressBar healthBar; // Odkaz na UI bar

    private void Start()
    {
        currentHP = maxHP;
        UpdateHealthBar();
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        currentHP = Mathf.Clamp(currentHP, 0, maxHP);
        UpdateHealthBar();

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
        UpdateHealthBar();
        Debug.Log("Player respawned with full HP.");
    }

    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            float progress = (float)currentHP / maxHP;
            healthBar.SetProgress(progress);
        }
    }
}
