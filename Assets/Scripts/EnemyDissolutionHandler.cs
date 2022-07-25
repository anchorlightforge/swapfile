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
    [SerializeField] bool multiRend;
    MeshRenderer[] _rends;

    public delegate void AnnounceDeath();
    public static event AnnounceDeath OnDeathAnnounced;
    void Start()
    {
        if(!multiRend)
        _rend = GetComponent<MeshRenderer>();
        else
        {
            _rends = GetComponentsInChildren<MeshRenderer>();
        }
        dissolve = minDissolve;
        //todo: accept skinned mesh renderer from model
        
    }

    public void Dissolve(Healthbar currentBar,bool destroyHealth)
    {
        StartCoroutine(Dissolution(currentBar,destroyHealth));
        StartCoroutine(Dissolution(currentBar,destroyHealth));
    }

    IEnumerator Dissolution(Healthbar currentBar, bool destroyHealhBar)
    {
        while(dissolve<maxDissolve)
        {
            dissolve += (Time.deltaTime * dissolveSpeed);
            yield return new WaitForSeconds(Mathf.Clamp(Time.deltaTime,0.01f,1));
            if(!multiRend) _rend.material.SetFloat("_Dissolution", dissolve);
            else
            {
                foreach (MeshRenderer rend in _rends)
                { rend.material.SetFloat("_Dissolution", dissolve); }
            }
        }
        currentBar.transform.parent = null;
        currentBar.Decouple();
        if(OnDeathAnnounced != null)
            OnDeathAnnounced();
        Destroy(this.gameObject);
        if(destroyHealhBar)
            Destroy(currentBar.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
