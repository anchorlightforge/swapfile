using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeSettings : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
