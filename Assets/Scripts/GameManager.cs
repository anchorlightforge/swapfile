using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixing;
    int highestStage = 1;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    public void Quit()
    {
#if !UNITY_WEBGL
        Application.Quit();
#endif
    }
    public void StartNewGame()
    {
        LoadStage(2);
    }
    void Start()
    {
        LoadGame();
    }

    public void SetSFXVol(Slider sfxVolSlider)
    {
        mixing.SetFloat("sfxVol", sfxVolSlider.value);
    }
    public void SetBGMVol(Slider bgmVolSlider)
    {
        mixing.SetFloat("bgmVol", bgmVolSlider.value);
    }

    public void PauseGame()
    {
/*        FindObjectOfType<WeaponHandling>().InputEnabled(false);*/
    }

    void Unpause()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        Time.timeScale = 1;
        var player = FindObjectOfType<PlayerMovement>();
        if(player!=null) player.Unpause();
    }

    public void LoadStage(int nextStageToLoad)
    {
        Unpause();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if(nextStageToLoad<SceneManager.sceneCountInBuildSettings)
        SceneManager.LoadScene(nextStageToLoad);
    }

    public void RestartStage() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public float GetSensitivity()
    {
        return mouseSensitivity;
    }
    public void SetSensitivity (float newSens)
    {
        mouseSensitivity = newSens;
        var player = FindObjectOfType<PlayerMovement>();
        if (player!=null)
            player.SetSensitivity(mouseSensitivity);
    }
    float mouseSensitivity;
    public float MouseSensitivity => mouseSensitivity;
    public void StageComplete()
    {
        highestStage++;
        SaveGame();
        int currentStage = SceneManager.GetActiveScene().buildIndex;
        if (currentStage < SceneManager.sceneCountInBuildSettings)
            LoadStage(currentStage++);
        else LoadStage(1);
    }

    public void ContinueGame()
    {
        LoadGame();
        LoadStage(highestStage);
    }

    public bool StageCheck(int input)
    {
        if (input > highestStage)
            return true;
        else return false;
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
