using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class Doorway : MonoBehaviour
{
    [SerializeField] BoxCollider doorCol;
    [SerializeField] Transform doorModel;
    AudioSource foley;
    [SerializeField]AudioClip openSound, closeSound;
    Transform targetPos;
    [SerializeField] bool isLocked;
    // Start is called before the first frame update
    [SerializeField]float yOffset = 2;
    public Enemy[] enemies;
    List<Enemy> enemiesList;
   public Doorway exitDoor;
    void Start()
    {
        foley = GetComponent<AudioSource>();
        EnemyDissolutionHandler.OnDeathAnnounced += Unlock;
        enemiesList=new List<Enemy>(enemies);
/*        targetPos = new GameObject("TargetPosRef");*/
    }

    [SerializeField] float doorMoveRate = 5;
    IEnumerator OpenDoor()
    {
        while (doorModel.position.y < transform.position.y + yOffset && open)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            doorModel.position += (Vector3.up * Time.deltaTime * doorMoveRate);
        }
        doorModel.position = transform.position + yOffset*Vector3.up;
    }
    bool open = false;
    IEnumerator CloseDoor()
    {
        while (doorModel.position.y > transform.position.y && !open)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            doorModel.position -= (Vector3.up * Time.deltaTime * doorMoveRate);
        }
        doorModel.position = transform.position;
    }


    void Open()
    {
        open = true;
        doorCol.enabled = false;
        StartCoroutine(OpenDoor());   //sonud effect
    }

    void Close()
    {
        open = false;
        StartCoroutine(CloseDoor());
        /*
        targetPosRef.position = transform.position;*/
        doorCol.enabled = true;
    }

    void Lock()
    {
        if (open)
            StartCoroutine(CloseDoor());
        isLocked = true;
        if (exitDoor != null)
        {
            //exitDoor.isLocked = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }
    void OnTriggerEnter(Collider c)
    {
        if (!isLocked)
        {
            if (c.CompareTag("Player"))
                Open();
        }
    }
    void OnTriggerExit(Collider c)
    {
        if (c.CompareTag("Player"))
            Close();
        if (this.enemiesList.Count == 0) return;
        //ActivateEnemies();
    }

    void ActivateEnemies()
    {
        Debug.Log(enemiesList.Count);
        if(enemiesList.Count>0)
        {
            Debug.Log("Shouldn't happen");
            Lock();
            for (int i = 0; i < enemiesList.Count; i++)
            {
                enemiesList[i].player = PlayerMovement.instance.transform;
                enemies[i].currentMode = EnemyModes.Active;
            }
        }
    }

    void Unlock()
    {
        if (!EnemiesHere())
        {
            isLocked = false;
            if(exitDoor != null)
            exitDoor.isLocked = false;
            EnemyDissolutionHandler.OnDeathAnnounced -= Unlock;
        }
    }

    bool EnemiesHere()
    {
        foreach (Enemy enemy in enemies)
        {
            if (enemy == null)
                enemiesList.Remove(enemy);
        }
        return enemiesList.Count > 0;
    }

}
