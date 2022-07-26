using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomBullets : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

    public bool isPlayer;
    private string PlayerOrEnemy;

    //Stats
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    //Damage
    public int explosionDamage;
    public float explosionRange;
    public float explosionForce;

    //Lifetime
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physics_mat;

    private void Start()
    {
        Setup();
        PlayerOrEnemy = isPlayer ? "Player" : "Enemy";
    }

    private void Update()
    {
        //When to explode:
        if (collisions > maxCollisions)
        {
            //Debug.Log("Collision Explosion");
            Explode();
        }

        //Count down lifetime
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0)
        {
            //Debug.Log("Timer Explosion");
            Explode();
        }
    }

    private void Explode()
    {
        //Instantiate explosion
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        //Check for enemies 
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        
        /*foreach (Collider c in enemies)
        {
            var hitObject = c.GetComponent<IHealth>();
            if(hitObject!=null)
            {
                Debug.Log("Take Damage: " + explosionDamage);
                hitObject.TakeDamage(explosionDamage);
            }
        }*/
        for (int i = 0; i < enemies.Length; i++)
        {
            //Get component of enemy and call Take Damage

            //Just an example!
            ///enemies[i].GetComponent<ShootingAi>().TakeDamage(explosionDamage);

            //Add explosion force (if enemy has a rigidbody)
            //if (enemies[i].GetComponent<Rigidbody>())
            //enemies[i].GetComponent<Rigidbody>().AddExplosionForce(explosionForce, transform.position, explosionRange);
            var hitObject = enemies[i].GetComponent<IHealth>();
            if (hitObject != null)
            {
                Debug.Log("Take Damage: " + explosionDamage);
                hitObject.TakeDamage(explosionDamage);
            }
        }

        //Add a little delay, just to make sure everything works fine
        //Invoke("Delay", 0.05f);
        Delay();
    }
    private void Delay()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Don't count collisions with other bullets
        if (collision.collider.CompareTag("Bullet")) return;

        //Count up collisions
        collisions++;

        //Explode if bullet hits an enemy directly and explodeOnTouch is activated
        if (collision.collider.CompareTag(PlayerOrEnemy) && explodeOnTouch)
        {
            Debug.Log("Explosion");
            Explode();
        }
    }

    private void Setup()
    {
        //Create a new Physic material
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;
        //Assign material to collider
        GetComponent<SphereCollider>().material = physics_mat;

        //Set gravity
        rb.useGravity = useGravity;
    }

    /// Just to visualize the explosion range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
