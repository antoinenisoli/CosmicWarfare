﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Sc_HealthSystem
{
    Sc_DestroyableEntity entity;
    [SerializeField] float maxHealth, currentHealth;
    public Slider healthSlider;

    public float CurrentHealth 
    { 
        get => currentHealth; 
        set
        {
            if (value < 0)
            {
                entity.Death();
                value = 0;
            }

            if (value > maxHealth)
                value = maxHealth;

            currentHealth = value;
        }

    }
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }

    public void Initialize(Sc_DestroyableEntity entity)
    {
        this.entity = entity;
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
        if (healthSlider == null)
            return;

        healthSlider.maxValue = MaxHealth;
        healthSlider.value = CurrentHealth;
    }
}
