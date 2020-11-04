using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_CameraController : MonoBehaviour
{
    Camera mainCam => GetComponent<Camera>();

    [SerializeField] Color boxColor = Color.white;
    [SerializeField] Vector2 panBounds;
    [SerializeField] Vector2 heightBounds;
    [SerializeField] float panSpeed = 3;
    [SerializeField] float scrollSpeed = 3;
    Vector3 basePosition;

    [ContextMenu("Reset Gizmo Postion")]
    public void ResetPos()
    {
        basePosition = transform.position;
    }

    private void Awake()
    {
        ResetPos();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = boxColor;
        Gizmos.DrawCube(basePosition, new Vector3(panBounds.x * 2, heightBounds.x * 2, panBounds.y * 2));
    }

    [ContextMenu("Clamp position in box")]
    public void ClampPosition()
    {
        Vector3 newPos = transform.position;
        newPos.x = Mathf.Clamp(newPos.x, -panBounds.x, panBounds.x);
        newPos.y = Mathf.Clamp(newPos.y, heightBounds.x, heightBounds.y);
        newPos.z = Mathf.Clamp(newPos.z, -panBounds.y, panBounds.y);
        transform.position = newPos;
    }

    public void ManualMove()
    {
        float hAxis = Input.GetAxisRaw("Horizontal");
        float vAxis = Input.GetAxisRaw("Vertical");
        float yAxis = Input.GetAxis("Mouse ScrollWheel");
        Vector3 axes = new Vector3(hAxis, 0, vAxis);

        transform.Translate(axes.normalized * Time.deltaTime * panSpeed, Space.World);
        transform.Translate(Vector3.forward * yAxis * Time.deltaTime * scrollSpeed, Space.Self);
    }

    public void ScreenEdges()
    {
        Vector3 mousePosition = mainCam.ScreenToViewportPoint(Input.mousePosition);
        
        if (mousePosition.x > 0.98f) 
        {
            transform.Translate(Vector3.right * Time.deltaTime * panSpeed, Space.World);
        }
        if (mousePosition.x < 0.02f)
        {
            transform.Translate(Vector3.left * Time.deltaTime * panSpeed, Space.World);
        }

        if (mousePosition.y > 0.98f)
        {
            transform.Translate(Vector3.forward * Time.deltaTime * panSpeed, Space.World);
        }
        if (mousePosition.y < 0.02f)
        {
            transform.Translate(Vector3.back * Time.deltaTime * panSpeed, Space.World);
        }
    }

    private void Update()
    {
        ManualMove();
        ScreenEdges();
        ClampPosition();
    }
}
