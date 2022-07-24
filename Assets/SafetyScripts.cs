using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafetyScripts : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        var gameMan = FindObjectOfType<GameManager>();
        if (gameMan == null) this.gameObject.AddComponent(typeof(GameManager));
        var enemyMan = FindObjectOfType<EnemyManager>();
        if (enemyMan == null) this.gameObject.AddComponent(typeof(EnemyManager));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
