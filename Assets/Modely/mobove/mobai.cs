using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class MobAI : NetworkBehaviour
{
    public float aggroRange = 10f;
    public float deaggroRange = 15f;
    public int maxHealth = 100;
    [SyncVar] private int currentHealth;

    private Transform target;
    private Vector3 spawnPosition;
    private NavMeshAgent agent;

    private bool hasAggro = false;
    private bool isReturning = false;

    public override void OnStartServer()
    {
        agent = GetComponent<NavMeshAgent>();
        spawnPosition = transform.position;
        currentHealth = maxHealth;

        InvokeRepeating(nameof(ScanForTargets), 0f, 0.5f);
    }

    [ServerCallback]
    void Update()
    {
        if (!hasAggro && !isReturning)
            return;

        if (hasAggro && target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance > deaggroRange)
            {
                LoseAggro();
            }
            else
            {
                agent.SetDestination(target.position);
            }
        }
        else if (isReturning)
        {
            float distance = Vector3.Distance(transform.position, spawnPosition);

            if (distance < 0.5f)
            {
                isReturning = false;
                currentHealth = maxHealth;
            }
        }
    }

    [Server]
    void ScanForTargets()
    {
        if (hasAggro) return;

        Collider[] hits = Physics.OverlapSphere(transform.position, aggroRange);

        foreach (var hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                target = hit.transform;
                hasAggro = true;
                break;
            }
        }
    }

    [Server]
    void LoseAggro()
    {
        hasAggro = false;
        target = null;
        isReturning = true;
        agent.SetDestination(spawnPosition);
    }

    [Server]
    public void TakeDamage(int amount, Transform attacker)
    {
        currentHealth -= amount;

        if (!hasAggro)
        {
            target = attacker;
            hasAggro = true;
        }

        if (currentHealth <= 0)
        {
            NetworkServer.Destroy(gameObject);
        }
    }
}
