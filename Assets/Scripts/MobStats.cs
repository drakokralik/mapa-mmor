using UnityEngine;

public class MobStats : MonoBehaviour
{
    public int maxHP = 100;
    private int currentHP;

    public int damage = 10;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log($"{gameObject.name} took {amount} damage. HP left: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        // zde pøidáš loot, XP, efekt smrti atd.
        Destroy(gameObject);
    }

    public int GetDamage()
    {
        return damage;
    }
}
