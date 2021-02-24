using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Unit : Entity, IShooter
{
    public UnitInfo UnitInfo => (UnitInfo)info;
    public UnitBehaviour behaviour;
    [HideInInspector] public NavMeshAgent agent;
    protected Animator anim;

    [Header("UNIT")]
    public SkinnedMeshRenderer meshRender;
    public UnitState currentState = UnitState.IsUnactive;
    public float stopDistance = 3.5f;
    [SerializeField] protected Material HighlightMat;
    public LayerMask ground = 1 >> 2;

    [Header("Fight")]
    public Entity lastTarget;
    [HideInInspector] public Vector3 attackPosition;

    public void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        agent.speed = UnitInfo.moveSpeed;
        baseMat = meshRender.material;
    }

    public override void Start()
    {
        base.Start();
        EventManager.Instance.onEndGame.AddListener(Deactivate);
    }

    public void SetState(UnitState state)
    {
        currentState = state;
        switch (currentState)
        {
            case UnitState.IsUnactive:
                behaviour = null;
                break;
            case UnitState.IsMoving:
                behaviour = new UnitBehaviour_Moving(this);
                break;
            case UnitState.IsChasing:
                behaviour = new UnitBehaviour_Chasing(this, UnitInfo);
                break;
            case UnitState.IsFighting:
                behaviour = new UnitBehaviour_Fighting(this, UnitInfo);
                break;
        }
    }

    public override void Death()
    {
        base.Death();
        anim.SetTrigger("Death");
        SetState(UnitState.IsUnactive);
    }

    public void Shoot()
    {
        anim.SetTrigger("Shoot");
    }

    public override float HealthbarOffset()
    {
        if (meshRender.TryGetComponent(out SkinnedMeshRenderer mesh))
            return 7.5f;

        return 0;
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
            VFXManager.Instance.InvokeVFX(FX_Event.LaserDamage, damageLocation, Quaternion.identity);
            VFXManager.Instance.InvokeVFX(FX_Event.UnitDamage, damageLocation, Quaternion.identity);
        }
    }

    public override Vector3 MeshClosestPoint(Vector3 from)
    {
        return GetComponentInChildren<Collider>().ClosestPoint(from);
    }

    public void Attack(Entity target) //start chase
    {
        lastTarget = target;
        SetState(UnitState.IsChasing);
        if (lastTarget.GetComponent<Building>())
            attackPosition = (lastTarget as Building).MeshClosestPoint(transform.position);
    }

    public void MoveTo(Vector3 pos)
    {
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(pos, path) && !health.isDead)
        {
            agent.isStopped = false;
            SetState(UnitState.IsMoving);
            agent.SetDestination(pos);
        }
    }

    public virtual void Behavior()
    {
        if (lastTarget && lastTarget.GetComponent<Unit>())
            attackPosition = (lastTarget as Unit).transform.position;

        if (behaviour != null)
            behaviour.Execute();
    }

    public override void Update()
    {
        if (health.isDead)
            return;

        foreach (var item in System.Enum.GetValues(typeof(UnitState)))
            anim.SetBool(item.ToString(), currentState == (UnitState)item);

        base.Update();
        Behavior();
    }
}
