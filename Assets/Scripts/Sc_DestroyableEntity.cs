using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_DestroyableEntity : MonoBehaviour
{
    [SerializeField] protected Sc_HealthSystem health;
    protected Material baseMat;

    public virtual void Awake()
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
}
