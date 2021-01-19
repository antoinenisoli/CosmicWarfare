using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Sc_ResourcesManager_Ally : Sc_ResourcesManager
{
    [SerializeField] GameObject floatingText;
    [SerializeField] float animDuration = 4f;

    public override void Start()
    {
        base.Start();
        ActualizeText();
    }

    public override void ModifyValue(int amount, ResourceType res)
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
        base.ModifyValue(amount, res);
        ActualizeText();
    }

    void ActualizeText()
    {
        for (int i = 0; i < resources.Length; i++)
        {
            resources[i].displayResource.text = resources[i].type + " : " + resources[i].CurrentAmount;
        }
    }
}
