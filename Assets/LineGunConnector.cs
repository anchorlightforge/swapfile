using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGunConnector : MonoBehaviour
{
    LineRenderer line;
    Transform[] objects;
    [SerializeField] float offsetRange = 0.04f;
    // Start is called before the first frame update
    void Start()
    {
        objects = GetComponentsInChildren<Transform>();
        line = GetComponent<LineRenderer>(); 
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var points = new Vector3[objects.Length];
        for (int i = 0; i < objects.Length; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-offsetRange, offsetRange), Random.Range(-offsetRange, offsetRange), Random.Range(-offsetRange, offsetRange));
            points[i] = objects[i].localPosition + offset;
        }
        line.SetPositions(points);
    }
}
