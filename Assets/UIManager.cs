using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    //Display text for ammo
    [SerializeField] MeshRenderer healthRend, ammoRend;
    [SerializeField] TextMeshProUGUI pistolAmmo, mgAmmo, sgAmmo;
    // Start is called before the first frame update
    float minHealthVal = -6, maxHealthVal = 8;
    [SerializeField] TextMeshProUGUI missionText;
    TimeSpan gameTimer;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        StartTimer();
        SetRoomName();
        SetAmmo(50);
        SetHealth(1);
    }
    float startTime;
    void StartTimer()
    {
        startTime = Time.realtimeSinceStartup;
    }

    void SetRoomName()
    {
        int buildIndexNew = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        if (rooms.Length > buildIndexNew)
            missionText.SetText(">DIRECTIVE L0"+buildIndexNew+"<\n" + rooms[buildIndexNew].SceneName);
        else missionText.SetText(">DIRECTIVE L0"+buildIndexNew+"<\n+"+UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        int hours = (int)(Time.timeSinceLevelLoad / 3600f);
        int minutes = (int)(Time.timeSinceLevelLoad / 60f) % 60;
        int seconds = (int)(Time.timeSinceLevelLoad % 60f);
        int milliseconds = (int)(Time.timeSinceLevelLoad * 1000f) % 1000;
        timerText.text = minutes.ToString("D2") + ":" + seconds.ToString("D2") + ":" + milliseconds.ToString("D2");
    }
    [SerializeField] TextMeshProUGUI timerText;
    int health;
    int ammo;
    [System.Serializable]
    public struct SceneInfo
    {
        public string SceneName;
    }
    [SerializeField] SceneInfo[] rooms;
    public void SetHealth(float healthPercent)
    {
        float healthPer = Mathf.Lerp(minHealthVal, maxHealthVal, healthPercent);
        healthRend.material.SetFloat("_ProgressBorder", healthPer);

    }
    
     public void SetAmmo(float newAmmo)
    {

        float ammoVal = ((float)ammo) / 50f;
        //ammoVal = (ammoVal * 14) - 6;
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
            pistolAmmo.SetText((health/ 1).ToString()+" (HP)");
            mgAmmo.SetText((health / 2).ToString()+ " (HP)") ;
            sgAmmo.SetText((health / 5).ToString()+" (HP)");
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
