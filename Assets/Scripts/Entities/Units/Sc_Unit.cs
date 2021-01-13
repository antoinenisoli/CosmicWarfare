using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum UnitState
{
    IsUnactive,
    IsMoving,
    IsAttacking,
    IsFighting,
}

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Sc_Unit : Sc_Entity
{
    public NavMeshAgent agent => GetComponent<NavMeshAgent>();
    protected MeshRenderer mr => GetComponentInChildren<MeshRenderer>();

    [Header("Selection")]    
    [SerializeField] protected Material HighlightMat;

    [Header("Fight")]
    public float shootRange = 5;   
    [HideInInspector] public float shootTimer;
    public LayerMask ground = 1 >> 2;
    public Sc_Entity lastTarget;
    public Vector3 attackPosition;

    [Header("Behaviour")]
    public UnitState currentState = UnitState.IsUnactive;
    [SerializeField] UnitBehaviour_Fighting Fighting;
    [SerializeField] UnitBehaviour_Moving Moving;
    [SerializeField] UnitBehaviour_Attacking Attacking;
    Dictionary<UnitState, UnitBehaviour> behaviours = new Dictionary<UnitState, UnitBehaviour>();

    public void Awake()
    {
        baseMat = mr.material;
        behaviours.Add(Attacking.thisState, Attacking);
        behaviours.Add(Moving.thisState, Moving);
        behaviours.Add(Fighting.thisState, Fighting);
    }

    [ContextMenu("Place unit")]
    void Place()
    {
        bool closestGround = Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 10, ground);
        if (closestGround)
            transform.position = hit.point + Vector3.up * transform.lossyScale.y;
    }

    public override void ModifyLife(float amount, Vector3 damageLocation)
    {
        base.ModifyLife(amount, damageLocation);
        if (amount < 0)
        {
            Sc_VFXManager.Instance.InvokeVFX(FX_Event.LaserDamage, damageLocation, Quaternion.identity);
            Sc_VFXManager.Instance.InvokeVFX(FX_Event.UnitDamage, damageLocation, Quaternion.identity);
        }
    }

    public void Attack(Sc_Entity target)
    {
        lastTarget = target;
        currentState = UnitState.IsAttacking;
    }

    public void MoveTo(Vector3 pos)
    {
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(pos, path))
        {
            agent.isStopped = false;
            currentState = UnitState.IsMoving;
            agent.SetDestination(pos);
        }
    }

    public virtual void Behavior()
    {
        if (lastTarget)
        {
            if (lastTarget.GetComponent<Sc_Building>())
                attackPosition = (lastTarget as Sc_Building).MeshClosestPoint(transform.position);
            else
                attackPosition = (lastTarget as Sc_Unit).transform.position;
        }

        if (behaviours.TryGetValue(currentState, out UnitBehaviour behaviour))
            behaviour.Execute(this);
        else
            shootTimer = 0;
    }

    public override void Update()
    {
        if (health.isDead)
            return;

        base.Update();
        Behavior();
    }
}
