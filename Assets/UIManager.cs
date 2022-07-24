using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    //Display text for ammo
    [SerializeField] MeshRenderer healthRend, ammoRend;
    [SerializeField] TextMeshProUGUI pistolAmmo, mgAmmo, sgAmmo;
    // Start is called before the first frame update
    float minHealthVal = -6, maxHealthVal = 8;
    void Start()
    {
        SetAmmo(1);
        SetHealth(1);
    }

    // Update is called once per frame
    void Update()
    {

    }
    int health;
    int ammo;
    [System.Serializable]
    public struct SceneInfo
    {
        public string SceneName;
    }
    [SerializeField] SceneInfo[] rooms;
    public void SetHealth(float healthPercent, float ammoPercent = 0)
    {
        float healthPer = Mathf.Lerp(minHealthVal, maxHealthVal, healthPercent);
        healthRend.material.SetFloat("_ProgressBorder", healthPer);

    }
    
     public void SetAmmo(float newAmmo)
    {

        float ammoVal = ((float)ammo) / 100f;
        float ammoPer = Mathf.Lerp(minHealthVal, maxHealthVal, ammoVal);
        ammoRend.material.SetFloat("_ProgressBorder", ammoPer);
        ammo = (int)newAmmo;
        if(ammo > 0)
        {
            pistolAmmo.SetText((ammo / 1).ToString());
            mgAmmo.SetText((ammo/2).ToString());
            sgAmmo.SetText((ammo / 5).ToString());
        }
        else
        {
            pistolAmmo.SetText((health/ 1).ToString()+" HP");
            mgAmmo.SetText((health / 2).ToString()+ " HP") ;
            sgAmmo.SetText((health / 5).ToString()+" HP");
        }
    }

    public void PauseMenu()
    {
        menuPanels[0].SetActive(true);
        SwitchMenu(0);
    }

    public void Unpause()
    {
        foreach  (GameObject panel in menuPanels)
        {
            panel.SetActive(false);
            FindObjectOfType<PlayerMovement>().Unpause();
        }
    }
    public void QuitToMenu()
    {
        FindObjectOfType<GameManager>().LoadStage(1);

    }

    public void QuitToOS()
    {
        FindObjectOfType<GameManager>().Quit();

    }
    public void RestartStage()
    {
        FindObjectOfType<GameManager>().RestartStage();
    }

    public void DeathScreen()
    {
        menuPanels[2].SetActive(true);
    }

    public void SwitchMenu(int menuID) 
    {
        foreach (GameObject panel in menuPanels)

        {
            if (panel != menuPanels[menuID])
                panel.SetActive(false);
        }
        menuPanels[menuID].SetActive(true);
    }
    [SerializeField] GameObject[] menuPanels;



}
