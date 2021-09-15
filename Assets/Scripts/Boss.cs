using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Boss : MonoBehaviour
{
    [SerializeField] int _damageAmount = 1;
    [SerializeField] ParticleSystem _impactParticles;
    [SerializeField] ParticleSystem _bossImpactParticles;
    [SerializeField] AudioClip _impactSound;
    bool cooldown = false;

    Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
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

    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {

    }
}
