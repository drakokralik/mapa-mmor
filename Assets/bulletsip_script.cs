using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletsip_script : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        Health playerHealth = collision.gameObject.GetComponent<Health>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(10);  // or any damage amount you want
        }
        Destroy(gameObject); // destroy bullet after hit
    }
}
}
