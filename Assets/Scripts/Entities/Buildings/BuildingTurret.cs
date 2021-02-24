using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BuildingTurret : Building, IShooter
{
    public UnitInfo UnitInfo => (UnitInfo)info;
    public override BuildingType buildingType => BuildingType.Turret;

    [Header("Turret")]
    [SerializeField] Transform mainMesh;
    [SerializeField] float animRate = 3;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] Collider[] detectedEnemies = new Collider[0];
    Entity lastTarget;
    float timer;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, UnitInfo.shootRange);
    }

    public Vector3 RandomPosition(float min, float max)
    {
        Vector3 random = new Vector3();
        random.x = Random.Range(min, max);
        random.y = transform.position.y;
        random.z = Random.Range(min, max);
        return random;
    }

    public override void UseBuilding()
    {
        timer += Time.deltaTime;
        if (lastTarget)
        {
            Vector3 _direction = (lastTarget.transform.position - transform.position).normalized;
            Quaternion _lookRotation = Quaternion.LookRotation(_direction);
            mainMesh.DORotateQuaternion(_lookRotation, 0.5f);

            if (timer > UnitInfo.shootRate)
            {
                timer = 0;
                VFXManager.Instance.InvokeVFX(FX_Event.ShootLaser, transform.position, mainMesh.rotation);
                lastTarget.ModifyLife(-UnitInfo.firePower, lastTarget.transform.position);
            }
        }
        else
        {
            if (timer > animRate)
            {
                timer = 0;
                Vector3 _direction = (RandomPosition(-500, 500) - transform.position).normalized;
                Quaternion _lookRotation = Quaternion.LookRotation(_direction);
                mainMesh.DORotateQuaternion(_lookRotation, 0.8f);
            }
        }
    }

    private void FixedUpdate()
    {
        detectedEnemies = Physics.OverlapSphere(transform.position, UnitInfo.shootRange, targetLayer);
        foreach (var enemy in detectedEnemies)
        {
            Entity detectedEntity = enemy.GetComponentInParent<Entity>();
            if (detectedEntity && detectedEntity.myTeam != myTeam)
            {
                lastTarget = detectedEntity;
                break;
            }
            else
                lastTarget = null;
        }
    }
}
