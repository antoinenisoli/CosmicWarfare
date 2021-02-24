using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UnitAnimation : MonoBehaviour
{
    [SerializeField] Transform shootPoint;
    [SerializeField] GameObject deadSoldier;
    Unit unit;

    private void Awake()
    {
        unit = GetComponentInParent<Unit>();
    }

    public void ShootFX()
    {
        if (unit.lastTarget)
        {
            unit.lastTarget.ModifyLife(-unit.UnitInfo.firePower, unit.lastTarget.MeshClosestPoint(unit.meshRender.transform.position));
            VFXManager.Instance.InvokeVFX(FX_Event.ShootLaser, shootPoint.position, shootPoint.rotation);
        }
    }

    public void StartDissolve()
    {
        GameObject corpse = Instantiate(deadSoldier, transform.position, transform.rotation, transform);
        corpse.GetComponent<DeadUnit>().Create(unit);
        Destroy(unit.gameObject);
    }
}
