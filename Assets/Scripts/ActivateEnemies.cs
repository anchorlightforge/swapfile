using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEnemies : MonoBehaviour
{
public List<Enemy> enemies = new List<Enemy>();

    private void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("Player"))
            AcitvateEnemi();
    }

    void AcitvateEnemi()
    {
        Debug.Log(enemies.Count);
        if (enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].player = PlayerMovement.instance.transform;
                enemies[i].currentMode = EnemyModes.Active;
            }
        }
    }
}
