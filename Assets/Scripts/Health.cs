using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{

    [SerializeField] int _maxHealth = 3;
    int _currentHealth;
    bool takingDamage;
    private void Start()
    {
        _currentHealth = _maxHealth;
        takingDamage = false;
    }
    public void TakeDamage(int damage)
    {
        if (!takingDamage)
        {
            takingDamage = true;
            _currentHealth -= damage;
            Debug.Log(gameObject.ToString() + " health: " + _currentHealth);
            Invoke("Cooldown", 0.2f);
        }
        
        if (_currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Cooldown()
    {
        takingDamage = false;
    }

    public void Kill()
    {
        gameObject.SetActive(false);
    }
}
