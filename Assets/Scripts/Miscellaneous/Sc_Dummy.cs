using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Sc_Dummy : MonoBehaviour
{
    [SerializeField] float animDuration = 1;

    private void Awake()
    {
        Vector3 currentScale = transform.localScale;
        transform.DOScale(currentScale * 1.5f, animDuration/3);
        transform.DOScale(Vector3.zero, animDuration / 3).SetDelay(animDuration/3);
    }

    private void Update()
    {
        if (transform.localScale.sqrMagnitude < 0.1f)
            Destroy(gameObject);
    }
}
