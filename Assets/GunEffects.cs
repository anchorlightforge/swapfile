using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEffects : MonoBehaviour
{
    [SerializeField] AnimatedPiece[] fistPieces;
    [SerializeField] AnimatedPiece[] shotgunPieces;
    [SerializeField] AnimatedPiece shotgunPump;
    [SerializeField] AnimatedPiece[] machineGunPieces;
    [SerializeField] AnimatedPiece[] pistolPieces;
    [SerializeField] Transform hidePoint;
    // Start is called before the first frame update
    void Start()
    {
        SwitchWeapon(0);
        foreach (AnimatedPiece piece in fistPieces)
        {
            //set offset
        }
    }

    public void SwitchWeapon(int weapon)
    {
        switch(weapon)
        {
            case 0:
                foreach (AnimatedPiece fistPiece in fistPieces) fistPiece.Activate();
                foreach (AnimatedPiece shotgunPiece in shotgunPieces) shotgunPiece.Hide(hidePoint.localPosition);
                shotgunPump.Hide(hidePoint.localPosition);
                foreach (AnimatedPiece mgPiece in machineGunPieces) mgPiece.Hide(hidePoint.localPosition);
                foreach (AnimatedPiece pistolPiece in pistolPieces) pistolPiece.Hide(hidePoint.localPosition);
                break;
            case 1:
                foreach (AnimatedPiece fistPiece in fistPieces) fistPiece.Hide(hidePoint.localPosition);
                foreach (AnimatedPiece shotgunPiece in shotgunPieces) shotgunPiece.Hide(hidePoint.localPosition);
                shotgunPump.Hide(hidePoint.localPosition);
                foreach (AnimatedPiece mgPiece in machineGunPieces) mgPiece.Hide(hidePoint.localPosition);
                foreach (AnimatedPiece pistolPiece in pistolPieces) pistolPiece.Activate();
             break;
            case 2:
                foreach (AnimatedPiece fistPiece in fistPieces) fistPiece.Hide(hidePoint.localPosition);
                foreach (AnimatedPiece shotgunPiece in shotgunPieces) shotgunPiece.Hide(hidePoint.localPosition);
                shotgunPump.Hide(hidePoint.localPosition);
                foreach (AnimatedPiece mgPiece in machineGunPieces) mgPiece.Activate();
                foreach (AnimatedPiece pistolPiece in pistolPieces) pistolPiece.Hide(hidePoint.localPosition);
                break;
            case 3:
                foreach (AnimatedPiece fistPiece in fistPieces) fistPiece.Hide(hidePoint.localPosition);
                foreach (AnimatedPiece shotgunPiece in shotgunPieces) shotgunPiece.Activate();
                shotgunPump.Activate();
                foreach (AnimatedPiece mgPiece in machineGunPieces) mgPiece.Hide(hidePoint.localPosition);
                foreach (AnimatedPiece pistolPiece in pistolPieces) pistolPiece.Hide(hidePoint.localPosition);
                break;


        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }
    }

    void ShotgunFire()
    {
        StartCoroutine(Pump());
    }
    IEnumerator Pump()
    {
        yield return new WaitForSeconds(.2f);
        shotgunPump.Hide(shotgunPump.transform.localPosition - (.2f*Vector3.forward));
        yield return new WaitForSeconds(.2f);
        shotgunPump.Activate();
    }
}
