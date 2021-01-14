using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum ResourceType
{
    Steel,
    Oil,
}

public abstract class Sc_ResourcesManager : MonoBehaviour
{
    public Resource[] resources;
    public Team myTeam;
    public bool gameEnded;

    [ColorUsage(true, true)]
    public Color teamColor = Color.blue;

    public virtual void ModifyValue(int amount, ResourceType res)
    {
        if (amount == 0)
            return;

        resources[(int)res].CurrentAmount += amount;
        Sc_EventManager.Instance.onCost.Invoke();
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
        if (gameEnded)
            return false;

        bool canPay = false;
        foreach (ResourceCost cost in costs)
        {
            Resource resource = GetResource(cost.resourceType);
            canPay = (resource.CurrentAmount + cost.value) >= 0;
        }

        return canPay;
    }
}
