using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Sc_Casern))]
public class Sc_EnemyAI_Casern : MonoBehaviour
{
    Sc_Casern thisCasern;

    [Header("Enemy spawn")]
    [SerializeField] int enemyIndex;
    [SerializeField] float spawnRate = 8f;
    float timer;

    private void Start()
    {
        thisCasern = GetComponent<Sc_Casern>();
    }

    private void Update()
    {
        if (thisCasern.resourceManager.gameEnded)
            return;

        timer += Time.deltaTime;
        if (timer > spawnRate)
        {
            timer = 0;
            if (thisCasern.CanPayUnit(enemyIndex))
                thisCasern.StartUnitProduction(enemyIndex, Team.Enemy);
        }
    }
}
