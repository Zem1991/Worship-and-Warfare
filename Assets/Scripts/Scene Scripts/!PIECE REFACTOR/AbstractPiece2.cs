using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPiece2 : MonoBehaviour
{
    protected SpriteRenderer spriteRenderer;
    protected Animator animator;

    [Header("Player owner/controller")]
    [SerializeField] protected bool canBeOwned;
    [SerializeField] protected Player owner;
    [SerializeField] protected bool canBeControlled;
    [SerializeField] protected Player controller;

    [Header("Identification")]
    public Sprite profilePicture;
    public AbstractTile currentTile;

    protected virtual void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        //Nothing yet...
    }

    protected virtual void Update()
    {
        AP2_AnimatorParameters();
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

    public bool FindOwner(out Player owner)
    {
        owner = this.owner;
        return canBeOwned;
    }

    public bool FindController(out Player owner)
    {
        owner = this.owner;
        return canBeControlled;
    }

    public abstract void AP2_AnimatorParameters();
    public abstract void AP2_PieceInteraction();
}
