using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZemDirections;

public class FieldTile : AbstractTile
{
    [Header("Tilesets Used")]
    public string lowerLandId;
    public string upperLandId;
    public string waterId;
    public string featureId;
    public string roadId;

    [Header("Renderers")]
    public SpriteRenderer landRenderer;
    public SpriteRenderer featureRenderer;

    public void ChangeLandSprite(Sprite s)
    {
        landRenderer.sprite = s;
    }

    public void ChangeFeatureSprite(Sprite s)
    {
        featureRenderer.sprite = s;
    }
}
