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

public abstract class Building : Entity
{
    public virtual BuildingType buildingType { get; }
    [HideInInspector] public ResourcesManager resourceManager;

    [Header("_BUILD")]
    public MeshRenderer meshRender;
    public bool busy;
    public BuildingState currentState = BuildingState.InPlacing;
    [SerializeField] Material movingMat, constructionMat, selectedMat;
    [SerializeField] GameObject dummyVersion;
    public bool isColliding;
    float delay;
    NavMeshSourceTag sourceTag;

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
                resourceManager = FindObjectOfType<ResourcesManagerAlly>();
                break;
            case Team.Enemy:
                resourceManager = FindObjectOfType<ResourcesManagerEnemy>();
                break;
        }

        sourceTag = GetComponentInChildren<NavMeshSourceTag>();
        baseMat = meshRender.material;
        shakeCam = FindObjectOfType<StressReceiver>();
        idleAnimations = GetComponentsInChildren<Animation>();

        if (currentState == BuildingState.InPlacing)
        {
            sourceTag.enabled = false;
            meshRender.material = movingMat;
            foreach (var anim in idleAnimations)
            {
                anim.Stop();
            }
        }
    }

    public override float HealthbarOffset()
    {
        if (meshRender.TryGetComponent(out MeshFilter mf))
            return mf.mesh.bounds.size.magnitude;

        return 0;
    }

    public override Vector3 MeshClosestPoint(Vector3 from)
    {
        return meshRender.GetComponent<Collider>().ClosestPoint(from);
    }

    public override void Death()
    {
        VFXManager.Instance.InvokeVFX(FX_Event.Explosion, transform.position, Quaternion.identity);
        SoundManager.instance.PlayAudio(AudioType.Explosion.ToString(), transform);
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
        isColliding = other.transform.root.GetComponentInChildren<Entity>();
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
            VFXManager.Instance.InvokeVFX(FX_Event.LaserDamage, damageLocation, Quaternion.identity);
        }
    }

    public void Place(float delay)
    {
        this.delay = delay;
        SoundManager.instance.PlayAudio(AudioType.NewBuilding.ToString(), transform);
        Transform childToTween = transform.GetChild(0);
        Vector3 baseScale = childToTween.localScale;

        childToTween.localScale = Vector3.one * 0.1f;
        childToTween.DOScale(baseScale, delay);
        GameObject constructionDummy = Instantiate(dummyVersion, transform.position, Quaternion.identity);
        constructionDummy.transform.localScale = baseScale;
        Destroy(constructionDummy, delay);
        meshRender.material = constructionMat;
        currentState = BuildingState.IsBuilding;
        SelectionManager.GenerateEntity(this);
        sourceTag.enabled = true;
    }

    public abstract void UseBuilding();

    public void SelectMe(bool select)
    {
        if (!EventSystem.current.IsPointerOverGameObject() && currentState == BuildingState.Builded)
        {
            selected = select;
            if (select)
                SoundManager.instance.PlayAudio("Select" + buildingType.ToString(), transform);
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
        SoundManager.instance.PlayAudio(AudioType.BuildFinished.ToString(), transform);
        currentState = BuildingState.Builded;
        foreach (var anim in idleAnimations)
            anim.Play();
    }

    public override void Update()
    {
        base.Update();
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
