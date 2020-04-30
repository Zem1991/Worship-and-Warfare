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
        hero.Initialize(this, UnitCategory.HERO);
        heroSlot = hero;

        unitSlots = new List<PartySlot>();
        for (int i = 0; i < PartyConstants.MAX_UNITS; i++)
        {
            PartySlot unit = Instantiate(prefabPartySlot, transform);
            unit.Initialize(this, UnitCategory.CREATURE, i);
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

    protected override AbstractSlot<AbstractPartyElement> CreateTempSlot(AbstractSlot<AbstractPartyElement> prefab, AbstractPartyElement item)
    {
        PartySlot temp = base.CreateTempSlot(prefab, item) as PartySlot;
        temp.slotType = item.partyElementType;
        return temp;
    }

    public override bool Add(AbstractPartyElement item)
    {
        PartySlot slot = null;
        switch (item.partyElementType)
        {
            case UnitCategory.HERO:
                if (!heroSlot.Has())
                {
                    slot = heroSlot;
                }
                break;
            case UnitCategory.CREATURE:
                foreach (PartySlot unitSlot in unitSlots)
                {
                    if (!unitSlot.Has())
                    {
                        slot = unitSlot;
                        break;
                    }
                }
                break;
            case UnitCategory.SUPPORT:
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

    public bool MergeOrAdd(Unit unit)
    {
        bool mergeOk = false;
        //Create an temporary PartySlot just to reuse the AddFromSlot function.
        PartySlot prefab = AllPrefabs.Instance.partySlot;
        PartySlot tempSlot = CreateTempSlot(prefab, unit) as PartySlot;
        foreach (PartySlot unitSlot in unitSlots)
        {
            mergeOk = Merge(tempSlot, unitSlot);
            if (mergeOk) break;
        }
        Destroy(tempSlot.gameObject);
        bool result = mergeOk;
        if (!result) result = Add(unit);
        return result;
    }

    public bool MergeOrSwap(PartySlot sourceSlot, PartySlot toSlot)
    {
        bool mergeOk = Merge(sourceSlot, toSlot);
        bool result = mergeOk;
        if (!result) result = Swap(sourceSlot, toSlot);
        return result;
    }

    public bool Merge(PartySlot sourceSlot, PartySlot targetSlot)
    {
        bool sameTypeSlots = sourceSlot.slotType == targetSlot.slotType;
        if (!sameTypeSlots) return false;

        //Merging slots should only work with creature slots.
        if (sourceSlot.slotType != UnitCategory.CREATURE) return false;

        Unit sourceObj = sourceSlot.Get() as Unit;
        if (!sourceObj) return false;

        Unit targetObj = targetSlot.Get() as Unit;
        if (!targetObj) return false;

        bool sameDBContent = sourceObj.dbData == targetObj.dbData;
        if (!sameDBContent) return false;

        //TODO Specific amount to merge? For now its throwing everything against the target slot.
        int amount = sourceObj.stackStats.Get();
        bool fullMerge = true;

        //targetObj.stackStats.Add(amount, true, false);
        targetObj.stackStats.Add(amount);
        if (fullMerge) sourceSlot.Clear();
        return true;
    }

    public bool Split(PartySlot sourceSlot, PartySlot targetSlot)
    {
        bool sameTypeSlots = sourceSlot.slotType == targetSlot.slotType;
        if (!sameTypeSlots) return false;

        //Splitting slots should only work with creature slots.
        if (sourceSlot.slotType != UnitCategory.CREATURE) return false;

        Unit sourceObj = sourceSlot.Get() as Unit;
        if (!sourceObj) return false;

        //The target slot should be empty.
        if (targetSlot.Get()) return false;

        //TODO Specific amount to split? For now its throwing half against the target slot.
        int fullAmount = sourceObj.stackStats.Get();

        //There should be at least 2 units in the stack for splitting.
        if (fullAmount < 2) return false;

        int amountDiv = fullAmount / 2;
        //int amountMod = fullAmount % 2;

        //targetObj.stackStats.Add(amountDiv, true, false);
        Unit prefabUnit = AllPrefabs.Instance.unit;
        Unit newUnit = Instantiate(prefabUnit, targetSlot.transform);
        newUnit.Initialize(sourceObj.dbData, amountDiv);

        sourceObj.stackStats.Subtract(amountDiv);
        targetSlot.Set(newUnit);
        return true;
    }

    public bool SplitHalfFast(PartySlot sourceSlot)
    {
        foreach (PartySlot slot in unitSlots)
        {
            if (Split(sourceSlot, slot)) return true;
        }
        return false;
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
