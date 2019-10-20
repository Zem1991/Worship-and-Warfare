using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public class CombatInputs : AbstractSingleton<CombatInputs>, IInputScheme, IShowableHideable
{
    [Header("Prefabs and Sprites")]
    public Sprite cursorSprite;
    public Sprite selectionSprite;
    public Sprite[] movementArrowSprites = new Sprite[8];
    public Sprite[] movementMarkerSprites = new Sprite[2];

    [Header("Cursor Data")]
    public InputHighlight cursorHighlight;
    public Vector2Int cursorPos;
    public CombatTile cursorTile;
    public AbstractCombatPiece2 cursorPiece;

    [Header("Selection Data")]
    public InputHighlight selectionHighlight;
    public Vector2Int selectionPos;
    public CombatTile selectionTile;
    public AbstractCombatPiece2 selectionPiece;
    public bool canCommandSelectedPiece;

    [Header("Movement Highlights")]
    public bool movementHighlightsUpdateFromCommand;
    public bool movementHighlightsUpdateOnPieceStop;
    public bool movementHighlightsUpdateOnMethodCall;
    public List<InputHighlight> movementHighlights = new List<InputHighlight>();

    [Header("Required Objects")]
    public InputManager im;
    public CombatInputRecorder recorder;
    public CameraController cameraController;

    public override void Awake()
    {
        InputManager im = InputManager.Instance;
        InputHighlight prefabHighlight = AllPrefabs.Instance.inputHighlight;

        cursorHighlight = Instantiate(prefabHighlight, transform);
        cursorHighlight.name = "Cursor Highlight";
        cursorHighlight.ChangeSprite(cursorSprite, im.highlightDefault);

        selectionHighlight = Instantiate(prefabHighlight, transform);
        selectionHighlight.name = "Selection Highlight";
        selectionHighlight.ChangeSprite(selectionSprite, im.highlightDefault);

        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        im = InputManager.Instance;
        recorder = GetComponent<CombatInputRecorder>();
        cameraController = GetComponentInChildren<CameraController>();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public CameraController CameraController()
    {
        return cameraController;
    }

    public bool IsGamePaused()
    {
        return GameManager.Instance.isPaused;
    }

    public bool IsCursorValid()
    {
        return !UIManager.Instance.focusedPanel;
    }

    public void UpdateInputs()
    {
        ManageWindows();

        if (!IsGamePaused() && !CombatUI.Instance.currentWindow)
        {
            if (CombatManager.Instance.IsCombatRunning())
            {
                //CameraControls();
                CursorHighlight();

                SelectionHighlight();
                SelectionCommand();

                EndTurn();
            }
        }
        MovementHighlights();
    }

    private void ManageWindows()
    {
        EscapeMenu();
        //Inventory();
    }

    private void EscapeMenu()
    {
        if (recorder.escapeMenuDown && CombatManager.Instance.IsCombatRunning())
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

    private void CursorHighlight()
    {
        cursorPos = Vector2Int.one * -1;
        cursorTile = null;
        cursorPiece = null;

        if (true)   //(im.cursorOnPlayArea)
        {
            Vector3 pos = im.mouseWorldPos;
            pos.x = Mathf.Floor(pos.x);
            pos.z = Mathf.Floor(pos.z);

            cursorPos = new Vector2Int((int)pos.x, (int)pos.z);

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

            if (cursorTile)
            {
                cursorHighlight.transform.position = cursorTile.transform.position;
                cursorHighlight.gameObject.SetActive(true);
            }
            else
            {
                cursorHighlight.gameObject.SetActive(false);
            }
        }
        else
        {
            cursorHighlight.gameObject.SetActive(false);
        }
    }

    private void SelectionHighlight()
    {
        if (recorder.selectionDown &&
            IsCursorValid())
        {
            Debug.LogWarning("SelectionHighlight() is not available in combat!");
        }

        AbstractCombatantPiece2 actp = selectionPiece as AbstractCombatantPiece2;
        if (actp)
        {
            if (actp.IMP_GetPieceMovement().inMovement)
            {
                selectionTile = actp.currentTile as CombatTile;
                selectionPos = selectionTile.posId;

                selectionHighlight.transform.position = actp.transform.position;
            }

            selectionHighlight.gameObject.SetActive(true);
            canCommandSelectedPiece = actp.IPO_GetOwner() == PlayerManager.Instance.localPlayer;
        }
        else
        {
            selectionHighlight.gameObject.SetActive(false);
            canCommandSelectedPiece = false;
        }
    }

    private void SelectionCommand()
    {
        if (recorder.commandDown &&
            IsCursorValid())
        {
            MakeSelectedPieceInteract(true);
        }
    }

    public void MakeSelectedPieceInteract(bool canPathfind)
    {
        AbstractCombatantPiece2 actp = selectionPiece as AbstractCombatantPiece2;
        if (selectionPiece && canCommandSelectedPiece && actp.ICP_IsIdle())
        {
            movementHighlightsUpdateFromCommand = true;

            if (actp.IMP_GetPieceMovement().inMovement)
            {
                actp.IMP_Stop();
                movementHighlightsUpdateOnPieceStop = true;
            }
            else
            {
                actp.ICP_InteractWithTile(cursorTile, canPathfind);
            }
        }
    }

    private void EndTurn()
    {
        if (recorder.endTurnDown)
        {
            AbstractCombatantPiece2 acp = CombatManager.Instance.currentPiece;
            PlayerManager pm = PlayerManager.Instance;
            if (acp.IPO_GetOwner() == pm.localPlayer) acp.ICP_EndTurn();
        }
    }

    public void RemoveMovementHighlights()
    {
        foreach (var item in movementHighlights)
        {
            Destroy(item.gameObject);
        }
        movementHighlights.Clear();
    }

    public void CreateMovementHighlights()
    {
        movementHighlightsUpdateOnMethodCall = true;
    }

    private void MovementHighlights()
    {
        AbstractCombatantPiece2 actp = selectionPiece as AbstractCombatantPiece2;
        bool clearThenReturn = false;

        if (!actp)
        {
            if (movementHighlights.Count > 0) clearThenReturn = true;
            else return;
        }
        else
        {
            bool condition1 = movementHighlightsUpdateFromCommand;
            bool condition2 = movementHighlightsUpdateOnPieceStop && !actp.IMP_GetPieceMovement().inMovement;
            bool condition3 = movementHighlightsUpdateOnMethodCall;
            if (!condition1 && 
                !condition2 &&
                !condition3) return;
        }

        RemoveMovementHighlights();
        if (clearThenReturn) return;

        List<PathNode> path = actp.IMP_GetPieceMovement().path;
        if (canCommandSelectedPiece &&
            actp.ICP_IsIdle() &&
            path != null)
        {
            InputManager im = InputManager.Instance;
            InputHighlight prefabHighlight = AllPrefabs.Instance.inputHighlight;

            movementHighlights = new List<InputHighlight>();
            int movePoints = actp.IMP_GetPieceMovement().movementPointsCurrent;
            int moveCost = 0;
            Color moveColor;

            int totalNodes = path.Count;
            for (int i = -1; i < totalNodes - 1; i++)
            {
                int nextI = i + 1;

                CombatTile currentTile = (i == -1 ? actp.currentTile : path[i].tile) as CombatTile;
                CombatTile nextTile = path[nextI].tile as CombatTile;

                moveCost += path[nextI].moveCost;
                moveColor = moveCost > movePoints ? im.highlightDenied : im.highlightAllowed;

                Vector3 fromPos = currentTile.transform.position;
                Vector3 toPos = nextTile.transform.position;

                Vector3 pos = Vector3.Lerp(fromPos, toPos, 0.5F);
                Quaternion rot = Quaternion.identity;
                OctoDirXZ dir = currentTile.GetNeighbourDirection(nextTile);

                nextI++;

                InputHighlight step = Instantiate(prefabHighlight, pos, rot, transform);
                movementHighlights.Add(step);
                step.name = "Step #" + nextI;
                step.ChangeSprite(movementArrowSprites[(int)dir], moveColor);

                InputHighlight marker = Instantiate(prefabHighlight, toPos, rot, transform);
                movementHighlights.Add(marker);
                if (nextI == totalNodes)
                {
                    marker.name = "Final Marker";
                    marker.ChangeSprite(movementMarkerSprites[1], moveColor);
                }
                else
                {
                    marker.name = "Marker #" + nextI;
                    marker.ChangeSprite(movementMarkerSprites[0], moveColor);
                }
            }
        }

        movementHighlightsUpdateFromCommand = false;
        movementHighlightsUpdateOnPieceStop = false;
        movementHighlightsUpdateOnMethodCall = false;
    }
}
