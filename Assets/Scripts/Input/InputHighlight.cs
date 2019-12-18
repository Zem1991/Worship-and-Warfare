using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHighlight : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public void ChangeSprite(Sprite s, Color c, int sortingOrder)
    {
        if (!spriteRenderer) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = s;
        spriteRenderer.color = c;
        spriteRenderer.sortingOrder = sortingOrder;
    }
}
