using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public static readonly float movementSpeed = 5F;

    [Header("Identification")]
    public Player owner;

    [Header("Contents")]
    public DB_Hero hero;
    public int heroExperience;
    public DB_Unit[] units;
    public int[] stackSizes;

    [Header("Pathfinding")]
    public List<PathNode> path = new List<PathNode>();
    public int pathCost;

    [Header("Movement")]
    public bool inMovement;
    public bool stopWasCalled;
    public Tile currentTile;
    public Tile targetTile;
    public Tile nextTile;
    public Vector3 nextPos;
    public Vector3 direction;
    public Vector3 velocity;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        Movement();
    }

    public void ChangeSprite(Sprite s)
    {
        spriteRenderer.sprite = s;
    }

    public bool HasPath(Tile targetTile)
    {
        return path?.Count > 0 && this.targetTile == targetTile;
    }

    public void SetPath(List<PathNode> path, int pathCost, Tile targetTile)
    {
        this.path = path;
        this.pathCost = pathCost;
        this.targetTile = targetTile;

        Debug.Log("PIECE " + name + " got a new path with size " + pathCost);
    }

    public void Move()
    {
        inMovement = true;
    }

    public void Stop()
    {
        stopWasCalled = true;
    }

    private void Movement()
    {
        if (!inMovement) return;

        bool clearData = false;
        Piece pieceToInteract = null;

        if (nextTile == null)
        {

            if (stopWasCalled ||
                path.Count <= 0)
            {
                stopWasCalled = false;
                clearData = true;
            }
            else
            {
                nextTile = path[0].tile;
                path.RemoveAt(0);

                // If the next tile is the target tile, and the target tile has a piece over it,
                // then instead of performing one more move we perform an interaction between pieces.
                if (nextTile == targetTile)
                {
                    pieceToInteract = nextTile.piece;
                    if (pieceToInteract) clearData = true;
                }
            }
        }

        if (clearData)
        {
            inMovement = false;
            nextPos = Vector3.zero;
            direction = Vector3.zero;
            velocity = Vector3.zero;
        }
        if (pieceToInteract)
        {
            nextTile = null;
            PieceManager.Singleton.PiecesAreInteracting(this, pieceToInteract);
        }

        if (inMovement)
        {
            Vector3 currentPos = transform.position;
            nextPos = nextTile.transform.position;

            direction = (nextPos - currentPos).normalized;
            velocity = direction * movementSpeed;

            Vector3 frameVelocity = velocity * Time.deltaTime;
            float distance = Vector3.Distance(currentPos, nextPos);
            if (frameVelocity.magnitude > distance) frameVelocity = Vector3.ClampMagnitude(frameVelocity, distance);
            transform.Translate(frameVelocity, Space.World);

            if (currentPos == nextPos)
            {
                //Doing this may seem redundant, but it actually fixes some floating point issues that can cause movement overshooting
                //Moving to the bottom or left edge of the grid without this fix may cause the Unit to be read as over a tile with coordinate equal to -1
                transform.position = nextPos;

                currentTile.piece = null;
                nextTile.piece = this;
                currentTile = nextTile;
                nextTile = null;
            }
        }
    }
}
