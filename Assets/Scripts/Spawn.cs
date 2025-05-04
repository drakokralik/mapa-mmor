using Mirror;
using UnityEngine;
using System.Collections.Generic;

public class CustomNetworkManager : NetworkManager
{
    public Transform[] spawnPoints;

    private int nextIndex = 0;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        // Vybere spawn point
        Transform start = spawnPoints[nextIndex % spawnPoints.Length];
        nextIndex++;

        // Spawne hráèe na daném místì
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);
    }
}
