using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideDebugMeshes : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (MeshRenderer rend in GetComponentsInChildren<MeshRenderer>())
            rend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
