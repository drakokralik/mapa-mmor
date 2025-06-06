using UnityEngine;

public class MobDamage : MonoBehaviour
{
    public int damageAmount = 10;
    public float attackCooldown = 1.5f;

    private float lastAttackTime = -999f;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Health playerHealth = other.GetComponent<Health>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(damageAmount);
                    Debug.Log($"Mob poökodil hr·Ëe o {damageAmount} HP");
                    lastAttackTime = Time.time;
                }
            }
        }
    }
}
