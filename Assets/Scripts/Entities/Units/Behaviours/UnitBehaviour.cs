using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitState
{
    IsUnactive,
    IsMoving,
    IsChasing,
    IsFighting,
}

[Serializable]
public abstract class UnitBehaviour
{
    public virtual UnitState currentState => UnitState.IsUnactive; 
    protected Unit unit;
    protected UnitInfo unitInfo;

    public UnitBehaviour(Unit unit)
    {
        this.unit = unit;
    }

    public abstract void Execute();
}
