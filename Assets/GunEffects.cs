using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEffects : MonoBehaviour
{
    Transform camDir;
    [SerializeField] AnimatedPiece[] fistPieces;
    [SerializeField] AnimatedPiece[] shotgunPieces;
    [SerializeField] AnimatedPiece shotgunPump;
    [SerializeField] AnimatedPiece[] machineGunPieces;
    [SerializeField] AnimatedPiece[] pistolPieces;
    [SerializeField] Transform hidePoint;
    // Start is called before the first frame update
    Vector3 defaultPos;
    void Start()
    {
        camDir = Camera.main.transform;
        foley = GetComponent<AudioSource>();
        defaultPos = transform.localPosition;
        SwitchWeapon(0);
        foreach (AnimatedPiece piece in fistPieces)
        {
            //set offset
        }
    }
    int currentWeapon;
    AudioSource foley;
    [SerializeField] LineGunConnector[] gunLines;
    [System.Serializable] struct WeaponSound
    {
        string name;
        public AudioClip[] shootSound;
    public AudioSource weaponFoley;
    }
    [SerializeField] WeaponSound[] gunSounds;
    [SerializeField] AudioClip switchWeaponSound;
    [SerializeField] float pitchShift = .25f;

    public void SwitchWeapon(int weapon)
    {
        currentWeapon = weapon;
        foley.pitch = 1+ Random.Range(0,pitchShift);
        foreach(LineGunConnector gunLine in gunLines)
        {
            Debug.Log("Current Weapon: " + currentWeapon);
            gunLine.ToggleVisuals(gunLine == gunLines[currentWeapon]);
        }
        foley.PlayOneShot(switchWeaponSound);
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
            case 4:
                foreach (AnimatedPiece fistPiece in fistPieces) fistPiece.Hide(hidePoint.localPosition);
                foreach (AnimatedPiece shotgunPiece in shotgunPieces) shotgunPiece.Hide(hidePoint.localPosition);
                shotgunPump.Hide(hidePoint.localPosition);
                foreach (AnimatedPiece mgPiece in machineGunPieces) mgPiece.Hide(hidePoint.localPosition);
                foreach (AnimatedPiece pistolPiece in pistolPieces) pistolPiece.Hide(hidePoint.localPosition);
                break;



        }
    }
    [SerializeField] float moveRate = 1.2f;
    [SerializeField] float swayAmp = 1;
    [SerializeField] float swayFreq = 1;
    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition,defaultPos + (knockBack*Vector3.back) + Vector3.up*(swayAmp*Mathf.Sin(Time.time * swayFreq)),moveRate*Time.deltaTime);
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }
    }

    public void Shoot(float knockback, float fireRate)
    {
        PlayAudioRandom(gunSounds[currentWeapon].weaponFoley, gunSounds[currentWeapon].shootSound);
        StartCoroutine(Knockback(knockback, fireRate));
    }
    float knockBack = 0;

    void PlayAudioRandom(AudioSource source, AudioClip[] sounds)
    {
        int rng = Random.Range(0, sounds.Length - 1);
        source.pitch = 1 + Random.Range(-.02f, .02f);
        source.PlayOneShot(sounds[rng]);
    }


    IEnumerator Knockback(float knockback,float fireRate)
    {

        knockBack = knockback; 
        yield return new WaitForSeconds(fireRate / 8);

        knockBack = 0;
        yield return new WaitForSeconds(fireRate / 2);


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
