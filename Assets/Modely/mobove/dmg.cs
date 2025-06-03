using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MobAttack : MonoBehaviour
{
    public float knockbackForce = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Mob collided with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth == null)
            {
                playerHealth = collision.gameObject.GetComponentInParent<PlayerHealth>();
            }

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10);
                Debug.Log("Player took 10 damage");
            }
            else
            {
                Debug.LogWarning("PlayerHealth script not found on player!");
            }

            Vector3 knockbackDirection = (transform.position - collision.transform.position).normalized;
            GetComponent<Rigidbody>().AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
    }
}
