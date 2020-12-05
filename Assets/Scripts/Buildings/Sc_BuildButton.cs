using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Purchase
{
    [SerializeField] string name;
    public GameObject prefab;
    public float creationDelay = 3;
    public ResourceCost[] costs = new ResourceCost[2];
}

public class Sc_BuildButton : MonoBehaviour
{
    Sc_GlobalBuilder builder => FindObjectOfType<Sc_GlobalBuilder>();
    Sc_ResourcesManager resourceManager => FindObjectOfType<Sc_ResourcesManager>();
    Button myButton => GetComponent<Button>();

    [SerializeField] Purchase building;

    public void SelectThisBuilding()
    {
        builder.SelectBuilding(building.prefab, building);
    }

    public void SetButton()
    {
        myButton.interactable = resourceManager.CanPay(building.costs);
    }

    private void Start()
    {
        Sc_EventManager.Instance.onCost.AddListener(SetButton);
        Sc_EventManager.Instance.onCost.Invoke();
    }
}
