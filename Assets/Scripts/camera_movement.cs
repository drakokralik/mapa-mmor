using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class camera_move : NetworkBehaviour
{
    public float CameraSensitivity = 150.0f;
    public float LookUpLimit = 70.0f;

    public bool EnableLook = true;

    public Transform PlayerBody;
    public Vector3 OffsetFromPlayer = new Vector3(0, 2, -5);

    public GameObject polySurface1, polySurface2;

    public float pitch = 0.0f; // vertical  
    private float yaw = 0.0f;   // horizontal

    private bool firstLook = true;

    private Camera cam;

    void Start()
    {
        if (isLocalPlayer)
        {

            //add layer PlayerHead to polySurface1 and polySurface2
            if (polySurface1 != null)
            {
                polySurface1.layer = LayerMask.NameToLayer("PlayerHead");
            }
            if (polySurface2 != null)
            {
                polySurface2.layer = LayerMask.NameToLayer("PlayerHead");
            }

            // Vypne UselessCamera
            GameObject uselessCam = GameObject.Find("UselessCamera");
            if (uselessCam != null)
            {
                Camera cam = uselessCam.GetComponent<Camera>();
                if (cam != null)
                {
                    cam.enabled = false;
                }
            }

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                canvas.SetActive(false);
            }
            cam = GetComponent<Camera>();
        }

        // Automaticky najde PlayerBody pokud nen� nastaven
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

    private void Hide()
    {
        cam.cullingMask &= ~(1 << LayerMask.NameToLayer("PlayerHead"));
    }

    private void Show()
    {
        cam.cullingMask |= 1 << LayerMask.NameToLayer("PlayerHead");
    }

    void Update()
    {
        if (PlayerBody == null || !EnableLook) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (firstLook)
            {
                firstLook = false;
                Show();
            }
            else
            {
                firstLook = true;
                Hide();
            }
        }

        float mouseX = Input.GetAxis("Mouse X") * CameraSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * CameraSensitivity * Time.deltaTime;


        yaw += mouseX;
        pitch -= mouseY;

        if (firstLook)
        {
            transform.position = PlayerBody.position;

            pitch = Mathf.Clamp(pitch, -90, 90);
            transform.rotation = Quaternion.Euler(pitch, yaw, 0);

        }
        else
        {

            pitch = Mathf.Clamp(pitch, -LookUpLimit + 90, LookUpLimit + 90);

            Vector3 offset = OffsetFromPlayer;
            if (offset == Vector3.zero)
                offset = new Vector3(0, 2, -5);

            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 desiredPosition = PlayerBody.position + rotation * offset;

            // Prevent camera clipping
            Vector3 playerHead = PlayerBody.position + Vector3.up * 1.0f;
            Vector3 direction = (desiredPosition - playerHead).normalized;
            float distance = offset.magnitude;

            RaycastHit hit;
            if (Physics.Raycast(playerHead, direction, out hit, distance))
            {
                transform.position = hit.point - direction * 0.1f;
            }
            else
            {
                transform.position = desiredPosition;
            }

            transform.LookAt(PlayerBody.position + Vector3.up * 1.0f);
        }
    }
}
