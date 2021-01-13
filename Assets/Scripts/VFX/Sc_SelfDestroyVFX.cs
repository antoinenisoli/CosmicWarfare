using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_SelfDestroyVFX : MonoBehaviour
{
    ParticleSystem vfx => GetComponent<ParticleSystem>();

    private void Awake()
    {
        vfx.Play();
        Destroy(gameObject, vfx.main.duration + vfx.main.startLifetimeMultiplier);
    }
}
