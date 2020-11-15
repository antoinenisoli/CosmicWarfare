using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_UnitEnemy : Sc_Unit
{
    [Header("Unit Enemy")]
    [SerializeField] LayerMask unitLayer;
    [SerializeField] Sc_UnitAlly currentEnemy;
    [SerializeField] float detectionRadius = 5;
    [SerializeField] List<Sc_UnitAlly> allEnemies = new List<Sc_UnitAlly>();

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
                Sc_UnitAlly ally = enemy.GetComponentInParent<Sc_UnitAlly>();
                if (ally && !allEnemies.Contains(ally))
                {
                    allEnemies.Add(ally);
                }
            }

            float maxDistance = Mathf.Infinity;
            if (allEnemies.Count > 0)
            {
                foreach (var enemy in allEnemies)
                {
                    if (enemy == null)
                    {
                        allEnemies.Remove(enemy);
                        return;
                    }

                    float distanceTo = Vector3.Distance(enemy.transform.position, transform.position);
                    if (distanceTo < maxDistance)
                    {
                        maxDistance = distanceTo;
                        currentEnemy = enemy;
                        Attack(currentEnemy);
                    }
                }
            }
        }

        if (currentEnemy)
            Debug.DrawLine(transform.position, currentEnemy.transform.position);
    }

    public override void Update()
    {
        if (HighlightMat != null)
            mr.material = highlighted ? HighlightMat : baseMat;

        DetectTargets();
        base.Update();
    }
}
