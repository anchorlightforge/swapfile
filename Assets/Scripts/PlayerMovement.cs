using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IHealth
{
    [SerializeField] Transform playerEyes;
    public static PlayerMovement instance;
    public Healthbar currentHealthbar;
    CharacterController charControl;
   public float healthRange;
   public LayerMask whatIsHealth;
    Transform camDir;
    [SerializeField] float fireRate;
    float timeToNextFire;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        var spawn = GameObject.FindGameObjectWithTag("SpawnPoint");
        if (spawn != null) transform.position = spawn.transform. position;
        Unpause();
        charControl = GetComponent<CharacterController>();
        camDir = Camera.main.transform;
    }

    public void MouseUnlock()
    {
        inputEnabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
    public void Pause()
    {
        inputEnabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
        FindObjectOfType<UIManager>().PauseMenu();
    }

    public void Unpause()
    {
        inputEnabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }


    [SerializeField] float gunRange = 200;

    float stamina; float maxStamina;

    /* bool CanFire()
     {
         //check stamina
         if (gunStaminaCost > stamina)
             return false;
         //check if a healthbar with ammo is held
         //check timetonextfire
         //if,return false;
         if (timeToNextFire > 0)
             return false;

         return true;
     }*/
    [SerializeField] float gunStaminaCost = 7f;
    [SerializeField] ParticleSystem gunFX;
    public void Fire()

    {
        timeToNextFire = fireRate;
        gunFX.Play();
        RaycastHit gunCheck;
        if (Physics.Raycast(transform.position, camDir.forward, out gunCheck, gunRange, enemyMask))
        {
            if (gunCheck.transform.TryGetComponent(out HealthStats enemy))
                enemy.TakeDamage(gunDamage);
            //deal damage if enemy found
        }
    }
    int gunDamage = 9;
    bool GroundCheck()
    {
        RaycastHit groundCheck;
        if (Physics.Raycast(transform.position, -transform.up, out groundCheck, groundCheckRange))
        {
            Debug.DrawRay(transform.position, -transform.up, Color.green, groundCheckRange);
            return true;
        }
        Debug.DrawRay(transform.position, -transform.up, Color.red, groundCheckRange);
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
        if(GroundCheck())
        {

        }
        else
        {
            downMomentum += Mathf.RoundToInt(fallForceRate * Time.deltaTime);
            charControl.Move(downMomentum * Vector3.down * Time.deltaTime);
            // charControl.MovePosition(Vector3.down*(9.8f*9.8f));

        }
        float moveHor = 0f, moveVert = 0f, lookHor = 0f, lookVert = 0f, joyLookHor = 0f, joyLookVert = 0f;
        if (inputEnabled)
        {

            moveHor = Input.GetAxisRaw("Horizontal");

            moveVert = Input.GetAxisRaw("Vertical");

            if (!joystickEnabled)
            {
                lookHor = Input.GetAxisRaw("Mouse X");
                lookVert = Input.GetAxisRaw("Mouse Y");
            }

            else
            {
                lookHor = Input.GetAxisRaw("Joy RX");
                lookVert = Input.GetAxisRaw("Joy RY");
            }
            /*          if(Input.GetButton("Fire1")&&CanFire())
                      {
                          Fire();
                      }
          */
            /*if (Input.GetButtonDown("Swap"))
            {
                //get 
                Debug.Log("Button Works");
                healthInAttackRange = Physics.CheckSphere(transform.position, healthRange, whatIsHealth);
                if (healthInAttackRange)
                {
                    Collider[] healthbars = Physics.OverlapSphere(transform.position, healthRange, whatIsHealth);
                    foreach (Collider col in healthbars)
                    {
                        Debug.Log(col.name);
                        Healthbar healthy = col.gameObject.GetComponent<Healthbar>();
                        if (healthy == null)
                            Debug.Log("Health Not THere");
                        if (healthy != null)
                        {
                            if (healthy.isPickupable)
                            {
                                currentHealthbar.Heal(healthy.containedRemainingHealth);
                                Destroy(healthy);
                            }
                        }
                    }
                }
                RaycastHit swapCheck;
                Physics.Raycast(camDir.position, camDir.forward, out swapCheck, swapRange);
                if (swapCheck.transform.TryGetComponent(out HealthStats targetHealth))
                {
                    Healthbar currentHealth = GetComponent<HealthStats>().CurrentHealthbar;
                    FindObjectOfType<SwapManager>().Swap(currentHealth, targetHealth.CurrentHealthbar);
                }
            }*/
        }

        

        stamina = Mathf.Clamp(stamina + Time.deltaTime, 0, maxStamina);
        Vector3 moveDir = new Vector3(moveHor, 0, moveVert);
        moveDir = moveDir.x * camDir.right.normalized + moveDir.z * transform.forward.normalized;

        Vector3 moveVel = (moveDir * moveSpeed);
        // Vector3 moveDir = (new Vector3(moveHor,0f,moveVert).normalized + transform.forward);
        charControl.Move(moveDir * moveSpeed * Time.deltaTime);


        transform.Rotate(Vector3.up, Time.deltaTime * lookSpeed * (lookHor));
        if (Mathf.Abs((lookVert) + playerEyes.transform.eulerAngles.x) == yAxisClamp)
        { joyLookVert = 0; lookVert = 0; }
        Vector3 rotEyesEuler =
            new Vector3(
                ((lookVert) * Time.deltaTime * -lookSpeed) + playerEyes.transform.eulerAngles.x,
                transform.eulerAngles.y, transform.eulerAngles.z);

        playerEyes.transform.rotation = Quaternion.Euler(rotEyesEuler);
    /*    Debug.Log(joyLookHor + " " + joyLookVert);
*/

    }

    public void Death()
    {
        inputEnabled = false;
        var obj = transform.GetChild(0);
        MouseUnlock();
        obj.GetComponent<BoxCollider>().enabled = true;
        obj.GetComponent<Rigidbody>().isKinematic = false;
        charControl.enabled = false;
        FindObjectOfType<UIManager>().DeathScreen();
        FindObjectOfType<GunEffects>().SwitchWeapon(4);
        FindObjectOfType<WeaponHandling>().EnableInput(false);
    }

    public void TakeDamage(int damage)
    {
        currentHealthbar.Hurt(damage);
    }

    public void GrabHealthBar()
    {

    }

    public void SetSensitivity (float newSens)
    {
        lookSpeed = newSens;
    }

    [SerializeField] float moveSpeed = 10;
    [SerializeField] LayerMask environmentMask, enemyMask;
    [SerializeField] float groundCheckRange = 1;
    [Header("Raycasting stats")]
    [SerializeField] float swapRange = 100;
    [Header("Look/turn controls")]
    bool inputEnabled = true;
    [SerializeField] bool joystickEnabled = false;
    [SerializeField] float lookSpeed = 100;
    [SerializeField] float yAxisClamp = 45;
    [SerializeField] float fallForceRate, downMomentum, maxSpeed;
    private bool healthInAttackRange;
}
