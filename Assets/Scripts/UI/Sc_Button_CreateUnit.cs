using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Sc_Button_CreateUnit : Sc_Button
{
    Sc_Casern casern => Sc_UIManager.instance.selectedCasern;
    [SerializeField] int index;

    public void InvokeCasern()
    {
        casern.StartCoroutine(casern.Create(index, casern.unitsToCreate[index].creationDelay));
        Image block = myButton.transform.parent.GetChild(1).GetComponent<Image>();
        Vector3 baseScale = block.transform.localScale;
        block.fillAmount = 1;
        block.transform.localScale = Vector3.one * 0.1f;
        block.transform.DOScale(baseScale, 0.2f);
        block.DOFillAmount(0, casern.unitsToCreate[index].creationDelay);
    }

    public override void ShowTooltip(bool value)
    {
        base.ShowTooltip(value);
        Sc_UIManager.instance.tooltip.TypeText(casern.unitsToCreate[index].ToString());
    }

    public override void SetButton()
    {
        if (casern)
        {
            myButton.interactable = resourceManager.CanPay(casern.unitsToCreate[index].costs);
            myButton.transform.parent.GetChild(1).GetComponent<Image>().raycastTarget = casern.busy;
        }
    }
}
