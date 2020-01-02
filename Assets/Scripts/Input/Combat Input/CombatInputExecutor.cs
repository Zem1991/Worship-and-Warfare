using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public class CombatInputExecutor : AbstractInputExecutor<CombatInputInterpreter, CombatInputListener>
{
    [Header("Cursor data")]
    public CombatTile cursorTile;
    public AbstractCombatPiece2 cursorPiece;
    public Vector2Int cursorPos;

    [Header("Selection data")]
    public CombatTile selectionTile;
    public AbstractCombatPiece2 selectionPiece;
    public Vector2Int selectionPos;

    [Header("Interaction data")]
    public bool canCommandSelectedPiece;
    public AbstractCombatPiece2 lastHighlightedPiece;

    protected override void ManageWindows()
    {
        EscapeMenu();
        //Inventory();
    }

    protected override bool HasCurrentWindow()
    {
        return CombatUI.Instance.currentWindow;
    }

    public override void ExecuteInputs()
    {
        ManageWindows();

        if (!IsGamePaused() && !HasCurrentWindow())
        {
            if (CombatManager.Instance.IsCombatRunning())
            {
                //CameraControls();
                CursorChange();

                SelectionChange();
                SelectionCommand();

                //StopOrResumeCommand();

                EndTurn();
            }
        }
    }

    private void EscapeMenu()
    {
        if (interpreter.escapeMenuDown && CombatManager.Instance.IsCombatRunning())
        {
            CombatManager.Instance.EscapeMenu();
        }
    }

    //private void CameraControls()
    //{
    //    float x = im.mouseAfterBorders.x + recorder.cameraAxes.x;
    //    float z = im.mouseAfterBorders.y + recorder.cameraAxes.z;
    //    Vector3 direction = new Vector3(x, 0, z);
    //    cameraController.MoveCamera(direction);
    //}

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

            CombatTile t = item.collider.GetComponentInParent<CombatTile>();
            AbstractCombatPiece2 p = item.collider.GetComponentInParent<AbstractCombatPiece2>();
            if (cursorTile == null && t) cursorTile = t;
            if (cursorPiece == null && p) cursorPiece = p;
        }
    }

    private void SelectionChange()
    {
        if (interpreter.selectionDown && IsCursorValid())
        {
            lastHighlightedPiece = null;

            selectionTile = cursorTile;
            selectionPiece = cursorPiece;
            selectionPos = cursorPos;

            AbstractCombatantPiece2 actp = selectionPiece as AbstractCombatantPiece2;
            if (actp)
            {
                canCommandSelectedPiece = actp.pieceOwner.GetOwner() == PlayerManager.Instance.localPlayer;
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

        lastHighlightedPiece = null;
        MakeSelectedPieceInteract(true);
    }

    public void MakeSelectedPieceInteract(bool canPathfind)
    {
        bool condition = selectionPiece && canCommandSelectedPiece && (selectionPiece as AbstractCombatActorPiece2).ICP_IsIdle();
        if (!condition) return;

        AbstractCombatantPiece2 actp = selectionPiece as AbstractCombatantPiece2;
        if (actp)
        {
            if (actp.pieceMovement.stateMove)
            {
                actp.ICP_Stop();
                return;
            }
            CombatTile targetTile = (canPathfind ? cursorTile : actp.targetTile) as CombatTile;
            actp.ICP_InteractWith(targetTile);
        }
        lastHighlightedPiece = null;
    }

    public void MakeSelectedPieceWait()
    {
        AbstractCombatantPiece2 actp = selectionPiece as AbstractCombatantPiece2;
        if (selectionPiece && canCommandSelectedPiece && actp && !actp.pieceCombatActions.stateWait)
        {
            StartCoroutine(actp.pieceCombatActions.Wait());
        }
    }

    public void MakeSelectedPieceDefend()
    {
        AbstractCombatActorPiece2 actp = selectionPiece as AbstractCombatActorPiece2;
        if (selectionPiece && canCommandSelectedPiece && actp)
        {
            StartCoroutine(actp.pieceCombatActions.Defend());
        }
    }

    //private void StopOrResumeCommand()
    //{
    //    if (interpreter.stopOrResumeCommandDown) CombatManager.Instance.Selection_Movement();
    //}

    private void EndTurn()
    {
        if (interpreter.endTurnDown)
        {
            AbstractCombatActorPiece2 acp = CombatManager.Instance.currentPiece;
            PlayerManager pm = PlayerManager.Instance;
            if (acp.pieceOwner.GetOwner() == pm.localPlayer) acp.ISTET_EndTurn();
        }
    }
}
