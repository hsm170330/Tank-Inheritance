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
    bool spawningBlocks = false;

    Rigidbody _rb;

    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatIsGround, whatIsPlayer;

    private GameState _currentGameState = GameState.IdleShoot;

    //Movement
    public Vector3 walkPoint, spawnPoint;
    bool LeftRight, X;

    bool walkPointL, walkPointR, walkPoint1, walkPoint2, walkPoint3, walkPoint4, spawn;

    bool atPlayer;

    //Shoot attack stuff
    public Transform[] attackPoints = new Transform[8];
    public Transform attackPoint1, attackPoint2, attackPoint3, attackPoint4, attackPoint5, attackPoint6,
        attackPoint7, attackPoint8;

    public float shootforce;
    public GameObject bullet;
    public GameObject BossBlock;

    public Material BossM;

    private Vector3 rotation;

    private Color original;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Tank").transform;
        rotation = transform.forward;
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
        atPlayer = false;

        attackPoints[0] = attackPoint1;
        attackPoints[1] = attackPoint2;
        attackPoints[2] = attackPoint3;
        attackPoints[3] = attackPoint4;
        attackPoints[4] = attackPoint5;
        attackPoints[5] = attackPoint6;
        attackPoints[6] = attackPoint7;
        attackPoints[7] = attackPoint8;

        original = BossM.GetColor("_Color");

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
            if (!spawningBlocks)
            {
                SpawnBlockPositions();
            }
            else
            {
                BossM.SetColor("_Color", original);
                Invoke("IdleStateChange", 5);
            }

        }
        transform.forward = rotation;

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
                Shoot();
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
                Shoot();
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
                Shoot();
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
                Shoot();
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
                Shoot();
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
                Shoot();
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
                Shoot();
            }
        }
        else if (walkPoint4)
        {
            walkPoint = new Vector3(spawnPoint.x + 4, spawnPoint.y, spawnPoint.z - 4);
            Vector3 distancetoWalkPoint = transform.position - walkPoint;
            if (distancetoWalkPoint.magnitude < 1f)
            {
                walkPoint4 = false;
                Shoot();
            }
        }
        else
        {
            walkPoint = spawnPoint;
            Vector3 distancetoWalkPoint = transform.position - walkPoint;
            if (distancetoWalkPoint.magnitude < 1f)
            {
                X = false;
                Shoot();
                BossM.SetColor("_Color", new Color(220, 0, 0));
                Invoke("ChargeStateChange", 5);
            }
        }
        agent.SetDestination(walkPoint);
    }

    private void Charge()
    {
        if (!atPlayer)
        {
            Invoke("StopCharge", 10f);
            Vector3 playerPosition = player.position;
            walkPoint = playerPosition;
            Vector3 distancetoWalkPoint = transform.position - walkPoint;
            if (distancetoWalkPoint.magnitude < 2f)
            {
                atPlayer = true;
            }
        }
        else
        {
            walkPoint = spawnPoint;
            Vector3 distancetoWalkPoint = transform.position - walkPoint;
            if (distancetoWalkPoint.magnitude < 1f)
            {
                BossM.SetColor("_Color", new Color(255, 255, 255));
                atPlayer = false;
                spawningBlocks = false;
                Invoke("BlockStateChange", 5);
            }
        }
       
        agent.SetDestination(walkPoint);
    }

    private void StopCharge()
    {
        atPlayer = true;
    }

    private void SpawnBlockPositions()
    {
        Debug.Log("Before for loop");
        for (int i = 0; i < 5; i++)
        {
            BossBlock = Instantiate(BossBlock, new Vector3(Random.Range(-5, 5), 5, Random.Range(-5, 5)), Quaternion.identity);
            BossBlock.SetActive(true);
            Debug.Log("Block Spawned");
        }
        Debug.Log("After for loop");
        spawningBlocks = true;
    }
    private void IdleStateChange()
    {
        
        _currentGameState = GameState.IdleShoot;
    }
    private void ChargeStateChange()
    {
        Debug.Log("Changing Game State to Charge");
        LeftRight = true;
        walkPointL = true;
        _currentGameState = GameState.Charge;
    }
    private void BlockStateChange()
    {
        _currentGameState = GameState.SpawnBlocks;
    }

    private void Shoot()
    {
        //direction to fire
        Vector3[] direction = new Vector3[8];
        GameObject[] currentBullet = new GameObject[8];

        for (int i = 0; i < attackPoints.Length; i++)
        {
            direction[i] = attackPoints[i].forward;

            //Instantiate bullet
            currentBullet[i] = Instantiate(bullet, attackPoints[i].position, Quaternion.identity);
            //rotate bullet to proper direction
            currentBullet[i].transform.forward = direction[i].normalized;
        }
        for (int i = 0; i < attackPoints.Length; i++)
        {
            // add force to bullet
            currentBullet[i].GetComponent<Rigidbody>().AddForce(direction[i].normalized * shootforce, ForceMode.Impulse);
        }
    }
}
