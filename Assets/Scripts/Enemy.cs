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
    Recovering,
}
public class Enemy : MonoBehaviour, IHealth
{
    public Rigidbody rb;

    public Healthbar currentHealthBar;
    public NavMeshAgent agent;

    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer;

    public float walkPointRange;
    public MeshRenderer[] enemyMaterials;
    protected Color originalColor;
    public float flashTime;

    [Header("Attacking")]
    public float timeBetweenAttacks;
    [SerializeField] protected bool alreadyAttacked;
    public GameObject projectile;

    [Header("States")]
    public float attackRange;
    public bool playerInAttackRange;



    //public float staggerTime;
    //public bool isStaggered;

    // save in sightrange for drones
    public EnemyModes currentMode;

    public virtual void Awake()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();

        currentMode = EnemyModes.Inactive;
        enemyMaterials = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < enemyMaterials.Length; i++)
        {
        originalColor = enemyMaterials[i].material.color;
        }
    }
    private void Start()
    {
       // player = GameObject.FindGameObjectWithTag("Player").transform;
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
                if (!playerInAttackRange)
                    currentMode = EnemyModes.Active;
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
            Debug.Log("The Enemy Attacks");

            alreadyAttacked = true;
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
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    public virtual IEnumerator EFlash(Color coloring, Material materialling = null)
    {
        foreach(MeshRenderer enemyMaterial in enemyMaterials)
        enemyMaterial.material.color = coloring;
        yield return new WaitForSeconds(flashTime);
        foreach (MeshRenderer enemyMaterial in enemyMaterials)
            enemyMaterial.material.color = originalColor;
    }
}
