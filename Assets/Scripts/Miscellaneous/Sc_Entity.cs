using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Entity : MonoBehaviour
{
    [Header("ENTITY")]
    public Sc_HealthSystem health;
    public Sc_EntityInfo info;
    protected Material baseMat;
    public bool highlighted;
    public MeshRenderer meshRender;
    public Team myTeam;

    public virtual void Start()
    {
        health.Initialize(this);
    }

    public virtual void ModifyLife(float amount, Vector3 damageLocation)
    {
        health.TakeDamages(amount);
    }

    public virtual void Death()
    {
        if (health.healthSlider)
            Destroy(health.healthSlider.gameObject);

        Destroy(gameObject);
    }

    public virtual void Update()
    {
        if (health.healthSlider)
        {
            health.healthSlider.gameObject.SetActive(health.CurrentHealth < health.MaxHealth);
        }
    }

    public override string ToString()
    {
        return "<b>" + info.entityName + "</b>" + "\n" + info.entityDescription;
    }
}
