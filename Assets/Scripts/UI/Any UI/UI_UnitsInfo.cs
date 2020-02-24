using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitsInfo : MonoBehaviour, IShowableHideable
{
    public UI_UnitInfo[] unitsInfo;

    //public void Awake()
    //{
    //    if (unitsInfo == null)
    //    {
    //        unitsInfo = new UI_UnitInfo[PartyConstants.MAX_UNITS];
    //        UI_UnitInfo[] foundComponents = GetComponentsInChildren<UI_UnitInfo>();
    //        for (int i = 0; i < foundComponents.Length; i++)
    //        {
    //            unitsInfo[i] = foundComponents[i];
    //        }
    //    }
    //}

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void RefreshInfo(PartySlot[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            Unit unit = slots[i]?.slotObj as Unit;
            unitsInfo[i].RefreshInfo(unit);
        }
    }
}
