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
        entities = FindObjectsOfType<Sc_DestroyableEntity>().ToList();
        foreach (var item in entities)
        {
            AssignToNew(item);
        }
    }

    private void Start()
    {
        Sc_EventManager.Instance.onNewUnit.AddListener(AssignToNew);
    }

    void AssignToNew(Sc_DestroyableEntity entity)
    {
        if (entity.health.healthSlider != null)
            return;

        GameObject bar = Instantiate(barPrefab, transform.position, Quaternion.identity, parent);
        bar.GetComponent<Sc_Healthbar>().Initialize(entity);
        entity.health.healthSlider = bar.GetComponent<Slider>();
    }
}
