using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[Serializable]
public class UnitBehaviour_Moving : UnitBehaviour
{
    public override UnitState thisState => UnitState.IsMoving;
    [SerializeField] float stopDistance = 3.5f;
    Sc_Unit unit;

    public override void Execute(Sc_Unit unit)
    {
        this.unit = unit;
        if (Vector3.Distance(unit.transform.position, unit.agent.destination) < stopDistance || !NavMesh.SamplePosition(unit.agent.destination, out _, 1.0f, NavMesh.AllAreas))
        {
            unit.agent.isStopped = true;
            unit.currentState = UnitState.IsUnactive;
        }
    }

    public override string ToString()
    {
        return "" + Vector3.Distance(unit.transform.position, unit.agent.destination);
    }
}
