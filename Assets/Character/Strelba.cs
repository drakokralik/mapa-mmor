using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;  // child objekt, odkud støelíš
    public float fireCooldown = 1f;
    private float cooldownTimer = 0f;

    void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && cooldownTimer <= 0f)
        {
            Shoot();
            cooldownTimer = fireCooldown;
        }
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
}
