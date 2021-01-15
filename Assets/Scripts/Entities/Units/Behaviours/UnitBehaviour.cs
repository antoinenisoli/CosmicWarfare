using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitState
{
    IsUnactive,
    IsMoving,
    IsAttacking,
    IsFighting,
}

[Serializable]
public abstract class UnitBehaviour
{
    public virtual UnitState currentState => UnitState.IsUnactive; 
    protected Sc_Unit unit;
    protected Sc_UnitInfo info;

    public UnitBehaviour(Sc_Unit unit)
    {
        this.unit = unit;
    }

    public abstract void Execute();
}
