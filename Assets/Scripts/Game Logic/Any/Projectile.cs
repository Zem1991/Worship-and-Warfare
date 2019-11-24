using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public static readonly float movementSpeed = 25F;

    protected Animator animator;
    protected SpriteRenderer mainSpriteRenderer;

    [Header("Setup")]
    public Vector3 casterPos;
    public Vector3 targetPos;
    public AbstractPiece2 casterPiece;
    public AbstractPiece2 targetPiece;
    public bool inMove;

    [Header("Movement")]
    public Vector3 direction;
    public Vector3 velocity;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
        if (renderers.Length > 0) mainSpriteRenderer = renderers[0];
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

    public void SetupAndGo(AttackStats attack, AbstractPiece2 casterPiece, Vector3 targetPos)
    {
        casterPos = casterPiece.transform.position;
        this.targetPos = targetPos;
        this.casterPiece = casterPiece;
    }

    public void SetupAndGo(AttackStats attack, AbstractPiece2 casterPiece, AbstractPiece2 targetPiece)
    {
        this.targetPiece = targetPiece;
        SetupAndGo(attack, casterPiece, targetPiece.transform.position);
    }

    public IEnumerator MakeTrajectory()
    {
        inMove = true;
        while (inMove)
        {
            Vector3 currentPos = transform.position;

            direction = (targetPos - currentPos).normalized;
            velocity = direction * movementSpeed;

            Vector3 frameVelocity = velocity * Time.deltaTime;
            float distance = Vector3.Distance(currentPos, targetPos);
            if (frameVelocity.magnitude > distance) frameVelocity = Vector3.ClampMagnitude(frameVelocity, distance);
            transform.Translate(frameVelocity, Space.World);

            if (currentPos == targetPos)
            {
                //Doing this may seem redundant, but it actually fixes some floating point issues that can cause movement overshooting.
                transform.position = targetPos;
                inMove = false;
            }

            yield return null;
        }
        Destroy(gameObject);
    }
}
