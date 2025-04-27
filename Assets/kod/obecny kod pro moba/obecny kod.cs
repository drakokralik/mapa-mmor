using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    [Header("Mob Stats")]
    public string mobName;
    public int maxHP;
    private int currentHP;
    public int attackDamage;
    public float attackCooldown;
    public float aggroRange;
    public GameObject lootPrefab;

    [Header("Respawn")]
    public GameObject prefabReference; // Pøidáno: originální prefab!

    private Dictionary<GameObject, float> threatTable = new Dictionary<GameObject, float>();
    private GameObject currentTarget;

    private void Start()
    {
        currentHP = maxHP;

        if (prefabReference == null)
        {
            Debug.LogWarning($"{mobName} nemá nastavený prefabReference! Nastav v inspektoru.");
        }
    }

    private void Update()
    {
        if (currentTarget != null)
        {
            AttackTarget();
        }
        else
        {
            SearchForTarget();
        }
    }

    public void TakeDamage(int amount, GameObject attacker)
    {
        currentHP -= amount;

        if (!threatTable.ContainsKey(attacker))
            threatTable.Add(attacker, 0);

        threatTable[attacker] += amount;

        UpdateAggro();

        if (currentHP <= 0)
        {
            Die();
        }
    }

    private void UpdateAggro()
    {
        float highestThreat = 0;
        GameObject topThreat = null;

        foreach (var entry in threatTable)
        {
            if (entry.Value > highestThreat)
            {
                highestThreat = entry.Value;
                topThreat = entry.Key;
            }
        }

        currentTarget = topThreat;
    }

    private void AttackTarget()
    {
        // Tady pøidáš animace a cooldown pozdìji
        Debug.Log($"{mobName} útoèí na {currentTarget.name} za {attackDamage} dmg!");
    }

    private void SearchForTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, aggroRange);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                currentTarget = collider.gameObject;
                break;
            }
        }
    }

    private void Die()
    {
        Debug.Log($"{mobName} zemøel.");

        if (lootPrefab != null)
        {
            Instantiate(lootPrefab, transform.position, Quaternion.identity);
        }

        RespawnManager respawnManager = FindObjectOfType<RespawnManager>();
        if (respawnManager != null && prefabReference != null)
        {
            respawnManager.RequestRespawn(prefabReference, transform.position);
        }

        Destroy(gameObject);
    }
}
