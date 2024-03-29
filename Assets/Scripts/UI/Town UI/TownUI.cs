﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI : AbstractSingleton<TownUI>, IUIScheme, IShowableHideable
{
    [Header("Town components")]
    public Image background;
    public RectTransform buildingsHolder;
    public List<TownUI_Structure> tuiBuildings = new List<TownUI_Structure>();

    [Header("Panels")]
    public TownUI_Panel_CoreButtons coreButtons;
    public TownUI_Panel_Resources resources;
    public TownUI_Panel_Timers timers;
    public TownUI_Panel_Minimap minimap;
    public TownUI_Panel_Town townPanel;
    public TownUI_Panel_Parties parties;
    public TownUI_Panel_Crest crest;

    [Header("Core windows")]
    public TownUI_Panel_BuildStructure buildStructure;
    public TownUI_Panel_RecruitHero recruitHero;
    public TownUI_Panel_RecruitCreature recruitCreature;

    [Header("Building windows")]
    public TownUI_Panel_Building_TownCenter bldgTownCenter;
    public TownUI_Panel_Building_Tavern bldgTavern;
    public TownUI_Panel_Building_Temple bldgTemple;
    public TownUI_Panel_Building_Castle bldgCastle;
    public TownUI_Panel_Building_MageGuild bldgMageGuild;
    public TownUI_Panel_Building_Workshop bldgWorkshop;

    [Header("Current Window")]
    public AbstractUIPanel currentWindow;

    public void Hide()
    {
        gameObject.SetActive(false);

        background.gameObject.SetActive(false);
        buildingsHolder.gameObject.SetActive(false);

        coreButtons.Hide();
        resources.Hide();
        timers.Hide();
        minimap.Hide();
        townPanel.Hide();
        parties.Hide();
        crest.Hide();

        CloseCurrentWindow();
    }

    public void Show()
    {
        gameObject.SetActive(true);

        background.gameObject.SetActive(true);
        buildingsHolder.gameObject.SetActive(true);

        coreButtons.Show();
        resources.Show();
        timers.Show();
        minimap.Show();
        townPanel.Show();
        parties.Show();
        crest.Show();
    }

    public void CloseCurrentWindow()
    {
        UIManager.Instance.PointerExit(currentWindow);
        if (currentWindow) currentWindow.Hide();
        currentWindow = null;
    }

    public void UpdatePanels()
    {
        Player player = PlayerManager.Instance.localPlayer;
        TownManager tm = TownManager.Instance;

        coreButtons.UpdatePanel();
        resources.UpdatePanel(player);
        timers.UpdatePanel();
        minimap.UpdatePanel();
        crest.UpdatePanel();

        townPanel.UpdatePanel(tm.townPiece.town);
        parties.UpdatePanel(tm.townPiece);

        //Don't update windows, only the HUD panels!
    }

    public void DestroyTown()
    {
        DestroyTownStructures();
    }

    public void CreateTown()
    {
        TownManager tm = TownManager.Instance;

        List<AbstractTownStructure> townStructures = tm.townPiece.town.GetStructures();
        //PartyPiece2 visitor = tm.townPiece.visitor;
        //PartyPiece2 garrison = tm.townPiece.garrison;

        CreateTownStructure(townStructures);
        //CreateVisitorParty(visitor);
        //CreateGarrisonParty(visitor);
    }

    private void DestroyTownStructures()
    {
        foreach (TownUI_Structure item in tuiBuildings) Destroy(item.gameObject);
        tuiBuildings.Clear();
    }

    private void CreateTownStructure(List<AbstractTownStructure> townStructures)
    {
        DestroyTownStructures();

        foreach (AbstractTownStructure bldg in townStructures)
        {
            CreateTownStructure(bldg);
        }
    }

    public void CreateTownStructure(AbstractTownStructure townBuilding)
    {
        TownUI_Structure prefab = AllPrefabs.Instance.tuiBuilding;
        TownUI_Structure newTUI = Instantiate(prefab, buildingsHolder.transform);
        newTUI.Initialize(townBuilding);
        tuiBuildings.Add(newTUI);
    }

    //BEGIN:    Core window functions
    //public void CW_BuildStructureHide()
    //{
    //    buildStructure.Hide();
    //    currentWindow = null;
    //    UIManager.Instance.PointerExit(buildStructure);
    //}
    public void CW_BuildStructureShow()
    {
        buildStructure.ShowOptions();
        buildStructure.Show();
        currentWindow = buildStructure;
    }
    //public void CW_RecruitHeroHide()
    //{
    //    recruitHero.Hide();
    //    currentWindow = null;
    //    UIManager.Instance.PointerExit(recruitHero);
    //}
    public void CW_RecruitHeroShow()
    {
        recruitHero.ShowOptions();
        recruitHero.Show();
        currentWindow = recruitHero;
    }
    //public void CW_RecruitCreatureHide()
    //{
    //    recruitCreature.Hide();
    //    currentWindow = null;
    //    UIManager.Instance.PointerExit(recruitCreature);
    //}
    public void CW_RecruitCreatureShow()
    {
        recruitCreature.ShowOptions();
        recruitCreature.Show();
        currentWindow = recruitCreature;
    }
    //END:      Core window functions

    //BEGIN:    Building window functions
    public void BW_ShowFromBuildingType(TownBuildingType type)
    {
        switch (type)
        {
            case TownBuildingType.TOWN_CENTER:
                BW_BuildingTownCenterShow();
                break;
            case TownBuildingType.TAVERN:
                BW_BuildingTavernShow();
                break;
            case TownBuildingType.TEMPLE:
                BW_BuildingTempleShow();
                break;
            case TownBuildingType.CASTLE:
                BW_BuildingCastleShow();
                break;
            case TownBuildingType.MAGE_GUILD:
                BW_BuildingMageGuildShow();
                break;
            case TownBuildingType.WORKSHOP:
                BW_BuildingWorkshopShow();
                break;
            default:
                Debug.LogWarning("No building window to show for " + type);
                break;
        }
    }
    //public void BW_BuildingTownCenterHide()
    //{
    //    bldgTownCenter.Hide();
    //    currentWindow = null;
    //    UIManager.Instance.PointerExit(bldgTownCenter);
    //}
    public void BW_BuildingTownCenterShow()
    {
        bldgTownCenter.UpdateUI();
        bldgTownCenter.Show();
        currentWindow = bldgTownCenter;
    }
    //public void BW_BuildingTavernHide()
    //{
    //    bldgTavern.Hide();
    //    currentWindow = null;
    //    UIManager.Instance.PointerExit(bldgTavern);
    //}
    public void BW_BuildingTavernShow()
    {
        bldgTavern.UpdateUI();
        bldgTavern.Show();
        currentWindow = bldgTavern;
    }
    //public void BW_BuildingTempleHide()
    //{
    //    bldgTemple.Hide();
    //    currentWindow = null;
    //    UIManager.Instance.PointerExit(bldgTemple);
    //}
    public void BW_BuildingTempleShow()
    {
        bldgTemple.UpdateUI();
        bldgTemple.Show();
        currentWindow = bldgTemple;
    }
    //public void BW_BuildingCastleHide()
    //{
    //    bldgCastle.Hide();
    //    currentWindow = null;
    //    UIManager.Instance.PointerExit(bldgCastle);
    //}
    public void BW_BuildingCastleShow()
    {
        bldgCastle.UpdateUI();
        bldgCastle.Show();
        currentWindow = bldgCastle;
    }
    //public void BW_BuildingMageGuildHide()
    //{
    //    bldgMageGuild.Hide();
    //    currentWindow = null;
    //    UIManager.Instance.PointerExit(bldgMageGuild);
    //}
    public void BW_BuildingMageGuildShow()
    {
        bldgMageGuild.UpdateUI();
        bldgMageGuild.Show();
        currentWindow = bldgMageGuild;
    }
    //public void BW_BuildingWorkshopHide()
    //{
    //    bldgWorkshop.Hide();
    //    currentWindow = null;
    //    UIManager.Instance.PointerExit(bldgWorkshop);
    //}
    public void BW_BuildingWorkshopShow()
    {
        bldgWorkshop.UpdateUI();
        bldgWorkshop.Show();
        currentWindow = bldgWorkshop;
    }
    //END:      Building window functions
}
