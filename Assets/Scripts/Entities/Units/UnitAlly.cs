using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitAlly : Unit
{
    [Header("Unit ally")]
    [SerializeField] GameObject selectedVFX;
    public bool selected;

    public override void Start()
    {
        base.Start();
        Select(false);
    }

    public bool CorrectPath(Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        return agent.CalculatePath(target, path);
    }

    public void Select(bool select)
    {
        selected = select;
        selectedVFX.SetActive(select);
        if (select)
            selectedVFX.GetComponent<ParticleSystem>().Play();
    }
}
