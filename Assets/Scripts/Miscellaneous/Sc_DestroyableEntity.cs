using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_DestroyableEntity : MonoBehaviour
{
    [Header("_DESTROYABLE ENTITY")]
    public string s_Name;
    [TextArea] public string s_description;
    public Sc_HealthSystem health;
    protected Material baseMat;
    public bool highlighted;
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
            health.healthSlider.gameObject.SetActive(health.CurrentHealth < health.MaxHealth);
    }

    public override string ToString()
    {
        return s_Name + "\n" + s_description;
    }
}
