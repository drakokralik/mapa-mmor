using UnityEngine;
using Mirror;

public class MobDamage : NetworkBehaviour
{
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private float damageCooldown = 1.5f;

    private bool canDamage = true;

    private void OnCollisionEnter(Collision collision)
    {
        if (!isServer) return; // jen server dává dmg

        if (canDamage && collision.gameObject.CompareTag("Player"))
        {
            Health health = collision.gameObject.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
                StartCoroutine(DamageCooldown());
            }
        }
    }

    private System.Collections.IEnumerator DamageCooldown()
    {
        canDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canDamage = true;
    }
}
