using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthStats : MonoBehaviour
{
    [SerializeField] float health;
    [SerializeField] float maxHealth;
    Healthbar healthbarActual;
    Healthbar healthbarAttached;
    public Healthbar CurrentHealthbar => healthbarAttached;
    //This component exists on all playing objects 
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void BindHP(Healthbar bar)
    {
        healthbarActual = bar;
    }
    public void AssignHB(Healthbar newBar)
    {
        healthbarAttached = newBar;
    }
    public void TakeDamage(int damage)
    {
        healthbarAttached.Hurt(damage);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
