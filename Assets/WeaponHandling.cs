using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponHandling : MonoBehaviour
{
    Transform camDir;
    [SerializeField] LayerMask enemyMask;
    // Start is called before the first frame update
    void Start()
    {
        camDir = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && CanFire())
        {
            Fire();
        }


    }

    [System.Serializable]
    public struct Weapon
    {
        public string weaponName;
        public int weaponID;
        public bool unlocked;
        public int healthCost;
        public int damage;
        public float fireRate;
        public float range;
        public int flechettes;
        public float spread;
    }

    Weapon[] weapons;
    int currentWeapon;
    [SerializeField] ParticleSystem gunFX;
    float timeToNextFire;
    [SerializeField] float fireRate;


    public void SwitchWeapon (int newWeapon)
    {

    }

    public void Fire()

    {
        timeToNextFire = weapons[currentWeapon].fireRate;
        gunFX.Play();
        RaycastHit gunCheck;
        if (Physics.Raycast(transform.position, camDir.forward, out gunCheck, weapons[currentWeapon].range, enemyMask))
        {
            if (gunCheck.transform.TryGetComponent(out HealthStats enemy))
                enemy.TakeDamage(weapons[currentWeapon].damage);
            //deal damage if enemy found
        }
    }

    bool CanFire()
    {
        //check stamina
        //check if a healthbar with ammo is held
        //check timetonextfire
        //if,return false;
        if (timeToNextFire > 0)
            return false;
         else return true;

    }
}
