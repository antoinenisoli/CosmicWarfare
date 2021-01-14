﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Sc_UIManager : MonoBehaviour
{
    public static Sc_UIManager instance;
    Sc_SelectionManager selectionManager;
    public Sc_Tooltip tooltip;
    public Sc_Casern selectedCasern;

    [Header("Description panel")]
    [SerializeField] Text displaySelection;
    [SerializeField] GameObject unitsPanel;

    [Header("Game ending")]
    [SerializeField] CanvasGroup fadeScreen;
    [SerializeField] float fadeDuration = 5;
    [SerializeField] Text displayEnding;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        selectionManager = FindObjectOfType<Sc_SelectionManager>();
        fadeScreen.DOFade(0, 0);
        fadeScreen.GetComponentInChildren<Image>().raycastTarget = false;
        fadeScreen.blocksRaycasts = false;
    }

    private void Start()
    {
        Sc_EventManager.Instance.onEndGame.AddListener(EndGame);
    }

    public void Load(string name)
    {
        SceneManager.LoadScene(name);
    }

    void EndGame(bool victory)
    {
        fadeScreen.DOFade(0.5f, fadeDuration);
        fadeScreen.GetComponentInChildren<Image>().raycastTarget = true;
        fadeScreen.blocksRaycasts = true;

        if (victory)
        {
            displayEnding.text = "Victory !";
            displayEnding.color = Color.green;
        }
        else
        {
            displayEnding.text = "Defeat...";
            displayEnding.color = Color.red;
        }
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
