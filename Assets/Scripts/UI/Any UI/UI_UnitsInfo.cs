using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitsInfo : MonoBehaviour, IShowableHideable
{
    public List<UI_UnitInfo> unitsInfo = new List<UI_UnitInfo>();

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void RefreshInfo(List<PartySlot> slots)
    {
        for (int i = 0; i < PartyConstants.MAX_UNITS; i++)
        {
            PartySlot partySlot = null;
            if (slots != null) partySlot = slots[i];
            unitsInfo[i].RefreshInfo(partySlot);
        }
    }
}
