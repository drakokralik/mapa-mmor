using UnityEngine;
using Mirror;

public class ArrowProjectile : NetworkBehaviour
{
    public int damage = 25;
    public float lifetime = 5f;

    private bool hasHitTarget = false;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHitTarget) return;

        // Poškoï objekt s tagem Monster nebo Player
        if (other.CompareTag("Monster") || other.CompareTag("Player"))
        {
            Health hp = other.GetComponent<Health>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
            }
        }

        hasHitTarget = true;
        NetworkServer.Destroy(gameObject);
    }
}
