using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityUnitInfo", menuName = "Entity/UnitInfo")]
public class Sc_UnitInfo : ScriptableObject
{
    public float firePower = 1;
    public float shootRange = 20;
    public float shootRate = 2f;

}
