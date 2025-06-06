using Mirror;
using UnityEngine;

public class MobRangedAttack : NetworkBehaviour
{
    [Header("Støelba")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 15f;
    public float attackCooldown = 2f;
    public float attackRange = 20f;
    public bool preferHighArc = false;

    private float lastAttackTime = 0f;

    [ServerCallback]
    void Update()
    {
        GameObject target = FindClosestPlayerInRange();
        if (target != null && Time.time - lastAttackTime >= attackCooldown)
        {
            if (CalculateBallisticVelocity(target.transform.position, shootPoint.position, projectileSpeed, preferHighArc, out Vector3 velocity))
            {
                Shoot(velocity);
                lastAttackTime = Time.time;
            }
        }
    }

    [Server]
    void Shoot(Vector3 velocity)
    {
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb)
        {
            rb.velocity = velocity;
            NetworkServer.Spawn(projectile);
        }
    }

    [Server]
    GameObject FindClosestPlayerInRange()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float closestSqrDist = attackRange * attackRange;

        foreach (GameObject player in players)
        {
            float sqrDist = (player.transform.position - transform.position).sqrMagnitude;
            if (sqrDist <= closestSqrDist)
            {
                closest = player;
                closestSqrDist = sqrDist;
            }
        }

        return closest;
    }

    bool CalculateBallisticVelocity(Vector3 target, Vector3 origin, float speed, bool highArc, out Vector3 velocity)
    {
        velocity = Vector3.zero;
        Vector3 delta = target - origin;
        Vector3 deltaXZ = new Vector3(delta.x, 0f, delta.z);
        float y = delta.y;
        float xz = deltaXZ.magnitude;

        float gravity = Mathf.Abs(Physics.gravity.y);
        float speedSquared = speed * speed;

        float discriminant = speedSquared * speedSquared - gravity * (gravity * xz * xz + 2 * y * speedSquared);

        if (discriminant < 0f)
        {
            return false; // No trajectory possible
        }

        float root = Mathf.Sqrt(discriminant);
        float angle = highArc
            ? Mathf.Atan2(speedSquared + root, gravity * xz)
            : Mathf.Atan2(speedSquared - root, gravity * xz);

        velocity = deltaXZ.normalized * speed * Mathf.Cos(angle);
        velocity.y = speed * Mathf.Sin(angle);

        return true;
    }

#if UNITY_EDITOR
    // Pro vizualizaci dosahu ve scénì
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
#endif
}
