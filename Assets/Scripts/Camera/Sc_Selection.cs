using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

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

    [SerializeField] List<Sc_UnitAlly> allPlayerUnits = new List<Sc_UnitAlly>();
    [SerializeField] List<Sc_UnitEnemy> allEnemyUnits = new List<Sc_UnitEnemy>();
    [SerializeField] List<Sc_Building> allBuildings = new List<Sc_Building>();

    [Header("Select")]
    [SerializeField] LayerMask unitLayer;
    [SerializeField] LayerMask buildingsLayer;
    [SerializeField] Detectables isDetecting;
    public Sc_Building selectedBuilding;
    RaycastHit hit;

    [Header("Rectangle selection")]
    [SerializeField] Color textureColor = Color.white;
    public List<Sc_UnitAlly> selectedUnits = new List<Sc_UnitAlly>();
    Rect selectRect;
    Vector3 mousePos;

    [Header("Move units")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject moveMark;
    [SerializeField] float dummyAnimDuration = 3;

    private void Awake()
    {
        allPlayerUnits = FindObjectsOfType<Sc_UnitAlly>().ToList();
        allEnemyUnits = FindObjectsOfType<Sc_UnitEnemy>().ToList();
        allBuildings = FindObjectsOfType<Sc_Building>().ToList();
    }

    private void Start()
    {
        Sc_EventManager.Instance.onNewUnit.AddListener(NewEntity);
    }

    public static void GenerateEntity(Sc_DestroyableEntity _new)
    {
        Sc_EventManager.Instance.onNewUnit.Invoke(_new);
    }

    public void NewEntity(Sc_DestroyableEntity entity)
    {
        print(entity.GetComponent<Sc_Building>());
        if (entity.GetComponent<Sc_Building>())
            allBuildings.Add(entity as Sc_Building);
        else if (entity.GetComponent<Sc_Unit>())
        {
            switch ((entity as Sc_Unit).myTeam)
            {
                case Team.Player:
                    allPlayerUnits.Add(entity as Sc_UnitAlly);
                    break;
                case Team.Enemy:
                    allEnemyUnits.Add(entity as Sc_UnitEnemy);
                    break;
            }
        }
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
            {
                GameObject dummy = Instantiate(moveMark, position + Vector3.up * 0.4f, Quaternion.FromToRotation(Vector3.up, hit.normal));
                Vector3 currentScale = dummy.transform.localScale;
                dummy.transform.DOScale(currentScale * 1.5f, dummyAnimDuration / 3);
                dummy.transform.DOScale(Vector3.zero, dummyAnimDuration / 3).SetDelay(dummyAnimDuration / 3);
                Destroy(dummy, dummyAnimDuration);
            }
        }
    }

    public void CleanList()
    {
        void Clean<T>(List<T> list)
        {
            foreach (var item in list)
            {
                if (item == null)
                    list.Remove(item);
            }
        }

        Clean(allEnemyUnits);
        Clean(allBuildings);
        Clean(allPlayerUnits);
    }

    void SelectUnits()
    {
        foreach (var ally in allPlayerUnits)
        {
            ally.highlighted = 
                (
                hit.collider != null 
                && ally.Equals(hit.collider.gameObject.GetComponentInParent<Sc_UnitAlly>()) 
                && !ally.selected 
                ) 
                || selectedUnits.Contains(ally)
                ;
        }

        foreach (var building in allBuildings)
        {
            building.highlighted =
                hit.collider != null
                && building.Equals(hit.collider.gameObject.GetComponentInParent<Sc_Building>())
                && !building.selected
                && building.myTeam == Team.Player
                && building.currentState == BuildingState.Builded
                ;
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

            if (selectedBuilding && !EventSystem.current.IsPointerOverGameObject())
            {
                selectedBuilding.SelectMe(false);
                selectedBuilding = null;
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
            else if (isDetecting == Detectables.Buildings)
            {
                Sc_Building pointedBuilding = hit.collider.GetComponentInParent<Sc_Building>();
                if (pointedBuilding.currentState == BuildingState.Builded)
                {
                    selectedBuilding = pointedBuilding;
                    selectedBuilding.SelectMe(true);
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
                    unit.Attack(target, hit.collider.ClosestPoint(unit.transform.position));                   
                }

                GameObject dummy = Instantiate(moveMark, target.transform.position + Vector3.up * 0.2f, Quaternion.identity);
                dummy.GetComponent<MeshRenderer>().material.color = Color.red;
                Vector3 currentScale = dummy.transform.localScale;
                dummy.transform.DOScale(currentScale * 1.5f, dummyAnimDuration / 3);
                dummy.transform.DOScale(Vector3.zero, dummyAnimDuration / 3).SetDelay(dummyAnimDuration / 3);
                Destroy(dummy, dummyAnimDuration);
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
        else if (Physics.Raycast(screenPoint, out hit, Mathf.Infinity, buildingsLayer))
            isDetecting = Detectables.Buildings;
        else if (Physics.Raycast(screenPoint, out hit, Mathf.Infinity, groundLayer))
            isDetecting = Detectables.Ground;
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
