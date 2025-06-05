using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 10;
    public float maxLifeTime = 5f;
    public float gravity = -9.81f;

    private Vector3 velocity;
    private Vector3 startPosition;
    private float lifeTimer = 0f;

    void Start()
    {
        startPosition = transform.position;
        // Poèáteèní rychlost rovná se rychlosti dopøedu + nìjaký vertikální náklon pro oblouk
        velocity = transform.forward * speed + Vector3.up * 5f;  // uprav tu 5f pro výšku oblouku
    }

    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= maxLifeTime)
        {
            Destroy(gameObject);
            return;
        }

        // Pohyb s vlivem gravitace
        velocity += Vector3.up * gravity * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;

        // Otoèení smìrem letu (volitelné)
        if (velocity != Vector3.zero)
            transform.forward = velocity.normalized;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Health health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
        else if (!other.CompareTag("Player") && !other.CompareTag("Bullet"))
        {
            Destroy(gameObject);
        }
    }
}
