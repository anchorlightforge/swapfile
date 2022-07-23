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
    void Start()
    {
        if(!frame)
        target = new GameObject(gameObject.name + " Refpoint").transform;
        offset = transform.localPosition;
    }

    public void Activate()
    {
        if(target)
        target.localPosition = offset;
    }

    public void Hide(Vector3 newHidePoint, bool random = true)
    {
        if(target&&random)
        {
            float dir = Random.Range(0.0f, Mathf.PI * 2);
            Vector3 V = new Vector3(Mathf.Sin(dir), Mathf.Cos(dir),0);
            V *= noUseRange;
            target.localPosition +=V;
        }
        if(target&&!random)
        {
            target.localPosition = newHidePoint;

        }
    }

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
    void LateUpdate()
    {
        Vector3 noiseOffset = CalculateNoise(noiseScaleMult);
        if(target)
        {
            Debug.DrawLine(target.localPosition, transform.localPosition,Color.blue, .5f);
            /*float noiseFunc = moveScale*Mathf.Cos(Time.time*moveFreq*noiseOffset);*/
            Vector3 noiseFunc = noiseScaleMult * new Vector3(Mathf.Sin(noiseOffset.x*.01f*moveFreq),Mathf.Sin(noiseOffset.y * moveFreq),Mathf.Sin(Random.Range(-noiseScale * .01f, noiseScale * .01f)* moveFreq));
            transform.localPosition = Vector3.Lerp(transform.localPosition, target.localPosition+noiseFunc, trackSpeed*Time.deltaTime);
        }
        //Todo: noise
    }
}
