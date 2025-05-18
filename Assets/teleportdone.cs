using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportdone : MonoBehaviour
{
    public Transform destination;  // The object where the player should be teleported to

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player") && destination != null)
        {
            other.transform.position = destination.position;

            // Optional: Uncomment this line to also match rotation
            // other.transform.rotation = destination.rotation;
        }
    }
}
