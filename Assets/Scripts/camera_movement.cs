using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_move : MonoBehaviour
{
    public float CameraSensitivity = 10.0f;
    public float LookUpLimit = 70.0f;

    public Transform PlayerBody;
    public Vector3 OffsetFromPlayer;

    private float xRot = 0.0f;
    private float yRot = 0.0f;

    void Start()
    {
        // Automaticky najde PlayerBody pokud není nastaven
        if (PlayerBody == null)
        {
            if (transform.root != null)
            {
                PlayerBody = transform.root;
            }
            else
            {
                Debug.LogError("PlayerBody is not assigned and could not be found!");
            }
        }
    }

    void Update()
    {
        if (PlayerBody == null)
            return; // bezpeènostní pojistka, kdyby náhodou

        float mouseX = Input.GetAxis("Mouse X") * CameraSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * CameraSensitivity * Time.deltaTime;

        xRot += mouseX;
        yRot -= mouseY;

        yRot = Mathf.Clamp(yRot, -LookUpLimit, LookUpLimit);

        transform.rotation = Quaternion.Euler(yRot, xRot, 0);
        transform.position = PlayerBody.position + OffsetFromPlayer;
    }
}
