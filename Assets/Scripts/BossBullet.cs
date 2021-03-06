using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    //Assignables
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask whatIsEnemies;

    // Audio
    [SerializeField] AudioClip _explosionSound;

    //damage particles
    [SerializeField] ParticleSystem _impactParticles;
    [SerializeField] AudioClip _impactSound;

    //Stats
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;
    public bool exploding;

    //Damage
    public int explosionDamage;
    public float explosionRange;

    //Lifetime
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physics_mat;

    private void Start()
    {
        exploding = false;
    }

    private void Update()
    {
        //When to explode
        if (collisions > maxCollisions && !exploding)
        {
            exploding = true;
            Explode();
        }

        //count down lifetime
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0 && !exploding)
        {
            Explode();
            exploding = true;
        }
    }

    private void Explode()
    {
        //Instantiate explosion
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        //Play Audio
        if (_explosionSound != null)
        {
            AudioHelper.PlayClip2D(_explosionSound, 1f);
        }

        //Check for enemies
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, whatIsEnemies);
        for (int i = 0; i < enemies.Length; i++)
        {
            //Get component of enemy and call Take Damage

            //enemies[i].GetComponent<Boss>().TakeDamage(explosionDamage);
        }

        //Add a little delay, just to make sure everything works fine
        Invoke("Delay", 0.05f);
    }

    private void Delay()
    {
        Debug.Log("explode");
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Don't count collisions with other bullets
        if (collision.collider.CompareTag("Bullet")) return;

        //Count up collisions
        collisions++;

        //Explode if bullet hits the player directly and explodeOnTouch is activated
        if (collision.collider.CompareTag("Player") && explodeOnTouch && !exploding)
        {
            Explode();
            exploding = true;
            Health health = collision.gameObject.GetComponent<Health>();
            health.TakeDamage(1);
            Instantiate(_impactParticles, collision.gameObject.transform.position, Quaternion.identity);
            AudioHelper.PlayClip2D(_impactSound, 1f);

            Debug.Log("Hit");
        }

        if (collision.collider.CompareTag("Wall"))
        {
            Debug.Log("Hit wall");
            Destroy(gameObject);
        }
    }
    private void Setup()
    {
        //Create a new Physic Material
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;
        //Assign material to collider
        GetComponent<SphereCollider>().material = physics_mat;

        //Set gravity
        rb.useGravity = useGravity;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);
    }
}
