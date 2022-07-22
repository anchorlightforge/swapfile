using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    public HealthStats Owner => currentOwner; 
    MeshRenderer _rend;
    Rigidbody rb;
    int health;
    int maxHealth;
    HealthStats controller;
    HealthStats currentOwner;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _rend = GetComponent<MeshRenderer>();
        //assign to the HealthStatsit spawned with
        if(transform.parent)controller = transform.parent.GetComponent<HealthStats>();
        if(controller)currentOwner = controller;
        controller.BindHP(this);
        currentOwner.AssignHB(this);
        //move to offset point

    }
    bool active = true;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hurt(int damage)
    {
        if(active)
        {

            health = Mathf.Clamp(health - damage,0,maxHealth);
            UpdateHealth();
            if(health<=0)
            {
                active = false;
                if (controller.TryGetComponent(out EnemyDissolutionHandler diss))
                    diss.Dissolve(this);
                else if (controller.CompareTag("Player"))
                {
                    //endgame condition failure
                }
            }
        }
    }

    void UpdateHealth()
    {
        float currentPer = (float)health / (float)maxHealth;
        _rend.material.SetFloat("_ProgressBorder", currentPer);
    }

    public void Heal()
    {
        health= maxHealth;
    }

    public void Decouple()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        //remove reference to current owner
        currentOwner = null;

    }
}
