using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Sc_Unit : Sc_Entity
{
    [HideInInspector] public NavMeshAgent agent;

    [Header("Selection")]    
    [SerializeField] protected Material HighlightMat;
    public LayerMask ground = 1 >> 2;

    [Header("Fight")]
    public float shootRange = 5;   
    [HideInInspector] public float shootTimer;
    public Sc_Entity lastTarget;
    public Vector3 attackPosition;

    [Header("Behaviour")]
    public UnitState currentState = UnitState.IsUnactive;
    [SerializeField] UnitBehaviour_Fighting Fighting = new UnitBehaviour_Fighting();
    [SerializeField] UnitBehaviour_Moving Moving = new UnitBehaviour_Moving();
    [SerializeField] UnitBehaviour_Attacking Attacking = new UnitBehaviour_Attacking();
    Dictionary<UnitState, UnitBehaviour> behaviours = new Dictionary<UnitState, UnitBehaviour>();

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        baseMat = meshRender.material;
        behaviours.Add(Attacking.thisState, Attacking);
        behaviours.Add(Moving.thisState, Moving);
        behaviours.Add(Fighting.thisState, Fighting);
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
        {
            currentState = UnitState.IsUnactive;
            return;
        }

        base.Update();
        Behavior();
    }
}
