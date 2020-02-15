using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI : AbstractSingleton<TownUI>, IUIScheme, IShowableHideable
{
    [Header("Town components")]
    public Image background;
    public RectTransform buildingsHolder;
    public List<TownUI_Building> tuiBuildings = new List<TownUI_Building>();

    [Header("Panels")]
    public TownUI_Panel_CoreButtons coreButtons;
    public TownUI_Panel_Resources resources;
    public TownUI_Panel_Timers timers;
    public TownUI_Panel_Minimap minimap;
    public TownUI_Panel_Town townPanel;
    public TownUI_Panel_Parties parties;
    public TownUI_Panel_Crest crest;

    [Header("Windows")]
    public TownUI_Panel_BuildStructure buildStructure;
    public TownUI_Panel_RecruitHero recruitHero;
    public TownUI_Panel_RecruitCreature recruitCreature;

    [Header("Current Window")]
    public AUIPanel currentWindow;

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

        BuildStructureHide();
        RecruitHeroHide();
        RecruitCreatureHide();
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
        if (currentWindow == buildStructure) BuildStructureHide();
        if (currentWindow == recruitHero) RecruitHeroHide();
        if (currentWindow == recruitCreature) RecruitCreatureHide();
    }

    public void UpdatePanels()
    {
        TownManager tm = TownManager.Instance;

        coreButtons.UpdatePanel();
        resources.UpdatePanel();
        timers.UpdatePanel();
        minimap.UpdatePanel();
        crest.UpdatePanel();

        Town town = tm.townPiece.town;
        townPanel.UpdatePanel(town);

        Party visitor = tm.townPiece.visitor.party as Party;
        Party garrison = tm.townPiece.town.garrison as Party;
        parties.UpdatePanel(visitor, garrison);
    }

    public void DestroyTown()
    {
        DestroyTownBuildings();
    }

    public void CreateTown()
    {
        TownManager tm = TownManager.Instance;

        List<TownBuilding> townBuildings = tm.townPiece.town.buildings;
        //PartyPiece2 visitor = tm.townPiece.visitor;
        //PartyPiece2 garrison = tm.townPiece.garrison;

        CreateTownBuildings(townBuildings);
        //CreateVisitorParty(visitor);
        //CreateGarrisonParty(visitor);
    }

    private void DestroyTownBuildings()
    {
        foreach (TownUI_Building item in tuiBuildings) Destroy(item.gameObject);
        tuiBuildings.Clear();
    }

    private void CreateTownBuildings(List<TownBuilding> townBuildings)
    {
        DestroyTownBuildings();

        foreach (TownBuilding bldg in townBuildings)
        {
            CreateTownBuilding(bldg);
        }
    }

    public void CreateTownBuilding(TownBuilding townBuilding)
    {
        TownUI_Building prefab = AllPrefabs.Instance.tuiBuilding;
        TownUI_Building newTUI = Instantiate(prefab, buildingsHolder.transform);
        newTUI.Initialize(townBuilding);
        tuiBuildings.Add(newTUI);
    }

    public void BuildStructureHide()
    {
        buildStructure.Hide();
        currentWindow = null;
        UIManager.Instance.PointerExit(buildStructure);
    }

    public void BuildStructureShow()
    {
        buildStructure.Show();
        buildStructure.ShowOptions();
        currentWindow = buildStructure;
    }

    public void RecruitHeroHide()
    {
        recruitHero.Hide();
        currentWindow = null;
        UIManager.Instance.PointerExit(recruitHero);
    }

    public void RecruitHeroShow()
    {
        recruitHero.Show();
        currentWindow = recruitHero;
    }

    public void RecruitCreatureHide()
    {
        recruitCreature.Hide();
        currentWindow = null;
        UIManager.Instance.PointerExit(recruitCreature);
    }

    public void RecruitCreatureShow()
    {
        recruitCreature.Show();
        currentWindow = recruitCreature;
    }
}
