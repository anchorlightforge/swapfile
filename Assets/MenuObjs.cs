using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuObjs : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (AnimatedPiece piece in GetComponentsInChildren<AnimatedPiece>())
            piece.Hide(transform.position,true);
        StartCoroutine(RandomLogic());
    }


    IEnumerator RandomLogic()
    {
        while(true)
        {
            yield return new WaitForSeconds(4f);
            foreach (AnimatedPiece piece in GetComponentsInChildren<AnimatedPiece>())
            {
                piece.ChangeNoUseRange(Random.Range(2, 3));
                piece.Hide(transform.position, true);
            }
            yield return new WaitForSeconds(4f);
            foreach (AnimatedPiece piece in GetComponentsInChildren<AnimatedPiece>())
                piece.Activate();

        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
