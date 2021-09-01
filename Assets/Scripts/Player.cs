using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TankController))]
public class Player : MonoBehaviour
{
    [SerializeField] int _maxHealth = 3;
    int _currentHealth;
    int _treasureCount;
    public bool invincible = false;
    public TextMeshProUGUI treasureText;

    TankController _tankController;

    private void Awake()
    {
        _tankController = GetComponent<TankController>();
    }
    // Start is called before the first frame update
    private void Start()
    {
        _currentHealth = _maxHealth;
        _treasureCount = 0;
        SetTreasureText();
    }

    public void IncreaseHealth(int amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);
        Debug.Log("Player's health: " + _currentHealth);
    }

    public void DecreaseHealth(int amount)
    {
        if (!invincible)
        {
            _currentHealth -= amount;
            Debug.Log("Player's health: " + _currentHealth);
            if (_currentHealth <= 0)
            {
                Kill();
            }
        }
        
    }

    public void ReduceSpeed(float amount)
    {
        _tankController.MaxSpeed -= amount;
        if (_tankController.MaxSpeed <= 0.05)
            _tankController.MaxSpeed = 0.05f;
        Debug.Log("Player's speed: " + _tankController.MaxSpeed);
    }

    public void IncreaseTreasure(int amount)
    {
        _treasureCount += amount;
        SetTreasureText();
        // Debug.Log("Player's treasure: " + _treasureCount);
    }

    void SetTreasureText()
    {
        treasureText.text = "Treasure: " + _treasureCount.ToString();
    }

    public void Kill()
    {
        gameObject.SetActive(false);
        // play particles
        // play sounds
    }
}
