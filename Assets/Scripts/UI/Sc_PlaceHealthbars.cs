using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sc_PlaceHealthbars : MonoBehaviour
{
    [SerializeField] GameObject barPrefab;
    [SerializeField] Transform parent;
    List<Sc_DestroyableEntity> entities = new List<Sc_DestroyableEntity>();

    private void Awake()
    {
        AssignHealthbars();
    }

    public void AssignHealthbars()
    {
        entities = FindObjectsOfType<Sc_DestroyableEntity>().ToList();
        foreach (var item in entities)
        {
            if (item.health.healthSlider != null)
                return;

            GameObject bar = Instantiate(barPrefab, transform.position, Quaternion.identity, parent);
            bar.GetComponent<Sc_Healthbar>().Initialize(item);
            item.health.healthSlider = bar.GetComponent<Slider>();
        }
    }
}
