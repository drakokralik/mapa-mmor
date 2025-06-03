using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MobAttack : MonoBehaviour
{
    public float knockbackForce = 5f;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10);
            }

            Vector3 knockbackDirection = (transform.position - collision.transform.position).normalized;
            GetComponent<Rigidbody>().AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
    }
}
