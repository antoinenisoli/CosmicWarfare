using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_DestroyableEntity : MonoBehaviour
{
    public Sc_HealthSystem health;
    protected Material baseMat;
    public bool highlighted;
    public Team myTeam;

    public virtual void Start()
    {
        health.Initialize(this);
    }

    public virtual void TakeDamages(float amount)
    {
        health.TakeDamages(amount);
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }

    public virtual void Update()
    {
        if (health.healthSlider)
            health.healthSlider.gameObject.SetActive(health.CurrentHealth < health.MaxHealth);
    }
}
