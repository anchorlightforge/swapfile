using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIPanelMask : MonoBehaviour
{
  [SerializeField] private bool hiddenByDefault;
  [SerializeField] private float maskRate;
    // Start is called before the first frame update
    void Start()
    {
          // if(hiddenByDefault)
          // {
          //   this.transform.localScale = new Vector3(0,1f,1f);
          // }
          // else{this.transform.localScale = new Vector3(1f,1f,1f);}
    }

    public void OnEnable()
    {
      StartCoroutine(MaskShow());
    }
    public void Hide()
    {
      StartCoroutine(MaskHide());
    }

    // Update is called once per frame
    IEnumerator MaskShow()
    {

      for(var i = 0f; i < 1f; i+=maskRate)
      {
        yield return new WaitForSecondsRealtime(0.016f);
        this.transform.localScale=new Vector3(transform.localScale.x+maskRate,1f,1f);
      }
        this.transform.localScale = new Vector3(1f,1f,1f);
        yield return new WaitForSecondsRealtime(0.072f);
    }

    IEnumerator MaskHide()
    {
      for(var i = 1f; i > 0f; i-=maskRate)
      {
        yield return new WaitForSecondsRealtime(0.016f);
        this.transform.localScale=new Vector3(transform.localScale.x-maskRate,1f,1f);
      }
      this.transform.localScale = new Vector3(0,1f,1f);
      gameObject.SetActive(false);
    }
}
