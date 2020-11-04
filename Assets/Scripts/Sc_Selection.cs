using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Sc_Selection : MonoBehaviour
{
    Camera mainCam => GetComponent<Camera>();
    List<Sc_Unit> allUnits;

    [Header("Select units")]
    [SerializeField] LayerMask unitLayer;    
    [SerializeField] Sc_Unit selectedUnit;
    bool onUnit;

    [Header("Rectangle selection")]
    [SerializeField] Color textureColor = Color.white;
    Rect selectRect;
    Vector3 mousePos;

    [Header("Move units")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject moveMark;
    bool onGround;

    private void Awake()
    {
        allUnits = FindObjectsOfType<Sc_Unit>().ToList();
    }

    void MoveUnits()
    {
        onGround = Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, groundLayer);
        if (selectedUnit && onGround && Input.GetMouseButtonDown(1))
        {
            Vector3 position = hit.point;
            selectedUnit.MoveTo(position);
            moveMark.transform.position = position + Vector3.up * 0.2f;
        }
    }

    void SelectUnits()
    {
        onUnit = Physics.Raycast(mainCam.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, unitLayer);
        if (Input.GetMouseButtonDown(0))
        {
            if (onUnit)
            {
                if (selectedUnit)
                    selectedUnit.Select(false);

                selectedUnit = hit.collider.GetComponentInParent<Sc_Unit>();
                selectedUnit.Select(true);
            }
            else if (selectedUnit)
            {
                selectedUnit.Select(false);
                selectedUnit = null;
            }
        }
    }

    private void OnGUI()
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, textureColor);
        texture.Apply();
        GUI.DrawTexture(selectRect, texture);
    }

    void SelectInBox()
    {
        if (Input.GetMouseButton(0))
        {
            if (Input.GetMouseButtonDown(0))
            {
                mousePos = Input.mousePosition;
            }

            selectRect = new Rect(mousePos.x, Screen.height - mousePos.y, Input.mousePosition.x - mousePos.x, -1 * (Input.mousePosition.y - mousePos.y));
        }

        if (Input.GetMouseButtonUp(0))
        {
            foreach (var item in allUnits)
            {
                if (selectRect.Contains(mainCam.WorldToScreenPoint(item.transform.position)))
                {
                    item.Select(true);
                }
            }

            selectRect = new Rect();
        }
    }

    private void Update()
    {
        SelectInBox();
        SelectUnits();
        MoveUnits();
    }
}
