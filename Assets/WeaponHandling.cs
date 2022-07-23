using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponHandling : MonoBehaviour
{
    [SerializeField] GunEffects gunModels;
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

        if (timeToNextFire > 0) timeToNextFire -= Time.deltaTime;

        if (Input.GetButton("Fire1") && CanFire())
        {
            Fire();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SwitchWeapon(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SwitchWeapon(3);
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

    [SerializeField] Weapon[] weapons;
    int currentWeapon;
    [SerializeField] ParticleSystem gunFX;
    float timeToNextFire;
    [SerializeField] float fireRate;


    public void SwitchWeapon (int newWeapon)
    {
        if (weapons[newWeapon].unlocked)
        {

            gunModels.SwitchWeapon(newWeapon);
            currentWeapon = newWeapon;
        }
    }

    Vector3 CalculateSpread(float spread)
    {
        return new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 0);
    }

    public void Fire()

    {
        timeToNextFire = weapons[currentWeapon].fireRate;
        gunFX.Play();
        RaycastHit gunCheck;
        Vector3 offset = CalculateSpread(weapons[currentWeapon].spread);
        for (int i = 0; i < weapons[currentWeapon].flechettes; i++)
        {
            if (Physics.Raycast(camDir.position, camDir.forward+offset, out gunCheck, weapons[currentWeapon].range, enemyMask))
            {
                if (gunCheck.transform.TryGetComponent(out Enemy enemy))
                    enemy.TakeDamage(weapons[currentWeapon].damage);
                //deal damage if enemy found
            }
        }
        Debug.DrawRay(camDir.position, camDir.forward + offset, Color.yellow, .5f);
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
