using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

enum Detectables
{
    Ground,
    Units,
    Buildings,
    None,
}

public class Sc_Selection : MonoBehaviour
{
    Camera mainCam => Camera.main;

    List<Sc_UnitAlly> allPlayerUnits = new List<Sc_UnitAlly>();
    List<Sc_UnitEnemy> allEnemyUnits = new List<Sc_UnitEnemy>();

    [Header("Select units")]
    [SerializeField] LayerMask unitLayer;
    [SerializeField] LayerMask buildingsLayer;
    [SerializeField] Detectables isDetecting;
    RaycastHit hit;

    [Header("Rectangle selection")]
    [SerializeField] Color textureColor = Color.white;
    [SerializeField] List<Sc_UnitAlly> selectedUnits = new List<Sc_UnitAlly>();
    Rect selectRect;
    Vector3 mousePos;

    [Header("Move units")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject moveMark;

    private void Awake()
    {
        allPlayerUnits = FindObjectsOfType<Sc_UnitAlly>().ToList();
        allEnemyUnits = FindObjectsOfType<Sc_UnitEnemy>().ToList();
    }

    void MoveUnits()
    {
        if (Input.GetMouseButtonDown(1) && selectedUnits.Count > 0 && isDetecting == Detectables.Ground)
        {
            bool validPath = false;
            Vector3 position = hit.point;
            foreach (var unit in selectedUnits)
            {
                if (unit.selected)
                    unit.MoveTo(position, out validPath);
            }

            if (validPath)
                Instantiate(moveMark, position + Vector3.up * 0.2f, Quaternion.identity);
        }
    }

    void SelectUnits()
    {
        foreach (var ally in allPlayerUnits)
        {
            ally.highlighted = 
                (hit.collider != null 
                && ally.Equals(hit.collider.gameObject.GetComponentInParent<Sc_UnitAlly>()) 
                && !ally.selected 
                && isDetecting == Detectables.Units) 
                || selectedUnits.Contains(ally);
        }

        foreach (var enemy in allEnemyUnits)
        {
            enemy.highlighted = hit.collider != null && selectedUnits.Count > 0 && enemy.Equals(hit.collider.gameObject.GetComponentInParent<Sc_UnitEnemy>());
        }

        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < allPlayerUnits.Count; i++)
            {
                allPlayerUnits[i].Select(false);
                selectedUnits.Clear();
            }

            if (isDetecting == Detectables.Units)
            {
                Sc_UnitAlly thisUnit = hit.collider.GetComponentInParent<Sc_UnitAlly>();
                if (!selectedUnits.Contains(thisUnit))
                {
                    thisUnit.Select(true);
                    selectedUnits.Add(thisUnit);
                }
            }
        }
    }

    void InteractUnits()
    {
        if (Input.GetMouseButtonDown(1) && (isDetecting == Detectables.Units || isDetecting == Detectables.Buildings)) //attack other units
        {
            Sc_DestroyableEntity target = hit.collider.GetComponentInParent<Sc_DestroyableEntity>();
            if (target)
            {
                foreach (var unit in selectedUnits)
                {
                    unit.Attack(target);
                }
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
            if (selectRect.size.y < 2 && selectRect.size.y < 2)
                return;

            foreach (var unit in allPlayerUnits)
            {
                if (unit == null)
                {
                    allPlayerUnits.Remove(unit);
                    return;
                }

                Vector3 unitPos = mainCam.WorldToScreenPoint(unit.transform.position);
                unitPos.y = Screen.height - unitPos.y;

                if (selectRect.Contains(unitPos))
                {
                    if (!selectedUnits.Contains(unit))
                        selectedUnits.Add(unit);
                }
                else
                {
                    if (selectedUnits.Contains(unit))
                        selectedUnits.Remove(unit);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {            
            selectRect = new Rect();

            if (selectedUnits.Count <= 0)
                return;

            for (int i = 0; i < selectedUnits.Count; i++)
            {
                selectedUnits[i].Select(true);
            }
        }
    }

    void DetectItems()
    {
        Ray screenPoint = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(screenPoint, out hit, Mathf.Infinity, unitLayer))
            isDetecting = Detectables.Units;
        else if (Physics.Raycast(screenPoint, out hit, Mathf.Infinity, groundLayer))
            isDetecting = Detectables.Ground;
        else if (Physics.Raycast(screenPoint, out hit, Mathf.Infinity, buildingsLayer))
            isDetecting = Detectables.Buildings;
        else
            isDetecting = Detectables.None;
    }

    private void Update()
    {
        DetectItems();
        SelectInBox();
        SelectUnits();
        InteractUnits();
        MoveUnits();
    }
}
