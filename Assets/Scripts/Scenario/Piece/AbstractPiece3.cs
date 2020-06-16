using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractPiece3 : MonoBehaviour
{
    [Header("Object components")]
    public Animator animator;
    public SpriteRenderer mainSpriteRenderer;

    [Header("Pathfinding references")]
    public AbstractTile pathDepartureTile;
    public AbstractTile pathPreviousTile;
    public AbstractTile currentTile;
    public AbstractTile pathNextTile;
    public AbstractTile pathArrivalTile;

    [Header("Targeting references")]
    public Vector3 targetPosition;
    public AbstractTile targetTile;
    public AbstractPiece3 targetPiece;
    public AES_Action targetAction;

    // Update is called once per frame
    protected virtual void Update()
    {
        AP3_UpdateAnimatorParameters();
    }

    public void SetAnimatorOverrideController(AnimatorOverrideController aoc)
    {
        if (!animator) animator = GetComponentInChildren<Animator>();
        animator.runtimeAnimatorController = aoc;
    }

    public void FlipSpriteHorizontally(bool flip)
    {
        mainSpriteRenderer.flipX = flip;
    }

    public void SetMainSprite(Sprite sprite, int sortingOrder)
    {
        mainSpriteRenderer.sprite = sprite;
        mainSpriteRenderer.sortingOrder = sortingOrder;
    }

    public IEnumerator WaitForAnimationStartAndEnd(string animationName)
    {
        //Debug.Log(GetType() + " '" + name + "' is now waiting for animation '" + animationName + "' to start and finish.");
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

    protected abstract void AP3_UpdateAnimatorParameters();
}
