using UnityEngine;
using Mirror;

public class MonsterSpawner : NetworkBehaviour
{
    [Header("Nastavení spawneru")]
    public GameObject monsterPrefab;
    public float spawnRadius = 5f;
    public float spawnInterval = 5f;
    public int maxMonsters = 10;

    private float lastSpawnTime = 0f;

    void Update()
    {
        
        if (!isServer) return;

        
        if (Time.time - lastSpawnTime > spawnInterval)
        {
            int currentMonsterCount = GameObject.FindGameObjectsWithTag("Monster").Length;

            if (currentMonsterCount < maxMonsters)
            {
                
                Vector2 offset2D = Random.insideUnitCircle * spawnRadius;
                Vector3 spawnPos = new Vector3(
                    transform.position.x + offset2D.x,
                    transform.position.y,
                    transform.position.z + offset2D.y
                );

                
                GameObject monster = Instantiate(monsterPrefab, spawnPos, Quaternion.identity);
                NetworkServer.Spawn(monster);
            }

            lastSpawnTime = Time.time;
        }
    }
}
