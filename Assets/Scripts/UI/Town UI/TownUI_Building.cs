using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI_Building : MonoBehaviour
{
    [Header("Static reference")]
    public Image image;

    [Header("Dynamic reference")]
    public TownBuilding townBuilding;

    private RectTransform rectTransform;

    public void Initialize(TownBuilding townBuilding)
    {
        rectTransform = GetComponent<RectTransform>();

        DB_TownBuilding dbTownBuilding = townBuilding.dbTownBuilding;

        this.townBuilding = townBuilding;
        name = dbTownBuilding.townBuildingName;
        image.sprite = dbTownBuilding.buildingImage;

        Vector2 pos = new Vector2(dbTownBuilding.rect.x, dbTownBuilding.rect.y);
        //Vector2 scale = new Vector2(dbTownBuilding.rect.x, dbTownBuilding.rect.y);
        rectTransform.anchoredPosition = pos;
        //rectTransform.sizeDelta = scale;  //TODO FIND OUT HOW TO SCALE THIS
    }
}
