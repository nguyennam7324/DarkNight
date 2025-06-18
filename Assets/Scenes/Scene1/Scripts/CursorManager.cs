using UnityEngine;

public class CursorManger : MonoBehaviour
{
    [SerializeField] private Texture2D cursorNormal;
    [SerializeField] private Texture2D cursorShot;
    [SerializeField] private Texture2D cursorReload;
    private Vector2 hostpot = new Vector2(16, 48);
    void Start()
    {
        Cursor.SetCursor(cursorNormal, hostpot, CursorMode.Auto);
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(cursorShot, hostpot, CursorMode.Auto);

        }
        else if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorNormal, hostpot, CursorMode.Auto);

        }
        if (Input.GetMouseButtonDown(1))
        {
            Cursor.SetCursor(cursorReload, hostpot, CursorMode.Auto);

        }
        else if (Input.GetMouseButtonUp(1))
        {
            Cursor.SetCursor(cursorNormal, hostpot, CursorMode.Auto);

        }
    }
}
