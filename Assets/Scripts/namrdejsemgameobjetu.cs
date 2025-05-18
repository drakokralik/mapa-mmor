using UnityEngine;

public class namrdejsemgameobjetu : MonoBehaviour
{
    public Camera playerCamera;             // Reference to the main camera
    public string grabbableTag = "movable"; // Tag of objects that can be picked up
    public Transform handTransform;         // Where the object will be held (e.g. empty object in player's hand)

    private GameObject heldObject;

    void Update()
    {
        if (Input.GetMouseButtonDown(2)) // Middle mouse button
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

        if (Physics.Raycast(ray, out hit, 3f)) // Adjust range if needed
        {
            if (hit.collider.CompareTag(grabbableTag))
            {
                heldObject = hit.collider.gameObject;

                // Disable physics
                Rigidbody rb = heldObject.GetComponent<Rigidbody>();
                if (rb) rb.isKinematic = true;

                // Move and parent to hand
                heldObject.transform.SetParent(handTransform);
                heldObject.transform.localPosition = Vector3.zero;
                heldObject.transform.localRotation = Quaternion.identity;

                Debug.Log("Picked up: " + heldObject.name);
            }
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
            if (rb) rb.isKinematic = false;

            heldObject = null;

            Debug.Log("Dropped object.");
        }
    }
}
