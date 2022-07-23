using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyModes
{
    Inactive,
    Active,
    Chasing,
    Attacking,
    Gathering,
    HasHealth,
}
public class Enemy : MonoBehaviour,IHealth
{
    [SerializeField] Healthbar currentHealthBar;
    public NavMeshAgent agent;

    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public float walkPointRange;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    [Header("States")]
    public float attackRange;
    public bool playerInAttackRange;

    
    public float staggerTime;
    public bool isStaggered;

    // save in sightrange for drones
    public EnemyModes currentMode;

    public virtual void Awake()
    {
         agent= GetComponent<NavMeshAgent>();
        currentMode = EnemyModes.Inactive;
    }

    protected virtual void Update()
    {
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        switch (currentMode)
        {
            case EnemyModes.Active:
                if (!playerInAttackRange)
                    currentMode = EnemyModes.Chasing;
                else
                    currentMode = EnemyModes.Attacking;
                break;

            case EnemyModes.Chasing:
                ChasePlayer();
                if (playerInAttackRange)
                    currentMode = EnemyModes.Attacking;
                break;

            case EnemyModes.Attacking:
                AttackPlayer();
                break;
        }
    }

    public virtual void Awaken()
    {
        currentMode = EnemyModes.Active;
    }

    public void TakeDamage(int damage)
    {
        currentHealthBar.Hurt(damage);
    }

    protected virtual void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    protected virtual void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Attack Code


            alreadyAttacked=true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    protected virtual void ResetAttack()
    {
        alreadyAttacked = false;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,attackRange);
    }
}
