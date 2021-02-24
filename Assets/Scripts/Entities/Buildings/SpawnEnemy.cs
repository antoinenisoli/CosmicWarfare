using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildingCasern))]
public class SpawnEnemy : MonoBehaviour
{
    BuildingCasern thisCasern;

    [Header("Enemy spawn")]
    [SerializeField] int enemyIndex;
    [SerializeField] float spawnRate = 8f;
    float timer;

    private void Start()
    {
        thisCasern = GetComponent<BuildingCasern>();
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
