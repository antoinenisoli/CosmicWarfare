using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ResourceType
{
    Wood,
    Oil,
}

public class Sc_ResourcesManager : MonoBehaviour
{
    public Resource[] resources;

    private void Start()
    {
        ActualizeText();
    }

    public void Cost(int amount, ResourceType res)
    {
        resources[(int)res].CurrentAmount -= amount;
        ActualizeText();
    }

    public Resource GetResource(ResourceType type)
    {
        foreach (Resource resource in resources)
        {
            if (resource.type.Equals(type))
                return resource;
        }

        return null;
    }

    public bool CanPay(ResourceCost[] costs)
    {
        bool canPay = false;
        foreach (ResourceCost cost in costs)
        {
            Resource resource = GetResource(cost.resourceType);
            canPay = (resource.CurrentAmount - cost.value) > 0;
        }

        return canPay;
    }

    public void ActualizeText()
    {
        for (int i = 0; i < resources.Length; i++)
        {
            resources[i].displayResource.text = resources[i].type + " : " + resources[i].CurrentAmount;
        }
    }
}
