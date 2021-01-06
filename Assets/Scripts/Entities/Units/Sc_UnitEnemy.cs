using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_UnitEnemy : Sc_Unit
{
    [Header("Unit Enemy")]
    [SerializeField] LayerMask unitLayer;
    [SerializeField] float detectionRadius = 5;
    [SerializeField] List<Sc_UnitAlly> allTargets = new List<Sc_UnitAlly>();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void DetectTargets()
    {
        if (currentState == UnitState.IsUnactive)
        {
            Collider[] detectedEnemies = Physics.OverlapSphere(transform.position, detectionRadius, unitLayer);
            foreach (var enemy in detectedEnemies)
            {
                Sc_UnitAlly playerUnit = enemy.GetComponentInParent<Sc_UnitAlly>();
                if (playerUnit && !allTargets.Contains(playerUnit))
                {
                    allTargets.Add(playerUnit);
                }
            }

            float maxDistance = Mathf.Infinity;
            if (allTargets.Count > 0)
            {
                foreach (var target in allTargets)
                {
                    if (target == null)
                    {
                        allTargets.Remove(target);
                        return;
                    }

                    float distanceTo = Vector3.Distance(target.transform.position, transform.position);
                    if (distanceTo < maxDistance)
                    {
                        maxDistance = distanceTo;
                        lastTarget = target;
                        Attack(lastTarget, lastTarget.transform.position);
                    }
                }
            }
        }

        if (lastTarget)
            Debug.DrawLine(transform.position, lastTarget.transform.position);
    }

    public override void Update()
    {
        if (HighlightMat != null)
            mr.material = highlighted ? HighlightMat : baseMat;

        DetectTargets();
        base.Update();
    }
}
