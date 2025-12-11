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

    private enum AIState { Idle, Chasing, Attacking }
    private AIState currentState = AIState.Idle;

    void Start()
    {
        currentHealth = maxHealth;
        agent.speed = moveSpeed;
        agent.stoppingDistance = attackRange;
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
            // Attack state
            ChangeState(AIState.Attacking);
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
            // Chase state
            ChangeState(AIState.Chasing);
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
        else
        {
            // Idle state
            ChangeState(AIState.Idle);
            agent.isStopped = true;
        }

        // Optional: Smooth walking animation based on velocity
        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    void ChangeState(AIState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        switch (currentState)
        {
            case AIState.Idle:
                animator.SetTrigger("Idle2");
                animator.SetBool("Walk", false);
                break;

            case AIState.Chasing:
                animator.SetBool("Walk", true);
                break;

            case AIState.Attacking:
                // Random attack will be handled in Attack() function
                animator.SetBool("Walk", false);
                break;
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
        // Pick a random attack
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

        // Damage the player if still in range
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
