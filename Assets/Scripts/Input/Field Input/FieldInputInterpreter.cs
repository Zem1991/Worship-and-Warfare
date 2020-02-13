using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldInputInterpreter : AbstractInputInterpreter<FieldInputListener>
{
    [Header("Inputs")]
    public Vector3 cursorAxes;
    public Vector3 cameraAxes;
    public bool escapeMenuDown;
    public bool selectionDown;
    public bool commandDown;
    public bool endTurnDown;
    public bool inventoryDown;
    public bool stopOrResumeCommandDown;

    // Update is called once per frame
    void Update()
    {
        cursorAxes = listener.CursorAxes();
        cameraAxes = listener.CameraAxes();
        escapeMenuDown = listener.EscapeMenuDown();
        selectionDown = listener.SelectionDown();
        commandDown = listener.CommandDown();
        endTurnDown = listener.EndTurnDown();
        inventoryDown = listener.InventoryDown();
        stopOrResumeCommandDown = listener.StopOrResumeCommandDown();
    }

    public void ClearInputs()
    {
        cursorAxes = Vector3.zero;
        cameraAxes = Vector3.zero;
        escapeMenuDown = false;
        selectionDown = false;
        commandDown = false;
        endTurnDown = false;
        inventoryDown = false;
        stopOrResumeCommandDown = false;
    }
}
