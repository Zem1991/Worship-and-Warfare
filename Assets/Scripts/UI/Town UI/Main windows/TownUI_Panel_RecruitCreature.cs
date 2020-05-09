using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TownUI_Panel_RecruitCreature : AbstractUIPanel
{
    [Header("UI Elements")]
    public RectTransform creatureOptionsHolder;
    public Text txtDescriptionAndCosts;
    public Button btnCancel;
    public Button btnRecruit;

    [Header("Options list")]
    public List<TownUI_Panel_RecruitCreature_CreatureOption> creatureOptions = new List<TownUI_Panel_RecruitCreature_CreatureOption>();

    public void ClearOptions()
    {
        foreach (TownUI_Panel_RecruitCreature_CreatureOption item in creatureOptions) Destroy(item.gameObject);
        creatureOptions.Clear();
    }

    public void ShowOptions()
    {
        ClearOptions();

        TownManager tm = TownManager.Instance;
        Town town = tm.townPiece.town;
        List<DB_CombatUnit> dbUnits = town.dbFaction.factionTree.GetUnits();

        TownUI_Panel_RecruitCreature_CreatureOption prefab = AllPrefabs.Instance.tuiCreatureOption;

        foreach (DB_CombatUnit dbUnit in dbUnits)
        {
            TownUI_Panel_RecruitCreature_CreatureOption newTuiRcCo = Instantiate(prefab, creatureOptionsHolder);
            newTuiRcCo.parentPanel = this;
            newTuiRcCo.dbUnit = dbUnit;
            newTuiRcCo.txtUnitName.text = dbUnit.unitNameSingular;
            newTuiRcCo.unitImage.sprite = dbUnit.profilePicture;
            newTuiRcCo.txtAvailable.text = "" + 9999;       //dbUnit.nameSingular; //TODO add current available amount to recruit
            newTuiRcCo.inpAmount.text = "" + 0;

            creatureOptions.Add(newTuiRcCo);
        }
    }

    public void CheckAmounts()
    {
        TownManager tm = TownManager.Instance;
        Player owner = tm.townPiece.pieceOwner.GetOwner();

        Dictionary<ResourceStats, int> costs = new Dictionary<ResourceStats, int>();
        foreach (TownUI_Panel_RecruitCreature_CreatureOption option in creatureOptions)
        {
            ResourceStats optionCosts = option.dbUnit.resourceStats;
            costs.Add(option.dbUnit.resourceStats, option.amount);
        }

        txtDescriptionAndCosts.text = owner.currentResources.WrittenForm(costs);
        btnRecruit.interactable = owner.currentResources.CanAfford(costs);
    }

    public void RecruitCreatures()
    {
        TownManager tm = TownManager.Instance;
        Town town = tm.townPiece.town;
        Party party = town.garrison;

        TownUI townUI = TownUI.Instance;
        townUI.CloseCurrentWindow();

        foreach (TownUI_Panel_RecruitCreature_CreatureOption option in creatureOptions)
        {
            DB_CombatUnit dbUnit = option.dbUnit;
            int amount = int.Parse(option.inpAmount.text);
            if (amount <= 0) continue;

            Unit prefab = AllPrefabs.Instance.unit;
            Unit unit = Instantiate(prefab, party.transform);
            unit.Initialize(dbUnit, amount);
            party.MergeOrAdd(unit);
        }
    }
}
