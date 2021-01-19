using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Team
{
    Player,
    Enemy,
}

public class Sc_GameManager : MonoBehaviour
{
    public static Sc_GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
}
