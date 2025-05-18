using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public int maxHP = 200;
    private int currentHP;

    public int damage = 25;

    void Start()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        Debug.Log($"Player took {amount} damage. HP left: {currentHP}");

        if (currentHP <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player died.");
        // restart, respawn, disable movement apod.
    }

    // Volání zvenèí pøi útoku na moba
    public void Attack(GameObject target)
    {
        MobStats mob = target.GetComponent<MobStats>();
        if (mob != null)
        {
            mob.TakeDamage(damage);
        }
    }
}
