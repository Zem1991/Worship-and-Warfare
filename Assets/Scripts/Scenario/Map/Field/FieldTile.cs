﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public class FieldTile : AbstractTile
{
    [Header("Database References")]
    public DB_Tileset db_tileset_lowerLand;
    public DB_Tileset db_tileset_water;
    public DB_Tileset db_tileset_feature;

    [Header("Tilesets Used")]
    public string lowerLandId;
    public string upperLandId;
    public string waterId;
    public string featureId;
    public string roadId;

    [Header("Object components")]
    public SpriteRenderer landRenderer;
    public SpriteRenderer waterRenderer;
    public SpriteRenderer featureRenderer;

    public void ChangeLandSprite(Sprite s)
    {
        landRenderer.sprite = s;
        landRenderer.sortingOrder = SpriteOrderConstants.TERRAIN_LAND;
    }

    public void ChangeWaterSprite(Sprite s)
    {
        waterRenderer.sprite = s;
        waterRenderer.sortingOrder = SpriteOrderConstants.TERRAIN_WATER;
    }

    public void ChangeFeatureSprite(Sprite s)
    {
        featureRenderer.sprite = s;
        featureRenderer.sortingOrder = SpriteOrderConstants.TERRAIN_FEATURE;
    }
}
