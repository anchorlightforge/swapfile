using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
List<Enemy> enemyList;
    private void Start()
    {
        Enemy[] enemies = GameObject.FindObjectsOfType(typeof(Enemy))as Enemy[];
        enemyList = new List<Enemy>(enemies);
    }

    void RemoveEnemy(Enemy enemy)
    {
        foreach (Enemy enemy2 in enemyList)
        {
            if(enemy2 == enemy)
                enemyList.Remove(enemy);
        }
    }

   public bool EnemyActive()
    {
        bool isEnemyActive = false;
        foreach (Enemy enemy in enemyList)
        {
            if (enemy.currentMode == EnemyModes.Active)
                isEnemyActive = true;
        }
        return isEnemyActive;
    }
}
