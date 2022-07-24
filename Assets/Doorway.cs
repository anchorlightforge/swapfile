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
    // Start is called before the first frame update
    [SerializeField]float yOffset = 2;
    void Start()
    {
        foley = GetComponent<AudioSource>();
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

    // Update is called once per frame
    void FixedUpdate()
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
