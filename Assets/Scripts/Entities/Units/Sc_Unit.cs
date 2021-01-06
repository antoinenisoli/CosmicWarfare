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
    protected NavMeshAgent agent => GetComponent<NavMeshAgent>();
    protected MeshRenderer mr => GetComponentInChildren<MeshRenderer>();
    protected Outline outline => mr.GetComponent<Outline>();

    [SerializeField] TrailRenderer trail;

    [Header("Selection")]    
    [SerializeField] protected Material HighlightMat;

    [Header("Characteristics")]
    [SerializeField] protected float firePower = 3;
    [SerializeField] protected UnitState currentState = UnitState.IsUnactive;

    [Header("Fight")]
    public GameObject shotFX;
    [SerializeField] protected float shootRange = 5;
    [SerializeField] protected float shootSpeed = 1.5f;
    protected float shootTimer;
    public LayerMask ground = 1 >> 2;
    [SerializeField] protected Sc_Entity lastTarget;
    [SerializeField] protected Vector3 attackPosition;

    public void Awake()
    {
        baseMat = mr.material;
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
            Sc_VFXManager.Instance.InvokeVFX(FX_Event.BuildingDamage, damageLocation);
            Sc_VFXManager.Instance.InvokeVFX(FX_Event.UnitDamage, damageLocation);
        }
    }

    public void Attack(Sc_Entity target, Vector3 pos)
    {
        lastTarget = target;
        attackPosition = pos;
        currentState = UnitState.IsAttacking;
    }

    public virtual void Shoot()
    {
        shootTimer = 0;
        lastTarget.ModifyLife(-firePower, attackPosition);
        Instantiate(shotFX, transform.position, transform.rotation);
    }

    public Vector3 ClosestPoint()
    {
        return GetComponentInChildren<Collider>().ClosestPoint(transform.position);
    }

    public virtual void Behavior()
    {
        if (lastTarget && lastTarget.GetComponent<Sc_Unit>())
            attackPosition = lastTarget.transform.position;

        switch (currentState)
        {
            case UnitState.IsMoving:
                if (Vector3.Distance(transform.position, agent.destination) < 3.5f)
                {
                    agent.isStopped = true;
                    currentState = UnitState.IsUnactive;
                }
                break;

            case UnitState.IsAttacking:
                if (Vector3.Distance(transform.position, attackPosition) < shootRange)
                {
                    currentState = UnitState.IsFighting;
                    agent.isStopped = true;
                }
                else
                {
                    agent.isStopped = false;
                    agent.SetDestination(lastTarget.transform.position);
                }
                break;

            case UnitState.IsFighting:
                if (lastTarget == null)
                {
                    currentState = UnitState.IsUnactive;
                    return;
                }

                if (Vector3.Distance(transform.position, attackPosition) < shootRange)
                {
                    shootTimer += Time.deltaTime;
                    agent.isStopped = true;
                    if (shootTimer > shootSpeed)
                    {
                        Shoot();
                    }
                }
                else
                {
                    shootTimer = 0;
                    Attack(lastTarget, ClosestPoint());
                }
                break;

            default:
                shootTimer = 0;
                break;
        }
    }

    public override void Update()
    {
        base.Update();
        Behavior();
        outline.enabled = highlighted ? true : false;
    }
}
