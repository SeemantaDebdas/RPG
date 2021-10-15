using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clickable : MonoBehaviour
{
    [SerializeField]
    Texture2D talkCursor;
    [SerializeField]
    CursorMode cursorMode = CursorMode.Auto;
    private void OnMouseEnter()
    {
        Vector2 hotspot = new Vector2(talkCursor.height / 2, talkCursor.width / 2);
        Cursor.SetCursor(talkCursor, hotspot, cursorMode);
    }

    private void OnMouseExit()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
