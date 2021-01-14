using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Test : MonoBehaviour
{
    [ContextMenu("Play Audio")]
    public void Play()
    {
        GetComponent<AudioSource>().Play();
    }
}
