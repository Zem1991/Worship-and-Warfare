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
        List<DB_HeroClass> dbClasses = town.dbFaction.factionTree.GetHeroClasses();

        TownUI_Panel_RecruitHero_HeroOption prefab = AllPrefabs.Instance.tuiHeroOption;

        foreach (DB_HeroClass dbClass in dbClasses)
        {
            List<DB_Hero> dbHeroes = town.dbFaction.factionTree.GetHeroes(dbClass);
            if (dbHeroes.Count <= 0) continue;

            int index = Random.Range(0, dbHeroes.Count - 1);    //TODO better hero selection
            DB_Hero dbHero = dbHeroes[index];

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

        DB_Hero dbHero = selectedOption.dbHero;
        Dictionary<ResourceStats, int> costs = dbHero.heroClass.resourceStats.GetCosts(1);

        txtDescriptionAndCosts.text = dbHero.heroClass.GetDescriptionWithCosts();
        btnRecruit.interactable = owner.resourceStats.CanAfford(costs);
    }

    public void RecruitHero()
    {
        TownManager tm = TownManager.Instance;
        Town town = tm.townPiece.town;

        Player owner = tm.townPiece.pieceOwner.GetOwner();
        Dictionary<ResourceStats, int> costs = selectedOption.dbHero.heroClass.resourceStats.GetCosts(1);
        owner.resourceStats.Subtract(costs);

        TownUI townUI = TownUI.Instance;
        townUI.CloseCurrentWindow();

        Hero prefab = AllPrefabs.Instance.hero;
        Party party = town.garrison;

        Hero hero = Instantiate(prefab, party.hero.transform);
        hero.Initialize(selectedOption.dbHero, null, null);
        party.hero.slotObj = hero;

        btnRecruit.interactable = false;
        selectedOption = null;
    }
}
