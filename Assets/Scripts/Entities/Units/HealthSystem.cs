﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class HealthSystem
{
    Entity entity;
    [SerializeField] float currentHealth;
    float maxHealth;
    public Slider healthSlider;
    public bool isDead;

    public float CurrentHealth 
    { 
        get => currentHealth; 
        set
        {
            if (value < 0 && !isDead)
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

    public void Initialize(Entity entity)
    {
        this.entity = entity;
        MaxHealth = entity.info.maxHealth;
        CurrentHealth = MaxHealth;
        SetSlider();
    }

    public void TakeDamages(float amount)
    {
        CurrentHealth += amount;
        SetSlider();
    }

    public void SetSlider()
    {
        if (healthSlider == null)
            return;

        healthSlider.maxValue = MaxHealth;
        healthSlider.value = CurrentHealth;
    }
}
