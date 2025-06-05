using Mirror;
using UnityEngine;

public class MobRangedAttack : NetworkBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float shootForce = 10f;
    public float attackCooldown = 2f;
    public float attackRange = 20f;
    public bool preferHighArc = false;

    private float lastAttackTime;

    [ServerCallback]
    private void Update()
    {
        GameObject target = FindClosestPlayer();
        if (target != null && Time.time - lastAttackTime > attackCooldown)
        {
            float dist = Vector3.Distance(transform.position, target.transform.position);
            if (dist <= attackRange)
            {
                ShootAt(target.transform.position);
            }
        }
    }

    [Server]
    void ShootAt(Vector3 target)
    {
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 velocity;
            bool success = CalculateBallisticVelocity(target, shootPoint.position, shootForce, preferHighArc, out velocity);
            if (success)
            {
                rb.velocity = velocity;
            }
            else
            {
                Debug.LogWarning("Cíl mimo dostøel.");
                Destroy(projectile);
                return;
            }
        }

        NetworkServer.Spawn(projectile);
        lastAttackTime = Time.time;
    }

    [Server]
    GameObject FindClosestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject p in players)
        {
            float d = Vector3.Distance(transform.position, p.transform.position);
            if (d < minDist)
            {
                closest = p;
                minDist = d;
            }
        }

        return closest;
    }

    bool CalculateBallisticVelocity(Vector3 target, Vector3 origin, float speed, bool highArc, out Vector3 result)
    {
        result = Vector3.zero;
        Vector3 toTarget = target - origin;
        Vector3 toTargetXZ = new Vector3(toTarget.x, 0, toTarget.z);
        float y = toTarget.y;
        float xz = toTargetXZ.magnitude;

        float gravity = -Physics.gravity.y;
        float speed2 = speed * speed;
        float speed4 = speed2 * speed2;
        float gx = gravity * xz;

        float discriminant = speed4 - gravity * (gravity * xz * xz + 2 * y * speed2);
        if (discriminant < 0)
        {
            return false; // No solution
        }

        float root = Mathf.Sqrt(discriminant);
        float lowAngle = Mathf.Atan2(speed2 - root, gx);
        float highAngle = Mathf.Atan2(speed2 + root, gx);
        float angle = highArc ? highAngle : lowAngle;

        Vector3 velocity = toTargetXZ.normalized * speed * Mathf.Cos(angle);
        velocity.y = speed * Mathf.Sin(angle);

        result = velocity;
        return true;
    }
}
