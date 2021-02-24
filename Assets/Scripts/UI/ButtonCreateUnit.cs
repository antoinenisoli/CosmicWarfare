using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ButtonCreateUnit : Button
{
    BuildingCasern Casern => UIManager.instance.selectedCasern;
    [SerializeField] int index;

    public void InvokeCasern()
    {
        Casern.StartUnitProduction(index, Team.Player);
        Image block = myButton.transform.parent.GetChild(1).GetComponent<Image>();
        Vector3 baseScale = block.transform.localScale;

        block.fillAmount = 1;
        block.transform.localScale = Vector3.one * 0.1f;
        block.transform.DOScale(baseScale, 0.2f);
        block.DOFillAmount(0, Casern.unitsToCreate[index].creationDelay);
        foreach (var item in Casern.unitsToCreate[index].costs)
            resourceManager.ModifyValue(item.value, item.resourceType);

        EventManager.Instance.onCost.Invoke();
    }

    public override void ShowTooltip(bool value)
    {
        base.ShowTooltip(value);
        UIManager.instance.tooltip.TypeText(Casern.unitsToCreate[index].ToString());
    }

    public override void SetButton()
    {
        myButton.interactable = Casern && resourceManager.CanPay(Casern.unitsToCreate[index].costs) && !Casern.busy;
    }
}
