using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : AbstractSlotContainer<PartySlot, AbstractPartyElement>
{
    [Header("Party contents")]
    public PartySlot hero;
    public PartySlot[] units;

    public void Initialize()
    {
        PartySlot prefabPartySlot = AllPrefabs.Instance.partySlot;

        PartySlot hero = Instantiate(prefabPartySlot, transform);
        hero.Initialize(this, PartyElementType.HERO);
        this.hero = hero;

        units = new PartySlot[PartyConstants.MAX_UNITS];
        for (int i = 0; i < units.Length; i++)
        {
            PartySlot unit = Instantiate(prefabPartySlot, transform);
            unit.Initialize(this, PartyElementType.CREATURE);
            units[i] = unit;
        }
    }

    public void Initialize(PartyData partyData)
    {
        Initialize();

        AbstractDBContentHandler<DB_Hero> dbHeroes = DBHandler_Hero.Instance;
        AbstractDBContentHandler<DB_Unit> dbUnits = DBHandler_Unit.Instance;

        Hero prefabHero = AllPrefabs.Instance.hero;
        Unit prefabUnit = AllPrefabs.Instance.unit;

        if (partyData.hero != null)
        {
            HeroData heroData = partyData.hero;

            string heroId = heroData.id;
            DB_Hero dbData = dbHeroes.Select(heroId);

            Hero actualHero = Instantiate(prefabHero, hero.transform);
            actualHero.Initialize(dbData, heroData.experienceData, heroData.inventoryData);
            hero.slotObj = actualHero;
        }

        if (partyData.units != null)
        {
            if (partyData.units.Length > PartyConstants.MAX_UNITS) Debug.LogWarning("There are more units than the piece can store!");
            int totalUnits = Mathf.Min(partyData.units.Length, PartyConstants.MAX_UNITS);

            for (int i = 0; i < totalUnits; i++)
            {
                UnitData unitData = partyData.units[i];

                string unitId = unitData.id;
                DB_Unit dbData = dbUnits.Select(unitId);

                Unit unit = Instantiate(prefabUnit, units[i].transform);
                unit.Initialize(dbData, unitData.stackData.stack_maximum);
                units[i].slotObj = unit;
            }
        }
    }

    public override bool AddSlotObject(AbstractPartyElement item)
    {
        throw new NotImplementedException();
    }

    public void ClearUnits()
    {
        foreach (PartySlot slot in units)
        {
            Destroy(slot.slotObj.gameObject);
            slot.slotObj = null;
        }
    }

    public void SetUnits(List<Unit> units)
    {
        ClearUnits();

        for (int i = 0; i < units.Count; i++)
        {
            this.units[i].slotObj = units[i];
        }
    }

    public bool AddUnt(Unit unit)
    {
        foreach (PartySlot slot in units)
        {
            if (slot.AddSlotObject(unit)) return true;
        }
        return false;
    }

    public bool RemoveUnt(Unit unit)
    {
        foreach (PartySlot slot in units)
        {
            if (slot.HasSlotObject(unit))
            {
                slot.RemoveSlotObject();
                return true;
            }
        }
        return false;
    }

    public Unit GetRelevantUnit()
    {
        //TODO make a better method than this
        foreach (PartySlot slot in units)
        {
            Unit unit = slot.slotObj as Unit;
            if (unit) return unit;
        }
        return null;
    }
}
