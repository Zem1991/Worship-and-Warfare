using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI_Panel_BuildStructure : AbstractUIPanel
{
    [Header("UI Elements")]
    public RectTransform structureOptionsHolder;
    public Text txtDescriptionAndCosts;
    public Button btnCancel;
    public Button btnBuild;

    [Header("Options list")]
    public List<TownUI_Panel_BuildStructure_StructureOption> structureOptions = new List<TownUI_Panel_BuildStructure_StructureOption>();
    public TownUI_Panel_BuildStructure_StructureOption selectedOption;

    public void ClearOptions()
    {
        foreach (TownUI_Panel_BuildStructure_StructureOption item in structureOptions) Destroy(item.gameObject);
        structureOptions.Clear();

        txtDescriptionAndCosts.text = "";

        btnBuild.interactable = false;
        selectedOption = null;
    }

    public void ShowOptions()
    {
        ClearOptions();

        TownManager tm = TownManager.Instance;
        Town town = tm.townPiece.town;
        List<DB_TownStructure> dbBuildings = town.dbFaction.factionTree.GetBuildings();
        List<AbstractTownStructure> townBuildings = town.GetStructures();     //TODO use an per building approach to better handler certain building types

        TownUI_Panel_BuildStructure_StructureOption prefab = AllPrefabs.Instance.tuiStructureOption;

        foreach (DB_TownStructure dbStruc in dbBuildings)
        {
            AbstractTownStructure foundTownBuilding = null;
            foreach (AbstractTownStructure item in townBuildings)
            {
                if (item.dbTownStructure == dbStruc)
                {
                    foundTownBuilding = item;
                    break;
                }
            }

            Color highlightColor = foundTownBuilding ? tm.highlightBuilt : tm.highlightAvailable;

            TownUI_Panel_BuildStructure_StructureOption newTuiBsSo = Instantiate(prefab, structureOptionsHolder);
            newTuiBsSo.parentPanel = this;
            newTuiBsSo.dbTownStructure = dbStruc;
            newTuiBsSo.townStructure = foundTownBuilding;
            newTuiBsSo.txtBuildingName.text = dbStruc.structureName;
            newTuiBsSo.buildingImage.sprite = dbStruc.structureImage;
            newTuiBsSo.highlightImage.color = highlightColor;

            structureOptions.Add(newTuiBsSo);
        }
    }

    public void SelectOption(TownUI_Panel_BuildStructure_StructureOption selectedOption)
    {
        this.selectedOption = selectedOption;
        if (!selectedOption.townStructure)
        {
            TownManager tm = TownManager.Instance;
            Player owner = tm.townPiece.pieceOwner.GetOwner();

            DB_TownStructure dbTownBuilding = selectedOption.dbTownStructure;
            Dictionary<ResourceStats2, int> costs = dbTownBuilding.resourceStats.GetCosts(1);

            txtDescriptionAndCosts.text =  dbTownBuilding.GetDescriptionWithCosts();
            btnBuild.interactable = owner.currentResources.CanAfford(costs);
        }
        else
        {
            txtDescriptionAndCosts.text = "Already built.";
            btnBuild.interactable = false;
        }
    }

    public void BuildStructure()
    {
        TownManager tm = TownManager.Instance;
        TownPiece3 tp = tm.townPiece;
        Town town = tp.town;
        Player owner = tp.pieceOwner.GetOwner();

        TownUI townUI = TownUI.Instance;
        townUI.CloseCurrentWindow();

        AbstractTownStructure newTB = town.BuildStructure(selectedOption.dbTownStructure, owner);
        townUI.CreateTownStructure(newTB);

        btnBuild.interactable = false;
        selectedOption = null;
    }
}
