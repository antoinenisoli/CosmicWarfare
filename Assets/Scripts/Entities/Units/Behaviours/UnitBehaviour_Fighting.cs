using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UnitBehaviour_Fighting : UnitBehaviour
{
    public override UnitState thisState => UnitState.IsFighting;
    [SerializeField] float shootSpeed = 1.5f;
    [SerializeField] protected float firePower = 3;

    public override void Execute(Sc_Unit unit)
    {
        if (unit.lastTarget.health.isDead || unit.lastTarget == null)
        {
            unit.currentState = UnitState.IsUnactive;
            return;
        }

        if (Vector3.Distance(unit.transform.position, unit.attackPosition) < unit.shootRange)
        {
            Vector3 _direction = (unit.attackPosition - unit.transform.position).normalized;
            Quaternion _lookRotation = Quaternion.LookRotation(_direction);
            unit.transform.rotation = _lookRotation;

            unit.shootTimer += Time.deltaTime;
            unit.agent.isStopped = true;
            if (unit.shootTimer > shootSpeed)
            {
                unit.shootTimer = 0;
                unit.lastTarget.ModifyLife(-firePower, unit.attackPosition);
                Sc_VFXManager.Instance.InvokeVFX(FX_Event.ShootLaser, unit.transform.position, unit.transform.rotation);
            }
        }
        else
        {
            unit.shootTimer = 0;
            unit.Attack(unit.lastTarget);
        }
    }
}
