using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceStats2 : MonoBehaviour
{
    [Header("Costs")]
    public long gold;
    public long ore;
    public long ale;
    public long crystals;
    public long sulfur;

    public void Initialize(ResourceData resourceData)
    {
        if (resourceData == null) return;

        gold = resourceData.gold;
        ore = resourceData.ore;
        ale = resourceData.ale;
        crystals = resourceData.crystals;
        sulfur = resourceData.sulfur;
    }

    private void IterateAndSum(Dictionary<ResourceStats2, int> costs, out long goldCost, out long oreCost, out long aleCost, out long crystalsCost, out long sulfurCost)
    {
        goldCost = 0;
        oreCost = 0;
        aleCost = 0;
        crystalsCost = 0;
        sulfurCost = 0;
        foreach (KeyValuePair<ResourceStats2, int> cost in costs)
        {
            goldCost += cost.Key.gold * cost.Value;
            oreCost += cost.Key.ore * cost.Value;
            aleCost += cost.Key.ale * cost.Value;
            crystalsCost += cost.Key.crystals * cost.Value;
            sulfurCost += cost.Key.sulfur * cost.Value;
        }
    }

    public string WrittenForm(Dictionary<ResourceStats2, int> costs)
    {
        string result = "";
        IterateAndSum(costs, out long goldCost, out long oreCost, out long aleCost, out long crystalsCost, out long sulphurCost);
        if (goldCost > 0) result += goldCost + " gold, ";
        if (oreCost > 0) result += oreCost + " ore, ";
        if (aleCost > 0) result += aleCost + " ale, ";
        if (crystalsCost > 0) result += crystalsCost + " crystals, ";
        if (sulphurCost > 0) result += sulphurCost + " sulphur, ";

        int commaIndex = result.LastIndexOf(',');
        if (commaIndex >= 0) result = result.Remove(commaIndex, 2);
        if (result == "") result = "Its free!";
        return result;
    }

    public Dictionary<ResourceStats2, int> GetCosts(int amount)
    {
        Dictionary<ResourceStats2, int> result = new Dictionary<ResourceStats2, int> { [this] = amount };
        return result;
    }

    public bool CanAfford(Dictionary<ResourceStats2, int> costs)
    {
        IterateAndSum(costs, out long goldCost, out long oreCost, out long aleCost, out long crystalsCost, out long sulfurCost);
        if (gold < goldCost) return false;
        if (ore < oreCost) return false;
        if (ale < aleCost) return false;
        if (crystals < crystalsCost) return false;
        if (sulfur < sulfurCost) return false;
        return true;
    }

    public bool Add(Dictionary<ResourceStats2, int> costs)
    {
        //if (!CanAfford(costs)) return false;
        IterateAndSum(costs, out long goldCost, out long oreCost, out long aleCost, out long crystalsCost, out long sulfurCost);
        gold += goldCost;
        ore += oreCost;
        ale += aleCost;
        crystals += crystalsCost;
        sulfur += sulfurCost;
        return true;
    }

    public bool Subtract(Dictionary<ResourceStats2, int> costs)
    {
        if (!CanAfford(costs)) return false;
        IterateAndSum(costs, out long goldCost, out long oreCost, out long aleCost, out long crystalsCost, out long sulfurCost);
        gold -= goldCost;
        ore -= oreCost;
        ale -= aleCost;
        crystals -= crystalsCost;
        sulfur -= sulfurCost;
        return true;
    }
}
