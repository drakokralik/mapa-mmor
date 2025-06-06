using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Nastavení HP")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Respawn (pouze hráč)")]
    public bool isPlayer = false;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {        

        HorizontalProgressBar healthBar = GameObject.FindWithTag("HP").GetComponent<HorizontalProgressBar>();
        currentHealth -= amount;
        healthBar.SetProgress((float)currentHealth / maxHealth);
        Debug.Log($"{gameObject.name} dostal damage: {amount}");

        if (currentHealth <= 0)
        {
            Die();
            healthBar.SetProgress(1f); 
        }
    }

    private void Die()
    {
        if (isPlayer)
        {
            RespawnPlayer();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void RespawnPlayer()
    {
        GameObject respawnObject = GameObject.FindWithTag("Respawn");

        if (respawnObject != null)
        {

            GetComponent<CharacterController>().enabled = false; 
            // Přesune hráče na pozici objektu s tagem "Respawn"
            transform.position = respawnObject.transform.position;
            transform.rotation = respawnObject.transform.rotation;
            GetComponent<CharacterController>().enabled = true; 

            currentHealth = maxHealth;

            Debug.Log("Hráč byl přesunut na respawn point.");
        }
        else
        {
            Debug.LogError("Nenalezen žádný objekt s tagem 'Respawn'. Přidej ho do scény.");
        }
    }

    public void FullHeal()
    {
        currentHealth = maxHealth;
    }
}
