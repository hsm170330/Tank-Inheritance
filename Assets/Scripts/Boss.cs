using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum GameState
{
    IdleShoot,
    Charge,
    SpawnBlocks
}
[RequireComponent(typeof(Rigidbody))]
public class Boss : MonoBehaviour
{
    [SerializeField] int _damageAmount = 1;
    [SerializeField] ParticleSystem _impactParticles;
    [SerializeField] ParticleSystem _bossImpactParticles;
    [SerializeField] AudioClip _impactSound;
    bool cooldown = false;

    Rigidbody _rb;

    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    private GameState _currentGameState = GameState.IdleShoot;

    //Movement
    public Vector3 walkPoint, spawnPoint;
    bool LeftRight, X;

    bool walkPointL, walkPointR, walkPoint1, walkPoint2, walkPoint3, walkPoint4, spawn;

    //Shoot attack stuff
    public Transform attackPoint;
    public float shootforce;
    public GameObject bullet;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Tank").transform;
        spawnPoint = transform.position;
        agent = GetComponent<NavMeshAgent>();
        LeftRight = true;
        X = false;
        walkPointL = true;
        walkPointR = false;
        walkPoint1 = false;
        walkPoint2 = false;
        walkPoint3 = false;
        walkPoint4 = false;
        spawn = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Health health = other.gameObject.GetComponent<Health>();
            if (health != null && !cooldown)
            {
                PlayerImpact(health);
                ImpactFeedback(other.gameObject.transform.position, _impactParticles);
                cooldown = true;
                Invoke("CooldownReset", 0.2f);
            }
        }
        else if (other.gameObject.CompareTag("Bullet"))
        {
            Health health = gameObject.GetComponent<Health>();
            PlayerImpact(health);
            ImpactFeedback(transform.position, _bossImpactParticles);
        }
        
    }

    public void CooldownReset()
    {
        cooldown = false;
    }
    protected virtual void PlayerImpact(Health health)
    {
        health.TakeDamage(_damageAmount);
    }

    private void ImpactFeedback(Vector3 position, ParticleSystem particles)
    {
        // particles
        if (particles != null)
        {
            Instantiate(particles, position, Quaternion.identity);
        }
        // audio
        if (_impactSound != null)
        {
            AudioHelper.PlayClip2D(_impactSound, 1f);
        }
    }

    private void Update()
    {
        if (_currentGameState == GameState.IdleShoot)
        {
            if (LeftRight)
            {
                LeftandRight();
            }
            else if (X)
            {
                XMove();
            }
        }
        else if (_currentGameState == GameState.Charge)
        {
            Charge();
        }
        else if (_currentGameState == GameState.SpawnBlocks)
        {
            SpawnBlocks();
        }
        transform.LookAt(player);

        //shoot test
        if (Input.GetKeyUp(KeyCode.F))
        {
            Shoot();
        }
    }

    private void LeftandRight()
    {
        if (walkPointL)
        {
            walkPoint = new Vector3(spawnPoint.x-8, spawnPoint.y, spawnPoint.z);
            Vector3 distancetoWalkPoint = transform.position - walkPoint;
            if (distancetoWalkPoint.magnitude < 1f)
            {
                walkPointL = false;
                walkPointR = true;
                Debug.Log("Left");
            }
        }
        else if (walkPointR)
        {
            walkPoint = new Vector3(spawnPoint.x + 8, spawnPoint.y, spawnPoint.z);
            Vector3 distancetoWalkPoint = transform.position - walkPoint;
            if (distancetoWalkPoint.magnitude < 1f)
            {
                walkPointR = false;
                Debug.Log("Right");
            }
        }
        else
        {
            walkPoint = spawnPoint;
            Vector3 distancetoWalkPoint = transform.position - walkPoint;
            if (distancetoWalkPoint.magnitude < 1f)
            {
                LeftRight = false;
                X = true;
                walkPoint1 = true;
            }
        }
        agent.SetDestination(walkPoint);
    }
    private void XMove()
    {
        if (walkPoint1)
        {
            walkPoint = new Vector3(spawnPoint.x - 4, spawnPoint.y, spawnPoint.z - 4);
            Vector3 distancetoWalkPoint = transform.position - walkPoint;
            if (distancetoWalkPoint.magnitude < 1f)
            {
                walkPoint1 = false;
                walkPoint2 = true;
            }
        }
        else if (walkPoint2)
        {
            walkPoint = new Vector3(spawnPoint.x + 4, spawnPoint.y, spawnPoint.z + 4);
            Vector3 distancetoWalkPoint = transform.position - walkPoint;
            if (distancetoWalkPoint.magnitude < 1f)
            {
                walkPoint2 = false;
                spawn = true;
            }
        }
        else if (spawn)
        {
            walkPoint = spawnPoint;
            Vector3 distancetoWalkPoint = transform.position - walkPoint;
            if (distancetoWalkPoint.magnitude < 1f)
            {
                spawn = false;
                walkPoint3 = true;
            }
        }
        else if (walkPoint3)
        {
            walkPoint = new Vector3(spawnPoint.x -4, spawnPoint.y, spawnPoint.z + 4);
            Vector3 distancetoWalkPoint = transform.position - walkPoint;
            if (distancetoWalkPoint.magnitude < 1f)
            {
                walkPoint3 = false;
                walkPoint4 = true;
            }
        }
        else if (walkPoint4)
        {
            walkPoint = new Vector3(spawnPoint.x + 4, spawnPoint.y, spawnPoint.z - 4);
            Vector3 distancetoWalkPoint = transform.position - walkPoint;
            if (distancetoWalkPoint.magnitude < 1f)
            {
                walkPoint4 = false;
            }
        }
        else
        {
            walkPoint = spawnPoint;
            Vector3 distancetoWalkPoint = transform.position - walkPoint;
            if (distancetoWalkPoint.magnitude < 1f)
            {
                X = false;
                LeftRight = true;
                walkPointL = true;
            }
        }
        agent.SetDestination(walkPoint);
    }

    private void Charge()
    {

    }

    private void SpawnBlocks()
    {

    }

    private void Shoot()
    {
        //direction to fire
        Vector3 direction = attackPoint.forward;

        //Instantiate bullet
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

        //rotate bullet to proper direction
        currentBullet.transform.forward = direction.normalized;

        // add force to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shootforce, ForceMode.Impulse);
    }
}
