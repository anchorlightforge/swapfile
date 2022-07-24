using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    //Display text for ammo
    MeshRenderer healthRend, ammoRend;
    TextMeshProUGUI pistolAmmo, mgAmmo, sgAmmo;
    // Start is called before the first frame update
    void Start()
    {
        SetHealth(1);
    }

    // Update is called once per frame
    void Update()
    {

    }
    int health;
    int ammo;

    public void SetHealth(float healthPercent, float ammoPercent = 0)
    {
        healthRend.material.SetFloat("ProgressBar", healthPercent);
        ammoRend.material.SetFloat("ProgressBar", ammoPercent);
        UpdateAmmo();
    }
    
     void UpdateAmmo()
    {
        if(ammo > 0)
        {
            pistolAmmo.SetText((health / 1).ToString() + (ammo / 1).ToString());
            mgAmmo.SetText((health / 2).ToString() + (ammo/2).ToString());
            sgAmmo.SetText((health / 5).ToString() + (ammo / 5).ToString());
        }
    }

    public void PauseMenu()
    {
        
    }

    public void SwitchMenu(int menuID) 
    {
        
    }



}
