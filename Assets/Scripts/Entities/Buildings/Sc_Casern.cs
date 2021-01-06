﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Casern : Sc_Building
{
    public override string type => "Casern";

    [Header("Casern")]
    [SerializeField] Transform spawnDestination;
    [SerializeField] Transform spawnTransform;
    public Purchase[] unitsToCreate;

    public IEnumerator Create(int index, float duration)
    {
        busy = true;
        yield return new WaitForSeconds(duration);
        GameObject newUnit = Instantiate(unitsToCreate[index].prefab, spawnTransform.position, Quaternion.identity);
        Sc_UnitAlly unit = newUnit.GetComponent<Sc_UnitAlly>();
        unit.MoveTo(spawnDestination.position, out _);
        Sc_Selection.GenerateEntity(unit);
        busy = false;
        Sc_EventManager.Instance.onCost.Invoke();
    }

    public override void UseBuilding()
    {
        
    }
}