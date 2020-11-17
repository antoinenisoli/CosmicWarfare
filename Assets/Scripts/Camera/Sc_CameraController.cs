﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_CameraController : MonoBehaviour
{
    Camera mainCam => Camera.main;

    [SerializeField] bool debug;
    [SerializeField] Color boxColor = Color.white;

    [Header("Move camera")]
    [SerializeField] Vector3 panBounds;
    [SerializeField] float smoothSpeed = 5;
    [SerializeField] float panSpeed = 3;
    Vector3 basePosition;
    Vector3 newPosition;
    Vector3 dragStartPosition;
    Vector3 dragCurrentPosition;

    [Header("Rotate camera")]
    [SerializeField] float rotationAmount;
    Quaternion newRotation;
    Vector3 rotateStartPosition;
    Vector3 rotateCurrentPosition;

    [Header("Zoom camera")]
    [SerializeField] Vector2 zoomLimits;
    [SerializeField] float zoomAmount = 30;
    Vector3 newZoom;

    [ContextMenu("Reset Box Position")]
    public void ResetPos()
    {
        basePosition = transform.position;
    }

    private void Awake()
    {
        ResetPos();
        newRotation = transform.rotation;
        newPosition = transform.position;
        newZoom = mainCam.transform.localPosition;
    }

    private void OnDrawGizmos()
    {
        if (!debug)
            return;

        Gizmos.color = boxColor;
        Gizmos.DrawCube(basePosition, new Vector3(panBounds.x * 2, zoomLimits.x, panBounds.z * 2));
    }

    [ContextMenu("Clamp position in box")]
    public void ClampPosition()
    {
        newPosition.x = Mathf.Clamp(newPosition.x, basePosition.x - panBounds.x, basePosition.x + panBounds.x);
        newPosition.z = Mathf.Clamp(newPosition.z, basePosition.z - panBounds.z, basePosition.z + panBounds.z);
    }

    public void CameraZoom()
    {
        float scrollAxis = Input.GetAxisRaw("Mouse ScrollWheel");
        Vector3 amount = new Vector3(0, -zoomAmount, zoomAmount);
        newZoom += amount * scrollAxis;
        newZoom.y = Mathf.Clamp(newZoom.y, -zoomLimits.y, zoomLimits.x);
        newZoom.z = Mathf.Clamp(newZoom.z, -zoomLimits.x, zoomLimits.y);
        mainCam.transform.localPosition = Vector3.Lerp(mainCam.transform.localPosition, newZoom, Time.deltaTime * smoothSpeed);
    }

    void CameraRotation()
    {
        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;
            Vector3 diff = rotateStartPosition - rotateCurrentPosition;
            rotateStartPosition = rotateCurrentPosition;
            newRotation *= Quaternion.Euler(Vector3.up * (-diff.x/5));
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * smoothSpeed);
    }

    void CameraDragPosition()
    {
        Plane plane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(1))
        {
            if (plane.Raycast(ray, out float entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }

        if (Input.GetMouseButton(1))
        {
            if (plane.Raycast(ray, out float entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);
                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }
    }

    public void CameraScreenPanning()
    {
        Vector3 mousePosition = mainCam.ScreenToViewportPoint(Input.mousePosition);
        
        if (mousePosition.x > 0.93f) 
            newPosition += (transform.right * panSpeed);
        if (mousePosition.x < 0.08f)
            newPosition += (transform.right * -panSpeed);

        if (mousePosition.y > 0.93f)
            newPosition += (transform.forward * panSpeed);
        if (mousePosition.y < 0.08f)
            newPosition += (transform.forward * -panSpeed);

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * smoothSpeed);
    }

    private void Update()
    {
        CameraZoom();
        CameraRotation();
        CameraScreenPanning();
        CameraDragPosition();
        ClampPosition();
    }
}