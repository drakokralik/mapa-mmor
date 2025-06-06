using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class mob_die : MonoBehaviour
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
        if (collision.gameObject.CompareTag("Bullet"))  // Make sure your bullet prefab has the tag "Bullet"
        {
        Destroy(gameObject);  // Destroy this mob
        }
    }

}
