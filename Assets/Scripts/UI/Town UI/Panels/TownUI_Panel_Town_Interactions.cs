using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI_Panel_Town_Interactions : AUIPanel
{
    public Button btnTownReport;
    public Button btnBuildStructure;
    public Button btnSpellResearch;
    public Button btnTechResearch;
    public Button btnTradeResources;
    public Button btnHireHero;
    public Button btnHireCreature;
    public Button btnHireSiege;

    public void HideButtons()
    {
        btnTownReport.gameObject.SetActive(false);
        btnBuildStructure.gameObject.SetActive(false);
        btnHireHero.gameObject.SetActive(false);
        btnHireCreature.gameObject.SetActive(false);
        btnHireSiege.gameObject.SetActive(false);
        btnTradeResources.gameObject.SetActive(false);
        btnSpellResearch.gameObject.SetActive(false);
        btnTechResearch.gameObject.SetActive(false);
    }

    public void ShowButtons()
    {
        btnTownReport.gameObject.SetActive(true);
        btnBuildStructure.gameObject.SetActive(true);
        btnHireHero.gameObject.SetActive(true);
        btnHireCreature.gameObject.SetActive(true);
        btnHireSiege.gameObject.SetActive(true);
        btnTradeResources.gameObject.SetActive(true);
        btnSpellResearch.gameObject.SetActive(true);
        btnTechResearch.gameObject.SetActive(true);
    }

    public void UpdatePanel()
    {
        //TODO this?
    }
}
