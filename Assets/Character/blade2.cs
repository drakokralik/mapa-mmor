using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public int damageAmount = 10;
    private WeaponSwing swing;
    private bool hasHit = false;

    void Start()
    {
        swing = GetComponent<WeaponSwing>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (!swing || !swing.isDamaging) return;
        if (hasHit) return; // už zasáhl v tomto švihu

        if (other.CompareTag("Enemy"))
        {
            var health = other.GetComponent<Health>();
            if (health != null)
            {
                health.TakeDamage(damageAmount);
                Debug.Log("Zásah!");
                hasHit = true; // už nemùže zasáhnout znovu v tomto švihu
            }
        }
    }

    void Update()
    {
        // Reset hasHit pokud zrovna neútoèíš
        if (!swing || !swing.isDamaging)
        {
            hasHit = false;
        }
    }
}
