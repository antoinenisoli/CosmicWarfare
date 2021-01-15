using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

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
    Turret,
}

public abstract class Sc_Building : Sc_Entity
{
    public virtual BuildingType buildingType { get; }
    [HideInInspector] public Sc_ResourcesManager resourceManager;

    [Header("_BUILD")]
    public bool busy;
    public BuildingState currentState = BuildingState.InPlacing;
    [SerializeField] Material movingMat, constructionMat, selectedMat;
    [SerializeField] GameObject dummyVersion;
    public bool isColliding;
    float delay;

    [Header("_DESTRUCTION")]
    [SerializeField] float destructionDelay = 2;
    [SerializeField] float fallPower = 15f;
    [Range(0, 1)] [SerializeField] protected float shakeAmount = 0.5f;
    protected StressReceiver shakeCam;

    [Header("_GAMEPLAY")]
    public bool selected;
    [SerializeField] Animation[] idleAnimations;

    public void Awake()
    {
        switch (myTeam)
        {
            case Team.Player:
                resourceManager = FindObjectOfType<Sc_ResourcesManager_Ally>();
                break;
            case Team.Enemy:
                resourceManager = FindObjectOfType<Sc_ResourcesManager_Enemy>();
                break;
        }

        baseMat = meshRender.material;
        shakeCam = FindObjectOfType<StressReceiver>();
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

    public Vector3 MeshClosestPoint(Vector3 from)
    {
        return meshRender.GetComponentInChildren<Collider>().ClosestPoint(from);
    }

    public override void Death()
    {
        Sc_VFXManager.Instance.InvokeVFX(FX_Event.Explosion, transform.position, Quaternion.identity);
        Sc_SoundManager.instance.PlayAudio(AudioType.Explosion.ToString(), transform);
        shakeCam.InduceStress(shakeAmount);
        health.isDead = true;
        if (health.healthSlider)
            Destroy(health.healthSlider.gameObject);

        float currentPosY = transform.position.y;
        transform.DOMoveY(currentPosY - fallPower, destructionDelay);
        Destroy(gameObject, destructionDelay);
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
        Sc_SoundManager.instance.PlayAudio(AudioType.NewBuilding.ToString(), transform);
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
        if (!EventSystem.current.IsPointerOverGameObject() && currentState == BuildingState.Builded)
        {
            selected = select;
            if (select)
            {
                Sc_SoundManager.instance.PlayAudio("Select" + buildingType.ToString(), transform);
            }
        }
        else
            selected = false;
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
        Sc_SoundManager.instance.PlayAudio(AudioType.BuildFinished.ToString(), transform);
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
                meshRender.material.SetColor("_EmissionColor", selected ? resourceManager.teamColor : Color.white);
                if (!resourceManager.gameEnded)
                    UseBuilding();
                break;
        }
    }
}
