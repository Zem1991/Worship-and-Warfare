using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI_Panel_RecruitHero : AbstractUIPanel
{
    [Header("UI Elements")]
    public RectTransform heroOptionsHolder;
    public Text txtDescriptionAndCosts;
    public Button btnCancel;
    public Button btnRecruit;

    [Header("Options list")]
    public List<TownUI_Panel_RecruitHero_HeroOption> heroOptions = new List<TownUI_Panel_RecruitHero_HeroOption>();
    public TownUI_Panel_RecruitHero_HeroOption selectedOption;

    public void ClearOptions()
    {
        foreach (TownUI_Panel_RecruitHero_HeroOption item in heroOptions) Destroy(item.gameObject);
        heroOptions.Clear();

        btnRecruit.interactable = false;
        selectedOption = null;
    }

    public void ShowOptions()
    {
        ClearOptions();

        TownManager tm = TownManager.Instance;
        Town town = tm.townPiece.town;
        List<DB_HeroUnit> dbClasses = town.dbFaction.factionTree.GetHeroClasses();

        TownUI_Panel_RecruitHero_HeroOption prefab = AllPrefabs.Instance.tuiHeroOption;

        foreach (DB_HeroUnit dbClass in dbClasses)
        {
            List<DB_HeroPerson> dbHeroes = town.dbFaction.factionTree.GetHeroes(dbClass);
            if (dbHeroes.Count <= 0) continue;

            int index = Random.Range(0, dbHeroes.Count - 1);    //TODO better hero selection
            DB_HeroPerson dbHero = dbHeroes[index];

            TownUI_Panel_RecruitHero_HeroOption newTuiRcCo = Instantiate(prefab, heroOptionsHolder);
            newTuiRcCo.parentPanel = this;
            newTuiRcCo.dbHero = dbHero;
            newTuiRcCo.txtHeroName.text = dbHero.heroName;
            newTuiRcCo.heroImage.sprite = dbHero.profilePicture;

            heroOptions.Add(newTuiRcCo);
        }
    }

    public void SelectOption(TownUI_Panel_RecruitHero_HeroOption selectedOption)
    {
        this.selectedOption = selectedOption;

        TownManager tm = TownManager.Instance;
        Player owner = tm.townPiece.pieceOwner.GetOwner();

        DB_HeroPerson dbHero = selectedOption.dbHero;
        Dictionary<ResourceStats2, int> costs = dbHero.heroClass.resourceStats.GetCosts(1);

        txtDescriptionAndCosts.text = dbHero.heroClass.GetDescriptionWithCosts();
        btnRecruit.interactable = owner.currentResources.CanAfford(costs);
    }

    public void RecruitHero()
    {
        TownManager tm = TownManager.Instance;
        Town town = tm.townPiece.town;
        Party party = town.GetGarrison();

        Player owner = tm.townPiece.pieceOwner.GetOwner();
        Dictionary<ResourceStats2, int> costs = selectedOption.dbHero.heroClass.resourceStats.GetCosts(1);
        owner.currentResources.Subtract(costs);

        TownUI townUI = TownUI.Instance;
        townUI.CloseCurrentWindow();

        HeroUnit prefab = AllPrefabs.Instance.heroUnit;
        HeroUnit hero = Instantiate(prefab, party.GetHeroSlot().transform);
        hero.Initialize(selectedOption.dbHero, null, null);
        party.Add(hero);

        btnRecruit.interactable = false;
        selectedOption = null;
    }
}
