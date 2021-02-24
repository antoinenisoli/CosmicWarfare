using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBuild : Button
{
    GlobalBuilder builder;
    [SerializeField] Purchase myPurchase;

    public override void Start()
    {
        base.Start();
        builder = FindObjectOfType<GlobalBuilder>();
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
        UIManager.instance.tooltip.TypeText(myPurchase.ToString());
    }
}
