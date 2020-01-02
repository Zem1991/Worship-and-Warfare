using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownInputListener : AbstractInputListener
{
    public string axis_cursorX = "Mouse X";
    public string axis_cursorZ = "Mouse Y";
    public string axis_cameraX = "Horizontal";
    public string axis_cameraZ = "Vertical";
    public KeyCode kbm_selection = KeyCode.Mouse0;
    public KeyCode kbm_command = KeyCode.Mouse1;

    public Vector3 CursorAxes()
    {
        Vector3 result = new Vector3
        {
            x = Input.GetAxisRaw(axis_cursorX),
            z = Input.GetAxisRaw(axis_cursorZ)
        };
        return result;
    }

    public Vector3 CameraAxes()
    {
        Vector3 result = new Vector3
        {
            x = Input.GetAxisRaw(axis_cameraX),
            z = Input.GetAxisRaw(axis_cameraZ)
        };
        return result;
    }

    public bool SelectionDown()
    {
        bool kbm = Input.GetKeyDown(kbm_selection);
        return kbm;
    }

    public bool CommandDown()
    {
        bool kbm = Input.GetKeyDown(kbm_command);
        return kbm;
    }
}
