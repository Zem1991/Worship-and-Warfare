using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public class CombatInputs : AbstractSingleton<CombatInputs>, IInputScheme, IShowableHideable
{
    [Header("Sprites")]
    public Sprite cursorSprite;
    public Sprite selectionSprite;
    public Sprite moveAreaSprite;
    public Sprite[] movePathArrowSprites = new Sprite[8];
    public Sprite[] movePathMarkerSprites = new Sprite[2];

    [Header("Cursor data")]
    public InputHighlight cursorHighlight;
    public CombatTile cursorTile;
    public AbstractCombatActorPiece2 cursorPiece;
    public Vector2Int cursorPos;

    [Header("Selection data")]
    public InputHighlight selectionHighlight;
    public CombatTile selectionTile;
    public AbstractCombatActorPiece2 selectionPiece;
    public Vector2Int selectionPos;

    [Header("Interaction data")]
    public bool canCommandSelectedPiece;
    public AbstractCombatActorPiece2 lastHighlightedPiece;

    [Header("Movement Highlights")]
    public List<InputHighlight> moveAreaHighlights = new List<InputHighlight>();
    public List<InputHighlight> movePathHighlights = new List<InputHighlight>();

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
        cursorHighlight.ChangeSprite(cursorSprite, im.highlightDefault, SpriteOrderConstants.CURSOR);

        //selectionHighlight = Instantiate(prefabHighlight, transform);
        //selectionHighlight.name = "Selection Highlight";
        selectionHighlight.ChangeSprite(selectionSprite, im.highlightDefault, SpriteOrderConstants.SELECTION);

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

        MoveHighlights();
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
                AbstractCombatActorPiece2 p = item.collider.GetComponentInParent<AbstractCombatActorPiece2>();
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

            selectionHighlight.gameObject.SetActive(true);

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
        AbstractCombatActorPiece2 actp = selectionPiece as AbstractCombatActorPiece2;
        if (selectionPiece && canCommandSelectedPiece && actp)
        {
            StartCoroutine(actp.pieceCombatActions.Defend());
        }
    }

    private void EndTurn()
    {
        if (recorder.endTurnDown)
        {
            AbstractCombatActorPiece2 acp = CombatManager.Instance.currentPiece;
            PlayerManager pm = PlayerManager.Instance;
            if (acp.pieceOwner.GetOwner() == pm.localPlayer) acp.ISTET_EndTurn();
        }
    }

    public void ResetHighlights()
    {
        lastHighlightedPiece = null;
    }

    public void RemoveMoveAreaHighlights()
    {
        foreach (var item in moveAreaHighlights)
        {
            Destroy(item.gameObject);
        }
        moveAreaHighlights.Clear();
    }

    public void RemoveMovePathHighlights()
    {
        foreach (var item in movePathHighlights)
        {
            Destroy(item.gameObject);
        }
        movePathHighlights.Clear();
    }

    private void MoveHighlights()
    {
        MoveAreaHighlights();
        MovePathHighlights();

        lastHighlightedPiece = selectionPiece;
    }

    private void MoveAreaHighlights()
    {
        if (selectionPiece && selectionPiece == lastHighlightedPiece) return;

        AbstractCombatantPiece2 actp = selectionPiece as AbstractCombatantPiece2;

        bool dontCreateHighlights = false;
        if (!canCommandSelectedPiece) dontCreateHighlights = true;
        if (!actp || !actp.ICP_IsIdle()) dontCreateHighlights = true;

        RemoveMoveAreaHighlights();
        if (dontCreateHighlights) return;

        moveAreaHighlights = InputHelper.MakeMoveAreaHighlights(actp, actp.pieceMovement,
            Pathfinder.HexHeuristic, true, false, false,
            transform, moveAreaSprite);
    }

    private void MovePathHighlights()
    {
        if (selectionPiece && selectionPiece == lastHighlightedPiece) return;

        AbstractCombatantPiece2 actp = selectionPiece as AbstractCombatantPiece2;

        bool dontCreateHighlights = false;
        if (!canCommandSelectedPiece) dontCreateHighlights = true;
        if (!actp || !actp.ICP_IsIdle()) dontCreateHighlights = true;

        RemoveMovePathHighlights();
        if (dontCreateHighlights) return;

        movePathHighlights = InputHelper.MakeMovePathHighlights(actp, actp.pieceMovement, transform, movePathArrowSprites, movePathMarkerSprites);
    }
}
