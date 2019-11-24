using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

[RequireComponent(typeof(AbstractCombatantPiece2))]
public class PieceMovement2 : MonoBehaviour
{
    private AbstractPiece2 piece;

    public static readonly float movementSpeed = 5F;

    [Header("Settings")]
    public bool canMove;

    [Header("States")]
    public bool stateMove;

    [Header("Actions")]
    public bool movementStart;
    public bool movementGoing;
    public bool movementEnd;

    [Header("Parameters")]
    public int movementPointsCurrent;
    public int movementPointsMax;
    public int pathTotalCost;
    [SerializeField] private List<PathNode> path = new List<PathNode>();
    [SerializeField] private bool inActualMovement;
    public bool stopWasCalled;
    public Vector3 nextPos;
    public Vector3 direction;
    public Vector3 velocity;

    private void Awake()
    {
        piece = GetComponent<AbstractPiece2>();
        LookAtDirection(OctoDirXZ.BACK);
    }

    public bool IsIdle()
    {
        return !stateMove;
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

    public void ResetMovementPoints(int movementPointsMax)
    {
        this.movementPointsMax = movementPointsMax;
        movementPointsCurrent = movementPointsMax;
    }

    public bool HasPath()
    {
        return path?.Count > 0;
    }

    public bool HasPath(AbstractTile targetTile)
    {
        return HasPath() && piece.pathTargetTile == targetTile;
    }

    public List<PathNode> GetPath()
    {
        return path;
    }

    public void SetPath(PathfindResults pathfindResults, AbstractTile targetTile)
    {
        piece.pathTargetTile = targetTile;

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

    /*
    *   BEGIN:  Movement and Stop
    */
    public IEnumerator Movement(AbstractTile targetTile)
    {
        ICommandablePiece commandablePiece = piece as ICommandablePiece;
        if (!commandablePiece.ICP_IsIdle()) yield break;

        if (!HasPath(targetTile))
        {
            PartyPiece2 partyPiece = piece as PartyPiece2;
            AbstractCombatantPiece2 combatantPiece = piece as AbstractCombatantPiece2;

            FieldTile fieldTile = targetTile as FieldTile;
            CombatTile combatTile = targetTile as CombatTile;

            if (partyPiece)
            {
                FieldManager.Instance.pieceHandler.Pathfind(partyPiece, fieldTile);
            }
            else if (combatantPiece)
            {
                CombatManager.Instance.pieceHandler.Pathfind(combatantPiece, combatTile);
            }
        }
        else
        {
            if (piece.currentTile != piece.pathTargetTile)
            {
                stateMove = true;
                //if (animateMovementStart) yield return StartCoroutine(MovementStart());   //TODO THIS LATER
                yield return StartCoroutine(MovementGoing());
                //if (animateMovementEnd) yield return StartCoroutine(MovementEnd());       //TODO THIS LATER
                stateMove = false;
            }
        }
    }
    private IEnumerator MovementStart()
    {
        movementStart = true;
        string stateName = "Movement start";
        yield return StartCoroutine(piece.WaitForAnimationStartAndEnd(stateName));
        movementStart = false;
    }
    private IEnumerator MovementGoing()
    {
        movementGoing = true;
        yield return StartCoroutine(ActualMovement());
        movementGoing = false;
    }
    private IEnumerator MovementEnd()
    {
        movementEnd = true;
        string stateName = "Movement end";
        yield return StartCoroutine(piece.WaitForAnimationStartAndEnd(stateName));
        movementEnd = false;
    }
    public void Stop()
    {
        stopWasCalled = stateMove;
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
                if (piece.pathNextTile == piece.pathTargetTile)
                {
                    piece.pathTargetTile = null;
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
                    path[0].moveCost <= movementPointsCurrent)
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
                //If the piece has a way to interact with other pieces, make it happen here.
                ICommandablePiece commandablePiece = piece as ICommandablePiece;
                if (commandablePiece != null)
                {
                    //StartCoroutine(commandablePiece.ICP_InteractWithTargetPiece(piece.pathNextTile.occupantPiece));
                    commandablePiece.ICP_InteractWithTargetPiece(piece.pathNextTile.occupantPiece);
                }

                //Doing this here prevents that a piece walks over the spot of another removed piece.
                piece.pathNextTile = null;
                doStop = true;
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
    }
    /*
    *   END:    Actual movement
    */
}
