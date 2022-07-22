using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackToPlayer : MonoBehaviour
{
    Transform player;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Eyes").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.position;
        transform.rotation = player.rotation; 
    }
}
