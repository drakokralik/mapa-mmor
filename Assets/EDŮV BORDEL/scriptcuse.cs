using System.Collections;
using UnityEngine;

public class scriptcuse : MonoBehaviour
{
    public AudioSource fireSound;
    public float forceAmount = 10f;
    public Animator anim;

    public GameObject animatedArrow;         // shown visually during animation
    public GameObject realArrowPrefab;       // physics arrow prefab
    public Transform arrowSpawnPoint;        // where to shoot from

    private bool isFiring = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isFiring && transform.parent != null)
        {
            isFiring = true;

            anim.SetTrigger("fire");

            if (fireSound != null)
                fireSound.Play();

            StartCoroutine(FireArrowAfterDelay());
        }
    }

    IEnumerator FireArrowAfterDelay()
    {
        yield return new WaitForSeconds(1f); // match with bow release
        anim.SetBool("fire", false);
        if (animatedArrow != null)
            animatedArrow.SetActive(false);

        GameObject arrowClone = Instantiate(realArrowPrefab, arrowSpawnPoint.position, arrowSpawnPoint.rotation);
        Rigidbody rb = arrowClone.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            Vector3 shootDirection = transform.forward;
            rb.AddForce(shootDirection.normalized * forceAmount, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(0.2f); // short delay before reset
        isFiring = false;

        if (animatedArrow != null)
            animatedArrow.SetActive(true);
    }
}
