using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sc_UIManager : MonoBehaviour
{
    public static Sc_UIManager instance;
    Sc_Selection selectionManager => FindObjectOfType<Sc_Selection>();

    [SerializeField] Text displaySelection;
    public Sc_Tooltip tooltip;

    [SerializeField] GameObject unitsPanel;
    public Sc_Casern selectedCasern;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void UseSelectedBuilding()
    {
        bool isCasern = selectionManager.selectedBuilding && selectionManager.selectedBuilding.GetType() == typeof(Sc_Casern);
        unitsPanel.SetActive(isCasern);

        if (selectionManager.selectedBuilding)
        {
            displaySelection.text = selectionManager.selectedBuilding.ToString();

            if (isCasern && selectedCasern != selectionManager.selectedBuilding.GetComponent<Sc_Casern>())
            {
                displaySelection.text = selectionManager.selectedBuilding.ToString();
                selectedCasern = selectionManager.selectedBuilding.GetComponent<Sc_Casern>();
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

    public void ShowTooltip(bool show)
    {
        tooltip.gameObject.SetActive(show);
    }

    private void Update()
    {
        UseSelectedBuilding();
    }
}
