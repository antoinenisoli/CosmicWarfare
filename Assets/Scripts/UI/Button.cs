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
        Entity entity = prefab.GetComponent<Entity>();
        foreach (var item in costs)
        {
            description += "\n" + "<color=red>" + item.value + " " + item.resourceType + "</color>";
        }

        return "<b>" + purchase_Name + "</b>" + description + "\n\n" + entity.info.entityDescription;
    }
}

public abstract class Button : MonoBehaviour
{
    protected ResourcesManagerAlly resourceManager;
    protected UnityEngine.UI.Button myButton;

    private void Awake()
    {
        resourceManager = FindObjectOfType<ResourcesManagerAlly>();
        myButton = GetComponent<UnityEngine.UI.Button>();
    }

    public virtual void Start()
    {
        EventManager.Instance.onCost.AddListener(SetButton);
        EventManager.Instance.onCost.Invoke();
    }

    public abstract void SetButton();

    public virtual void ShowTooltip(bool value)
    {
        UIManager.instance.ShowTooltip(value);
        UIManager.instance.tooltip.PlaceTool(GetComponent<RectTransform>());
    }
}
