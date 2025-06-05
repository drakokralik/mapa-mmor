using UnityEngine;
using Mirror;

public class Health : NetworkBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SyncVar] private int currentHealth;

    [SerializeField] private Transform respawnPoint;

    private CharacterController cc;
    private Rigidbody rb;

    public override void OnStartServer()
    {
        currentHealth = maxHealth;

        cc = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();

        if (respawnPoint == null)
        {
            GameObject respawnObj = GameObject.FindWithTag("Respawn");
            if (respawnObj != null)
                respawnPoint = respawnObj.transform;
        }
    }

    [Server]
    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"[SERVER] Hráč dostal damage: {amount}, zbývá HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Debug.Log("[SERVER] Spouštím respawn.");
            Respawn();
        }
    }

    [Server]
    private void Respawn()
    {
        if (respawnPoint == null)
        {
            Debug.LogWarning("Respawn point není nastaven!");
            return;
        }

        // Deaktivuj fyziku a pohyb
        if (cc != null) cc.enabled = false;
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        // Přesuň hráče
        transform.position = new Vector3(
            respawnPoint.position.x,
            respawnPoint.position.y,
            respawnPoint.position.z
        );

        // Resetuj HP
        currentHealth = maxHealth;

        // Malé zpoždění před opětovným zapnutím komponent (async na serveru)
        StartCoroutine(ReenableAfterDelay(0.1f));

        Debug.Log("[SERVER] Hráč respawnut a HP obnovena.");
    }

    private System.Collections.IEnumerator ReenableAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (rb != null)
        {
            rb.isKinematic = false;
        }

        if (cc != null)
        {
            cc.enabled = true;
        }
    }

    public void SetRespawnPoint(Transform point)
    {
        respawnPoint = point;
    }
}
