using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_OilSource : MonoBehaviour
{
    [SerializeField] float areaRadius = 6;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, areaRadius);
    }
}
