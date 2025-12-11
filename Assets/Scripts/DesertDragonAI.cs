using UnityEngine;
using UnityEngine.AI;

public class DesertDragonAI : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public Transform player;
    public NavMeshAgent agent;

    [Header("Stats")]
    public float maxHealth = 500f;
    public float currentHealth;
    public float moveSpeed = 3.5f;
    public float attackRange = 5f;
    public float chaseRange = 20f;
    public float attackDamage = 50f;
    public float attackCooldown = 3f;

    private float lastAttackTime;

    void Start()
    {
        currentHealth = maxHealth;
        agent.speed = moveSpeed;
    }

    void Update()
    {
        if (currentHealth <= 0f)
        {
            Die();
            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            agent.isStopped = true;
            FacePlayer();
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
        else if (distance <= chaseRange)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
            animator.SetBool("Walk", true);
        }
        else
        {
            agent.isStopped = true;
            animator.SetBool("Walk", false);
            animator.SetTrigger("Idle2");
        }
    }

    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 5f);
    }

    void Attack()
    {
        int attackIndex = Random.Range(0, 3);
        switch (attackIndex)
        {
            case 0:
                animator.SetTrigger("Basic Attack");
                break;
            case 1:
                animator.SetTrigger("Claw Attack");
                break;
            case 2:
                animator.SetTrigger("Horn Attack");
                break;
        }

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            PlayerStats stats = player.GetComponent<PlayerStats>();
            if (stats != null)
            {
                stats.TakeDamage(attackDamage);
            }
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        animator.SetTrigger("Get Hit");
    }

    private void Die()
    {
        animator.SetTrigger("Die");
        agent.isStopped = true;
        this.enabled = false;
        Destroy(gameObject, 5f);
    }
}