using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Charger : Enemy
{
    [SerializeField] float chargeForce;
    [SerializeField] int chargeAttackDamage;
    [SerializeField] float chargeAttackRange;
    [SerializeField] bool isCharging = false;
    protected override void ChasePlayer()
    {
        base.ChasePlayer();
    }

    protected override void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);


        if (!alreadyAttacked)
        {
            transform.LookAt(player);

            ///Attack code here
            ChargeAttack();
            //Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            //rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code
            if (rb.velocity.magnitude < 15.0f)
            {
                alreadyAttacked = true;
                isCharging = false;
            }
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ChargeAttack()
    {
        rb.AddForce(transform.forward* chargeForce, ForceMode.Impulse);
        isCharging = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isCharging)
        {
            var hitObject = collision.gameObject.GetComponent<IHealth>();
            if (hitObject == null) return;
            hitObject.TakeDamage(chargeAttackDamage);
        }
    }
}
