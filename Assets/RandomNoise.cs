using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomNoise : MonoBehaviour
{
    Camera mainCam;
    float defaultFOV;
    [SerializeField] float minFOV= 60;
    [SerializeField] float maxFOV = 70;
    [SerializeField] float noiseRange = .02f;
    Transform targetRot;
    [SerializeField] float fovRate = 2;
    float direction = 1
;    Quaternion defaultRot;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        targetRot = new GameObject("targetrotref").transform;
        defaultRot = transform.rotation;
        defaultFOV = mainCam.fieldOfView;
        StartCoroutine(RandomRotation());
    }

    IEnumerator RandomRotation()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            targetRot.rotation =  defaultRot * Quaternion.Euler(new Vector3(Random.Range(-noiseRange, noiseRange), Random.Range(-noiseRange, noiseRange), Random.Range(-noiseRange, noiseRange)));
        }
    }
    [SerializeField] float trackRate = .3f;
    // Update is called once per frame
    void LateUpdate()
    {
        if(mainCam.fieldOfView < minFOV)
        {
            direction = 1;
            mainCam.fieldOfView = minFOV;
        }
        if (mainCam.fieldOfView > maxFOV)
        {
            direction = -1;
            mainCam.fieldOfView = maxFOV;
        }

        mainCam.fieldOfView +=  (direction * fovRate * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation,targetRot.rotation,trackRate * Time.deltaTime);
    }
}
