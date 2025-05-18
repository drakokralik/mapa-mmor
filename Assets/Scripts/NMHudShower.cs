using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class NMHudShower : MonoBehaviour
{

    private NetworkManagerHUD nmHud;
    private CustomNetworkManager customNetworkManager;

    private player_move playerMove;
    private camera_move camMove;

    // Start is called before the first frame update
    void Start()
    {
        nmHud = FindObjectOfType<NetworkManagerHUD>();
        customNetworkManager = FindObjectOfType<CustomNetworkManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (nmHud != null)
        {
            if (playerMove == null)
            {
                playerMove = FindObjectOfType<player_move>();
            }

            if (camMove == null)
            {
                camMove = FindObjectOfType<camera_move>();
            }

            if (Input.GetKey(KeyCode.F1) || !customNetworkManager.isNetworkActive)
            {
                nmHud.enabled = true;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                if (playerMove != null && camMove != null)
                {
                    playerMove.CanMove = false;
                    camMove.EnableLook = false;
                }
            }
            else
            {
                nmHud.enabled = false;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                if (playerMove != null && camMove != null)
                {
                    playerMove.CanMove = true;
                    camMove.EnableLook = true;
                }
            }
        }
    }
}
