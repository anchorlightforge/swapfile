using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class ChangeSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float baseVol;
        mixing.GetFloat("bgmVol", out baseVol);
        bgmVolSlider.value = baseVol;
        mixing.GetFloat("sfxVol",out baseVol);
        sfxVolSlider.value = baseVol;
    
    }
    [SerializeField] AudioMixer mixing;
    [SerializeField] Slider sfxVolSlider, bgmVolSlider;
    public void RenderScale(int scale)
    {
        switch(scale)
        {

            case 0:
                QualitySettings.resolutionScalingFixedDPIFactor = 0.5f;
                break;
            case 1:
                QualitySettings.resolutionScalingFixedDPIFactor = 1;
                break;
            case 2:
                QualitySettings.resolutionScalingFixedDPIFactor = 1.25f;
                break;
            case 3:
                QualitySettings.resolutionScalingFixedDPIFactor = 1.5f;
                break;
        }
    }
    public void SetSFXVol()
    {
        mixing.SetFloat("sfxVol", sfxVolSlider.value);
    }
    public void SetBGMVol()
    {
        mixing.SetFloat("bgmVol", sfxVolSlider.value);
    }
    public void QualitySet(int quality)
    {
        QualitySettings.SetQualityLevel(quality);
    }

    public void MouseSensitivity(Slider sens)
    {
        FindObjectOfType<GameManager>().SetSensitivity( sens.value);
    }
    public void Fullscreen(int fs)
    {
        if (fs == 0) Screen.fullScreen = true;
        if (fs == 1) Screen.fullScreen = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
