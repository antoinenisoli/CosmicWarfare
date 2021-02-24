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
    [SerializeField] Vector2 offset;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        Cursor.SetCursor(cursorIcons[(int)currentState], offset, CursorMode.Auto);
    }
}
