using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Casern : Sc_Building
{
    public override BuildingType type => BuildingType.Casern;

    [Header("Casern")]
    [SerializeField] Transform spawnDestination;
    [SerializeField] Transform spawnTransform;
    public Purchase[] unitsToCreate;

    public void StartUnitProduction(int index, Team unitTeam)
    {
        StartCoroutine(Production(index, unitTeam));
    }

    public IEnumerator Production(int index, Team unitTeam)
    {
        busy = true;
        yield return new WaitForSeconds(unitsToCreate[index].creationDelay);
        GameObject newUnit = Instantiate(unitsToCreate[index].prefab, spawnTransform.position, Quaternion.identity);
        switch (unitTeam)
        {
            case Team.Player:
                Sc_UnitAlly playerUnit = newUnit.GetComponent<Sc_UnitAlly>();
                playerUnit.MoveTo(spawnDestination.position);
                Sc_SelectionManager.GenerateEntity(playerUnit);
                Sc_VFXManager.Instance.InvokeVFX(FX_Event.NewAlly, spawnTransform.position, Quaternion.identity);
                break;

            case Team.Enemy:
                Sc_UnitEnemy enemyUnit = newUnit.GetComponent<Sc_UnitEnemy>();
                enemyUnit.MoveTo(spawnDestination.position);
                Sc_SelectionManager.GenerateEntity(enemyUnit);
                Sc_VFXManager.Instance.InvokeVFX(FX_Event.NewEnemy, spawnTransform.position, Quaternion.identity);
                break;
        }

        busy = false;
        Sc_EventManager.Instance.onCost.Invoke();
    }

    public override void UseBuilding()
    {
        
    }
}
