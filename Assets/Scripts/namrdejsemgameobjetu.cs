using UnityEngine;

public class namrdejsemgameobjetu : MonoBehaviour
{
    public Camera playerCamera;             // Reference to the main camera
    public string grabbableTag = "Weapon"; // Tag of objects that can be picked up
    public Transform handTransform;         // Where the object will be held (e.g. empty object in player's hand)

    private GameObject heldObject;

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Middle mouse button
        {
            if (heldObject == null)
            {
                TryPickupObject();
            }
            else
            {
                DropObject();
            }
        }
    }

    void TryPickupObject()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0)); // Center of screen
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f)) // Adjust range if needed
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
                    // Apply the same local position and rotation you shared

                heldObject.transform.localPosition = new Vector3(-0.0085f, -0.0045f, -0.037f);

                heldObject.transform.localRotation = Quaternion.Euler(13.212f, -200f, -101.864f); // Example rotation in degrees

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

            Debug.Log("Dropped objct: " + heldObject.name);
            heldObject = null;
        }
    }
}
