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

    [Header("Cursor data")]
    public InputHighlight cursorHighlight;
    public Vector2Int cursorPos;
    public FieldTile cursorTile;
    public AbstractFieldPiece2 cursorPiece;

    [Header("Selection data")]
    public InputHighlight selectionHighlight;
    public Vector2Int selectionPos;
    public FieldTile selectionTile;
    public AbstractFieldPiece2 selectionPiece;

    [Header("Interaction data")]
    public bool canCommandSelectedPiece;
    public AbstractFieldPiece2 lastHighlightedPiece;

    [Header("Movement Highlights")]
    public List<InputHighlight> movementHighlights = new List<InputHighlight>();

    [Header("Required Objects")]
    public InputManager im;
    public FieldInputRecorder recorder;
    public CameraController cameraController;

    public override void Awake()
    {
        im = InputManager.Instance;
        //InputHighlight prefabHighlight = AllPrefabs.Instance.inputHighlight;

        //cursorHighlight = Instantiate(prefabHighlight, transform);
        //cursorHighlight.name = "Cursor Highlight";
        cursorHighlight.ChangeSprite(cursorSprite, im.highlightDefault);

        //selectionHighlight = Instantiate(prefabHighlight, transform);
        //selectionHighlight.name = "Selection Highlight";
        selectionHighlight.ChangeSprite(selectionSprite, im.highlightDefault);

        base.Awake();
    }

    // Start is called before the first frame update
    void Start()
    {
        recorder = GetComponent<FieldInputRecorder>();
        cameraController = GetComponentInChildren<CameraController>();
    }

    public void Hide()
    {
        //gameObject.SetActive(false);
        cursorHighlight.gameObject.SetActive(false);
        selectionHighlight.gameObject.SetActive(false);
    }

    public void Show()
    {
        //gameObject.SetActive(true);
        cursorHighlight.gameObject.SetActive(true);
        selectionHighlight.gameObject.SetActive(true);
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
        if (recorder.inventoryDown) FieldManager.Instance.Selection_Inventory();
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
        if (!cursorHighlight) return;

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
                AbstractFieldPiece2 p = item.collider.GetComponentInParent<AbstractFieldPiece2>();
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
    }

    private void SelectionHighlight()
    {
        if (!selectionHighlight) return;

        if (recorder.selectionDown && IsCursorValid())
        {
            lastHighlightedPiece = null;

            if (true)   //(im.cursorOnPlayArea)
            {
                selectionPos = cursorPos;
                selectionTile = cursorTile;
                selectionPiece = cursorPiece;

                selectionHighlight.transform.position = cursorHighlight.transform.position;
            }
        }

        if (selectionPiece)
        {
            selectionPos = selectionTile.posId;
            selectionTile = selectionPiece.currentTile as FieldTile;
            selectionHighlight.transform.position = selectionPiece.transform.position;

            PartyPiece2 pp = selectionPiece as PartyPiece2;
            if (pp)
            {
                selectionHighlight.gameObject.SetActive(true);
                canCommandSelectedPiece = pp.pieceOwner.GetOwner() == PlayerManager.Instance.localPlayer;
            }
            else
            {
                canCommandSelectedPiece = false;
            }
        }
        else
        {
            selectionHighlight.gameObject.SetActive(false);
            canCommandSelectedPiece = false;
        }
    }

    private void SelectionCommand()
    {
        if (!recorder.commandDown || !IsCursorValid()) return;

        lastHighlightedPiece = null;
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
                return;
            }
            FieldTile targetTile = (canPathfind ? cursorTile : pp.targetTile) as FieldTile;
            pp.ICP_InteractWith(targetTile);
        }
        lastHighlightedPiece = null;
    }

    private void StopOrResumeCommand()
    {
        if (recorder.stopOrResumeCommandDown) FieldManager.Instance.Selection_Movement();
    }

    private void EndTurn()
    {
        if (recorder.endTurnDown)
        {
            FieldManager fm = FieldManager.Instance;
            PlayerManager pm = PlayerManager.Instance;
            if (fm.currentPlayer == pm.localPlayer) FieldManager.Instance.EndTurn();
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

    public void ResetHighlights()
    {
        lastHighlightedPiece = null;
    }

    private void MovementHighlights()
    {
        if (selectionPiece && selectionPiece == lastHighlightedPiece) return;

        bool dontCreateHighlights = false;

        if (!canCommandSelectedPiece) dontCreateHighlights = true;

        PartyPiece2 pp = selectionPiece as PartyPiece2;
        if (!pp || !pp.ICP_IsIdle()) dontCreateHighlights = true;

        RemoveMovementHighlights();
        if (dontCreateHighlights) return;

        movementHighlights = InputHelper.MakeMovementHighlights(pp, pp.pieceMovement, transform, movementArrowSprites, movementMarkerSprites);
        lastHighlightedPiece = selectionPiece;
    }
}
