using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Charger : Enemy
{
    [SerializeField] float chargeForce;
    [SerializeField] int chargeAttackDamage;
    [SerializeField] bool isCharging = false;
    protected override void ChasePlayer()
    {
        base.ChasePlayer();
        transform.LookAt(player);
    }

    protected override void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);


        if (!alreadyAttacked)
        {
            if (!isCharging)
            {
                transform.LookAt(player);
            }

            ///Attack code here
            isCharging = true;
            StartCoroutine(FlashNDash());
            ///End of attack code

            alreadyAttacked = true;

            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    protected override void ResetAttack()
    {
        base.ResetAttack();
        rb.velocity = Vector3.zero;

        ChangeSkinMaterial(originalMateral, 1);
        isCharging = false;
    }
    void ChargeAttack()
    {
        rb.angularVelocity = Vector3.zero;
        rb.AddForce(transform.forward * chargeForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isCharging)
        {
            var hitObject = collision.gameObject.GetComponent<IHealth>();
            if (hitObject == null) return;
            hitObject.TakeDamage(chargeAttackDamage);
            Rigidbody hitRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            if (hitRigidbody == null) return;
            hitRigidbody.AddForce(transform.forward * rb.velocity.magnitude, ForceMode.Impulse);
        }
    }

    IEnumerator FlashNDash()
    {
            ChangeSkinMaterial(enemyMateral, 1);
        transform.LookAt(player);
        yield return new WaitForSeconds(1f);

        ChargeAttack();
    }
    //public Color Lerp(Color firstColor, Color secondColor, float speed) =>  Color.Lerp(firstColor, secondColor, Mathf.Sin(Time.time* speed));

}
