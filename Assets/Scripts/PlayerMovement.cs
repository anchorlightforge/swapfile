using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Transform playerEyes;
    CharacterController charControl;
    Transform camDir;
    [SerializeField] float fireRate;
    float timeToNextFire;
    // Start is called before the first frame update
    void Start()
    {
        Unpause();
        charControl = GetComponent<CharacterController>();
        camDir = Camera.main.transform;
    }

    void Pause()
    {
        inputEnabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    void Unpause()
    {
        inputEnabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1;
    }


    [SerializeField] float gunRange = 200;

    float stamina; float maxStamina;
    
    bool CanFire()
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
    }
    [SerializeField] float gunStaminaCost = 7f;
    [SerializeField] ParticleSystem gunFX;
    public void Fire()

    {
        timeToNextFire = fireRate;
        gunFX.Play();
        RaycastHit gunCheck;
        if(Physics.Raycast(transform.position,camDir.forward,out gunCheck,gunRange,enemyMask))
        {
            if (gunCheck.transform.TryGetComponent(out HealthStats enemy))
                enemy.TakeDamage(gunDamage);
            //deal damage if enemy found
        }
    }
    int gunDamage = 9; 
[SerializeField] LayerMask environmentMask,enemyMask;
[SerializeField] float groundCheckRange = 1;
    bool GroundCheck()
    {
        RaycastHit groundCheck;
        if(Physics.Raycast(transform.position,-transform.up,out groundCheck,groundCheckRange))
        {
            Debug.DrawRay(transform.position,-transform.up,Color.green,groundCheckRange);
            return true;
        }
        Debug.DrawRay(transform.position,-transform.up,Color.red,groundCheckRange);
        return false;
    }
    [SerializeField] float moveSpeed = 10;
    
    // Update is called once per frame
    void Update()
    {
        if (timeToNextFire > 0) timeToNextFire -= Time.deltaTime;

        if(GroundCheck())
        {
            
        }
        else 
        {
            downMomentum+=Mathf.RoundToInt(fallForceRate*Time.deltaTime);
            charControl.Move(downMomentum*Vector3.down*Time.deltaTime);
            // charControl.MovePosition(Vector3.down*(9.8f*9.8f));

        }
        float moveHor = Input.GetAxisRaw("Horizontal");
        
        float moveVert = Input.GetAxisRaw("Vertical");
        
        float lookHor = Input.GetAxisRaw("Mouse X");
        float lookVert = Input.GetAxisRaw("Mouse Y");

        float joyLookHor = Input.GetAxisRaw("Joy RX");
        float joyLookVert = Input.GetAxisRaw("Joy RY");
        
        if(Input.GetButton("Fire1")&&CanFire())
        {
            Fire();
        }



        float swapRange = 100;
        if(Input.GetButtonDown("Swap"))
        {
            //get 
            RaycastHit swapCheck;
            Physics.Raycast(camDir.position,camDir.forward,out swapCheck,swapRange);
            if (swapCheck.transform.TryGetComponent(out HealthStats targetHealth))
            {
                Healthbar currentHealth = GetComponent<HealthStats>().CurrentHealthbar;
                FindObjectOfType<SwapManager>().Swap(currentHealth, targetHealth.CurrentHealthbar);
            }
        }
        stamina = Mathf.Clamp(stamina + Time.deltaTime,0,maxStamina);
        Vector3 moveDir = new Vector3(moveHor, 0, moveVert);
        moveDir = moveDir.x * camDir.right.normalized + moveDir.z * camDir.forward.normalized;
        
        Vector3 moveVel = (moveDir * moveSpeed); 
        // Vector3 moveDir = (new Vector3(moveHor,0f,moveVert).normalized + transform.forward);
        charControl.Move(moveDir*moveSpeed*Time.deltaTime);
        

    transform.Rotate(Vector3.up,Time.deltaTime * lookSpeed * (lookHor));
    if(Mathf.Abs((lookVert) + playerEyes.transform.eulerAngles.x) == yAxisClamp )
    {joyLookVert = 0;lookVert=0;}
        Vector3 rotEyesEuler =
            new Vector3(
                ((lookVert) * Time.deltaTime * -lookSpeed) + playerEyes.transform.eulerAngles.x,
                transform.eulerAngles.y, transform.eulerAngles.z) ;

    playerEyes.transform.rotation = Quaternion.Euler(rotEyesEuler);
        Debug.Log(joyLookHor + " "+joyLookVert);
   

    }
    [SerializeField] float lookSpeed = 100;
    [SerializeField] float yAxisClamp = 45;
    [SerializeField] float fallForceRate,moveBaseRate,downMomentum,maxSpeed;
    bool inputEnabled=true;
}