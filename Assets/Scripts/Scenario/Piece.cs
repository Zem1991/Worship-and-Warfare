using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Data")]
    public new string name = "Unnamed Piece";
    public Vector2 mapPosition;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
}
