using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Standard : Enemy
{

    protected override void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Attack Code
            Debug.Log("The Enemy Attacks");

            if (projectile != null)
            {
                Rigidbody _rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                _rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
                _rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            }

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }

    protected override void ChasePlayer()
    {
        base.ChasePlayer();
    }
    protected override void Update()
    {
        base.Update();
    }
}
