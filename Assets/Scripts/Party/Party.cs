using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : AbstractSlotContainer<PartySlot, AbstractPartyElement>
{
    [Header("Party slots")]
    [SerializeField] private PartySlot hero;
    [SerializeField] private List<PartySlot> units;

    public void Initialize()
    {
        PartySlot prefabPartySlot = AllPrefabs.Instance.partySlot;

        PartySlot hero = Instantiate(prefabPartySlot, transform);
        hero.Initialize(this, PartyElementType.HERO);
        this.hero = hero;

        units = new List<PartySlot>();
        for (int i = 0; i < PartyConstants.MAX_UNITS; i++)
        {
            PartySlot unit = Instantiate(prefabPartySlot, transform);
            unit.Initialize(this, PartyElementType.CREATURE, i);
            units.Add(unit);
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
            hero.Set(actualHero);
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
                units[i].Set(unit);
            }
        }
    }

    public override bool Add(AbstractPartyElement item)
    {
        foreach (PartySlot slot in units)
        {
            if (!slot.Has())
            {
                slot.Set(item);
                return true;
            }
        }
        return false;
    }

    public override bool Remove(AbstractPartyElement item)
    {
        foreach (PartySlot slot in units)
        {
            if (slot.Has(item))
            {
                slot.Clear();
                return true;
            }
        }
        return false;
    }

    public override bool Swap(AbstractSlot<AbstractPartyElement> fromSlot, AbstractSlot<AbstractPartyElement> toSlot)
    {
        throw new NotImplementedException();
    }

    public void ClearParty()
    {
        if (hero.Get()) Destroy(hero.Get().gameObject);
        hero.Set(null);

        foreach (PartySlot unitSlot in units)
        {
            if (unitSlot.Get()) Destroy(unitSlot.Get().gameObject);
            unitSlot.Set(null);
        }

        Debug.LogWarning("PARTY CLEARED!");
    }

    public AbstractPartyElement GetMostRelevant()
    {
        AbstractPartyElement result = hero.Get();
        if (!result)
        {
            //TODO Need to actually identify which unit in the party is the most relevant.
            foreach (PartySlot slot in units)
            {
                result = slot.Get() as Unit;
                if (result) break;
            }
        }
        return result;
    }

    public PartySlot GetHeroSlot()
    {
        return hero;
    }

    public PartySlot GetUnitSlot(int id)
    {
        return units[id];
    }

    public List<PartySlot> GetUnitSlots()
    {
        return units;
    }
}
