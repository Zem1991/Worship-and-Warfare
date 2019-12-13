using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public class CombatTile : AbstractTile
{
    [Header("Database References")]
    public DB_Tileset db_tileset_battleground;

    [Header("Renderers")]
    public SpriteRenderer landRenderer;

    [Header("Dead pieces")]
    public List<AbstractCombatActorPiece2> deadPieces = new List<AbstractCombatActorPiece2>();

    public void ChangeLandSprite(Sprite s)
    {
        landRenderer.sprite = s;
    }
}
