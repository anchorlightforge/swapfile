using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int highestStage = 1;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        LoadGame();
    }

    void Unpause()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        Time.timeScale = 1;
    }

    void LoadStage(int nextStageToLoad)
    {
        Unpause();
        SceneManager.LoadScene(nextStageToLoad);
    }
    public void StageComplete()
    {
        highestStage++;
        int currentStage = SceneManager.GetActiveScene().buildIndex;
        if (currentStage < SceneManager.sceneCountInBuildSettings)
            LoadStage(currentStage++);
        else LoadStage(1);
    }

    public void SaveGame()
    {
        PlayerPrefs.SetInt("maxStage", highestStage);
        PlayerPrefs.Save();
    }
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("maxStage"))
        {
            highestStage = PlayerPrefs.GetInt("maxStage");
        }
        else PlayerPrefs.SetInt("maxStage", 1);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
