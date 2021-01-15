using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class UnitBehaviour_Attacking : UnitBehaviour
{
    public override UnitState currentState => State;
    public static UnitState State => UnitState.IsAttacking;

    public UnitBehaviour_Attacking(Sc_Unit unit, Sc_UnitInfo info) : base(unit)
    {
        this.unit = unit;
        this.info = info;
    }

    public override void Execute()
    {
        bool getPoint = NavMesh.SamplePosition(unit.attackPosition, out NavMeshHit hit, 100, NavMesh.AllAreas);
        if (Vector3.Distance(unit.transform.position, unit.attackPosition) < info.shootRange)
        {
            unit.agent.isStopped = true;
            unit.currentState = UnitState.IsFighting;
        }
        else if (getPoint)
        {
            unit.agent.isStopped = false;
            unit.agent.SetDestination(hit.position);
        }
    }
}
