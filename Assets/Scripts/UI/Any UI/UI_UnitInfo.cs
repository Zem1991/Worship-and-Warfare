using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitInfo : MonoBehaviour//, IShowableHideable
{
    [Header("Object components")]
    public Image imgUnitPortrait;
    public Text txtUnitName;
    public Text txtUnitStack;

    public void ClearInfo()
    {
        if (imgUnitPortrait) imgUnitPortrait.sprite = null;
        if (txtUnitName) txtUnitName.text = "--";
        if (txtUnitStack) txtUnitStack.text = "--";
    }

    public void RefreshInfo(PartySlot slot)
    {
        CombatUnit unit = slot?.Get() as CombatUnit;
        RefreshInfo(unit);
    }

    public void RefreshInfo(CombatUnit unit)
    {
        ClearInfo();
        if (unit == null) return;

        if (imgUnitPortrait) imgUnitPortrait.sprite = unit.GetDBCombatUnit().profilePicture;
        if (txtUnitName) txtUnitName.text = unit.GetDBCombatUnit().unitNameSingular;
        if (txtUnitStack) txtUnitStack.text = unit.GetStackHealthStats().GetStackSize().ToString();// + "/" + unit.stackStats.stack_maximum;
    }
}
