using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHighlight : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void ChangeSprite(Sprite s)
    {
        spriteRenderer.sprite = s;
    }
}
