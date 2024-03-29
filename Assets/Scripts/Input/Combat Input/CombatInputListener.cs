﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatInputListener : AbstractInputListener
{
    public string axis_cursorX = "Mouse X";
    public string axis_cursorZ = "Mouse Y";
    public string axis_cameraX = "Horizontal";
    public string axis_cameraZ = "Vertical";
    public KeyCode kbm_escapeMenu = KeyCode.Escape;
    public KeyCode kbm_selection = KeyCode.Mouse0;
    public KeyCode kbm_command = KeyCode.Mouse1;
    public KeyCode kbm_endTurn = KeyCode.Backspace;
    public KeyCode kbm_inspector = KeyCode.E;

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

    public bool EscapeMenuDown()
    {
        bool kbm = Input.GetKeyDown(kbm_escapeMenu);
        return kbm;
    }

    public bool CommandDown()
    {
        bool kbm = Input.GetKeyDown(kbm_command);
        return kbm;
    }

    public bool EndTurnDown()
    {
        bool kbm = Input.GetKeyDown(kbm_endTurn);
        return kbm;
    }

    public bool InspectorDown()
    {
        bool kbm = Input.GetKeyDown(kbm_inspector);
        return kbm;
    }
}
