using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceHealthbars : MonoBehaviour
{
    [SerializeField] GameObject barPrefab;
    [SerializeField] Transform parent;
    List<Entity> entities = new List<Entity>();

    private void Awake()
    {
        entities = FindObjectsOfType<Entity>().ToList();
        foreach (var item in entities)
        {
            AssignToNew(item);
        }
    }

    private void Start()
    {
        EventManager.Instance.onNewUnit.AddListener(AssignToNew);
    }

    void AssignToNew(Entity entity)
    {
        if (entity.health.healthSlider != null)
            return;

        GameObject bar = Instantiate(barPrefab, transform.position, Quaternion.identity, parent);
        bar.transform.localScale = Vector3.one + Vector3.one * (0.1f * entity.HealthbarOffset());
        entity.health.healthSlider = bar.GetComponent<Slider>();
        bar.GetComponent<Healthbar>().Initialize(entity);
        entity.health.SetSlider();
    }
}
