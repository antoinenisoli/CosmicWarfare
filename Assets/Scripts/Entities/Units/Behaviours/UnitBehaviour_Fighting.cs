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

    public UnitBehaviour_Fighting(Unit unit, Sc_UnitInfo unitInfo) : base(unit)
    {
        this.unit = unit;
        this.unitInfo = unitInfo;
        shootTimer = unitInfo.shootRate/2;
    }

    public override void Execute()
    {
        if (unit.lastTarget.health.isDead || unit.lastTarget == null)
        {
            unit.SetState(UnitState.IsUnactive);
            return;
        }

        if (Vector3.Distance(unit.transform.position, unit.attackPosition) < unitInfo.shootRange)
        {
            Vector3 _direction = (unit.attackPosition - unit.transform.position).normalized;
            Quaternion _lookRotation = Quaternion.LookRotation(_direction);
            unit.transform.rotation = _lookRotation;
            unit.meshRender.transform.rotation = _lookRotation;
            shootTimer += Time.deltaTime;
            unit.agent.isStopped = true;

            if (shootTimer > unitInfo.shootRate)
                Shoot();
        }
        else
        {
            shootTimer = 0;
            unit.Attack(unit.lastTarget);
        }
    }

    void Shoot()
    {
        shootTimer = 0;
        unit.Shoot();
    }
}


