using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sc_BuildManager : MonoBehaviour
{
    Camera cam => Camera.main;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] GameObject[] buildings;
    bool isBuilding;
    Sc_Building lastBuilding;
    Vector3 pos;

    public void SelectBuilding(int index)
    {
        GameObject newBuilding = Instantiate(buildings[index], transform.position, Quaternion.identity);
        lastBuilding = newBuilding.GetComponent<Sc_Building>();
        isBuilding = true;
    }

    private void Update()
    {
        if (isBuilding)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            bool detectGround = Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, groundLayer);
            if (detectGround)
            {
                lastBuilding.transform.position = hit.point;
                lastBuilding.transform.LookAt(hit.normal);
            }

            if (Input.GetMouseButtonDown(0))
            {
                lastBuilding.Place();
                isBuilding = false;
            }
        }
    }
}
