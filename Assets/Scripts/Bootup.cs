using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bootup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartupMaterials();
    }

    void StartupMaterials()
    {
        StartCoroutine(WaitForInput());
    }
    IEnumerator WaitForInput()
    {
        while(!Input.anyKeyDown)
            yield return null;
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
