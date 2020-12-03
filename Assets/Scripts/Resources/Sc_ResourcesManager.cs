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
    [Serializable]
    class Resource
    {
        public string name;
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

    [SerializeField] Resource[] resources;

    private void Start()
    {
        ActualizeText();
    }

    public void Cost(ResourceType res, int amount)
    {
        resources[(int)res].CurrentAmount -= amount;
        ActualizeText();
    }

    public void ActualizeText()
    {
        for (int i = 0; i < resources.Length; i++)
        {
            resources[i].displayResource.text = resources[i].name + " : " + resources[i].CurrentAmount;
        }
    }
}
