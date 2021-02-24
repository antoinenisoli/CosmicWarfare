using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class UnitBehaviour_Chasing : UnitBehaviour
{
    public override UnitState currentState => State;
    public static UnitState State => UnitState.IsChasing;

    public UnitBehaviour_Chasing(Unit unit, UnitInfo unitInfo) : base(unit)
    {
        this.unit = unit;
        this.unitInfo = unitInfo;
    }

    public override void Execute()
    {
        bool getPoint = NavMesh.SamplePosition(unit.attackPosition, out NavMeshHit hit, 100, NavMesh.AllAreas);
        if (Vector3.Distance(unit.transform.position, unit.attackPosition) < unitInfo.shootRange)
        {
            unit.agent.isStopped = true;
            unit.SetState(UnitState.IsFighting);
        }
        else if (getPoint)
        {
            unit.agent.isStopped = false;
            unit.agent.SetDestination(hit.position);
        }
    }
}
