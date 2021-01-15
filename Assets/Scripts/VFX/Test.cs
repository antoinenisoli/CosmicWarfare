using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class Test : MonoBehaviour
{
    [ContextMenu("TEST")]
    public void Try()
    {
        Type[] types = Assembly.GetExecutingAssembly().GetTypes();
        Type[] possible = (from Type type in types where type.IsSubclassOf(typeof(UnitBehaviour)) select type).ToArray();
        foreach (var item in possible)
        {
            print(item.Name);
        }
    }
}
