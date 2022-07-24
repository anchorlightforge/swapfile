using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedPiece : MonoBehaviour
{
    Transform target;
    Vector3 offset;
    // Start is called before the first frame update
    [SerializeField] bool frame = false;
    [SerializeField] float noUseRange = 4;
    [SerializeField] float trackingNoise = .1f;
    float nextNoise;
    Quaternion startRot;

    public void ChangeNoUseRange(float newRange)
    {
        noUseRange = newRange;
    }
    void Awake()
    {
        startRot = transform.localRotation;
        if(!frame)
        target = new GameObject(gameObject.name + " Refpoint").transform;
        offset = transform.localPosition;

    }
    void Start()
    {
    }

    public void Activate()
    {
        if(target)
        {

        target.localPosition = offset;
        target.localRotation = startRot;
        }
        else startRot = transform.localRotation;
        nextNoise = Random.Range(-trackingNoise, trackingNoise);
    }

    public void Hide(Vector3 newHidePoint, bool random = true)
    {
        if(target&&random)
        {
            float dir = Random.Range(0.0f, Mathf.PI * 2);
            Vector3 V = new Vector3(Mathf.Sin(dir), Mathf.Cos(dir),0);
            V *= noUseRange;
            target.localPosition +=V;
            target.localRotation = Quaternion.Euler(new Vector3(Random.Range(0,360), Random.Range(0, 360),Random.Range(0, 360)));
        }
        if(target&&!random)
        {
            target.localPosition = newHidePoint;

        }
    }

    [SerializeField] float turnSpeed = .6f;
    [SerializeField] float noiseScaleMult = 0.01f;
    Vector3 CalculateNoise(float noiseScale)
    {
        return new Vector3(Random.Range(-noiseScale, noiseScale), Random.Range(-noiseScale, noiseScale), Random.Range(-noiseScale, noiseScale));
    }
    [SerializeField] float moveScale = 1.4f;
    [SerializeField] float moveFreq = .24f;
    // Update is called once per frame
    [SerializeField] float trackSpeed = 0.23f;
    float noiseScale = .01f;
    float knockBack = 0;
    void LateUpdate()
    {
        Vector3 noiseOffset = CalculateNoise(noiseScaleMult);
        if(target)
        {
            Debug.DrawLine(target.localPosition, transform.localPosition,Color.blue, .5f);
            /*float noiseFunc = moveScale*Mathf.Cos(Time.time*moveFreq*noiseOffset);*/
            Vector3 noiseFunc = noiseScaleMult * new Vector3(Mathf.Sin(noiseOffset.x*.01f*moveFreq),Mathf.Sin(noiseOffset.y * moveFreq),Mathf.Sin(Random.Range(-noiseScale * .01f, noiseScale * .01f)* moveFreq));
            transform.localPosition = Vector3.Lerp(transform.localPosition, target.localPosition+ noiseFunc + knockBack * -transform.forward, (trackSpeed+nextNoise)*Time.deltaTime);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, target.localRotation, turnSpeed*Time.deltaTime) ;
        }
        //Todo: noise
    }
}
