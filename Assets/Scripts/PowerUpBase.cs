using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class PowerUpBase : MonoBehaviour
{
    protected abstract void PowerUp(Player player);
    protected abstract void PowerDown(Player player);
    [SerializeField] float powerupDuration = 5;

    [SerializeField] float _movementSpeed = 1;
    [SerializeField] ParticleSystem _collectParticles;
    [SerializeField] AudioClip _collectSound;
    [SerializeField] GameObject _visualsToDeactivate = null;

    Collider _colliderToDeactivate = null;

    Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _colliderToDeactivate = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        Movement(_rb);
    }

    protected virtual void Movement(Rigidbody rb)
    {
        // calculate rotation
        Quaternion turnOffset = Quaternion.Euler(0, _movementSpeed, 0);
        rb.MoveRotation(_rb.rotation * turnOffset);
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();
        if (player != null)
        {
            // spawn particles & sfx because we need to diable object
            Feedback();

            StartCoroutine(PowerupSequence(player));
        }
    }

    IEnumerator PowerupSequence(Player player)
    {
        PowerUp(player);
        // simulate diabling object without actually
        // disabling it so scripts can still run
        DisableObject();

        // wait for the required duration
        yield return new WaitForSeconds(powerupDuration);
        // reset
        PowerDown(player);
        DeleteObject();
    }

    public void DisableObject()
    {
        // diable collider, so it can't be retriggered
        _colliderToDeactivate.enabled = false;
        // disable visuals, to simulate deactivated
        _visualsToDeactivate.SetActive(false);
    }

    public void DeleteObject()
    {
        gameObject.SetActive(false);
    }

    private void Feedback()
    {
        // particles
        if (_collectParticles != null)
        {
            _collectParticles = Instantiate(_collectParticles, transform.position, Quaternion.identity);
        }
        // audio. TODO - consider Object Pooling for performance
        if (_collectSound != null)
        {
            AudioHelper.PlayClip2D(_collectSound, 1f);
        }
    }
}
