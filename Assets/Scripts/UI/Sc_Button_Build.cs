using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Button_Build : Sc_Button
{
    Sc_GlobalBuilder builder => FindObjectOfType<Sc_GlobalBuilder>();
    [SerializeField] Purchase myPurchase;

    private void Start()
    {
        Sc_EventManager.Instance.onCost.AddListener(SetButton);
        Sc_EventManager.Instance.onCost.Invoke();
    }

    public void SelectThisBuilding()
    {
        builder.SelectBuilding(myPurchase.prefab, myPurchase);
    }

    public override void SetButton()
    {
        myButton.interactable = resourceManager.CanPay(myPurchase.costs);
    }

    public override void ShowTooltip(bool value)
    {
        base.ShowTooltip(value);
        Sc_UIManager.instance.tooltip.TypeText(myPurchase.ToString());
    }
}
