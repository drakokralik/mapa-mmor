using UnityEngine;
using UnityEngine.AI;

public class MobAI : MonoBehaviour
{
    private Transform target;               // Hráè (automaticky najde objekt s tagem "Player")
    public float aggroRange = 10f;          // Dosah, kdy zaène mob sledovat hráèe
    public float returnRange = 15f;         // Maximální vzdálenost, kam mob mùže zajít
    public float stoppingDistance = 2f;     // Vzdálenost, pøi které mob zastaví u hráèe

    public int maxHealth = 100;
    private int currentHealth;

    private Vector3 spawnPosition;
    private NavMeshAgent agent;
    private bool isReturning = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spawnPosition = transform.position;
        currentHealth = maxHealth;

        // Najdi hráèe podle tagu
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
        else
        {
            Debug.LogError("MobAI: Nenalezen objekt s tagem Player!");
        }
    }

    void Update()
    {
        if (target == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);
        float distanceToSpawn = Vector3.Distance(transform.position, spawnPosition);

        if (distanceToPlayer <= aggroRange)
        {
            isReturning = false;
            agent.stoppingDistance = stoppingDistance;
            agent.SetDestination(target.position);
        }
        else if (distanceToPlayer > aggroRange && distanceToSpawn > 1f)
        {
            isReturning = true;
            agent.stoppingDistance = 0f;
            agent.SetDestination(spawnPosition);
        }
        else if (isReturning && distanceToSpawn <= 1f)
        {
            isReturning = false;
            currentHealth = maxHealth; // Vyléèí se
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
