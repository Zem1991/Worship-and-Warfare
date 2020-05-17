using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CombatActorInspector : AbstractUIPanel, IShowableHideable
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

    public void RefreshInfo(CombatantPiece3 acap)
    {
        HeroUnitPiece3 hero = acap as HeroUnitPiece3;
        CombatUnitPiece3 unit = acap as CombatUnitPiece3;

        string name = "Unknown piece";
        if (hero) name = hero.GetHeroUnit().dbHeroPerson.heroName;
        else if (unit) name = unit.GetCombatUnit().GetDBCombatUnit().unitNameSingular;
        txtName.text = name;

        main.RefreshInfo(acap);
        offense.RefreshInfo(acap);
        defense.RefreshInfo(acap);
        movement.RefreshInfo(acap);
        other.RefreshInfo(acap);
    }
}
