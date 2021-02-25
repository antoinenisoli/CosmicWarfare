using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingCasern : Building
{
    public override BuildingType buildingType => BuildingType.Casern;

    [Header("Casern")]
    [SerializeField] Transform spawnDestination;
    [SerializeField] Transform spawnTransform;

    public Purchase[] unitsToCreate;
    [HideInInspector] public bool[] Busies;

    public override void Start()
    {
        base.Start();
        Busies = new bool[unitsToCreate.Length];
    }

    public void StartUnitProduction(int index, Team unitTeam)
    {
        StartCoroutine(Production(index, unitTeam));
    }

    public bool CanPayUnit(int unitIndex)
    {
        return resourceManager.CanPay(unitsToCreate[unitIndex].costs);
    }

    public IEnumerator Production(int index, Team unitTeam)
    {
        Busies[index] = true;
        yield return new WaitForSeconds(unitsToCreate[index].creationDelay);
        SoundManager.instance.PlayAudio(AudioType.NewUnit.ToString(), transform);
        GameObject newUnit = Instantiate(unitsToCreate[index].prefab, spawnTransform.position, Quaternion.identity);
        switch (unitTeam)
        {
            case Team.Player:
                UnitAlly playerUnit = newUnit.GetComponent<UnitAlly>();
                playerUnit.MoveTo(spawnDestination.position);
                SelectionManager.GenerateEntity(playerUnit);
                VFXManager.Instance.InvokeVFX(FX_Event.NewAlly, spawnTransform.position, Quaternion.identity);
                break;

            case Team.Enemy:
                UnitEnemy enemyUnit = newUnit.GetComponent<UnitEnemy>();
                foreach (var cost in unitsToCreate[index].costs)
                {
                    resourceManager.ModifyValue(cost.value, cost.resourceType);
                }

                enemyUnit.MoveTo(spawnDestination.position);
                SelectionManager.GenerateEntity(enemyUnit);
                VFXManager.Instance.InvokeVFX(FX_Event.NewEnemy, spawnTransform.position, Quaternion.identity);
                break;
        }

        Busies[index] = false;
    }

    public override void UseBuilding()
    {
        
    }
}
