using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI_Panel_Building_Temple : TownUI_Panel_Building
{
    public void UpdateUI()
    {
        Town town = TownManager.Instance.townPiece.town;
        TownBuilding building = town.temple;
        buildingImage.sprite = building.dbTownStructure.structureImage;
        buildingName.text = building.dbTownStructure.structureName;
        buildingDescription.text = building.dbTownStructure.structureDescription;
    }
}
