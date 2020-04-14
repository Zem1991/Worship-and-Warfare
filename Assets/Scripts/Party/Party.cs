using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Party : AbstractSlotContainer<PartySlot, AbstractPartyElement>
{
    [Header("Party slots")]
    [SerializeField] private PartySlot heroSlot;
    [SerializeField] private List<PartySlot> unitSlots;

    public void Initialize()
    {
        PartySlot prefabPartySlot = AllPrefabs.Instance.partySlot;

        PartySlot hero = Instantiate(prefabPartySlot, transform);
        hero.Initialize(this, PartyElementType.HERO);
        this.heroSlot = hero;

        unitSlots = new List<PartySlot>();
        for (int i = 0; i < PartyConstants.MAX_UNITS; i++)
        {
            PartySlot unit = Instantiate(prefabPartySlot, transform);
            unit.Initialize(this, PartyElementType.CREATURE, i);
            unitSlots.Add(unit);
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

            Hero actualHero = Instantiate(prefabHero, heroSlot.transform);
            actualHero.Initialize(dbData, heroData.experienceData, heroData.inventoryData);
            heroSlot.Set(actualHero);
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

                Unit unit = Instantiate(prefabUnit, unitSlots[i].transform);
                unit.Initialize(dbData, unitData.stackData.stack_maximum);
                unitSlots[i].Set(unit);
            }
        }
    }

    public override bool Add(AbstractPartyElement item)
    {
        PartySlot slot = null;
        switch (item.partyElementType)
        {
            case PartyElementType.HERO:
                if (!heroSlot.Has())
                {
                    slot = heroSlot;
                }
                break;
            case PartyElementType.CREATURE:
                foreach (PartySlot unitSlot in unitSlots)
                {
                    if (!unitSlot.Has())
                    {
                        slot = unitSlot;
                        break;
                    }
                }
                break;
            case PartyElementType.SIEGE_ENGINE:
                break;
            default:
                break;
        }

        bool result;
        if (slot)
        {
            //Create an temporary PartySlot just to reuse the AddFromSlot function.
            PartySlot prefab = AllPrefabs.Instance.partySlot;
            PartySlot temp = CreateTempSlot(prefab, item) as PartySlot;
            result = Swap(temp, slot);
            Destroy(temp.gameObject);
        }
        else
        {
            //We cannot add this element to the party.
            result = false;
        }
        return result;
    }

    public override bool Remove(AbstractPartyElement item)
    {
        foreach (PartySlot slot in unitSlots)
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
        PartySlot fromSlotFix = fromSlot as PartySlot;
        PartySlot toSlotFix = toSlot as PartySlot;

        bool sameTypeSlots = fromSlotFix.slotType == toSlotFix.slotType;

        bool result = false;
        if (sameTypeSlots)
        {
            //Here we just exchange items between slots.
            AbstractPartyElement fromItem = fromSlot.Get();
            fromSlot.Set(toSlot.Get());
            toSlot.Set(fromItem);
            result = true;
        }
        return result;
    }

    public void ClearParty()
    {
        if (heroSlot.Get()) Destroy(heroSlot.Get().gameObject);
        heroSlot.Set(null);

        foreach (PartySlot unitSlot in unitSlots)
        {
            if (unitSlot.Get()) Destroy(unitSlot.Get().gameObject);
            unitSlot.Set(null);
        }

        Debug.LogWarning("PARTY CLEARED!");
    }

    public AbstractPartyElement GetMostRelevant()
    {
        AbstractPartyElement result = heroSlot.Get();
        if (!result)
        {
            //TODO Need to actually identify which unit in the party is the most relevant.
            foreach (PartySlot slot in unitSlots)
            {
                result = slot.Get() as Unit;
                if (result) break;
            }
        }
        return result;
    }

    public PartySlot GetHeroSlot()
    {
        return heroSlot;
    }

    public PartySlot GetUnitSlot(int id)
    {
        return unitSlots[id];
    }

    public List<PartySlot> GetUnitSlots()
    {
        return unitSlots;
    }
}
