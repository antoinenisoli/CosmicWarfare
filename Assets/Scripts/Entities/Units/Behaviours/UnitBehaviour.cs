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
    public virtual UnitState thisState => UnitState.IsUnactive;
    public abstract void Execute(Sc_Unit unit);
}
