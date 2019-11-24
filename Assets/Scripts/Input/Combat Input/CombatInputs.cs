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

    [Header("Interaction data")]
    public bool canCommandSelectedPiece;
    public AbstractCombatPiece2 lastHighlightedPiece;

    [Header("Movement Highlights")]
    public List<InputHighlight> movementHighlights = new List<InputHighlight>();

    [Header("Required Objects")]
    public InputManager im;
    public CombatInputRecorder recorder;
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
        recorder = GetComponent<CombatInputRecorder>();
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
    }

    private void SelectionHighlight()
    {
        if (!selectionHighlight) return;

        if (recorder.selectionDown && IsCursorValid())
        {
            Debug.LogWarning("SelectionHighlight() is not available in combat!");
        }

        if (selectionPiece)
        {
            selectionPos = selectionTile.posId;
            selectionTile = selectionPiece.currentTile as CombatTile;
            selectionHighlight.transform.position = selectionPiece.transform.position;

            AbstractCombatantPiece2 actp = selectionPiece as AbstractCombatantPiece2;
            if (actp)
            {
                selectionHighlight.gameObject.SetActive(true);
                canCommandSelectedPiece = actp.pieceOwner.GetOwner() == PlayerManager.Instance.localPlayer;
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
        bool condition = selectionPiece && canCommandSelectedPiece && selectionPiece.ICP_IsIdle();
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
        AbstractCombatPiece2 actp = selectionPiece as AbstractCombatPiece2;
        if (selectionPiece && canCommandSelectedPiece && actp)
        {
            StartCoroutine(actp.pieceCombatActions.Defend());
        }
    }

    private void EndTurn()
    {
        //TODO maybe default this to the Defend action?
        if (recorder.endTurnDown)
        {
            AbstractCombatPiece2 acp = CombatManager.Instance.currentPiece;
            PlayerManager pm = PlayerManager.Instance;
            if (acp.pieceOwner.GetOwner() == pm.localPlayer) acp.ISTET_EndTurn();
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

        AbstractCombatantPiece2 actp = selectionPiece as AbstractCombatantPiece2;
        if (!actp || !actp.ICP_IsIdle()) dontCreateHighlights = true;

        RemoveMovementHighlights();
        if (dontCreateHighlights) return;

        movementHighlights = InputHelper.MakeMovementHighlights(actp, actp.pieceMovement, transform, movementArrowSprites, movementMarkerSprites);
        lastHighlightedPiece = selectionPiece;
    }
}
