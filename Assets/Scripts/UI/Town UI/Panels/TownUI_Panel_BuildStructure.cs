using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI_Panel_BuildStructure : AUIPanel
{
    [Header("UI Elements")]
    public RectTransform structureOptionsHolder;
    public Button btnCancel;
    public Button btnBuild;

    [Header("Options list")]
    public List<TownUI_Panel_BuildStructure_StructureOption> structureOptions = new List<TownUI_Panel_BuildStructure_StructureOption>();
    public TownUI_Panel_BuildStructure_StructureOption selectedOption;

    public void ClearOptions()
    {
        foreach (TownUI_Panel_BuildStructure_StructureOption item in structureOptions) Destroy(item.gameObject);
        structureOptions.Clear();

        btnBuild.interactable = false;
        selectedOption = null;
    }

    public void ShowOptions()
    {
        ClearOptions();

        TownManager tm = TownManager.Instance;
        Town town = tm.townPiece.town;
        List<DB_TownBuilding> dbBuildings = town.dbFaction.townBuildings;
        List<TownBuilding> townBuildings = town.buildings;

        TownUI_Panel_BuildStructure_StructureOption prefab = AllPrefabs.Instance.tuiStructureOption;

        foreach (DB_TownBuilding dbBldg in dbBuildings)
        {
            bool found = false;
            foreach (TownBuilding item in townBuildings)
            {
                if (item.dbTownBuilding == dbBldg)
                {
                    found = true;
                    break;
                }
            }

            Color highlightColor = found ? tm.highlightBuilt : tm.highlightAvailable;

            TownUI_Panel_BuildStructure_StructureOption newTuiPbsSo = Instantiate(prefab, structureOptionsHolder);
            newTuiPbsSo.parentPanel = this;
            newTuiPbsSo.dbTownBuilding = dbBldg;
            newTuiPbsSo.txtBuildingName.text = dbBldg.townBuildingName;
            newTuiPbsSo.buildingImage.sprite = dbBldg.buildingImage;
            newTuiPbsSo.highlightImage.color = highlightColor;

            structureOptions.Add(newTuiPbsSo);
        }
    }

    public void SelectOption(TownUI_Panel_BuildStructure_StructureOption townUI_Panel_BuildStructure_StructureOption)
    {
        btnBuild.interactable = true;
        selectedOption = townUI_Panel_BuildStructure_StructureOption;
    }

    public void BuildStructure()
    {
        TownManager tm = TownManager.Instance;
        Town town = tm.townPiece.town;
        TownUI townUI = TownUI.Instance;
        townUI.CloseCurrentWindow();
        TownBuilding newTB = town.BuildStructure(selectedOption.dbTownBuilding);
        townUI.CreateTownBuilding(newTB);
    }
}
