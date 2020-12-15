using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FX_Event
{
    BuildingDamage,
    Heal,
}

public class Sc_VFXManager : MonoBehaviour
{
    public static Sc_VFXManager Instance;
    [SerializeField] List<VFX> allEffects = new List<VFX>();
    Dictionary<FX_Event, GameObject> d_vfx = new Dictionary<FX_Event, GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        foreach (var item in allEffects)
        {
            d_vfx.Add(item.m_event, item.fx);
        }
    }

    public void InvokeVFX(FX_Event e, Vector3 _pos)
    {
        GameObject newFX = Instantiate(d_vfx[e]);
        newFX.transform.position = _pos;
    }
}
