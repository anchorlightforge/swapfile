using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    [SerializeField] GameObject[] menuItems;
    public void SwitchMenu (int menutoggle)
    {
        foreach (GameObject menuItem in menuItems)
        {
            if(menuItem!=menuItems[menutoggle])
                menuItem.SetActive(false);
        }
        menuItems[menutoggle].SetActive(true);
    }
    public void Quit()
    {
        FindObjectOfType<GameManager>().Quit();
    }
    public void StartGame()
    {
        FindObjectOfType<GameManager>().StartNewGame();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
