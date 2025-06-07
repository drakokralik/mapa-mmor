using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletsip_script : MonoBehaviour
{
    public float activationDelay = 0.3f; // time after spawn before collisions are active
    private bool canCollide = false;

    void Start()
    {
        // Start the delayed activation
        Invoke(nameof(ActivateCollision), activationDelay);
    }

    void ActivateCollision()
    {
        canCollide = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!canCollide) return; // ignore collisions during delay

        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(10); // apply damage
            }
            Destroy(gameObject); // destroy bullet after hit
        }
    }
}
