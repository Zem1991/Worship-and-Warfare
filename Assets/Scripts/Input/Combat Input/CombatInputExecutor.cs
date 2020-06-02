using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public class CombatInputExecutor : AbstractInputExecutor<CombatInputInterpreter, CombatInputListener>
{
    [Header("Cursor data")]
    public CombatTile cursorTile;
    public AbstractCombatPiece3 cursorPiece;
    public Vector2Int cursorPos;

    [Header("Selection data")]
    public CombatTile selectionTile;
    public AbstractCombatPiece3 selectionPiece;
    public Vector2Int selectionPos;

    [Header("Interaction data")]
    public bool canCommandSelectedPiece;

    protected override void ManageWindows()
    {
        if (CombatManager.Instance.IsCombatRunning())
        {
            EscapeMenu();
            CombatActorInspector();
        }
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
        if (interpreter.escapeMenuDown) CombatManager.Instance.EscapeMenu();
    }

    private void CombatActorInspector()
    {
        if (interpreter.inspectorDown) CombatManager.Instance.CombatActorInspector();
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
            AbstractCombatPiece3 p = item.collider.GetComponentInParent<AbstractCombatPiece3>();
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

            CombatantPiece3 actp = selectionPiece as CombatantPiece3;
            if (actp)
            {
                canCommandSelectedPiece = actp.pieceOwner.Get() == PlayerManager.Instance.localPlayer;
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

        CombatantPiece3 actp = selectionPiece as CombatantPiece3;
        if (actp)
        {
            //if (actp.pieceMovement.stateMove)
            if (!actp.ICP_IsIdle())
            {
                actp.ICP_Stop();
            }
            else
            {
                CombatTile targetTile = (canPathfind ? cursorTile : actp.targetTile) as CombatTile;
                actp.ICP_InteractWith(targetTile);
                CombatSceneHighlights.Instance.PathChange();
            }
        }
    }

    public void MakeSelectedPieceWait()
    {
        CombatantPiece3 actp = selectionPiece as CombatantPiece3;
        if (!actp) return;

        if (selectionPiece && canCommandSelectedPiece && !actp.pieceCombatActions.stateWait)
        {
            StartCoroutine(actp.pieceCombatActions.Wait());
        }
    }
    public void MakeSelectedPieceDefend()
    {
        CombatantPiece3 actp = selectionPiece as CombatantPiece3;
        if (!actp) return;

        if (selectionPiece && canCommandSelectedPiece && !actp.pieceCombatActions.stateDefend)
        {
            StartCoroutine(actp.pieceCombatActions.Defend());
        }
    }
    public void MakeSelectedPieceUseAbility(int abilityId)
    {
        CombatantPiece3 actp = selectionPiece as CombatantPiece3;
        if (!actp) return;

        if (selectionPiece && canCommandSelectedPiece && !actp.pieceCombatActions.stateDefend)
        {
            DB_Ability ability = actp.abilityStats.GetFromId(abilityId);
            List<AbstractTile> targetArea = new List<AbstractTile> { actp.currentTile };    //TODO: is casting against self!
            StartCoroutine(actp.pieceCombatActions.Ability(abilityId, ability, targetArea));     
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
            CombatantPiece3 acp = CombatManager.Instance.currentPiece;
            PlayerManager pm = PlayerManager.Instance;
            if (acp.pieceOwner.Get() == pm.localPlayer) acp.ISTET_EndTurn();
        }
    }
}
