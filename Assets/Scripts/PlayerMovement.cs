using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController charControl;
    Transform camDir;
    // Start is called before the first frame update
    void Start()
    {
        camDir = Camera.main.transform;
    }
    [SerializeField] float gunRange = 200;
    public void Fire()
    {
        RaycastHit gunCheck;
        if(Physics.Raycast(transform.position,camDir.forward,out gunCheck,gunRange,enemyMask))
        {
            //deal damage if enemy found
        }
    }
[SerializeField] LayerMask environmentMask,enemyMask;
[SerializeField] float groundCheckRange = 1;
    bool GroundCheck()
    {
        RaycastHit groundCheck;
        if(Physics.Raycast(transform.position,-transform.up,out groundCheck,groundCheckRange))
        {
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if(GroundCheck())
        {
            
        }
        else 
        {
            charControl.Move(Vector3.down*(9.8f*9.8f));

        }
        float moveHor = Input.GetAxisRaw("Horizontal");
        
        float moveVert = Input.GetAxisRaw("Vertical");
        
        float lookHor = Input.GetAxisRaw("Mouse X");
        float lookVert = Input.GetAxisRaw("Mouse Y");

        float joyLookHor = Input.GetAxisRaw("Joy RX");
        float joyLookVert = Input.GetAxisRaw("Joy RY");
        
        if(Input.GetButtonDown("Fire1"))
        {
            Fire();
        }


        if(Input.GetButtonDown("Swap"))
        {

        }
    }
}
