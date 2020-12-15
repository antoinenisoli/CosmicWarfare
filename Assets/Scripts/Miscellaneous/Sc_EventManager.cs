using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Sc_EventManager : MonoBehaviour
{
    public static Sc_EventManager Instance;
    public UnityEvent onCost = new UnityEvent();

    public class Entity_UnityEvent : UnityEvent<Sc_DestroyableEntity> { }
    public Entity_UnityEvent onNewUnit = new Entity_UnityEvent();

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }
}
