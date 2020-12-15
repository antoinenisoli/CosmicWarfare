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

public abstract class Sc_Building : Sc_DestroyableEntity
{
    protected Sc_ResourcesManager resourceManager => FindObjectOfType<Sc_ResourcesManager>();

    public virtual string type { get; }

    [Header("_BUILD")]    
    public BuildingState currentState = BuildingState.InPlacing;
    [SerializeField] MeshRenderer meshRender;
    [SerializeField] Material movingMat, constructionMat, selectedMat;
    [SerializeField] GameObject dummyVersion;
    public bool isColliding;
    float delay;
    Outline outline;

    [Header("_GAMEPLAY")]
    public bool selected;
    [SerializeField] Animation[] idleAnimations;

    private void Awake()
    {
        baseMat = meshRender.material;
        outline = meshRender.GetComponent<Outline>();
        idleAnimations = GetComponentsInChildren<Animation>();

        if (currentState == BuildingState.InPlacing)
        {
            meshRender.material = movingMat;
            foreach (var anim in idleAnimations)
            {
                anim.Stop();
            }
        }
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
            Sc_VFXManager.Instance.InvokeVFX(FX_Event.BuildingDamage, damageLocation);
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
        Sc_Selection.GenerateEntity(this);
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
                {
                    Build();
                }
                break;
            case BuildingState.Builded:
                if (selected)
                {
                    meshRender.material = selectedMat;
                }
                else
                {
                    meshRender.material = baseMat;
                }

                UseBuilding();
                break;
        }
    }
}
