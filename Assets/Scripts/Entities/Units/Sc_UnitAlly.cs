using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sc_UnitAlly : Sc_Unit
{
    [Header("Unit ally")]
    [SerializeField] protected Material SelectedMat;
    [SerializeField] GameObject selectedVFX;
    public bool selected;

    public bool CorrectPath(Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        return agent.CalculatePath(target, path);
    }

    public void Select(bool select)
    {
        selected = select;
    }

    public override void Update()
    {
        selectedVFX.SetActive(selected);
        if (HighlightMat != null && SelectedMat != null)
            meshRender.material = selected ? HighlightMat : baseMat;

        base.Update();
    }
}
