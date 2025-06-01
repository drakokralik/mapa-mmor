using Mirror;
using UnityEngine;
using System.Collections;

public class teleport_of_script : MonoBehaviour
{
    public Transform teleportTarget;

    private void OnTriggerEnter(Collider other)
    {
        if (!NetworkServer.active) return;

        NetworkIdentity identity = other.GetComponent<NetworkIdentity>();
        if (identity != null && identity.connectionToClient != null)
        {
            StartCoroutine(TeleportWithCharacterController(identity.gameObject));
        }
    }

    private IEnumerator TeleportWithCharacterController(GameObject player)
    {
        CharacterController cc = player.GetComponent<CharacterController>();

        if (cc != null)
        {
            cc.enabled = false;                // Disable CC to avoid physics interference
            player.transform.position = teleportTarget.position;
            yield return null;                 // Wait one frame
            cc.enabled = true;                 // Re-enable CC
        }
        else
        {
            // Just in case no CharacterController (fallback)
            player.transform.position = teleportTarget.position;
        }
    }
}
