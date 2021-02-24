using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sc_PlaceHealthbars : MonoBehaviour
{
    [SerializeField] GameObject barPrefab;
    [SerializeField] Transform parent;
    List<Sc_Entity> entities = new List<Sc_Entity>();

    private void Awake()
    {
        entities = FindObjectsOfType<Sc_Entity>().ToList();
        foreach (var item in entities)
        {
            AssignToNew(item);
        }
    }

    private void Start()
    {
        Sc_EventManager.Instance.onNewUnit.AddListener(AssignToNew);
    }

    void AssignToNew(Sc_Entity entity)
    {
        if (entity.health.healthSlider != null)
            return;

        GameObject bar = Instantiate(barPrefab, transform.position, Quaternion.identity, parent);
        bar.transform.localScale = Vector3.one + Vector3.one * (0.1f * entity.HealthbarOffset());
        entity.health.healthSlider = bar.GetComponent<Slider>();
        bar.GetComponent<Sc_Healthbar>().Initialize(entity);
        entity.health.SetSlider();
    }
}
