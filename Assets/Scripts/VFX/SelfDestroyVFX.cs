using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroyVFX : MonoBehaviour
{
    private void Awake()
    {
        ParticleSystem vfx = GetComponent<ParticleSystem>();
        vfx.Play();
        Destroy(gameObject, vfx.main.duration + vfx.main.startLifetimeMultiplier);
    }
}
