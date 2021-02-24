using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ResourceCost
{
    public int value = 15;
    public ResourceType resourceType;
}

public class GlobalBuilder : MonoBehaviour
{
    ResourcesManagerAlly resourceManager;
    Camera cam;

    BuildingMainBase mainBase;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask oilLayer;
    [SerializeField] Transform parent;

    [Range(0,10)]
    [SerializeField] float maxBuildAngle = 8;
    bool isBuilding;
    Building lastBuilding;
    Purchase lastBuildingCost;

    private void Start()
    {
        resourceManager = FindObjectOfType<ResourcesManagerAlly>();
        cam = Camera.main;
        foreach (BuildingMainBase _base in FindObjectsOfType<BuildingMainBase>())
        {
            if (_base.myTeam.Equals(Team.Player))
            {
                mainBase = _base;
                return;
            }
        }
    }

    public void SelectBuilding(GameObject building, Purchase purchase)
    {
        GameObject newBuilding = Instantiate(building, parent);
        lastBuilding = newBuilding.GetComponent<Building>();
        isBuilding = true;
        mainBase.SwitchBuildingMode(true);
        lastBuildingCost = purchase;
    }

    void ExitBuild()
    {
        mainBase.SwitchBuildingMode(false);
        lastBuildingCost = null;
        lastBuilding = null;
        isBuilding = false;
    }

    void Construction()
    {
        if (isBuilding)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            bool detectGround = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer);
            if (detectGround)
            {
                lastBuilding.transform.position = hit.point;
                lastBuilding.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                float angle = Vector3.Angle(hit.normal, Vector3.up);
                bool canPlace = angle < maxBuildAngle && !lastBuilding.isColliding && mainBase.InRange(lastBuilding.transform.position);

                if (lastBuilding.buildingType == BuildingType.Engine)
                    lastBuilding.CanBePlaced(canPlace && Physics.Raycast(ray, Mathf.Infinity, oilLayer));
                else
                    lastBuilding.CanBePlaced(canPlace);

                if (Input.GetMouseButtonDown(0) && canPlace)
                {
                    lastBuilding.Place(lastBuildingCost.creationDelay);
                    foreach (var cost in lastBuildingCost.costs)
                    {
                        resourceManager.ModifyValue(cost.value, cost.resourceType);
                    }

                    ExitBuild();
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                if (lastBuilding)
                    Destroy(lastBuilding.gameObject);

                ExitBuild();
            }
        }
    }

    private void Update()
    {
        Construction();
    }
}
