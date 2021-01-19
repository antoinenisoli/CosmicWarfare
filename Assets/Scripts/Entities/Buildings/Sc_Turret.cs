using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sc_Turret : Sc_Building
{
    public override BuildingType buildingType => BuildingType.Turret;

    [Header("Turret")]
    [SerializeField] Transform mainMesh;
    [SerializeField] Sc_UnitInfo fightInfo;
    [SerializeField] float animRate = 3;
    [SerializeField] LayerMask targetLayer;
    [SerializeField] Collider[] detectedEnemies = new Collider[0];
    Sc_Entity lastTarget;
    float timer;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fightInfo.shootRange);
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

            if (timer > fightInfo.shootRate)
            {
                timer = 0;
                Sc_VFXManager.Instance.InvokeVFX(FX_Event.ShootLaser, transform.position, mainMesh.rotation);
                lastTarget.ModifyLife(-fightInfo.firePower, lastTarget.transform.position);
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
        detectedEnemies = Physics.OverlapSphere(transform.position, fightInfo.shootRange, targetLayer);
        foreach (var enemy in detectedEnemies)
        {
            Sc_Entity detectedEntity = enemy.GetComponentInParent<Sc_Entity>();
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
