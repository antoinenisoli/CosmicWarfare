using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;

interface IUnitFormation
{
    void DoFormation(Sc_SelectionManager thisSelection, Vector3 targetPosition);
}
