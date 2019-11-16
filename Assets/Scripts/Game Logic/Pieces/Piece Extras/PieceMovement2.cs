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
        return HasPath() && piece.targetTile == targetTile;
    }

    public List<PathNode> GetPath()
    {
        return path;
    }

    public void SetPath(PathfindResults pathfindResults, AbstractTile targetTile)
    {
        path = pathfindResults.path;
        pathTotalCost = Mathf.CeilToInt(pathfindResults.pathTotalCost);
        piece.targetTile = targetTile;
        //Debug.Log("PIECE " + name + " got a new path with size " + pathCost);

        if (path != null && path.Count > 1)
        {
            piece.nextTile = null;

            AbstractTile from = path[0].tile;
            AbstractTile to = path[1].tile;
            OctoDirXZ dir = from.GetNeighbourDirection(to);
            LookAtDirection(dir);
        }
    }

    /*
    *   BEGIN:  Movement and Stop
    */
    public IEnumerator Movement()
    {
        ICommandablePiece commandablePiece = piece as ICommandablePiece;
        if (!commandablePiece.ICP_IsIdle()) yield break;

        bool animateStartAndEnd = piece as AbstractCombatPiece2;

        if (piece.currentTile != piece.targetTile)
        {
            stateMove = true;
            if (animateStartAndEnd) yield return StartCoroutine(MovementStart());
            yield return StartCoroutine(MovementGoing());
            if (animateStartAndEnd) yield return StartCoroutine(MovementEnd());
            stateMove = false;
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
    public IEnumerator Stop()
    {
        stopWasCalled = stateMove;
        yield return true;
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

            if (piece.nextTile && currentPos == nextPos)
            {
                //Doing this may seem redundant, but it actually fixes some floating point issues that can cause movement overshooting.
                //Moving to the bottom edge or left edge of the grid without this fix may cause the piece to be read as over a tile with coordinate equal to -1.
                transform.position = nextPos;

                piece.currentTile.occupantPiece = null;
                piece.nextTile.occupantPiece = piece;
                piece.currentTile = piece.nextTile;
                piece.nextTile = null;
            }

            if (!piece.nextTile)
            {
                if (!stopWasCalled &&
                    path.Count > 0 &&
                    path[0].moveCost <= movementPointsCurrent)
                {
                    PathNode pNode = path[0];
                    movementPointsCurrent -= pNode.moveCost;
                    path.RemoveAt(0);

                    piece.nextTile = pNode.tile;
                    OctoDirXZ dirToLook = piece.currentTile.GetNeighbourDirection(piece.nextTile);
                    LookAtDirection(dirToLook);
                }
                else
                {
                    stopWasCalled = false;
                    doStop = true;
                }
            }

            if (piece.nextTile && piece.nextTile.occupantPiece)
            {
                //If the piece has a way to interact with other pieces, make it happen here.
                ICommandablePiece commandablePiece = piece as ICommandablePiece;
                if (commandablePiece != null)
                {
                    commandablePiece.ICP_InteractWithTargetPiece(piece.nextTile.occupantPiece, false);
                }

                //Doing this here prevents that a piece walks over the spot of another removed piece.
                piece.nextTile = null;
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
                nextPos = piece.nextTile.transform.position;
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
