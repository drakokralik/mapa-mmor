using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int damage = 10;
    public float attackCooldown = 0.5f;

    private float lastAttackTime;

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time < lastAttackTime + attackCooldown) return;

        MobAI mob = other.GetComponent<MobAI>();
        if (mob != null)
        {
            mob.TakeDamage(damage, transform.root); // pøedáváme hráèe
            lastAttackTime = Time.time;
        }
    }
}
