using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeonteleport : MonoBehaviour

{
    public Transform teleportDestination; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  
        {
            other.transform.position = teleportDestination.position;
        }
    }
}