using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerUI : NetworkBehaviour
{
    [Tooltip("Drag sem sv�j HealthBar prefab (UI)")]
    public GameObject healthBarPrefab;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // Disable pregame canvas and enable ingame canvas
        GameObject pregameCanvas = GameObject.FindGameObjectWithTag("canvas_pregame");
        if (pregameCanvas != null)
            pregameCanvas.SetActive(false);

        GameObject ingameCanvas = GameObject.FindGameObjectWithTag("canvas_ingame");
        if (ingameCanvas != null)
            ingameCanvas.SetActive(true);

        // 1) Najdi v sc�n� Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas nenalezen� v sc�n�!");
            return;
        }



        // 3) Napoj sv�j health syst�m (pokud m� nap�. Slider)
        //    var slider = hb.GetComponentInChildren<Slider>();
        //    slider.value = currentHealth / maxHealth;
        //
        //    A pak v Update healthu aktualizuj slider.
    }
}
