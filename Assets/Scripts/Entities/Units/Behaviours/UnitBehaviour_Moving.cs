using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class UnitBehaviour_Moving : UnitBehaviour
{
    public override UnitState currentState => State;
    public static UnitState State => UnitState.IsMoving;

    public UnitBehaviour_Moving(Unit unit) : base(unit)
    {
        this.unit = unit;
    }

    public override void Execute()
    {
        if (Vector3.Distance(unit.transform.position, unit.agent.destination) < unit.stopDistance || !NavMesh.SamplePosition(unit.agent.destination, out _, 1.0f, NavMesh.AllAreas))
        {
            unit.agent.isStopped = true;
            unit.SetState(UnitState.IsUnactive);
        }
    }
}
