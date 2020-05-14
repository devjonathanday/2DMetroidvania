using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth;

    [SerializeField] [ReadOnlyField] float currentHealth;
    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            currentHealth = value;

            if(currentHealth <= 0)
            {
                OnDeathEvent.Invoke();
            }
            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(float amount)
    {
        CurrentHealth -= amount;
    }

    public delegate void HealthEventDelegate();

    public HealthEventDelegate OnDeathEvent;
}
