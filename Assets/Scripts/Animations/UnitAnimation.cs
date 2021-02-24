using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    [SerializeField] Transform shootPoint;
    Unit unit;

    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
    }

    public void ShootFX()
    {
        unit.lastTarget.ModifyLife(-unit.UnitInfo.firePower, unit.lastTarget.MeshClosestPoint(unit.meshRender.transform.position));
        Sc_VFXManager.Instance.InvokeVFX(FX_Event.ShootLaser, shootPoint.position, shootPoint.rotation);
    }

    public void StartDissolve()
    {

    }
}
