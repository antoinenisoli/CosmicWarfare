using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Sc_Casern))]
public class Sc_EnemyAI_Casern : MonoBehaviour
{
    Sc_Casern thisCasern;

    [Header("Enemy spawn")]
    [SerializeField] float spawnRate = 8f;
    float timer;

    private void Start()
    {
        thisCasern = GetComponent<Sc_Casern>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > spawnRate)
        {
            timer = 0;
            thisCasern.StartUnitProduction(0, Team.Enemy);
        }
    }
}
