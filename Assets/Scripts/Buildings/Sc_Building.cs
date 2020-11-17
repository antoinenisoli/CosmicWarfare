using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Player,
    Enemy,
}

public class Sc_Building : Sc_DestroyableEntity
{
    [SerializeField] protected Team myTeam;
}
