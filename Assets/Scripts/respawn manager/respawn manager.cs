using System.Collections;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public float defaultRespawnDelay = 10f;

    public void RequestRespawn(GameObject prefab, Vector3 spawnPosition)
    {
        StartCoroutine(RespawnCoroutine(prefab, spawnPosition));
    }

    private IEnumerator RespawnCoroutine(GameObject prefab, Vector3 spawnPosition)
    {
        yield return new WaitForSeconds(defaultRespawnDelay);

        Instantiate(prefab, spawnPosition, Quaternion.identity);
    }
}
