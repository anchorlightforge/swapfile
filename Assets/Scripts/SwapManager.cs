using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Swap(Healthbar playerHealth, Healthbar oppositeHealth)
    {
        playerHealth.Owner.GetComponent<HealthStats>().AssignHB(oppositeHealth);
        oppositeHealth.Owner.GetComponent<HealthStats>().AssignHB(playerHealth);
    }
    public void FeedMachine(Healthbar playerHealth)
    {

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
