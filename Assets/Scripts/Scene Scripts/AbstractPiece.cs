﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public abstract class AbstractPiece : MonoBehaviour
{
    //private SpriteRenderer spriteRenderer;
    protected Animator animator;

    public static readonly float movementSpeed = 5F;

    [Header("Identification")]
    public Player owner;

    [Header("Pathfinding")]
    public List<PathNode> path = new List<PathNode>();
    public int pathCost;

    [Header("Movement")]
    public bool inMovement;
    public bool stopWasCalled;
    public AbstractTile currentTile;
    public AbstractTile targetTile;
    public AbstractTile nextTile;
    public Vector3 nextPos;
    public Vector3 direction;
    public Vector3 velocity;

    public virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        LookAtDirection(OctoDirXZ.BACK);
    }

    void Update()
    {
        AnimatorVariables();
        Movement();
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

    public void SetPath(List<PathNode> path, int pathCost, AbstractTile targetTile)
    {
        this.path = path;
        this.pathCost = pathCost;
        this.targetTile = targetTile;
        Debug.Log("PIECE " + name + " got a new path with size " + pathCost);

        if (path != null && path.Count > 1)
        {
            AbstractTile from = path[0].tile;
            AbstractTile to = path[1].tile;
            OctoDirXZ dir = from.GetNeighbourDirection(to);
            LookAtDirection(dir);
        }
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
    }

    public void Move()
    {
        inMovement = true;
    }

    public void Stop()
    {
        stopWasCalled = true;
    }

    protected virtual void Movement()
    {
        if (!inMovement) return;

        Vector3 currentPos = transform.position;
        bool doStop = false;
        AbstractPiece pieceToInteract = null;

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
            if (stopWasCalled ||
                path.Count <= 0)
            {
                stopWasCalled = false;
                doStop = true;
            }
            else
            {
                nextTile = path[0].tile;
                path.RemoveAt(0);

                // If the next tile is the target tile, and the target tile has a piece over it,
                // then instead of performing one more move we perform an interaction between pieces.
                if (nextTile == targetTile)
                {
                    pieceToInteract = nextTile.occupantPiece;
                    if (pieceToInteract) doStop = true;
                }
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

        if (pieceToInteract)
        {
            nextTile = null;
            InteractWithPiece(pieceToInteract);
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

    protected abstract void AnimatorVariables();
    protected abstract void InteractWithPiece(AbstractPiece target);
}
