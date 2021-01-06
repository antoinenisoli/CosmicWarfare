using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_Healthbar : MonoBehaviour
{
    [SerializeField] Sc_Entity myEntity;
    [SerializeField] Vector3 offset = new Vector3(0, 5, 0);
    Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    public void Initialize(Sc_Entity entity)
    {
        myEntity = entity;
    }

    private void LateUpdate()
    {
        if (myEntity != null)
        {
            Vector3 targetPositionScreenPoint = mainCam.WorldToScreenPoint(myEntity.transform.position);
            transform.position = targetPositionScreenPoint + offset;
        }
    }
}
