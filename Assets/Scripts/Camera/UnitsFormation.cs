using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;

public enum FormationType
{
    Rectangle,
}

[Serializable]
public class UnitsFormation : IUnitFormation
{
    public void DoFormation(SelectionManager thisSelection, Vector3 targetPosition)
    {
        Vector3 pos = targetPosition;
        int counter = -1;
        int xIncrement = -1;

        float xOffset = 10;
        float yOffset = 10;
        float sqrt = Mathf.Sqrt(10);
        float startX = targetPosition.x;

        for (int i = 0; i < thisSelection.selectedUnits.Count; i++)
        {
            UnitAlly unit = thisSelection.selectedUnits[i].GetComponent<UnitAlly>();
            counter++;
            xIncrement++;
            if (xIncrement > 1)
                xIncrement = 1;

            pos.x += xIncrement * xOffset;
            if (counter == Mathf.Floor(sqrt))
            {
                counter = 0;
                pos.x = startX;
                pos.z += 1 + yOffset;
            }

            if (NavMesh.SamplePosition(pos, out NavMeshHit navHit, 10f, NavMesh.AllAreas))
            {
                if (unit.selected)
                    unit.MoveTo(pos);

                thisSelection.CreateDummy(navHit.position, Quaternion.FromToRotation(Vector3.up, thisSelection.hit.normal), Color.white);             
            }
        }
    }
}
