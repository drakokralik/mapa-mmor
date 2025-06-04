using System.Collections;
using UnityEngine;

public class scriptcuse : MonoBehaviour
{
    public AudioSource fireSound;
    public float forceAmount = 50f;
    public Animator anim;
    public Rigidbody arrowRb;

    public Transform arrowParentBeforeShot; // set this in inspector to the holder (e.g. weapon)
    public Transform arrowDetachPoint;      // empty GameObject at arrow tip, where it will fly from

    private bool hasFired = false;

    void Start()
    {
        anim = GetComponent<Animator>();
        arrowRb.isKinematic = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && !hasFired)
        {
            // Trigger fire animation
            anim.SetBool("fire", true);
            fireSound.Play();

            hasFired = true;

            // Fire the arrow after short delay so animation can play
            StartCoroutine(FireArrowAfterDelay());
        }
    }

    IEnumerator FireArrowAfterDelay()
    {
        yield return new WaitForSeconds(7f / 60f); // wait ~7 frames (adjust if needed)

        // Detach arrow and apply force
        arrowRb.transform.parent = null; // unparent from weapon
        arrowRb.isKinematic = false;
        arrowRb.MovePosition(arrowDetachPoint.position); // optional: snap to tip
        arrowRb.AddForce(Vector3.left * forceAmount, ForceMode.Impulse); // adjust direction

        // Reset animator bool after short time
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("fire", false);
    }
}
