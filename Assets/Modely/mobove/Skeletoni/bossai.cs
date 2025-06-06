using Mirror;
using UnityEngine;
using UnityEngine.AI;

public class BossBehavior : NetworkBehaviour
{
    public int maxHealth = 1000;
    [SyncVar] private int currentHealth;

    public float meleeRange = 3f;
    public int meleeDamage = 40;

    public float rangedCooldown = 5f;
    public float meleeCooldown = 3f;
    private float lastRangedTime;
    private float lastMeleeTime;

    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileForce = 20f;

    private GameObject currentTarget;
    private NavMeshAgent agent;

    private enum Phase { Phase1, Phase2 }
    private Phase currentPhase = Phase.Phase1;

    private void Start()
    {
        currentHealth = maxHealth;
        agent = GetComponent<NavMeshAgent>();
    }

    [ServerCallback]
    private void Update()
    {
        if (!agent) return;

        currentTarget = FindClosestPlayer();
        if (currentTarget == null) return;

        float distance = Vector3.Distance(transform.position, currentTarget.transform.position);
        agent.SetDestination(currentTarget.transform.position);

        if (distance <= meleeRange && Time.time > lastMeleeTime)
        {
            lastMeleeTime = Time.time + meleeCooldown;
            MeleeAttack();
        }
        else if (distance <= 15f && Time.time > lastRangedTime)
        {
            lastRangedTime = Time.time + rangedCooldown;
            RangedAttack(currentTarget.transform.position);
        }

        UpdatePhase();
    }

    [Server]
    void MeleeAttack()
    {
        if (currentTarget.TryGetComponent(out Health health))
        {
            health.TakeDamage(meleeDamage);
            Debug.Log("Boss provedl melee útok!");
        }
    }

    [Server]
    void RangedAttack(Vector3 targetPosition)
    {
        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = proj.GetComponent<Rigidbody>();

        Vector3 dir = (targetPosition - shootPoint.position).normalized;
        rb.velocity = dir * projectileForce;

        NetworkServer.Spawn(proj);
    }

    [Server]
    void UpdatePhase()
    {
        if (currentPhase == Phase.Phase1 && currentHealth < maxHealth * 0.5f)
        {
            currentPhase = Phase.Phase2;
            rangedCooldown = 3f;
            meleeCooldown = 2f;
            meleeDamage += 20;
            Debug.Log("Boss pøešel do fáze 2!");
        }
    }

    [Server]
    GameObject FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject player in players)
        {
            float dist = Vector3.Distance(transform.position, player.transform.position);
            if (dist < minDist)
            {
                closest = player;
                minDist = dist;
            }
        }
        return closest;
    }

    [Server]
    public void TakeDamage(int dmg)
    {
        currentHealth -= dmg;
        if (currentHealth <= 0)
        {
            NetworkServer.Destroy(gameObject);
        }
    }
}
