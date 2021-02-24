using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DeadUnit : MonoBehaviour
{
    [SerializeField] float dissolveDuration = 3;
    SkinnedMeshRenderer[] renderers;
    float timer;

    public void Create(Unit unit)
    {
        renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        transform.parent = null;
        foreach (var item in renderers)
            item.material = unit.meshRender.material;
    }

    private void Update()
    {
        if (timer > dissolveDuration)
            Destroy(gameObject);
        else
        {
            timer += Time.deltaTime;
            foreach (var item in renderers)
                item.material.SetFloat("_DissolveAmount", Mathf.Lerp(item.material.GetFloat("_DissolveAmount"), 1, Time.deltaTime));
        }
    }
}
