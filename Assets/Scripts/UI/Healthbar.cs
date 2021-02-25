using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healthbar : MonoBehaviour
{
    [SerializeField] Entity myEntity;
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    public void Initialize(Entity entity)
    {
        myEntity = entity;
    }

    private void LateUpdate()
    {
        if (myEntity != null)
        {
            Vector3 targetPositionScreenPoint = cam.WorldToScreenPoint(myEntity.transform.position);
            transform.position = targetPositionScreenPoint + Vector3.up * myEntity.UIOffset;
        }
    }
}
