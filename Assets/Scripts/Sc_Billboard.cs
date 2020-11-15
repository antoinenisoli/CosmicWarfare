using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Sc_Billboard : MonoBehaviour
{
    Camera m_Camera => Camera.main;

    private void LateUpdate()
    {
        if (m_Camera != null)
        {
            transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward, m_Camera.transform.rotation * Vector3.up);
            Vector3 eulerAngles = transform.eulerAngles;
            transform.eulerAngles = eulerAngles;
        }
    }
}
