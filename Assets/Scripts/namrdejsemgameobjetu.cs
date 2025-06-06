using UnityEngine;

public class namrdejsemgameobjetu : MonoBehaviour
{
    public Camera playerCamera;             // Reference to the main camera
    public string grabbableTag = "Weapon";   // Tag of objects that can be picked up
    public Transform handTransform;         // Where the object will be held (e.g. empty object in player's hand)

    private GameObject heldObject;

    public float gunRotationSpeed = 2f;      // Adjust sensitivity
    private float gunPitch = 0f;             // To clamp the rotation if you want
    private Quaternion defaultGunRotation;   // Store default rotation when grabbed

    void Update()
    {
        // Pick up gun with LEFT mouse button
        if (Input.GetMouseButtonDown(0))
        {
            if (heldObject == null)
            {
                TryPickupObject();
            }
        }

        // Drop gun with RIGHT mouse button
        if (Input.GetMouseButtonDown(1))
        {
            if (heldObject != null)
            {
                DropObject();
            }
        }

        // If holding gun, allow Y mouse rotation
        if (heldObject != null)
        {
            AdjustGunPitchWithMouseY();
        }
    }

    void TryPickupObject()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Center of screen
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            Debug.Log("Raycast hit: " + hit.collider.name);

            if (hit.collider.CompareTag(grabbableTag))
            {
                Debug.Log("Object has correct tag: " + hit.collider.name);
                heldObject = hit.collider.gameObject;

                // Disable physics
                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.isKinematic = true;
                    Debug.Log("Set Rigidbody to kinematic");
                }
                else
                {
                    Debug.LogWarning("No Rigidbody found on object!");
                }

                // Move and parent to hand
                heldObject.transform.SetParent(handTransform);
                heldObject.transform.localPosition = Vector3.zero;
                heldObject.transform.localRotation = Quaternion.identity;

                // Apply your desired local position and rotation
                heldObject.transform.localPosition = new Vector3(-0.0085f, -0.0045f, -0.037f);

                Quaternion defaultRotation = Quaternion.Euler(13.212f, -200f, -101.864f);
                heldObject.transform.localRotation = defaultRotation;

                // Store this rotation for adjustment
                defaultGunRotation = defaultRotation;
                gunPitch = 0f; // Reset pitch when picking up

                Debug.Log("Picked up: " + heldObject.name);
            }
            else
            {
                Debug.Log("Hit object does not have tag: " + grabbableTag);
            }
        }
        else
        {
            Debug.Log("Raycast did not hit anything");
        }
    }

    void DropObject()
    {
        if (heldObject != null)
        {
            // Unparent
            heldObject.transform.SetParent(null);

            // Enable physics
            Rigidbody rb = heldObject.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = false;
                Debug.Log("Set Rigidbody to non-kinematic");
            }
            else
            {
                Debug.LogWarning("No Rigidbody found on held object!");
            }

            Debug.Log("Dropped object: " + heldObject.name);
            heldObject = null;
        }
    }

    void AdjustGunPitchWithMouseY()
    {
        float mouseY = Input.GetAxis("Mouse Y") * gunRotationSpeed;
        gunPitch -= mouseY;
        gunPitch = Mathf.Clamp(gunPitch, -45f, 45f); // Limit pitch (optional)

        // Combine default rotation with pitch adjustment around local X
        Quaternion pitchRotation = Quaternion.Euler(gunPitch, 0f, 0f);
        heldObject.transform.localRotation = defaultGunRotation * pitchRotation;
        Debug.Log("Pitch: " + gunPitch);
    }
}
