using UnityEngine;

public class RangedSlime : MonoBehaviour
{
    [Header("Stats")]
    public string slimeName = "Ranged Slime";
    public int maxHP = 50;
    private int currentHP;
    public float attackRange = 10f;
    public int attackDamage = 10;
    public float attackCooldown = 2f;

    [Header("Attack")]
    public GameObject projectilePrefab;
    public Transform firePoint;

    private float lastAttackTime;
    private GameObject target;

    private void Start()
    {
        currentHP = maxHP;
        InvokeRepeating(nameof(FindTarget), 0, 1f);
    }

    private void Update()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= attackRange && Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    private void FindTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                target = collider.gameObject;
                return;
            }
        }
        target = null;
    }

    private void Attack()
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null && target != null)
            {
                Vector3 direction = (target.transform.position - firePoint.position).normalized;
                rb.velocity = direction * 10f;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}