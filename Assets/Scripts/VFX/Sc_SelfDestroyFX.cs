using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_SelfDestroyFX : MonoBehaviour
{
    ParticleSystem fx => GetComponent<ParticleSystem>();

    private void Awake()
    {
        fx.Play();
        Destroy(gameObject, fx.main.duration + fx.main.startLifetimeMultiplier);
    }
}
