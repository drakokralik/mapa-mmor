using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletsip_script : MonoBehaviour
{
    public float activationDelay = 0.3f; 
    private bool canCollide = false;

    void Start()
    {
        Invoke(nameof(ActivateCollision), activationDelay);
    }

    void ActivateCollision()
    {
        canCollide = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!canCollide) return; 

       
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Monster"))
        {
            Health targetHealth = collision.gameObject.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(25); 
            }

            Destroy(gameObject); 
        }
    }
}
