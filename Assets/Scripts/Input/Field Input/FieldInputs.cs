using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public class FieldInputs : AbstractSingleton<FieldInputs>, IInputScheme, IShowableHideable
{
    [Header("Sprites")]
    public Sprite cursorSprite;
    public Sprite selectionSprite;
    public Sprite[] movementArrowSprites = new Sprite[8];
    public Sprite[] movementMarkerSprites = new Sprite[2];

    [Header("Cursor Data")]
    public InputHighlight cursorHighlight;
    public Vector2Int cursorPos;
    public FieldTile cursorTile;
    public AbstractFieldPiece cursorPiece;

    [Header("Selection Data")]
    public InputHighlight selectionHighlight;
    public Vector2Int selectionPos;
    public FieldTile selectionTile;
    public AbstractFieldPiece selectionPiece;
    public bool canCommandSelectedPiece;

    [Header("Movement Highlights")]
    public bool movementHighlightsUpdateFromCommand;
    public bool movementHighlightsUpdateOnPieceStop;
    public bool movementHighlightsUpdateOnMethodCall;
    public List<InputHighlight> movementHighlights = new List<InputHighlight>();

    [Header("Required Objects")]
    public InputManager im;
    public FieldInputRecorder recorder;
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
        recorder = GetComponent<FieldInputRecorder>();
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

        if (!IsGamePaused() && !FieldUI.Instance.currentWindow)
        {
            CameraControls();
            CursorHighlight();

            SelectionHighlight();
            SelectionCommand();

            StopOrResumeCommand();

            EndTurn();
        }
        MovementHighlights();
    }

    private void ManageWindows()
    {
        EscapeMenu();
        Inventory();
    }

    private void EscapeMenu()
    {
        if (recorder.escapeMenuDown) FieldManager.Instance.EscapeMenu();
    }

    private void Inventory()
    {
        if (recorder.inventoryDown) FieldManager.Instance.Inventory();
    }

    private void CameraControls()
    {
        float x = im.mouseAfterBorders.x + recorder.cameraAxes.x;
        float z = im.mouseAfterBorders.y + recorder.cameraAxes.z;
        Vector3 direction = new Vector3(x, 0, z);
        cameraController.MoveCamera(direction);
    }

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

                FieldTile t = item.collider.GetComponentInParent<FieldTile>();
                AbstractFieldPiece p = item.collider.GetComponentInParent<AbstractFieldPiece>();
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
            movementHighlightsUpdateFromCommand = true;

            if (true)   //(im.cursorOnPlayArea)
            {
                selectionPos = cursorPos;
                selectionTile = cursorTile;
                selectionPiece = cursorPiece;

                selectionHighlight.transform.position = cursorHighlight.transform.position;
            }
            else
            {
                selectionPos = Vector2Int.one * -1;
                selectionTile = null;
                selectionPiece = null;
            }
        }

        if (selectionPiece)
        {
            if (selectionPiece.inMovement)
            {
                selectionTile = selectionPiece.currentTile as FieldTile;
                selectionPos = selectionTile.posId;

                selectionHighlight.transform.position = selectionPiece.transform.position;
            }

            selectionHighlight.gameObject.SetActive(true);
            canCommandSelectedPiece = selectionPiece.owner == PlayerManager.Instance.localPlayer;
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
        if (selectionPiece && canCommandSelectedPiece)
        {
            movementHighlightsUpdateFromCommand = true;

            if (selectionPiece.inMovement)
            {
                selectionPiece.Stop();
                movementHighlightsUpdateOnPieceStop = true;
            }
            else
            {
                selectionPiece.InteractWithTile(cursorTile, canPathfind);
            }
        }
    }

    private void StopOrResumeCommand()
    {
        if (recorder.stopOrResumeCommandDown) FieldManager.Instance.MakeSelectedPieceMove();
    }

    private void EndTurn()
    {
        if (recorder.endTurnDown)
        {
            GameManager gm = GameManager.Instance;
            PlayerManager pm = PlayerManager.Instance;
            if (gm.currentPlayer == pm.localPlayer) FieldManager.Instance.EndTurn();
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
        bool clearThenReturn = false;
        if (!selectionPiece)
        {
            if (movementHighlights.Count > 0) clearThenReturn = true;
            else return;
        }
        else
        {
            bool condition1 = movementHighlightsUpdateFromCommand;
            bool condition2 = movementHighlightsUpdateOnPieceStop && !selectionPiece.inMovement;
            bool condition3 = movementHighlightsUpdateOnMethodCall;
            if (!condition1 &&
                !condition2 &&
                !condition3) return;
        }

        RemoveMovementHighlights();
        if (clearThenReturn) return;

        List<PathNode> path = selectionPiece.path;
        if (canCommandSelectedPiece &&
            selectionPiece.IsIdle() &&
            path != null)
        {
            InputManager im = InputManager.Instance;
            InputHighlight prefabHighlight = AllPrefabs.Instance.inputHighlight;

            movementHighlights = new List<InputHighlight>();
            int movePoints = selectionPiece.movementPointsCurrent;
            int moveCost = 0;
            Color moveColor;

            int totalNodes = path.Count;
            for (int i = -1; i < totalNodes - 1; i++)
            {
                int nextI = i + 1;

                FieldTile currentTile = (i == -1 ? selectionPiece.currentTile : path[i].tile) as FieldTile;
                FieldTile nextTile = path[nextI].tile as FieldTile;

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
