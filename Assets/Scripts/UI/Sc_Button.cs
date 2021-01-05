using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Purchase
{
    [SerializeField] string purchase_Name;
    public GameObject prefab;
    public float creationDelay = 3;
    public ResourceCost[] costs = new ResourceCost[2];

    public override string ToString()
    {
        string description = "";
        foreach (var item in costs)
        {
            description += "\n" + item.value + " " + item.resourceType;
        }

        return purchase_Name + description;
    }
}

public abstract class Sc_Button : MonoBehaviour
{
    protected Sc_ResourcesManager resourceManager => FindObjectOfType<Sc_ResourcesManager>();
    protected Button myButton => GetComponent<Button>();

    private void Start()
    {
        Sc_EventManager.Instance.onCost.AddListener(SetButton);
        Sc_EventManager.Instance.onCost.Invoke();
    }

    public abstract void SetButton();

    public virtual void ShowTooltip(bool value)
    {
        Sc_UIManager.instance.ShowTooltip(value);
        Sc_UIManager.instance.tooltip.PlaceTool(GetComponent<RectTransform>());
    }
}
