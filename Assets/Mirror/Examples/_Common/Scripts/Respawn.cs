using System.Collections;
using UnityEngine;
using Mirror;

namespace Mirror.Examples.Common
{
    public static class Respawn
    {
        public static void RespawnPlayer(bool respawn, byte respawnTime, NetworkConnectionToClient conn)
        {
            NetworkManager.singleton.StartCoroutine(DoRespawn(respawn, respawnTime, conn));
        }

        static IEnumerator DoRespawn(bool respawn, byte respawnTime, NetworkConnectionToClient conn)
        {
            yield return null;

            if (!respawn)
            {
                NetworkServer.RemovePlayerForConnection(conn, RemovePlayerOptions.Destroy);
                yield break;
            }

            GameObject playerObject = conn.identity.gameObject;
            NetworkServer.RemovePlayerForConnection(conn, RemovePlayerOptions.Unspawn);

            yield return new WaitForSeconds(respawnTime);

            Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
            Vector3 position = spawnPoint != null ? spawnPoint.position : Vector3.up * 5f;
            Quaternion rotation = spawnPoint != null ? spawnPoint.rotation : Quaternion.identity;

            playerObject.transform.SetPositionAndRotation(position, rotation);

            NetworkServer.AddPlayerForConnection(conn, playerObject);

            if (playerObject.TryGetComponent(out PlayerCombat pc))
            {
                pc.ResetStats();
            }
        }
    }
}
