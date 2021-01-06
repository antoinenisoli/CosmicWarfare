using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityInfo", menuName = "Entity/Info")]
public class Sc_EntityInfo : ScriptableObject
{
    public string entityName;
    [TextArea] public string entityDescription;
    public int maxHealth;
}
