using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Sc_OilSource : MonoBehaviour
{
    SphereCollider col => GetComponent<SphereCollider>();
    [SerializeField] float areaRadius = 18;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, areaRadius);
    }

    private void Update()
    {
        col.radius = areaRadius;
    }
}
