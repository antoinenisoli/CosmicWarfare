using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [Header("ENTITY")]
    public EntityInfo info;
    public HealthSystem health;
    public Team myTeam;

    [Header("Graphics")]
    public bool highlighted;
    protected Material baseMat;
    protected Outline[] outlines;
    public float UIOffset = 28;
    public float UIScale = 5;

    public virtual void Start()
    {
        outlines = GetComponentsInChildren<Outline>();
        health.Initialize(this);
    }

    public virtual void ModifyLife(float amount, Vector3 damageLocation)
    {
        health.TakeDamages(amount);
    }

    public abstract Vector3 MeshClosestPoint(Vector3 from);

    public virtual void Death()
    {
        health.isDead = true;
        if (health.healthSlider)
            Destroy(health.healthSlider.gameObject);
    }

    public virtual void Update()
    {
        if (health.healthSlider)
            health.healthSlider.gameObject.SetActive(health.CurrentHealth < health.MaxHealth);

        if (outlines.Length > 0)
            foreach (var item in outlines)
                item.enabled = highlighted;
    }

    public override string ToString()
    {
        return "<b>" + info.entityName + "</b>" + "\n" + info.entityDescription;
    }
}
