using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public class TownInputExecutor : AbstractInputExecutor<TownInputInterpreter, TownInputListener>
{
    public override void Awake()
    {
        base.Awake();
    }

    protected override void ManageWindows()
    {
        //EscapeMenu();
        //Inventory();
    }

    protected override bool HasCurrentWindow()
    {
        return FieldUI.Instance.currentWindow;
    }

    public override void ExecuteInputs()
    {
        //if (IsGamePaused())
        //{

        //}
        //else
        //{
        //    CameraControls();
        //    CursorHighlight();

        //    SelectionHighlight();
        //    SelectionCommand();
        //}
    }
}
