using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Sc_Unit : MonoBehaviour
{
    NavMeshAgent agent => GetComponent<NavMeshAgent>();
    MeshRenderer mr => GetComponentInChildren<MeshRenderer>();

    [SerializeField] bool selected;
    [SerializeField] Material highlight;
    Material baseMat;

    private void Awake()
    {
        baseMat = mr.material;
    }

    public void Select(bool select)
    {
        selected = select;
    }

    public void MoveTo(Vector3 pos)
    {
        agent.SetDestination(pos);
    }

    private void Update()
    {
        mr.material = selected ? highlight : baseMat;
    }
}
