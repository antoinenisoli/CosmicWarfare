using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sc_EventManager : MonoBehaviour
{
    public static Sc_EventManager Instance;
    [HideInInspector] public UnityEvent onCost = new UnityEvent();

    public class EntityEvent : UnityEvent<Sc_DestroyableEntity> { }
    [HideInInspector] public EntityEvent onNewUnit = new EntityEvent();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
}
