using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI_Panel_Building_MageGuild : TownUI_Panel_Building
{
    public void UpdateUI()
    {
        Town town = TownManager.Instance.townPiece.town;
        TownBuilding building = town.mageGuild;
        buildingImage.sprite = building.dbTownStructure.structureImage;
        buildingName.text = building.dbTownStructure.structureName;
        buildingDescription.text = building.dbTownStructure.structureDescription;
    }
}
