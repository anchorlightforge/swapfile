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

    public Renderer droneRenderer;
    Color originalColor;

    public Transform Turrets;

    public Transform firePosition1, firePosition2;

    public GameObject healthBarLocation;
    [SerializeField] private LayerMask whatIsHealthBar, whatIsEnemy;

    public float speed;

    private void Start()
    {
        if (Healthbar.m_MyEvent != null)
            Healthbar.m_MyEvent.AddListener(SetAndFindNearByHealthBar);
        originalColor=droneRenderer.material.color;
    }

    protected override void Update()
    {
        healthBarInNabRange = Physics.CheckSphere(transform.position, healthBarNabRange, whatIsHealthBar);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        enemyInRange = Physics.CheckSphere(transform.position, sightRange, whatIsEnemy);

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
                if (!playerInAttackRange)
                    currentMode = EnemyModes.Active;
                break;

            case EnemyModes.Gathering:
                GoToHealthBar(healthBarLocation);
                break;

            case EnemyModes.HasHealth:
                DestributeHealth();
                break;
        }
    }
    protected override void ChasePlayer()
    {
        base.ChasePlayer();
        Vector3 direction = player.position - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);
    }

    protected override void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        Vector3 direction = player.position - transform.position;
        Quaternion toRotation=Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, speed * Time.deltaTime);

        //transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Attack Code
            Debug.Log("The Enemy Attacks");
            if (projectile != null)
            {
                Rigidbody _rb1 = Instantiate(projectile, firePosition1.position/*transform.position*/, Quaternion.identity).GetComponent<Rigidbody>();
                Rigidbody _rb2 = Instantiate(projectile, firePosition2.position/*transform.position*/, Quaternion.identity).GetComponent<Rigidbody>();
                _rb1.AddForce(transform.forward * 32f, ForceMode.Impulse);
                _rb2.AddForce(transform.forward * 32f, ForceMode.Impulse);
            }
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
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
            StartCoroutine(EFlash(Color.green));
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
        if (enemyInRange)
        {
            Debug.Log("Enemy In Range of the Drone");
            if (FindClosestEnemyFromRange() != null)
            {
                transform.LookAt(FindClosestEnemyFromRange().gameObject.transform);
                FindClosestEnemyFromRange().currentHealthBar.Heal((int)currentHealthCapacitor);
                FindClosestEnemyFromRange().EFlash(Color.green);
                currentMode=EnemyModes.Active;
            }
        }
    }

    Enemy FindClosestEnemyFromRange()
    {
        Enemy closestEnemy = null;
        Collider[] enemies = Physics.OverlapSphere(transform.position, sightRange, whatIsEnemy);
        List<Collider> enemyList = new List<Collider>(enemies);

        Transform tMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;

        foreach (Collider c in enemyList)
        {
            int cCurrent = c.GetComponent<Enemy>().currentHealthBar.controllerHealth;
            int cMax = c.GetComponent<Enemy>().currentHealthBar.maxControllerHealth;
            Transform cTransform = c.transform;
            float dist = Vector3.Distance(cTransform.position, currentPos);

            // Remove whoever has high health
            if (cCurrent > cMax - (cMax / 4))
            {
                // Possible issue Later
                enemyList.Remove(c);
            }

            if (dist < minDist)
            {
                tMin = cTransform;
                minDist = dist;
                closestEnemy = c.GetComponent<Enemy>();
            }
        }
        return closestEnemy;
    }

    public override IEnumerator EFlash(Color coloring,Material materialling=null)
    {
        droneRenderer.material.color = coloring;
        yield return new WaitForSeconds(flashTime);
        droneRenderer.material.color = originalColor;
    }
}
