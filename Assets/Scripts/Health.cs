using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour, IDamageable
{

    [SerializeField] int _maxHealth = 3;
    int _currentHealth;
    bool takingDamage;

    public Slider slider;
    private void Start()
    {
        _currentHealth = _maxHealth;
        slider.maxValue = _maxHealth;
        slider.value = _currentHealth;
        takingDamage = false;
    }
    public void TakeDamage(int damage)
    {
        if (!takingDamage)
        {
            takingDamage = true;
            _currentHealth -= damage;
            slider.value = _currentHealth;
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
