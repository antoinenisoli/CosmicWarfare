using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FX_Event
{
    LaserDamage,
    UnitDamage,
    Heal,
    NewAlly,
    NewEnemy,
    Explosion,
    ShootLaser,
}

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;
    [SerializeField] List<VFX> allEffects = new List<VFX>();
    Dictionary<FX_Event, GameObject> storedVFX = new Dictionary<FX_Event, GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        foreach (var effect in allEffects)
            if (!storedVFX.ContainsKey(effect.thisEvent))
                storedVFX.Add(effect.thisEvent, effect.fx);
    }

    public void InvokeVFX(FX_Event fxEvent, Vector3 _pos, Quaternion _rot)
    {
        if (storedVFX.TryGetValue(fxEvent, out _))
        {           
            GameObject newFX = Instantiate(storedVFX[fxEvent], _pos, _rot);
            newFX.transform.position = _pos;
            if (_rot == Quaternion.identity)
                newFX.transform.rotation = storedVFX[fxEvent].transform.rotation;
            else
                newFX.transform.rotation = _rot;

            if (!newFX.GetComponent<SelfDestroyVFX>())
                newFX.AddComponent<SelfDestroyVFX>();
        }
        else
            Debug.LogError("There is a event gameobject missing.");
    }
}
