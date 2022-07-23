using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class MyGameObjectEvent :UnityEvent<GameObject>
{

}

public class Healthbar : MonoBehaviour, IHealth
{
    //public delegate void WhenDropped(GameObject currentHealthBar);
    //public static event WhenDropped OnDropped;

    public static MyGameObjectEvent m_MyEvent;

    //public HealthStats Owner => currentOwner; 
    public GameObject currentOwner;
    MeshRenderer _rend;
    Rigidbody rb;

    int controllerHealth;
    [SerializeField] int maxControllerHealth;

    int hbHealth;
    [SerializeField] int maxHBHealth;

    [HideInInspector] public int containedRemainingHealth;
    //HealthStats controller;
    //HealthStats currentOwner;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        _rend = GetComponent<MeshRenderer>();
        controllerHealth = maxControllerHealth;
        hbHealth = maxHBHealth;
        //assign to the HealthStatsit spawned with
        //if(transform.parent)controller = transform.parent.GetComponent<HealthStats>();
        //if(controller)currentOwner = controller;
        //controller.BindHP(this);
        //currentOwner.AssignHB(this);
        //move to offset point

    }
    bool active = true;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        // if bar is active and doesn't belong to the player
        if (active && !currentOwner.CompareTag("Player"))
        {
            hbHealth = Mathf.Clamp(hbHealth - damage, 0, maxHBHealth);
            if (hbHealth <= 0 && controllerHealth > 0)
            {
                active = false;
                containedRemainingHealth = controllerHealth;
                if (currentOwner.TryGetComponent(out EnemyDissolutionHandler diss))
                    diss.Dissolve(this);
                Decouple();
            }
        }
    }

    public void Hurt(int damage)
    {
        if (active)
        {

            controllerHealth = Mathf.Clamp(controllerHealth - damage, 0, maxControllerHealth);

            // hbHealth also takes damage if this isn't the player
            if (!currentOwner.CompareTag("Player"))
                TakeDamage(damage);

            UpdateHealth();
            if (controllerHealth <= 0)
            {
                active = false;
                if (currentOwner.TryGetComponent(out EnemyDissolutionHandler diss))
                    diss.Dissolve(this);
                else if (currentOwner.CompareTag("Player"))
                {
                    //endgame condition failure
                }
            }
        }
    }

    void UpdateHealth()
    {
        float currentPer = (float)controllerHealth / (float)maxControllerHealth;
        _rend.material.SetFloat("_ProgressBorder", currentPer);
    }

    public void Heal(int healthGained)
    {
        controllerHealth = Mathf.Clamp(controllerHealth + healthGained, 0, maxControllerHealth);
        UpdateHealth();
    }

    public void Decouple()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        //remove reference to current owner
        currentOwner = null;
       // if (OnDropped != null)
            //OnDropped(this.gameObject);

        if(m_MyEvent != null)
        m_MyEvent.Invoke(this.gameObject);
    }
}
