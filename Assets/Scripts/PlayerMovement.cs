using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveHor = Input.GetAxisRaw("Horizontal");
        
        float moveVert = Input.GetAxisRaw("Vertical");
        
        float lookHor = Input.GetAxisRaw("Mouse X");
        float lookVert = Input.GetAxisRaw("Mouse Y");

        float joyLookHor = Input.GetAxisRaw("Joy RX");
        float joyLookVert = Input.GetAxisRaw("Joy RY");
        
        if(Input.GetButtonDown("Fire1"))
        {

        }

        if(Input.GetButtonDown("Swap"))
        {

        }
    }
}
