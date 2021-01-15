using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Sc_Unit : Sc_Entity
{
    [HideInInspector] public NavMeshAgent agent;

    [Header("UNIT")]
    [SerializeField] Sc_UnitInfo unitInfo;
    public UnitState currentState = UnitState.IsUnactive;
    public float stopDistance = 3.5f;
    [SerializeField] protected Material HighlightMat;
    public LayerMask ground = 1 >> 2;

    [Header("Fight")]
    public Sc_Entity lastTarget;
    [HideInInspector] public Vector3 attackPosition;
    Dictionary<UnitState, UnitBehaviour> behaviours = new Dictionary<UnitState, UnitBehaviour>();

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        baseMat = meshRender.material;
        behaviours.Add(UnitBehaviour_Attacking.State, new UnitBehaviour_Attacking(this, unitInfo));
        behaviours.Add(UnitBehaviour_Moving.State, new UnitBehaviour_Moving(this));
        behaviours.Add(UnitBehaviour_Fighting.State, new UnitBehaviour_Fighting(this, unitInfo));
    }

    public override void Start()
    {
        base.Start();
        Sc_EventManager.Instance.onEndGame.AddListener(Deactivate);
    }

    void Deactivate(bool b)
    {
        health.isDead = true;
        StopAllCoroutines();
        lastTarget = null;
        agent.isStopped = true;
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
        if (lastTarget.GetComponent<Sc_Building>())
            attackPosition = (lastTarget as Sc_Building).MeshClosestPoint(transform.position);
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
        if (lastTarget && lastTarget.GetComponent<Sc_Unit>())
        {
            attackPosition = (lastTarget as Sc_Unit).transform.position;
        }

        if (behaviours.TryGetValue(currentState, out UnitBehaviour behaviour))
            behaviour.Execute();
    }

    public override void Update()
    {
        if (health.isDead)
        {
            currentState = UnitState.IsUnactive;
            return;
        }

        base.Update();
        Behavior();
    }
}
