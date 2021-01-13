using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum BuildingState
{
    InPlacing,
    IsBuilding,
    Builded,
}

public enum BuildingType
{
    MainBase,
    Casern,
    Engine,
}

public abstract class Sc_Building : Sc_Entity
{
    public virtual BuildingType type { get; }
    protected Sc_ResourcesManager resourceManager;

    [Header("_BUILD")]
    public bool busy;
    public BuildingState currentState = BuildingState.InPlacing;    
    [SerializeField] Material movingMat, constructionMat, selectedMat;
    [SerializeField] GameObject dummyVersion;
    public bool isColliding;
    float delay;

    [Header("_GAMEPLAY")]
    public bool selected;
    [SerializeField] Animation[] idleAnimations;

    void Awake()
    {
        baseMat = meshRender.material;
        idleAnimations = GetComponentsInChildren<Animation>();

        if (currentState == BuildingState.InPlacing)
        {
            meshRender.material = movingMat;
            foreach (var anim in idleAnimations)
            {
                anim.Stop();
            }
        }

        switch (myTeam)
        {
            case Team.Player:
                resourceManager = FindObjectOfType<Sc_ResourcesManager_Ally>();
                break;
            case Team.Enemy:
                resourceManager = FindObjectOfType<Sc_ResourcesManager_Enemy>();
                break;
        }
    }

    public Vector3 MeshClosestPoint(Vector3 from)
    {
        return meshRender.GetComponentInChildren<Collider>().ClosestPoint(from);
    }

    public override void Death()
    {
        Sc_VFXManager.Instance.InvokeVFX(FX_Event.Explosion, transform.position, Quaternion.identity);
        base.Death();
    }

    private void OnTriggerStay(Collider other)
    {
        isColliding = other.GetComponent<Sc_Building>() || other.GetComponentInParent<Sc_Unit>();
    }

    private void OnTriggerExit(Collider other)
    {
        isColliding = false;
    }

    public override void ModifyLife(float amount, Vector3 damageLocation)
    {
        base.ModifyLife(amount, damageLocation);
        if (amount < 0)
        {
            Sc_VFXManager.Instance.InvokeVFX(FX_Event.LaserDamage, damageLocation, Quaternion.identity);
        }
    }

    public void Place(float delay)
    {
        this.delay = delay;

        Vector3 baseScale = transform.GetChild(0).localScale;
        transform.GetChild(0).localScale = Vector3.one * 0.1f;
        transform.GetChild(0).DOScale(baseScale, delay);
        GameObject constructionDummy = Instantiate(dummyVersion, transform.position, Quaternion.identity);
        constructionDummy.transform.localScale = baseScale;
        Destroy(constructionDummy, delay);
        meshRender.material = constructionMat;
        currentState = BuildingState.IsBuilding;
        Sc_SelectionManager.GenerateEntity(this);
    }

    public abstract void UseBuilding();

    public void SelectMe(bool select)
    {
        if (currentState != BuildingState.Builded)
            return;

        selected = select;
    }

    public void CanBePlaced(bool canBePlaced)
    {
        Color newCol = canBePlaced ? Color.green : Color.red;
        newCol.a = 0.2f;
        meshRender.material.SetColor("_Color", newCol);
    }

    void Build()
    {
        meshRender.material = baseMat;
        delay = 0;
        currentState = BuildingState.Builded;
        foreach (var anim in idleAnimations)
        {
            anim.Play();
        }
    }

    public override void Update()
    {
        base.Update();
        if (outline)
            outline.enabled = highlighted;

        switch (currentState)
        {
            case BuildingState.IsBuilding:
                if (delay > 0)
                    delay -= Time.deltaTime;
                else
                    Build();
                break;

            case BuildingState.Builded:
                meshRender.material = selected ? selectedMat : baseMat;
                UseBuilding();
                break;
        }
    }
}
