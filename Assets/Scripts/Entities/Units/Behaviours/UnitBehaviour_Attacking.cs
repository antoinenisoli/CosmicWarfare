using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBehaviour_Attacking : UnitBehaviour
{
    public override UnitState thisState => UnitState.IsAttacking;
    public override void Execute(Sc_Unit unit)
    {
        if (Vector3.Distance(unit.transform.position, unit.attackPosition) < unit.shootRange)
        {
            unit.agent.isStopped = true;
            unit.currentState = UnitState.IsFighting;
        }
        else
        {
            unit.agent.isStopped = false;
            if (unit.lastTarget != null)
                unit.agent.SetDestination(unit.lastTarget.transform.position);
        }
    }
}
