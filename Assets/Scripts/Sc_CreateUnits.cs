using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Sc_CreateUnits : MonoBehaviour
{
    Sc_Selection selectionManager => FindObjectOfType<Sc_Selection>();
    Sc_ResourcesManager resourceManager => FindObjectOfType<Sc_ResourcesManager>();
    Button[] buttons => GetComponentsInChildren<Button>();

    [SerializeField] GameObject showUnits;
    [SerializeField] Text displaySelection;
    public Sc_Casern casern;

    public void CallCasern(int i)
    {
        casern.StartCoroutine(casern.Create(i, casern.unitsToCreate[i].creationDelay));
        Image block = buttons[i].transform.parent.GetChild(1).GetComponent<Image>();
        Vector3 baseScale = block.transform.localScale;
        block.fillAmount = 1;
        block.transform.localScale = Vector3.one * 0.1f;
        block.transform.DOScale(baseScale, 0.2f);
        block.DOFillAmount(0, casern.unitsToCreate[i].creationDelay);
    }

    void UseSelectedBuilding()
    {
        bool isCasern = selectionManager.selectedBuilding && selectionManager.selectedBuilding.GetType() == typeof(Sc_Casern);
        showUnits.SetActive(isCasern);

        if (selectionManager.selectedBuilding)
        {
            displaySelection.text = selectionManager.selectedBuilding.ToString();

            if (isCasern && casern != selectionManager.selectedBuilding.GetComponent<Sc_Casern>())
            {
                displaySelection.text = selectionManager.selectedBuilding.ToString(); 
                casern = selectionManager.selectedBuilding.GetComponent<Sc_Casern>();
            }
        }
        else if (selectionManager.selectedUnits.Count > 0)
        {
            displaySelection.text = selectionManager.selectedUnits[0].ToString(); 
        }
        else
        {
            displaySelection.text = "";
        }
    }

    void CasernButtons()
    {
        if (casern)
        {
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].interactable = resourceManager.CanPay(casern.unitsToCreate[i].costs);
                buttons[i].transform.parent.GetChild(1).GetComponent<Image>().raycastTarget = casern.busy;
            }
        }
    }

    private void Update()
    {
        UseSelectedBuilding();
        CasernButtons();
    }
}
