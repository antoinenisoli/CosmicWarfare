using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using DG.Tweening;

enum Detectables
{
    Ground,
    HoverUnit,
    Buildings,
    None,
}

public class Sc_SelectionManager : MonoBehaviour
{
    Camera mainCam;
    bool stopGame;
    [SerializeField] List<Sc_UnitAlly> allPlayerUnits = new List<Sc_UnitAlly>();
    [SerializeField] List<Sc_UnitEnemy> allEnemyUnits = new List<Sc_UnitEnemy>();
    [SerializeField] List<Sc_Building> allBuildings = new List<Sc_Building>();

    [Header("Select")]
    [SerializeField] MouseState mouseState;
    [SerializeField] LayerMask unitLayer;
    [SerializeField] LayerMask buildingsLayer;
    [SerializeField] Detectables isDetecting;
    public Sc_Building selectedBuilding;
    public RaycastHit hit;
    bool invalid, attackUnit;

    [Header("Rectangle selection")]
    [SerializeField] Color textureColor = Color.white;
    public List<Sc_UnitAlly> selectedUnits = new List<Sc_UnitAlly>();
    Rect selectRect;
    Vector3 mousePos;

    [Header("Move units")]
    [SerializeField] LayerMask groundLayer;
    [SerializeField] FormationType formationType;
    UnitsFormation formation = new UnitsFormation();
    public GameObject moveMark;
    public float dummyAnimDuration = 3;

    private void Awake()
    {
        mainCam = Camera.main;
        allPlayerUnits = FindObjectsOfType<Sc_UnitAlly>().ToList();
        allEnemyUnits = FindObjectsOfType<Sc_UnitEnemy>().ToList();
        allBuildings = FindObjectsOfType<Sc_Building>().ToList();
    }

    private void Start()
    {
        Sc_EventManager.Instance.onNewUnit.AddListener(NewEntity);
        Sc_EventManager.Instance.onEndGame.AddListener(StopGame);
    }

    void StopGame(bool b)
    {
        stopGame = true;
    }

    public static void GenerateEntity(Sc_Entity _new)
    {
        Sc_EventManager.Instance.onNewUnit.Invoke(_new);
    }

    public void NewEntity(Sc_Entity entity)
    {
        if (entity.GetComponent<Sc_Building>())
            allBuildings.Add(entity as Sc_Building);
        else if (entity.GetComponent<Sc_UnitAlly>())
            allPlayerUnits.Add(entity as Sc_UnitAlly);
        else if (entity.GetComponent<Sc_UnitEnemy>())
            allEnemyUnits.Add(entity as Sc_UnitEnemy);
    }

    void MoveUnits()
    {
        if (hit.transform != null)
        {
            Vector3 position = hit.point;
            invalid = !NavMesh.SamplePosition(position, out _, 5f, NavMesh.AllAreas) && selectedUnits.Count > 0;
        }
        else
            invalid = false;

        if (selectedUnits.Count > 0 && isDetecting == Detectables.Ground)
        {
            Vector3 position = hit.point;           
            if (Input.GetMouseButtonDown(1) && !invalid)
                formation.DoFormation(this, position);
        }
    }

    void SelectUnits()
    {
        foreach (var ally in allPlayerUnits)
        {
            if (hit.collider != null)
            {
                Sc_UnitAlly unit = hit.collider.gameObject.GetComponentInParent<Sc_UnitAlly>();
                ally.highlighted =
                (
                hit.collider != null
                && !EventSystem.current.IsPointerOverGameObject()
                && ally.Equals(unit)
                && !ally.selected
                )
                || selectedUnits.Contains(ally)
                ;
            }
            else
                ally.highlighted = false;
        }

        foreach (var enemy in allEnemyUnits)
        {
            if (hit.collider != null)
            {
                Sc_UnitEnemy unit = hit.collider.gameObject.GetComponentInParent<Sc_UnitEnemy>();
                enemy.highlighted =
                    hit.collider != null
                    && selectedUnits.Count > 0
                    && enemy.Equals(unit)
                    && !EventSystem.current.IsPointerOverGameObject()
                    ;
            }
            else
                enemy.highlighted = false;
        }

        foreach (var building in allBuildings)
        {
            if (hit.collider != null)
            {
                Sc_Building buildScript = hit.collider.gameObject.GetComponentInParent<Sc_Building>();
                bool hover = building.Equals(buildScript) && !building.selected && !EventSystem.current.IsPointerOverGameObject();
                bool teamCondition = true;

                if (building.myTeam == Team.Player)
                    teamCondition = building.currentState == BuildingState.Builded;
                else
                    teamCondition = selectedUnits.Count > 0;

                building.highlighted = hover && teamCondition;
            }
            else
                building.highlighted = false;
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            for (int i = 0; i < allPlayerUnits.Count; i++)
            {
                allPlayerUnits[i].Select(false);
                selectedUnits.Clear();
            }

            if (selectedBuilding)
            {
                selectedBuilding.SelectMe(false);
                selectedBuilding = null;
            }

            if (isDetecting == Detectables.HoverUnit) //select a unit
            {
                Sc_UnitAlly thisUnit = hit.collider.GetComponentInParent<Sc_UnitAlly>();
                if (thisUnit && !selectedUnits.Contains(thisUnit))
                {
                    thisUnit.Select(true);
                    selectedUnits.Add(thisUnit);
                }
            }
            else if (isDetecting == Detectables.Buildings) //select a building
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
        if (hit.transform != null)
        {
            Sc_Entity target = hit.collider.GetComponentInParent<Sc_Entity>();
            attackUnit = selectedUnits.Count > 0 && target && target.myTeam == Team.Enemy;
            if (attackUnit)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    foreach (var unit in selectedUnits)
                        unit.Attack(target);

                    CreateDummy(target.transform.position, Quaternion.identity, Color.red);
                }
            }
        }
        else
            attackUnit = false;
    }

    public void CreateDummy(Vector3 position, Quaternion rotation, Color color)
    {
        GameObject dummy = Instantiate(moveMark, position + Vector3.up * 0.2f, rotation);
        dummy.GetComponent<MeshRenderer>().material.color = color;
        Vector3 currentScale = dummy.transform.localScale;
        dummy.transform.DOScale(currentScale * 1.5f, dummyAnimDuration / 3);
        dummy.transform.DOScale(Vector3.zero, dummyAnimDuration / 3).SetDelay(dummyAnimDuration / 3);
        Destroy(dummy, dummyAnimDuration);
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
                mousePos = Input.mousePosition;

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
            isDetecting = Detectables.HoverUnit;
        else if (Physics.Raycast(screenPoint, out hit, Mathf.Infinity, buildingsLayer))
            isDetecting = Detectables.Buildings;
        else if (Physics.Raycast(screenPoint, out hit, Mathf.Infinity, groundLayer))
            isDetecting = Detectables.Ground;
        else
            isDetecting = Detectables.None;
    }

    void ManageCursor()
    {
        if (attackUnit)
            Sc_CursorManager.instance.currentState = MouseState.Attack;
        else if (invalid)
            Sc_CursorManager.instance.currentState = MouseState.Invalid;
        else
            Sc_CursorManager.instance.currentState = MouseState.Valid;
    }

    private void Update()
    {
        if (stopGame)
            return;

        DetectItems();
        SelectInBox();
        SelectUnits();
        ManageCursor();
        InteractUnits();
        MoveUnits();
    }
}
