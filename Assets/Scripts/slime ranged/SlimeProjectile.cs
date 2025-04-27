using UnityEngine;

public class SlimeProjectile : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit by slime projectile for " + damage + " damage!");
            Destroy(gameObject);
        }
    }
}