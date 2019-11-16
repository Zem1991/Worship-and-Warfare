using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHighlight : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public void ChangeSprite(Sprite s, Color c)
    {
        if (!spriteRenderer) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = s;
        spriteRenderer.color = c;
    }
}
