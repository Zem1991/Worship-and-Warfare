using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public class FieldInputExecutor : AbstractInputExecutor<FieldInputInterpreter, FieldInputListener>
{
    [Header("Cursor data")]
    public FieldTile cursorTile;
    public AbstractFieldPiece2 cursorPiece;
    public Vector2Int cursorPos;

    [Header("Selection data")]
    public FieldTile selectionTile;
    public AbstractFieldPiece2 selectionPiece;
    public Vector2Int selectionPos;

    [Header("Interaction data")]
    public bool canCommandSelectedPiece;

    protected override void ManageWindows()
    {
        EscapeMenu();
        Inventory();
    }

    protected override bool HasCurrentWindow()
    {
        return FieldUI.Instance.currentWindow;
    }

    public override void ExecuteInputs()
    {
        ManageWindows();

        if (!IsGamePaused() && !HasCurrentWindow())
        {
            CameraControls();
            CursorChange();

            SelectionChange();
            SelectionCommand();

            StopOrResumeCommand();

            EndTurn();
        }
    }

    private void EscapeMenu()
    {
        if (interpreter.escapeMenuDown) FieldManager.Instance.EscapeMenu();
    }

    private void Inventory()
    {
        if (interpreter.inventoryDown) FieldManager.Instance.Selection_Inventory();
    }

    private void CameraControls()
    {
        float x = im.mouseAfterBorders.x + interpreter.cameraAxes.x;
        float z = im.mouseAfterBorders.y + interpreter.cameraAxes.z;
        Vector3 direction = new Vector3(x, 0, z);
        cameraController.MoveCamera(direction);
    }

    private void CursorChange()
    {
        Vector3 pos = im.mouseWorldPos;
        pos.x = Mathf.Floor(pos.x);
        pos.z = Mathf.Floor(pos.z);

        cursorPos = new Vector2Int((int)pos.x, (int)pos.z);
        cursorTile = null;
        cursorPiece = null;

        Camera cam = GameManager.Instance.mainCamera;
        Ray ray = cam.ScreenPointToRay(im.mouseScreenPos);
        RaycastHit[] hits = new RaycastHit[2];
        Physics.RaycastNonAlloc(ray, hits);

        foreach (var item in hits)
        {
            if (item.collider == null) continue;

            FieldTile t = item.collider.GetComponentInParent<FieldTile>();
            AbstractFieldPiece2 p = item.collider.GetComponentInParent<AbstractFieldPiece2>();
            if (cursorTile == null && t) cursorTile = t;
            if (cursorPiece == null && p) cursorPiece = p;
        }
    }

    private void SelectionChange()
    {
        if (interpreter.selectionDown && IsCursorValid())
        {
            selectionTile = cursorTile;
            selectionPiece = cursorPiece;
            selectionPos = cursorPos;

            PartyPiece2 pp = selectionPiece as PartyPiece2;
            if (pp)
            {
                canCommandSelectedPiece = pp.pieceOwner.GetOwner() == PlayerManager.Instance.localPlayer;
            }
            else
            {
                canCommandSelectedPiece = false;
            }
        }
    }

    private void SelectionCommand()
    {
        if (!interpreter.commandDown || !IsCursorValid()) return;
        MakeSelectedPieceInteract(true);
    }

    public void MakeSelectedPieceInteract(bool canPathfind)
    {
        bool condition = selectionPiece && canCommandSelectedPiece;
        if (!condition) return;

        PartyPiece2 pp = selectionPiece as PartyPiece2;
        if (pp)
        {
            if (pp.pieceMovement.stateMove)
            {
                pp.ICP_Stop();
            }
            else
            {
                FieldTile targetTile = (canPathfind ? cursorTile : pp.targetTile) as FieldTile;
                pp.ICP_InteractWith(targetTile);
                FieldSceneHighlights.Instance.PathChange();
            }
        }
    }

    private void StopOrResumeCommand()
    {
        if (interpreter.stopOrResumeCommandDown) FieldManager.Instance.Selection_Movement();
    }

    private void EndTurn()
    {
        if (interpreter.endTurnDown)
        {
            FieldManager fm = FieldManager.Instance;
            PlayerManager pm = PlayerManager.Instance;
            if (fm.currentPlayer == pm.localPlayer) FieldManager.Instance.EndTurn();
        }
    }
}
