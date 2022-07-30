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

    [Header("Detect Player")]
    bool playerInRange;
    public LayerMask whatIsPlayer;
    public float playerRange;

    //public HealthStats Owner => currentOwner; 
    public GameObject currentOwner;
    [SerializeField] MeshRenderer _rend;
    [SerializeField] MeshRenderer skin;
    [SerializeField] Material messMaterial;
    Material originalMaterial;
    public GameObject cube;
    Rigidbody rb;

    public int controllerHealth;
    public int maxControllerHealth;

    public int hbHealth;
    [SerializeField] int maxHBHealth;

    [HideInInspector] public int containedRemainingHealth;
    //HealthStats controller;
    //HealthStats currentOwner;
    // Start is called before the first frame update
    void Start()
    {
        foley = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        _rend = GetComponent<MeshRenderer>();
        originalMaterial = skin.material;
        controllerHealth = maxControllerHealth;
        hbHealth = maxHBHealth;
        //assign to the HealthStatsit spawned with
        //if(transform.parent)controller = transform.parent.GetComponent<HealthStats>();
        if (currentOwner == null) currentOwner = transform.parent.gameObject;
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

    public void HealthBarBreak()
    {
        pickupHealth = controllerHealth / 4;
        controllerHealth = 1;
        Decouple();
        pickup = true;
    }
    bool pickup;
    int pickupHealth = 0;
    public void TakeDamage(int damage)
    {
        // if bar is active and doesn't belong to the player
        if (active && !currentOwner.CompareTag("Player"))
        {
            hbHealth = Mathf.Clamp(hbHealth - damage, 0, maxHBHealth);
            StartCoroutine(EFlash(messMaterial));
            if (hbHealth <= 0 && controllerHealth > 0)
            {
                active = false;
                containedRemainingHealth = controllerHealth;
                if (currentOwner.TryGetComponent(out EnemyDissolutionHandler diss))
                    diss.Dissolve(this, false);
                Decouple();
            }
        }
    }

    public void Hurt(int damage)
    {
        if (active)
        {
            Debug.Log("Controller Health: "+controllerHealth);

            controllerHealth = Mathf.Clamp(controllerHealth - damage, 0, maxControllerHealth);
            Debug.Log("Controller Health: " + controllerHealth);

            // hbHealth also takes damage if this isn't the player
            if (!currentOwner.CompareTag("Player"))
                TakeDamage(damage);

            UpdateHealth();
            if (controllerHealth <= 0)
            {
                active = false;
                if (currentOwner.TryGetComponent(out EnemyDissolutionHandler diss))
                {
                    diss.Dissolve(this, true);
                    Debug.Log("Fade Away");

                }
                else if (currentOwner.CompareTag("Player"))
                {
                    currentOwner.GetComponent<PlayerMovement>().Death();
                    //endgame condition failure
                }
            }
        }
    }

    void UpdateHealth()
    {
        float currentPer = (float)controllerHealth / (float)maxControllerHealth;
        if(currentOwner!=null)
        {

        if (currentOwner.CompareTag("Enemy"))
            currentPer = (currentPer * 14) - 6;
        }
        _rend.material.SetFloat("_ProgressBorder", currentPer);
        if(currentOwner!=null)
        {
            if (currentOwner.CompareTag("Player")) UIManager.Instance.SetHealth(currentPer);
        }    
    }

    public void Heal(int healthGained)
    {
        if (controllerHealth + healthGained > 100 && this.transform.parent.CompareTag("Player"))
        {
            FindObjectOfType<WeaponHandling>().AddAmmo(healthGained + controllerHealth - 100);
        }
        controllerHealth = Mathf.Clamp(controllerHealth + healthGained, 0, maxControllerHealth);
        UpdateHealth();
    }
    bool down;

    private void OnCollisionEnter(Collision collision)
    {
        if (down)
        {
            foley.PlayOneShot(hitGroundSound);
        }
        if (pickup && collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerMovement>().currentHealthbar.Heal(containedRemainingHealth);
            this.gameObject.SetActive(false);
        }
    }
    AudioSource foley;
    [SerializeField] AudioClip hitGroundSound;

    public void Decouple()
    {
        GetComponent<MeshCollider>().isTrigger = false;
        rb.isKinematic = false;
        rb.useGravity = true;
        down = true;
        cube.SetActive(false);
        rb.AddTorque(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)) * 15);
        //remove reference to current owner
        currentOwner = null;
        // if (OnDropped != null)
        //OnDropped(this.gameObject);
        pickup = true;
        if (m_MyEvent != null)
            m_MyEvent.Invoke(this.gameObject);
    }

    public void MoveTowardsPlayer()
    {
        if (pickup)
        {
            playerInRange = Physics.CheckSphere(transform.position, playerRange, whatIsPlayer);
            if (playerInRange)
            {
                Vector3 dir = (PlayerMovement.instance.gameObject.transform.position - transform.position).normalized;
                rb.AddForce(dir * 50, ForceMode.Force);
            }
        }
    }

    public IEnumerator EFlash(Material _material)
    {
        skin.material = _material;
        yield return new WaitForSeconds(.5f);
        skin.material = originalMaterial;
    }
}
