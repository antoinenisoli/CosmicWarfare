using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Entity : MonoBehaviour
{
    [Header("ENTITY")]
    public Sc_HealthSystem health;
    public Sc_EntityInfo info;
    public Team myTeam;

    [Header("Graphics")]
    public bool highlighted;
    protected Material baseMat;
    public MeshRenderer meshRender;
    protected Outline outline;

    public virtual void Start()
    {
        outline = meshRender.GetComponent<Outline>();
        health.Initialize(this);
    }

    public virtual void ModifyLife(float amount, Vector3 damageLocation)
    {
        health.TakeDamages(amount);
    }

    public virtual void Death()
    {
        health.isDead = true;
        if (health.healthSlider)
            Destroy(health.healthSlider.gameObject);

        Destroy(gameObject);
    }

    public virtual void Update()
    {
        if (health.healthSlider)
            health.healthSlider.gameObject.SetActive(health.CurrentHealth < health.MaxHealth);

        outline.enabled = highlighted ? true : false;
    }

    public override string ToString()
    {
        return "<b>" + info.entityName + "</b>" + "\n" + info.entityDescription;
    }
}
