using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Standard : Enemy
{
    [SerializeField] MeshRenderer[] chargeModels;
    [SerializeField] GameObject bulletPoint;

    public override void Awake()
    {
        base.Awake();
        //hide the charger pieces
        foreach (MeshRenderer chargeModel in chargeModels)
            chargeModel.gameObject.SetActive(false);
    }

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
                Vector3 direction = (player.position - transform.position).normalized;
                Debug.DrawLine(transform.position, direction * 50,Color.black);
                Rigidbody _rb = Instantiate(projectile, bulletPoint.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
                _rb.AddForce(transform.forward * 50f, ForceMode.Impulse);
                //_rb.AddForce(transform.up * 8f, ForceMode.Impulse);
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
