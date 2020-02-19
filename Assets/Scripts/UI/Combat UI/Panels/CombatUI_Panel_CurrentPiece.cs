using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatUI_Panel_CurrentPiece : AbstractUIPanel
{
    public Image unitPortrait;
    public Button btnWait;
    public Button btnDefend;
    public Button btnAbility1;
    public Button btnAbility2;
    public Button btnAbility3;

    public void UpdatePanel(AbstractCombatantPiece2 actp, bool canCommandSelectedPiece)
    {
        if (actp == null) return;

        CombatantHeroPiece2 asHero = actp as CombatantHeroPiece2;
        CombatantUnitPiece2 asUnit = actp as CombatantUnitPiece2;

        Sprite portrait = null;
        if (asHero) portrait = asHero.hero.dbData.profilePicture;
        else if (asUnit) portrait = asUnit.unit.dbData.profilePicture;
        unitPortrait.sprite = portrait;

        btnWait.interactable = !actp.pieceCombatActions.stateWait;
        btnWait.gameObject.SetActive(canCommandSelectedPiece);

        btnDefend.gameObject.SetActive(canCommandSelectedPiece);

        btnAbility1.gameObject.SetActive(false);
        btnAbility2.gameObject.SetActive(false);
        btnAbility3.gameObject.SetActive(false);
    }
}
