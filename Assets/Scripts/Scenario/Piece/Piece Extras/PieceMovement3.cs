using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public class PieceMovement3 : MonoBehaviour
{
    private static readonly float movementSpeed = 5F;
    private AbstractPiece3 piece;

    [Header("States")]
    public bool stateMovement;

    [Header("Substates")]
    public bool stateMovementStart;
    public bool stateMovementGoing;
    public bool stateMovementEnd;

    [Header("Movement points")]
    public int movementPointsCurrent;
    public int movementPointsMax;

    [Header("Current parameters")]
    [SerializeField] private bool inActualMovement;
    [SerializeField] private bool stopWasCalled;
    [SerializeField] private Vector3 nextPos;
    [SerializeField] private Vector3 direction;
    [SerializeField] private Vector3 velocity;

    [Header("Path references")]
    
    [SerializeField] private int pathTotalCost;
    [SerializeField] private List<PathNode> path = new List<PathNode>();

    private void Awake()
    {
        piece = GetComponent<AbstractPiece3>();
        LookAtDirection(OctoDirXZ.BACK);
    }

    public bool IsIdle()
    {
        return !stateMovement;
    }

    public void ResetMovementPoints(int movementPointsMax)
    {
        this.movementPointsMax = movementPointsMax;
        movementPointsCurrent = movementPointsMax;
    }

    public Vector3 GetDirection()
    {
        return direction;
    }

    public void LookAtDirection(OctoDirXZ dir)
    {
        direction = Vector3.zero;
        switch (dir)
        {
            case OctoDirXZ.BACK_LEFT:
                direction.x = -1;
                direction.z = -1;
                break;
            case OctoDirXZ.BACK:
                direction.z = -1;
                break;
            case OctoDirXZ.BACK_RIGHT:
                direction.x = 1;
                direction.z = -1;
                break;
            case OctoDirXZ.LEFT:
                direction.x = -1;
                break;
            case OctoDirXZ.RIGHT:
                direction.x = 1;
                break;
            case OctoDirXZ.FRONT_LEFT:
                direction.x = -1;
                direction.z = 1;
                break;
            case OctoDirXZ.FRONT:
                direction.z = 1;
                break;
            case OctoDirXZ.FRONT_RIGHT:
                direction.x = 1;
                direction.z = 1;
                break;
        }
        if (direction.x == -1) piece.FlipSpriteHorizontally(true);
        if (direction.x == 1) piece.FlipSpriteHorizontally(false);
    }

    public bool HasPath()
    {
        return path?.Count > 0;
    }

    public bool HasPath(AbstractTile targetTile)
    {
        return HasPath() && piece.pathArrivalTile == targetTile;
    }

    public List<PathNode> GetPath()
    {
        return path;
    }

    public void SetPath(PathfindResults pathfindResults, AbstractTile targetTile)
    {
        piece.pathArrivalTile = targetTile;

        path = pathfindResults.path;
        pathTotalCost = Mathf.CeilToInt(pathfindResults.pathTotalCost);
        //Debug.Log("PIECE " + name + " got a new path with size " + pathCost);

        if (path != null && path.Count > 0)
        {
            piece.pathNextTile = null;

            AbstractTile from = piece.currentTile;
            AbstractTile to = path[0].tile;
            OctoDirXZ dir = from.GetNeighbourDirection(to);
            LookAtDirection(dir);
        }
    }

    private bool CanDoNextStep()
    {
        return path != null && path.Count > 0 && path[0].moveCost <= movementPointsCurrent;
    }

    /*
    *   BEGIN:  Movement and Stop
    */
    public IEnumerator Movement(AbstractTile targetTile)
    {
        ICommandablePiece commandablePiece = piece as ICommandablePiece;
        if (!commandablePiece.ICP_IsIdle()) yield break;

        if (!HasPath(targetTile))
        {
            PartyPiece3 partyPiece = piece as PartyPiece3;
            UnitPiece3 unitPiece = piece as UnitPiece3;

            FieldTile fieldTile = targetTile as FieldTile;
            CombatTile combatTile = targetTile as CombatTile;

            if (partyPiece)
            {
                FieldManager.Instance.pieceHandler.Pathfind(partyPiece, fieldTile);
            }
            else if (unitPiece)
            {
                CombatManager.Instance.pieceHandler.Pathfind(unitPiece, combatTile);
            }
        }
        else
        {
            if (piece.currentTile != piece.pathArrivalTile &&
                CanDoNextStep())
            {
                stateMovement = true;
                //if (animateMovementStart) yield return StartCoroutine(MovementStart());   //TODO THIS LATER
                yield return StartCoroutine(MovementGoing());
                //if (animateMovementEnd) yield return StartCoroutine(MovementEnd());       //TODO THIS LATER
                stateMovement = false;
            }
        }
    }
    private IEnumerator MovementStart()
    {
        stateMovementStart = true;
        string stateName = "Movement start";
        yield return StartCoroutine(piece.WaitForAnimationStartAndEnd(stateName));
        stateMovementStart = false;
    }
    private IEnumerator MovementGoing()
    {
        stateMovementGoing = true;
        yield return StartCoroutine(ActualMovement());
        stateMovementGoing = false;
    }
    private IEnumerator MovementEnd()
    {
        stateMovementEnd = true;
        string stateName = "Movement end";
        yield return StartCoroutine(piece.WaitForAnimationStartAndEnd(stateName));
        stateMovementEnd = false;
    }
    public void Stop()
    {
        stopWasCalled = stateMovement;
        //yield return true;
    }
    /*
    *   END:    Movement and Stop
    */

    /*
    *   BEGIN:  Actual movement
    */
    private IEnumerator ActualMovement()
    {
        ICommandablePiece commandablePiece = piece as ICommandablePiece;
        bool doInteract = false;
        AbstractPiece3 interactionTarget = null;

        inActualMovement = true;
        while (inActualMovement)
        {
            Vector3 currentPos = transform.position;
            bool doStop = false;

            if (piece.pathNextTile && currentPos == nextPos)
            {
                //Doing this may seem redundant, but it actually fixes some floating point issues that can cause movement overshooting.
                //Moving to the bottom edge or left edge of the grid without this fix may cause the piece to be read as over a tile with coordinate equal to -1.
                transform.position = nextPos;

                //Clears the target references when we move over it, ending the path.
                if (piece.pathNextTile == piece.pathArrivalTile)
                {
                    piece.pathArrivalTile = null;
                    piece.targetTile = null;
                    piece.targetPiece = null;
                }

                piece.currentTile.occupantPiece = null;
                piece.pathNextTile.occupantPiece = piece;
                piece.currentTile = piece.pathNextTile;
                piece.pathNextTile = null;
            }

            if (!piece.pathNextTile)
            {
                if (!stopWasCalled &&
                    path.Count > 0 &&
                    CanDoNextStep())
                {
                    PathNode pNode = path[0];
                    movementPointsCurrent -= pNode.moveCost;
                    path.RemoveAt(0);

                    piece.pathNextTile = pNode.tile;
                    OctoDirXZ dirToLook = piece.currentTile.GetNeighbourDirection(piece.pathNextTile);
                    LookAtDirection(dirToLook);
                }
                else
                {
                    stopWasCalled = false;
                    doStop = true;
                }
            }

            if (piece.pathNextTile && piece.pathNextTile.occupantPiece)
            {
                doStop = true;
                doInteract = true;
                interactionTarget = piece.pathNextTile.occupantPiece;

                //Doing this here prevents that a piece walks over the spot of another removed piece.
                piece.pathNextTile = null;
            }

            if (doStop)
            {
                inActualMovement = false;
                nextPos = Vector3.zero;
                velocity = Vector3.zero;
            }
            else
            {
                nextPos = piece.pathNextTile.transform.position;
            }

            if (inActualMovement)
            {
                direction = (nextPos - currentPos).normalized;
                velocity = direction * movementSpeed;

                Vector3 frameVelocity = velocity * Time.deltaTime;
                float distance = Vector3.Distance(currentPos, nextPos);
                if (frameVelocity.magnitude > distance) frameVelocity = Vector3.ClampMagnitude(frameVelocity, distance);
                transform.Translate(frameVelocity, Space.World);
            }

            yield return null;
        }

        //If the piece has a way to interact with other pieces, make it happen here.
        if (doInteract && interactionTarget)
        {
            yield return
                StartCoroutine(commandablePiece.ICP_InteractWithTargetPiece(interactionTarget));
            //commandablePiece.ICP_InteractWithTargetPiece(interactionTarget);
        }
    }
    /*
    *   END:    Actual movement
    */
}
