using UnityEngine;
using Mirror;
using System.Linq;

public class MonsterAI : NetworkBehaviour
{
    public float aggroRange = 10f;
    public float leashRange = 20f;
    public float attackRange = 2f;
    public float moveSpeed = 3f;
    public float attackCooldown = 2f;
    public int maxHP = 100;
    public int damage = 10;

    [SyncVar] public int currentHP;
    [SyncVar] private GameObject target;

    private Vector3 spawnPoint;
    private float lastAttackTime;

    public override void OnStartServer()
    {
        currentHP = maxHP;
        spawnPoint = transform.position;
        InvokeRepeating(nameof(FindTarget), 0, 1f);
    }

    [Server]
    void FindTarget()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        var nearest = players
            .Where(p => Vector3.Distance(transform.position, p.transform.position) <= aggroRange)
            .OrderBy(p => Vector3.Distance(transform.position, p.transform.position))
            .FirstOrDefault();

        if (nearest != null)
            target = nearest;
    }

    [ServerCallback]
    void Update()
    {
        if (currentHP <= 0)
        {
            NetworkServer.Destroy(gameObject);
            return;
        }

        if (target == null)
        {
            ReturnToSpawn();
            return;
        }

        float distToTarget = Vector3.Distance(transform.position, target.transform.position);
        float distFromSpawn = Vector3.Distance(transform.position, spawnPoint);

        if (distFromSpawn > leashRange)
        {
            target = null;
            ReturnToSpawn();
            return;
        }

        if (distToTarget > attackRange)
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
        else if (Time.time - lastAttackTime > attackCooldown)
        {
            if (target.TryGetComponent<PlayerCombat>(out var combat))
            {
                combat.TakeDamage(damage);
                lastAttackTime = Time.time;
            }
        }
    }

    [Server]
    void ReturnToSpawn()
    {
        if (Vector3.Distance(transform.position, spawnPoint) > 1f)
        {
            Vector3 dir = (spawnPoint - transform.position).normalized;
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }

    [Server]
    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            // drop loot, xp
        }
    }
}
