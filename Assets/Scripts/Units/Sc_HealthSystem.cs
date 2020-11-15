using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Sc_HealthSystem
{
    Sc_Unit unit;
    [SerializeField] float maxHealth, currentHealth;
    [SerializeField] Slider healthSlider;

    public float CurrentHealth 
    { 
        get => currentHealth; 
        set
        {
            if (value < 0)
            {
                unit.Death();
                value = 0;
            }

            if (value > maxHealth)
                value = maxHealth;

            currentHealth = value;
        }

    }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }

    public void Initialize(Sc_Unit unit)
    {
        this.unit = unit;
        CurrentHealth = MaxHealth;
        SetSlider();
    }

    public void TakeDamages(float amount)
    {
        CurrentHealth -= amount;
        SetSlider();
    }

    void SetSlider()
    {
        healthSlider.maxValue = MaxHealth;
        healthSlider.value = CurrentHealth;
    }
}
