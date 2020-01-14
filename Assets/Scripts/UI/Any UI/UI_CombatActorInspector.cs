using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CombatActorInspector : AUIPanel, IShowableHideable
{
    [Header("UI objects")]
    public Text txtName;
    public Button btnClose;

    [Header("Inspector parts")]
    public UI_CombatActorInspector_MainStats main;
    public UI_CombatActorInspector_OffenseStats offense;
    public UI_CombatActorInspector_DefenseStats defense;
    public UI_CombatActorInspector_MovementStats movement;
    public UI_CombatActorInspector_OtherStats other;

    public void RefreshInfo(AbstractCombatActorPiece2 acap)
    {
        CombatantHeroPiece2 hero = acap as CombatantHeroPiece2;
        CombatantUnitPiece2 unit = acap as CombatantUnitPiece2;

        string name = "Unknown piece";
        if (hero) name = hero.hero.dbData.heroName;
        else if (unit) name = unit.unit.dbData.nameSingular;
        txtName.text = name;

        main.RefreshInfo(acap);
        offense.RefreshInfo(acap);
        defense.RefreshInfo(acap);
        movement.RefreshInfo(acap);
        other.RefreshInfo(acap);
    }
}
