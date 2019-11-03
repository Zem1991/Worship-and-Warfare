using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPiece2 : MonoBehaviour
{
    protected Animator animator;
    protected SpriteRenderer mainSpriteRenderer;
    protected SpriteRenderer flagSpriteRenderer;

    [Header("Player owner/controller")]
    [SerializeField] protected bool canBeOwned;
    [SerializeField] protected Player owner;
    [SerializeField] protected bool canBeControlled;
    [SerializeField] protected Player controller;

    [Header("Identification")]
    public AbstractTile currentTile;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        if (renderers.Length > 0) mainSpriteRenderer = renderers[0];
        if (renderers.Length > 1) flagSpriteRenderer = renderers[1];
    }

    protected virtual void Start()
    {
        //Nothing yet...
    }

    protected virtual void Update()
    {
        AP2_AnimatorParameters();
    }

    public void SetAnimatorOverrideController(AnimatorOverrideController aoc)
    {
        if (!animator) animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = aoc;
    }

    public void FlipSpriteHorizontally(bool flip)
    {
        if (!mainSpriteRenderer) mainSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[0];
        mainSpriteRenderer.flipX = flip;
    }

    public void SetMainSprite(Sprite flag)
    {
        if (!mainSpriteRenderer) mainSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[0];
        mainSpriteRenderer.sprite = flag;
    }

    public void SetFlagSprite(Sprite flag)
    {
        if (!flagSpriteRenderer) flagSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1];
        flagSpriteRenderer.sprite = flag;
    }

    public abstract void AP2_AnimatorParameters();
    public abstract void AP2_PieceInteraction();
}
