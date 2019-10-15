using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public abstract class AbstractPiece : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;

    public static readonly float movementSpeed = 5F;

    [Header("Identification")]
    public Player owner;

    [Header("Movement variables")]
    public List<PathNode> path = new List<PathNode>();
    public int pathTotalCost;
    public int movementPointsCurrent;
    public int movementPointsMax;
    public bool inMovement;
    public bool stopWasCalled;

    [Header("Movement references")]
    public AbstractTile currentTile;
    public AbstractTile targetTile;
    public AbstractTile nextTile;
    public Vector3 nextPos;
    public Vector3 direction;
    public Vector3 velocity;

    [Header("Targeting")]
    public AbstractPiece targetPiece;

    public void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
        LookAtDirection(OctoDirXZ.BACK);
    }

    public virtual void Update()
    {
        AnimatorParameters();
        MakeMove();
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
        if (direction.x == -1) FlipSpriteHorizontally(true);
        if (direction.x == 1) FlipSpriteHorizontally(false);
    }

    public void FlipSpriteHorizontally(bool flip)
    {
        if (!spriteRenderer) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.flipX = flip;
    }

    public void SetAnimatorOverrideController(AnimatorOverrideController aoc)
    {
        if (!animator) animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = aoc;
    }

    public bool HasPath()
    {
        return path?.Count > 0;
    }

    public bool HasPath(AbstractTile targetTile)
    {
        return HasPath() && this.targetTile == targetTile;
    }

    public void SetPath(PathfindResults pathfindResults, AbstractTile targetTile)
    {
        path = pathfindResults.path;
        pathTotalCost = Mathf.CeilToInt(pathfindResults.pathTotalCost);
        this.targetTile = targetTile;
        //Debug.Log("PIECE " + name + " got a new path with size " + pathCost);

        if (path != null && path.Count > 1)
        {
            nextTile = null;

            AbstractTile from = path[0].tile;
            AbstractTile to = path[1].tile;
            OctoDirXZ dir = from.GetNeighbourDirection(to);
            LookAtDirection(dir);
        }
    }

    public void Move()
    {
        inMovement = true;
    }

    public void Stop()
    {
        stopWasCalled = true;
    }

    public virtual bool IsIdle()
    {
        return !inMovement;
    }

    protected virtual void MakeMove()
    {
        if (!inMovement) return;

        Vector3 currentPos = transform.position;
        bool doStop = false;

        if (nextTile && currentPos == nextPos)
        {
            //Doing this may seem redundant, but it actually fixes some floating point issues that can cause movement overshooting
            //Moving to the bottom or left edge of the grid without this fix may cause the Unit to be read as over a tile with coordinate equal to -1
            transform.position = nextPos;

            currentTile.occupantPiece = null;
            nextTile.occupantPiece = this;
            currentTile = nextTile;
            nextTile = null;
        }

        if (nextTile == null)
        {
            if (!stopWasCalled &&
                path.Count > 0 &&
                path[0].moveCost <= movementPointsCurrent)
            {
                PathNode pNode = path[0];
                movementPointsCurrent -= pNode.moveCost;
                path.RemoveAt(0);

                nextTile = pNode.tile;
                OctoDirXZ dirToLook = currentTile.GetNeighbourDirection(nextTile);
                LookAtDirection(dirToLook);

                // If the next tile is the target tile, and the target tile has a piece over it,
                // then instead of performing one more move we perform an interaction between pieces.
                if (nextTile == targetTile)
                {
                    targetPiece = nextTile.occupantPiece;
                    if (targetPiece) doStop = true;
                }
            }
            else
            {
                stopWasCalled = false;
                doStop = true;
            }
        }

        if (doStop)
        {
            inMovement = false;
            nextPos = Vector3.zero;
            velocity = Vector3.zero;
        }
        else
        {
            nextPos = nextTile.transform.position;
        }

        if (targetPiece)
        {
            nextTile = null;    //Doing this here prevents that a piece walks over the spot of another removed piece.
            PerformPieceInteraction();
        }

        if (inMovement)
        {
            direction = (nextPos - currentPos).normalized;
            velocity = direction * movementSpeed;

            Vector3 frameVelocity = velocity * Time.deltaTime;
            float distance = Vector3.Distance(currentPos, nextPos);
            if (frameVelocity.magnitude > distance) frameVelocity = Vector3.ClampMagnitude(frameVelocity, distance);
            transform.Translate(frameVelocity, Space.World);
        }
    }

    protected abstract void AnimatorParameters();
    public abstract void ResetMovementPoints();
    public abstract void StartTurn();
    public abstract void EndTurn();
    public abstract void InteractWithTile(AbstractTile target, bool canPathfind);
    public abstract void InteractWithPiece(AbstractPiece target, bool canPathfind);
    public abstract void PerformPieceInteraction();
}
