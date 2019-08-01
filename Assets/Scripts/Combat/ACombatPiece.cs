using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ACombatPiece : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public CombatTile tile { get; set; }

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void ChangeSprite(Sprite s)
    {
        spriteRenderer.sprite = s;
    }
}
