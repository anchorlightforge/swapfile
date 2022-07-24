using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;   

public class VersionChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TextMeshProUGUI>().SetText("Version " + Application.version + "\nCreated for Reload / ITK Mini-Jam #1\nJuly 2022");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
