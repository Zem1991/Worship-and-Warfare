using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : AbstractSingleton<InputManager>
{
    [Header("Current Scheme")]
    public IInputScheme scheme;

    [Header("Mouse Position")]
    public Vector3 mouseScreenPos;
    public Vector3 mouseWorldPos;
    public Vector3 mouseAfterBorders;
    public bool cursorOnPlayArea;

    [Header("Highlight Colors")]
    public Color highlightDefault = Color.white;
    public Color highlightAllowed = Color.green;
    public Color highlightDenied = Color.red;

    // Update is called once per frame
    void Update()
    {
        if (scheme != null)
        {
            UpdateMousePosition();
            scheme.UpdateInputs();
        }
    }

    public void ChangeScheme(GameScheme gs)
    {
        switch (gs)
        {
            case GameScheme.FIELD:
                scheme = FieldInputs.Instance;
                break;
            case GameScheme.TOWN:
                scheme = TownInputs.Instance;
                break;
            case GameScheme.COMBAT:
                scheme = CombatInputs.Instance;
                break;
            default:
                Debug.LogWarning("No input scheme found!");
                scheme = null;
                break;
        }
    }

    public Vector3 ScreenToWorld(Vector2 screenPos)
    {
        Vector3 result = Vector3.one * -1;
        Camera cam = GameManager.Instance.mainCamera;
        Ray ray = cam.ScreenPointToRay(screenPos);
        Plane xzPlane = new Plane(Vector3.up, Vector3.zero);
        if (xzPlane.Raycast(ray, out float distance))
        {
            result = ray.GetPoint(distance);
        }
        return result;
    }

    private void UpdateMousePosition()
    {
        float width = Screen.width - 1;
        float height = Screen.height - 1;

        mouseScreenPos = Input.mousePosition;
        mouseWorldPos = ScreenToWorld(mouseScreenPos);
        mouseAfterBorders = Vector3.zero;
        if (mouseScreenPos.x < 1)
            mouseAfterBorders.x--;
        if (mouseScreenPos.x > width)
            mouseAfterBorders.x++;
        if (mouseScreenPos.y < 1)
            mouseAfterBorders.y--;
        if (mouseScreenPos.y > height)
            mouseAfterBorders.y++;

        ScenarioManager sm = ScenarioManager.Instance;
        cursorOnPlayArea = sm.IsWithinBounds(mouseWorldPos);
    }
}
