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

    public void UpdatePanel(CombatantPiece3 actp, bool canCommandSelectedPiece)
    {
        if (actp == null) return;

        UnitPiece3 unitPiece = actp as UnitPiece3;
        Sprite sprite = unitPiece ? unitPiece.abstractUnit.AU_GetProfileImage() : null;

        unitPortrait.sprite = sprite;
        UpdateWait(actp, canCommandSelectedPiece);
        UpdateDefend(actp, canCommandSelectedPiece);
        UpdateAbility1(actp, canCommandSelectedPiece);
        UpdateAbility2(actp, canCommandSelectedPiece);
        UpdateAbility3(actp, canCommandSelectedPiece);
    }

    private void UpdateWait(CombatantPiece3 actp, bool canCommandSelectedPiece)
    {
        bool canDoIt = actp.pieceCombatActions.canWait;
        bool notInState = !actp.pieceCombatActions.stateWait;
        btnWait.interactable = canCommandSelectedPiece && canDoIt && notInState;
    }

    private void UpdateDefend(CombatantPiece3 actp, bool canCommandSelectedPiece)
    {
        bool canDoIt = actp.pieceCombatActions.canDefend;
        bool notInState = !actp.pieceCombatActions.stateDefend;
        btnDefend.interactable = canCommandSelectedPiece && canDoIt && notInState;
    }

    private void UpdateAbility1(CombatantPiece3 actp, bool canCommandSelectedPiece)
    {
        bool canDoIt = true;    //TODO: can use abilities?
        DB_Ability ability = actp.abilityStats.ability1;
        btnAbility1.interactable = canCommandSelectedPiece && canDoIt && ability;

        //TODO: better button icon thing
        btnAbility1.image.sprite = ability ? ability.sprite : null;
    }

    private void UpdateAbility2(CombatantPiece3 actp, bool canCommandSelectedPiece)
    {
        bool canDoIt = true;    //TODO: can use abilities?
        DB_Ability ability = actp.abilityStats.ability2;
        btnAbility2.interactable = canCommandSelectedPiece && canDoIt && ability;

        //TODO: better button icon thing
        btnAbility2.image.sprite = ability ? ability.sprite : null;
    }

    private void UpdateAbility3(CombatantPiece3 actp, bool canCommandSelectedPiece)
    {
        bool canDoIt = true;    //TODO: can use abilities?
        DB_Ability ability = actp.abilityStats.ability3;
        btnAbility3.interactable = canCommandSelectedPiece && canDoIt && ability;

        //TODO: better button icon thing
        btnAbility3.image.sprite = ability ? ability.sprite : null;
    }
}
