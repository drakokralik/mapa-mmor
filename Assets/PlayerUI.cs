using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerUI : NetworkBehaviour
{
    [Tooltip("Drag sem svùj HealthBar prefab (UI)")]
    public GameObject healthBarPrefab;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();

        // 1) Najdi v scénì Canvas
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas nenalezený v scénì!");
            return;
        }

        // 2) Instancuj HealthBar prefab a pøiøaï ho do Canvasu
        GameObject hb = Instantiate(healthBarPrefab);
        hb.transform.SetParent(canvas.transform, false);  // zachová UI mìøítko

        // 3) Napoj svùj health systém (pokud máš napø. Slider)
        //    var slider = hb.GetComponentInChildren<Slider>();
        //    slider.value = currentHealth / maxHealth;
        //
        //    A pak v Update healthu aktualizuj slider.
    }
}
