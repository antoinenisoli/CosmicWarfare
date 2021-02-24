using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitEnemy : Unit
{
    [Header("Unit Enemy")]
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float detectionRadius = 5;
    [SerializeField] Collider[] detectedEnemies = new Collider[0];
    [SerializeField] List<Entity> allTargets = new List<Entity>();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    public void DetectTargets()
    {
        if (currentState == UnitState.IsUnactive)
        {
            detectedEnemies = Physics.OverlapSphere(transform.position, detectionRadius, playerLayer);
            foreach (var enemy in detectedEnemies)
            {
                Entity detectedEntity = enemy.GetComponentInParent<Entity>();
                if (detectedEntity && !allTargets.Contains(detectedEntity) && detectedEntity.myTeam != myTeam)
                    allTargets.Add(detectedEntity);
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
                        Attack(lastTarget);
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
            meshRender.material = highlighted ? HighlightMat : baseMat;

        DetectTargets();
        base.Update();
    }
}
