using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDissolutionHandler : MonoBehaviour
{
    MeshRenderer _rend;
    [SerializeField] float dissolve;
    [SerializeField] float maxDissolve, minDissolve;
    [SerializeField] float dissolveSpeed = 8;
    // Start is called before the first frame update
    /// <summary>
    /// Call Dissolve() to kill enemy.
    /// </summary>
    void Start()
    {
        _rend = GetComponent<MeshRenderer>();
        dissolve = minDissolve;
        //todo: accept skinned mesh renderer from model
        
    }

    public void Dissolve(Healthbar currentBar)
    {
        StartCoroutine(Dissolution(currentBar));
    }

    IEnumerator Dissolution(Healthbar currentBar)
    {
        while(dissolve<maxDissolve)
        {
            dissolve += (Time.deltaTime * dissolveSpeed);
            yield return new WaitForSeconds(Mathf.Clamp(Time.deltaTime,0.01f,1));
            _rend.material.SetFloat("_Dissolution", dissolve);
        }
        currentBar.transform.parent = null;
        currentBar.Decouple();
        Destroy(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
