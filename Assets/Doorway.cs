using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doorway : MonoBehaviour
{
    [SerializeField] BoxCollider doorCol;
    [SerializeField] Transform doorModel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Open()
    {
        doorCol.enabled = false;
    }

    void Close()
    {
        doorCol.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider c)
    {
        if(c.CompareTag("Player"))
            Open();

    }
    void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("Player"))
            Close();

    }
}
