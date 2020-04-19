using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitInfo : MonoBehaviour//, IShowableHideable
{
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
        Unit unit = slot?.Get() as Unit;
        RefreshInfo(unit);
    }

    public void RefreshInfo(Unit unit)
    {
        ClearInfo();
        if (unit == null) return;

        if (imgUnitPortrait) imgUnitPortrait.sprite = unit.dbData.profilePicture;
        if (txtUnitName) txtUnitName.text = unit.dbData.nameSingular;
        if (txtUnitStack) txtUnitStack.text = unit.stackStats.stack_current + "/" + unit.stackStats.stack_maximum;
    }
}
