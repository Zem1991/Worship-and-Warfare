using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_UnitInfo : MonoBehaviour//, IShowableHideable
{
    public Text txtUnitName;
    public Image imgUnitProfile;

    public void RefreshInfo(DB_Unit unit)
    {
        txtUnitName.text = "--";
        imgUnitProfile.sprite = null;

        if (unit == null) return;

        txtUnitName.text = unit.nameSingular;
        imgUnitProfile.sprite = unit.profilePicture;
    }
}
