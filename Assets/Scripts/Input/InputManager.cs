using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : AbstractSingleton<InputManager>
{
    [Header("Current Scheme")]
    public ISceneInputs sceneInputs;

    [Header("Mouse Position")]
    public Vector3 mouseScreenPos;
    public Vector3 mouseWorldPos;
    public Vector3 mouseAfterBorders;
    public bool cursorOnPlayArea;

    // Update is called once per frame
    void Update()
    {
        UpdateMousePosition();
        if (sceneInputs != null) sceneInputs.ExecuteInputs();
    }

    public void ChangeScheme(GameScheme gs)
    {
        if (sceneInputs != null) sceneInputs.ClearInputs();

        switch (gs)
        {
            case GameScheme.FIELD:
                sceneInputs = FieldSceneInputs.Instance;
                break;
            case GameScheme.TOWN:
                sceneInputs = TownSceneInputs.Instance;
                break;
            case GameScheme.COMBAT:
                sceneInputs = CombatSceneInputs.Instance;
                break;
            default:
                Debug.LogWarning("No input scheme found!");
                sceneInputs = null;
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
