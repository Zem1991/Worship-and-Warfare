using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPiece2 : MonoBehaviour
{
    protected Animator animator;
    protected SpriteRenderer mainSpriteRenderer;
    protected SpriteRenderer flagSpriteRenderer;

    [Header("Pathfinding references")]
    public AbstractTile currentTile;
    public AbstractTile pathNextTile;
    public AbstractTile pathTargetTile;

    [Header("Targeting references")]
    public AbstractTile targetTile;
    public AbstractPiece2 targetPiece;

    protected virtual void ManualAwake()
    {
        animator = GetComponentInChildren<Animator>();

        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        if (renderers.Length > 0) mainSpriteRenderer = renderers[0];
        if (renderers.Length > 1) flagSpriteRenderer = renderers[1];
    }

    protected virtual void Update()
    {
        AP2_UpdateAnimatorParameters();
    }

    public void SetAnimatorOverrideController(AnimatorOverrideController aoc)
    {
        if (!animator) animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = aoc;
    }

    public IEnumerator WaitForAnimationStartAndEnd(string animationName)
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
        while (!state.IsName(animationName))
        {
            yield return null;
            state = animator.GetCurrentAnimatorStateInfo(0);
        }
        while (state.normalizedTime < 1)
        {
            yield return null;
            state = animator.GetCurrentAnimatorStateInfo(0);
        }
    }

    public void FlipSpriteHorizontally(bool flip)
    {
        if (!mainSpriteRenderer) mainSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[0];
        mainSpriteRenderer.flipX = flip;
    }

    public void SetMainSprite(Sprite sprite)
    {
        if (!mainSpriteRenderer) mainSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[0];
        mainSpriteRenderer.sprite = sprite;
    }

    public void SetFlagSprite(Sprite sprite)
    {
        if (!flagSpriteRenderer) flagSpriteRenderer = GetComponentsInChildren<SpriteRenderer>()[1];
        flagSpriteRenderer.sprite = sprite;
    }

    protected abstract void AP2_UpdateAnimatorParameters();
}
