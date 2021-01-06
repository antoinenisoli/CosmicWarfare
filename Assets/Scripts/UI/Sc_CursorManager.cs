using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MouseState
{
    Valid,
    Invalid,
    Attack,
}

public class Sc_CursorManager : MonoBehaviour
{
    public static Sc_CursorManager instance;
    [SerializeField] Texture2D[] cursorIcons;
    [SerializeField] Vector2 offset;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void SetCursorState(MouseState state)
    {
        Cursor.SetCursor(cursorIcons[(int)state], offset, CursorMode.Auto);
    }

    private void Update()
    {
        Cursor.SetCursor(cursorIcons[0], offset, CursorMode.Auto);
    }
}
