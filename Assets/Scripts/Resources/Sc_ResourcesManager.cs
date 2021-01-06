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

public class Sc_ResourcesManager : MonoBehaviour
{
    public Resource[] resources;
    [SerializeField] GameObject floatingText;
    [SerializeField] float animDuration = 4f;

    private void Start()
    {
        ActualizeText();
    }

    public void ModifyValue(int amount, ResourceType res)
    {
        if (amount == 0)
            return;

        GameObject txt = Instantiate(floatingText, resources[(int)res].displayResource.transform);
        RectTransform txtRect = txt.GetComponent<RectTransform>();
        txtRect.position = resources[(int)res].displayResource.transform.position + Vector3.up * 25;
        txtRect.DOMoveY(txtRect.position.y + 30, animDuration);
        Text thisText = txt.GetComponent<Text>();        
        thisText.DOFade(0, animDuration);

        if (amount < 0)
        {
            thisText.text = "" + amount;
            thisText.color = Color.red;
        }
        else
        {
            thisText.text = "+" + amount;
            thisText.color = Color.green;
        }

        Destroy(txt, animDuration);
        resources[(int)res].CurrentAmount += amount;
        ActualizeText();
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
        bool canPay = false;
        foreach (ResourceCost cost in costs)
        {
            Resource resource = GetResource(cost.resourceType);
            canPay = (resource.CurrentAmount + cost.value) >= 0;
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
