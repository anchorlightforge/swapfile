using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineGunConnector : MonoBehaviour
{
    LineRenderer line;
    Transform[] objects;
    [SerializeField] float offsetRange = 0.04f;
    // Start is called before the first frame update
    
    void Awake()
    {
        foreach (Transform obj in GetComponentsInChildren<Transform>())
            if (!obj.gameObject.activeSelf) Destroy(obj.gameObject);
        objects = GetComponentsInChildren<Transform>();
        line = GetComponent<LineRenderer>();
        line.positionCount= objects.Length;

    }
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(line.enabled)
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

    public void ToggleVisuals(bool enabled)
    {
        line.enabled = enabled;
    }
}
