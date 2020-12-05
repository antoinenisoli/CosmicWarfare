using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Resource
{
    public ResourceType type;
    public Text displayResource;
    [SerializeField] int currentAmount;
    [SerializeField] int maxAmount;
    public int CurrentAmount
    {
        get => currentAmount;

        set
        {
            if (value > maxAmount)
                value = maxAmount;

            if (value < 0)
                value = 0;

            currentAmount = value;
        }
    }

    public int MaxAmount { get => maxAmount; set => maxAmount = value; }
}
