using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Turret : Sc_Building
{
    [SerializeField] LayerMask enemyMask;
    [SerializeField] float detectionRadius;

    public override void UseBuilding()
    {
        Collider[] detectedColliders = Physics.OverlapSphere(transform.position, detectionRadius, enemyMask);
    }
}
