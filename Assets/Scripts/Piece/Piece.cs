using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Identification")]
    public Player owner;

    [Header("Contents")]
    public DB_Hero hero;
    public int heroExperience;
    public DB_Unit[] units;
    public int[] stackSizes;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public Vector2Int GetPosition()
    {
        Vector2Int result = new Vector2Int();
        result.x = (int)transform.position.x;
        result.y = (int)transform.position.y;
        return result;
    }
}
