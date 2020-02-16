﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI_Panel_Building_Workshop : TownUI_Panel_Building
{
    public void UpdateUI()
    {
        Town town = TownManager.Instance.townPiece.town;
        TownBuilding building = town.workshop;
        buildingImage.sprite = building.dbTownBuilding.buildingImage;
        buildingName.text = building.dbTownBuilding.townBuildingName;
        buildingDescription.text = building.dbTownBuilding.buildingDescription;
    }
}
