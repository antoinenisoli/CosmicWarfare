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
        Sc_Entity entity = prefab.GetComponent<Sc_Entity>();
        foreach (var item in costs)
        {
            description += "\n" + "<color=red>" + item.value + " " + item.resourceType + "</color>";
        }

        return "<b>" + purchase_Name + "</b>" + description + "\n\n" + entity.info.entityDescription;
    }
}

public abstract class Sc_Button : MonoBehaviour
{
    protected Sc_ResourcesManager resourceManager => FindObjectOfType<Sc_ResourcesManager>();
    protected Button myButton => GetComponent<Button>();

    public virtual void Start()
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
