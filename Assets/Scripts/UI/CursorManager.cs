using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseState
{
    Valid,
    Invalid,
    Attack,
}

public class CursorManager : MonoBehaviour
{
    public static CursorManager instance;
    public MouseState currentState;
    [SerializeField] Texture2D[] cursorIcons;
    [SerializeField] Vector2 offsetPC, offsetWeb;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        Texture2D cursor = cursorIcons[(int)currentState];

#if UNITY_WEBGL
        Cursor.SetCursor(cursor, offsetWeb, CursorMode.ForceSoftware);
#else
        Cursor.SetCursor(cursor, offsetPC, CursorMode.Auto);
#endif
    }
}
