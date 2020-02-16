using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TownUI_Panel_BuildStructure_StructureOption : MonoBehaviour, IPointerClickHandler
{
    [Header("Static reference")]
    public Text txtBuildingName;
    public Image buildingImage;
    public Image highlightImage;

    [Header("Dynamic reference")]
    public TownUI_Panel_BuildStructure parentPanel;
    public DB_TownBuilding dbTownBuilding;
    public TownBuilding townBuilding;

    public void OnPointerClick(PointerEventData eventData)
    {
        parentPanel.SelectOption(this);
    }
}
