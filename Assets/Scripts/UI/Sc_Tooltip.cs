using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sc_Tooltip : MonoBehaviour
{
    Text myText => GetComponentInChildren<Text>();

    public void TypeText(string description)
    {
        myText.text = description;
    }

    public void PlaceTool(RectTransform target)
    {
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.position = target.position;
    }
}
