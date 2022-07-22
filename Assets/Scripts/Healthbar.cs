using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    int health;
    int maxHealth;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hurt(int damage)
    {
        health = Mathf.Clamp(health - damage,0,maxHealth);
    }

    public void Heal()
    {
        health= maxHealth;
    }
}
