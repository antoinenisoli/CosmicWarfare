using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OilSource : MonoBehaviour
{
    [SerializeField] SphereCollider col;
    [SerializeField] float areaRadius = 18;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, areaRadius);
    }

    private void Awake()
    {
        col = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        col.radius = areaRadius;
    }
}
