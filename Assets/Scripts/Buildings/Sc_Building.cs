using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum BuildingState
{
    InPlacing,
    IsBuilding,
    Builded,
}

public class Sc_Building : Sc_DestroyableEntity
{
    [Serializable]
    public class BuildingCost
    {
        public int cost = 15;
        public ResourceType resource;
    }
    Sc_ResourcesManager resourceManager => FindObjectOfType<Sc_ResourcesManager>();

    [Header("Build")]
    public BuildingCost[] costs = new BuildingCost[2];
    [SerializeField] BuildingState currentState = BuildingState.InPlacing;
    [SerializeField] MeshRenderer meshRender;
    [SerializeField] Material movingMat, constructionMat;
    [SerializeField] float buildingTime = 15;
    float delay;

    [Header("Gameplay")]
    [SerializeField] string bName;
    [SerializeField] protected Team myTeam;

    private void Awake()
    {
        baseMat = meshRender.material;
    }

    public void Place()
    {
        delay = buildingTime;
        currentState = BuildingState.IsBuilding;

        foreach (BuildingCost item in costs)
        {
            resourceManager.Cost(item.cost, item.resource);
        }
    }

    public override void Update()
    {
        base.Update();
        meshRender.material = currentState == BuildingState.InPlacing ? movingMat : currentState == BuildingState.IsBuilding ? constructionMat : baseMat;

        switch (currentState)
        {
            case BuildingState.InPlacing:
                break;
            case BuildingState.IsBuilding:
                if (delay > 0)
                {
                    delay -= Time.deltaTime;
                }
                else
                {
                    delay = 0;
                    currentState = BuildingState.Builded;
                    print(bName + " is build !");
                }
                break;
            case BuildingState.Builded:
                break;
        }
    }
}
