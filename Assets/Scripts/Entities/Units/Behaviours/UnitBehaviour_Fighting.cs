using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitBehaviour_Fighting : UnitBehaviour
{
    public override UnitState currentState => State;
    public static UnitState State => UnitState.IsFighting;
    float shootTimer;    

    public UnitBehaviour_Fighting(Sc_Unit unit, Sc_UnitInfo info) : base(unit)
    {
        this.unit = unit;
        this.info = info;
    }

    public override void Execute()
    {
        if (unit.lastTarget.health.isDead || unit.lastTarget == null)
        {
            unit.currentState = UnitState.IsUnactive;
            return;
        }

        if (Vector3.Distance(unit.transform.position, unit.attackPosition) < info.shootRange)
        {
            Vector3 _direction = (unit.attackPosition - unit.transform.position).normalized;
            Quaternion _lookRotation = Quaternion.LookRotation(_direction);
            unit.transform.rotation = _lookRotation;
            shootTimer += Time.deltaTime;
            unit.agent.isStopped = true;
            if (shootTimer > info.shootRate)
            {
                shootTimer = 0;
                unit.lastTarget.ModifyLife(-info.firePower, unit.attackPosition);
                Sc_VFXManager.Instance.InvokeVFX(FX_Event.ShootLaser, unit.transform.position, unit.transform.rotation);
            }
        }
        else
        {
            shootTimer = 0;
            unit.Attack(unit.lastTarget);
        }
    }
}


