using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sc_UnitAlly : Sc_Unit
{
    [Header("Unit ally")]
    [SerializeField] protected Material SelectedMat;
    public bool selected;

    public void MoveTo(Vector3 pos)
    {
        agent.isStopped = false;
        currentState = UnitState.IsMoving;
        agent.SetDestination(pos);
    }

    public void Select(bool select)
    {
        selected = select;
    }

    public override void Update()
    {
        if (HighlightMat != null && SelectedMat != null)
            mr.material = selected ? HighlightMat : baseMat;

        base.Update();
    }
}
