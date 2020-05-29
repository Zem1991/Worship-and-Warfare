using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TownUI_Structure : MonoBehaviour, IPointerClickHandler
{
    [Header("Object components")]
    public Image image;

    [Header("Other references")]
    public AbstractTownStructure townStructure;

    private RectTransform rectTransform;

    public void Initialize(AbstractTownStructure townStructure)
    {
        rectTransform = GetComponent<RectTransform>();

        DB_TownStructure dbTownStructure = townStructure.dbTownStructure;

        this.townStructure = townStructure;
        name = dbTownStructure.structureName;
        image.sprite = dbTownStructure.structureImage;

        Vector2 pos = new Vector2(dbTownStructure.rect.x, dbTownStructure.rect.y);
        //Vector2 scale = new Vector2(dbTownBuilding.rect.x, dbTownBuilding.rect.y);
        rectTransform.anchoredPosition = pos;
        //rectTransform.sizeDelta = scale;  //TODO FIND OUT HOW TO SCALE THIS
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TownBuilding townBuilding = townStructure as TownBuilding;
        if (townBuilding)
        {
            TownUI.Instance.BW_ShowFromBuildingType(townBuilding.GetDBTownBuilding().buildingType);
        }
    }
}
