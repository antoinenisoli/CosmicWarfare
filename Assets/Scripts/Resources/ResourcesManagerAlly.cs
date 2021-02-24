using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ResourcesManagerAlly : ResourcesManager
{
    [SerializeField] GameObject floatingText;
    [SerializeField] float animDuration = 4f;

    public override void Start()
    {
        base.Start();
        ActualizeText();
    }

    public void CreateFloatingText(int amount, ResourceType res, Vector3 position)
    {
        GameObject txt = Instantiate(floatingText, resources[(int)res].displayResource.transform);
        RectTransform txtRect = txt.GetComponent<RectTransform>();
        txtRect.position = position;
        txtRect.DOMoveY(txtRect.position.y + 30, animDuration);
        Text thisText = txt.GetComponent<Text>();
        thisText.DOFade(0, animDuration);

        thisText.text = amount < 0 ? "" + amount : "+" + amount;
        thisText.color = amount < 0 ? Color.red : Color.green;
        Destroy(txt, animDuration);
        ActualizeText();
    }

    public override void ModifyValue(int amount, ResourceType res)
    {
        if (amount == 0)
            return;

        Vector3 offsetPosition = resources[(int)res].displayResource.transform.position + Vector3.up * 25;
        CreateFloatingText(amount, res, offsetPosition);
        ActualizeText();
    }

    void ActualizeText()
    {
        for (int i = 0; i < resources.Length; i++)
            resources[i].displayResource.text = resources[i].type + " : " + resources[i].CurrentAmount;
    }
}
