using UnityEngine;

public class EldritchProjectile : MonoBehaviour
{
    [Header("Explosion Settings")]
    public float radius = 5f;                 // Blast radius
    public float damage = 50f;                // Damage dealt
    public LayerMask damageLayers;            // Layers that can be damaged
    public GameObject impactVFX;              // Optional visual effect
    public float destroyAfter = 3f;           // Auto destroy

    void Start()
    {
        Destroy(gameObject, destroyAfter);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        // Spawn VFX
        if (impactVFX != null)
            Instantiate(impactVFX, transform.position, Quaternion.identity);

        // Detect all colliders in radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, damageLayers);
        foreach (Collider hit in hitColliders)
        {
            // Apply damage to Target script
            Target target = hit.GetComponent<Target>();
            if (target != null)
                target.TakeDamage(damage);

            // Optional knockback
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(500f, transform.position, radius);
        }

        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
