using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Drone : Enemy
{
    public float healthBarNabRange;
    public bool healthBarInNabRange;

    float currentHealthCapacitor;
    public float maxHealthCapacitor;

    public float sightRange;
    public bool healthBarInSightRange;

    public bool enemyInRange;
    public float healTimer;

    public GameObject healthBarLocation;
    private LayerMask whatIsHealthBar;

    private void Start()
    {
        Healthbar.m_MyEvent.AddListener(SetAndFindNearByHealthBar);
    }

    protected override void Update()
    {
        healthBarInNabRange = Physics.CheckSphere(transform.position, healthBarNabRange, whatIsHealthBar);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        switch (currentMode)
        {
            case EnemyModes.Active:
                if (!healthBarInSightRange)
                {
                    if (!playerInAttackRange)
                        currentMode = EnemyModes.Chasing;
                    else
                        currentMode = EnemyModes.Attacking;
                }
                else
                    currentMode = EnemyModes.Gathering;
                break;

            case EnemyModes.Chasing:
                ChasePlayer();
                if (playerInAttackRange && !healthBarInSightRange)
                    currentMode = EnemyModes.Attacking;
                break;

            case EnemyModes.Attacking:
                AttackPlayer();
                break;

            case EnemyModes.Gathering:
                GoToHealthBar(healthBarLocation);
                break;

            case EnemyModes.HasHealth:
                break;
        }
    }
    protected override void ChasePlayer()
    {
        base.ChasePlayer();
    }

    protected override void AttackPlayer()
    {
        base.AttackPlayer();
    }

    void SetAndFindNearByHealthBar(GameObject currentHealthBar)
    {
        healthBarLocation = currentHealthBar;
        if (healthBarLocation != null)
        {
            if (Vector3.Distance(healthBarLocation.transform.position, transform.position) <= sightRange)
            {
                healthBarInSightRange = true;
                currentMode = EnemyModes.Gathering;
            }
        }
    }

    void GoToHealthBar(GameObject healthBar)
    {
        agent.SetDestination(healthBar.transform.position);
        if (healthBarLocation == null)
        {
            healthBarInSightRange = false;
            currentMode = EnemyModes.Active;
            return;
        }
        if (healthBarInNabRange)
            GrabHealthBar();
    }

    void GrabHealthBar()
    {
        RaycastHit hit;
        Vector3 healthBarDirection = (healthBarLocation.transform.position - this.transform.position).normalized;
        if (Physics.Raycast(transform.position, healthBarDirection, out hit, healthBarNabRange))
        {
            currentHealthCapacitor = Mathf.Clamp(currentHealthCapacitor + healthBarLocation.GetComponent<Healthbar>().containedRemainingHealth, 0, maxHealthCapacitor);
            Destroy(healthBarLocation);
            currentMode = EnemyModes.HasHealth;
        }
        else
        {
            Debug.Log("Enemy missed the health bar");
            currentMode = EnemyModes.Active;
        }
    }

    void DestributeHealth()
    {

    }
}
