using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedPiece : MonoBehaviour
{
    Transform target;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        target = new GameObject(gameObject.name + " Refpoint").transform;
        offset = transform.localPosition;
    }

    public void Activate()
    {
        target.localPosition = offset;
    }

    public void Hide(Vector3 newHidePoint)
    {
        target.localPosition = newHidePoint;
    }

    // Update is called once per frame
    [SerializeField] float trackSpeed = 0.23f;
    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, trackSpeed);
        //Todo: noise
    }
}
