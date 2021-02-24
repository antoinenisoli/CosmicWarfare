using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityInfo", menuName = "Entity/EntityInfo")]
public class EntityInfo : ScriptableObject
{
    [Header("Entity")]
    public string entityName;
    [TextArea] public string entityDescription;
    public int maxHealth;
}
