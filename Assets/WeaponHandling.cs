using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Music;

public class WeaponHandling : MonoBehaviour
{
    [SerializeField] GunEffects gunModels;
    Transform camDir;


    MusicManager musicMan;
    [SerializeField] LayerMask enemyMask;
    // Start is called before the first frame update
    void Start()
    {
        musicMan = FindObjectOfType<MusicManager>();
        camDir = Camera.main.transform;
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Healthbar>();
        UIManager.Instance.SetAmmo(ammo);
    }
    Healthbar playerHealth;




    // Update is called once per frame
    void Update()
    {

        if (timeToNextFire > 0) timeToNextFire -= Time.deltaTime;

        if (inputEnabled)
        {
            if (Input.GetButton("Fire1") && CanFire())
            {
                Fire();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                if (currentWeapon != 0)
                    SwitchWeapon(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                if (currentWeapon != 1)
                    SwitchWeapon(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                if (currentWeapon != 2)
                    SwitchWeapon(2);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                if (currentWeapon != 3)
                    SwitchWeapon(3);
            }
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
        public float knockBack;
        public float range;
        public int flechettes;
        public float spread;
    }

    [SerializeField] Weapon[] weapons;
    int currentWeapon;
    int ammo = 50;
    [SerializeField] ParticleSystem gunFX;
    float timeToNextFire;
    [SerializeField] float fireRate;

    public void AddAmmo(int ammoAdded)
    {
        ammo += ammoAdded;
        UIManager.Instance.SetAmmo(ammo);
    }

    public void SwitchWeapon (int newWeapon)
    {
        if (weapons[newWeapon].unlocked && newWeapon!=currentWeapon)
   
        {

            gunModels.SwitchWeapon(newWeapon);
            currentWeapon = newWeapon;
        }
    }

    Vector3 CalculateSpread(float spread)
    {
        return new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread), 0);
    }
    [SerializeField] GameObject hitDecal;
    
    
    bool inputEnabled = true;
    public void EnableInput(bool enable)
    { inputEnabled = enable; }
    public void Fire()

    {
        musicMan.ShootGun();
        timeToNextFire = weapons[currentWeapon].fireRate;
        gunFX.Play();
        RaycastHit gunCheck;
        Vector3 offset = CalculateSpread(weapons[currentWeapon].spread);
        for (int i = 0; i < weapons[currentWeapon].flechettes; i++)
        {
            if (Physics.Raycast(camDir.position, camDir.forward+offset, out gunCheck, weapons[currentWeapon].range, enemyMask))
            {
                var gunHit = Instantiate(hitDecal, gunCheck.point,Quaternion.LookRotation(gunCheck.normal));
                Destroy(gunHit, 5f);

                var hitObject =gunCheck.collider.GetComponent<IHealth>();
                if (hitObject != null)
                    hitObject.TakeDamage(weapons[currentWeapon].damage);
                /*if (gunCheck.transform.TryGetComponent(out IHealth enemy))
                {
                    enemy.TakeDamage(weapons[currentWeapon].damage);
                }*/

                    /*else if(gunCheck.transform.TryGetComponent(out Healthbar hitHealth))
                    {
                        hitHealth.hbHealth -= weapons[currentWeapon].damage;
                        if (hitHealth.hbHealth < 0)
                            hitHealth.HealthBarBreak();
                    }*/
                    //deal damage if enemy found
            }
            Debug.Log(gunCheck.point);
        Debug.DrawLine(camDir.position, camDir.position + (camDir.forward+ offset)*weapons[currentWeapon].range, Color.yellow, .5f);
        }
        gunModels.Shoot(weapons[currentWeapon].knockBack,weapons[currentWeapon].fireRate);
        ammo -= weapons[currentWeapon].healthCost;
        if(ammo<0)
        {
            playerHealth.Hurt(Mathf.Abs(ammo/2));
            ammo = 0;
        }
        UIManager.Instance.SetAmmo(ammo);
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
